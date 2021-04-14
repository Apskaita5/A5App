using System;
using System.Threading.Tasks;
using A5Soft.DAL.Core;


namespace A5Soft.A5App.Repositories
{
    /// <summary>
    /// base interface for ORM service providers per tenant
    /// </summary>
    public interface IOrmServiceProvider
    {

        /// <summary>
        /// Gets an ORM service for security database.
        /// </summary>
        /// <returns>an ORM service for security database</returns>
        IOrmService GetServiceForSecurity();

        /// <summary>
        /// Gets an ORM service for tenant database.
        /// </summary>
        /// <param name="tenantId">an id of the tenant to get an ORM service for</param>
        /// <returns>an ORM service for tenant database</returns>
        /// <exception cref="ArgumentNullException">if tenantId is not specified</exception>
        /// <exception cref="InvalidOperationException">if no such tenant</exception>
        Task<IOrmService> GetServiceAsync(string tenantId);

    }
}