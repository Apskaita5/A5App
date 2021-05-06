using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;

namespace A5Soft.A5App.Repositories
{
    /// <summary>
    /// base interface for <see cref="ISqlDictionary"/> providers that are required
    /// for <see cref="ISecurityOrmServiceProvider"/> and <see cref="ITenantOrmServiceProvider"/>
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public interface ISqlDictionaryProvider
    {
        /// <summary>
        /// Gets an SQL dictionary.
        /// </summary>
        ISqlDictionary GetSqlDictionary();
    }
}
