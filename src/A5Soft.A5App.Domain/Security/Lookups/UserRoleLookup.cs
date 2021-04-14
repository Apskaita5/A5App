using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using A5Soft.CARMA.Domain.Metadata.DataAnnotations;

namespace A5Soft.A5App.Domain.Security.Lookups
{
    /// <summary>
    /// a lookup for <see cref="IUserRole"/> reference
    /// </summary>
    [Serializable]
    public class UserRoleLookup : LookupBase
    {
        private static readonly IDomainEntityIdentity _customRoleIdentity =
            new GuidDomainEntityIdentity(new Guid("B2729FD2-94FD-46DF-9A3F-6DC993DDED91"), typeof(UserRole));

        protected string _name;
        protected string _description;
           

        protected UserRoleLookup() { }


        /// <inheritdoc cref="IUserRole.Name"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
        Description = nameof(Resources.Security_IUserRole_Name_Description),
            Name = nameof(Resources.Security_IUserRole_Name_Name),
            ShortName = nameof(Resources.Security_IUserRole_Name_ShortName))]
        public string Name => _name;

        /// <inheritdoc cref="IUserRole.Description"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_IUserRole_Description_Description),
            Name = nameof(Resources.Security_IUserRole_Description_Name),
            ShortName = nameof(Resources.Security_IUserRole_Description_ShortName))]
        public string Description => _description;

        /// <summary>
        /// Gets a value indicating whether the lookup value is a placeholder for a custom role,
        /// i.e. not an actual role.
        /// </summary>
        [IgnorePropertyMetadata]
        public bool IsCustomRole => _id.IsSameIdentityAs(_customRoleIdentity);


        /// <summary>
        /// Gets a placeholder for custom user permissions.
        /// </summary>
        public static UserRoleLookup CustomRole 
            => new UserRoleLookup()
        {
            _id = _customRoleIdentity,
            _description = Resources.Security_UserRoleLookup_Custom_Role_Description,
            _name = Resources.Security_UserRoleLookup_Custom_Role_Name
        };


        public override string ToString()
        {
            return _name;
        }

    }
}
