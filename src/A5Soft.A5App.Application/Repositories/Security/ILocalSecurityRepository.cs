using A5Soft.CARMA.Domain;
using System;
using System.Collections.Generic;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using System.Threading.Tasks;

namespace A5Soft.A5App.Application.Repositories.Security
{
    /// <summary>
    /// repository for basic administrative functions on local (desktop) app
    /// </summary>  
    [Service(ServiceLifetime.Transient)]
    public interface ILocalSecurityRepository
    {

        /// <summary>
        /// updates password for the current database
        /// </summary>
        /// <param name="newPassword">new password for the database</param>
        void UpdatePassword(string newPassword);

        /// <summary>
        /// Fetches a list of tenants (local databases).
        /// </summary>
        Task<List<TenantLookup>> FetchTenantLookupAsync();

        /// <summary>
        /// tries to login a local database and returns response based on the result
        /// (success/require password/error).
        /// </summary>
        /// <param name="tenantId">an id of the tenant (database) to login to</param>
        /// <param name="password">a password (if any)</param>
        Task<LocalLoginResponse> FetchLoginResponseAsync(Guid tenantId, string password);

    }
}
