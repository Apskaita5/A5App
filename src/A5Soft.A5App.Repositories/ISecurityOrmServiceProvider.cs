using System.Threading.Tasks;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;


namespace A5Soft.A5App.Repositories
{
    /// <summary>
    /// base interface for ORM service providers for security database
    /// </summary> 
    [Service(ServiceLifetime.Singleton)]
    public interface ISecurityOrmServiceProvider
    {
        /// <summary>
        /// Gets an ORM service for security database.
        /// </summary>
        /// <returns>an ORM service for security database</returns>
        IOrmService GetService();

        /// <summary>
        /// Initializes security database, i.e.:
        /// - creates the database if it does not exist;
        /// - creates tables if it is empty;
        /// - checks and repairs schema errors;
        /// - adds the first user.
        /// </summary>
        Task InitializeAsync();
    }
}