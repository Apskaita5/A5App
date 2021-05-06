using System.Collections.Generic;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security.Users
{
    /// <summary>
    /// A use case that updates an existing <see cref="Domain.Security.User"/>.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_UpdateUser_Name))]
    [UseCase(ServiceLifetime.Transient, typeof(List<UserGroupLookup>), typeof(List<UserRoleLookup>))]
    public interface IUpdateUserUseCase : IAuthorizedUseCase
    {
        /// <summary>
        /// Updates an existing <see cref="Domain.Security.User"/> using the data specified.
        /// </summary>
        /// <param name="domainDto">data for an updated <see cref="Domain.Security.User"/></param>
        /// <returns>the updated <see cref="Domain.Security.User"/></returns>
        [RemoteMethod]
        Task<User> SaveAsync(IUser domainDto);
    }
}
