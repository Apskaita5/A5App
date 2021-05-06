using System;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Repositories;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Repositories
{
    /// <inheritdoc cref="ITenantTransactionalRepository"/>
    [DefaultServiceImplementation(typeof(ITenantTransactionalRepository))]
    public class TenantTransactionalRepository : ITenantTransactionalRepository
    {
        private readonly ITenantOrmServiceProvider _ormServiceProvider;
        private readonly Guid? _tenantId;


        public TenantTransactionalRepository(ITenantOrmServiceProvider ormServiceProvider, ClaimsIdentity user)
        {
            if (null == user) throw new ArgumentNullException(nameof(user));

            _ormServiceProvider = ormServiceProvider ?? throw new ArgumentNullException(nameof(ormServiceProvider));
            _tenantId = user.TenantSid();
        }


        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            if (null == action) throw new ArgumentNullException(nameof(action));

            if (!_tenantId.HasValue) throw new InvalidOperationException("No tenant selected.");

            var ormService = await _ormServiceProvider.GetServiceAsync(_tenantId.Value);

            await ormService.Agent.ExecuteInTransactionAsync(async () =>
            {
                await action();
            });
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            if (null == action) throw new ArgumentNullException(nameof(action));

            if (!_tenantId.HasValue) throw new InvalidOperationException("No tenant selected.");

            var ormService = await _ormServiceProvider.GetServiceAsync(_tenantId.Value);

            return await ormService.Agent.FetchInTransactionAsync(() =>
            {
                return action();
            });
        }
    }
}
