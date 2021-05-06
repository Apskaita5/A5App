using A5Soft.CARMA.Domain;
using System;
using System.Threading.Tasks;

namespace A5Soft.A5App.Application.Repositories
{
    /// <summary>
    /// A repository for managing transactions for security scoped data.
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public interface ISecurityTransactionalRepository
    {
        /// <summary>
        /// Executes action within a single transaction.
        /// </summary>
        /// <param name="action"></param>
        Task ExecuteInTransactionAsync(Func<Task> action);

        /// <summary>
        /// Executes function within a single transaction and returns its result.
        /// </summary>
        /// <typeparam name="T">type of the function result.</typeparam>
        /// <param name="action">function to execute</param>
        /// <returns>a result of the function executed</returns>
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action);

    }
}
