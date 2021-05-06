using A5Soft.CARMA.Domain;
using System;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Repositories;

namespace A5Soft.A5App.Repositories
{
    /// <inheritdoc cref="ISecurityTransactionalRepository"/>
    [DefaultServiceImplementation(typeof(ISecurityTransactionalRepository))]
    public class SecurityTransactionalRepository : ISecurityTransactionalRepository
    {
        private readonly ISecurityOrmServiceProvider _ormServiceProvider;


        public SecurityTransactionalRepository(ISecurityOrmServiceProvider ormServiceProvider)
        {
            _ormServiceProvider = ormServiceProvider ?? throw new ArgumentNullException(nameof(ormServiceProvider));
        }


        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            if (null == action) throw new ArgumentNullException(nameof(action));

            var ormService = _ormServiceProvider.GetService();

            await ormService.Agent.ExecuteInTransactionAsync(async () =>
            {
                await action();
            });
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            if (null == action) throw new ArgumentNullException(nameof(action));

            var ormService = _ormServiceProvider.GetService();

            return await ormService.Agent.FetchInTransactionAsync(() =>
            {
                return action();
            });
        }
    }
}
