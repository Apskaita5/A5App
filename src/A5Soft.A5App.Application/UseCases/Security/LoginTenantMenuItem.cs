using System;
using System.Security.Claims;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.Navigation;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A menu item for tenant login/logout action. 
    /// </summary>
    [Serializable]
    public class LoginTenantMenuItem : MenuItem
    {
        /// <inheritdoc />
        public LoginTenantMenuItem(TenantLookup tenant) : base()
        {
            Name = tenant?.Name ?? Resources.LoginTenantMenuItem_Logout;
            this.ItemType = MenuItemType.UseCase;
            this.UseCaseType = typeof(ILoginTenantUseCase);
            TenantId = tenant?.Id;
        }


        public IDomainEntityIdentity TenantId { get; }


        /// <inheritdoc />
        public override string GetDescription()
        {
            return string.Empty;
        }

        /// <inheritdoc />
        public override string GetDisplayName()
        {
            return Name;
        }

        /// <inheritdoc />
        protected override void SetAuthorization(IAuthorizationProvider authorizationProvider, ClaimsIdentity user)
        {
            if (TenantId.IsNullOrNew()) IsEnabled = user.TenantSid().HasValue;
            else IsEnabled = true;
        }
    }
}
