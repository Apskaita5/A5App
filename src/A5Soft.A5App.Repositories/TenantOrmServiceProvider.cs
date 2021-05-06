using A5Soft.A5App.Application.Infrastructure;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A5Soft.A5App.Repositories
{
    /// <summary>
    /// default ORM service provider per tenant for server
    /// </summary>
    [DefaultServiceImplementation(typeof(ITenantOrmServiceProvider), BuildConfiguration.Server)]
    public class TenantOrmServiceProvider : ITenantOrmServiceProvider
    {
        private readonly IDatabaseConfiguration _configuration;
        private readonly ISqlDictionaryProvider _sqlDictionaryProvider;
        private readonly ICacheProvider _cache;
        private readonly IPluginProvider _pluginProvider;
        private readonly ISecurityOrmServiceProvider _securityOrmServiceProvider;


        public TenantOrmServiceProvider(IDatabaseConfiguration configuration, ICacheProvider cache,
            ISecurityOrmServiceProvider securityOrmServiceProvider, IPluginProvider pluginProvider,
            ISqlDictionaryProvider sqlDictionaryProvider)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _securityOrmServiceProvider = securityOrmServiceProvider ??
                throw new ArgumentNullException(nameof(securityOrmServiceProvider));
            _pluginProvider = pluginProvider ?? throw new ArgumentNullException(nameof(pluginProvider));
            _sqlDictionaryProvider = sqlDictionaryProvider ?? throw new ArgumentNullException(nameof(sqlDictionaryProvider));

            if (_configuration.SqlServerIsFileBased && _configuration.DatabaseFilesFolderPath.IsNullOrWhiteSpace())
                throw new ArgumentException("Databases folder path is not configured.", nameof(configuration));
            if (null == _configuration.ConnStrings || !_configuration.ConnStrings.Any()) throw new ArgumentException(
                "Conn strings are not configured.", nameof(configuration));
        }


        /// <inheritdoc cref="ITenantOrmServiceProvider.GetServiceAsync"/>
        public async Task<IOrmService> GetServiceAsync(Guid tenantId)
        {
            if (tenantId == Guid.Empty) throw new ArgumentNullException(nameof(tenantId));

            var dictionary = await _cache.GetOrCreate(async () => await CreateServiceDictionaryAsync());

            if (!dictionary.ContainsKey(tenantId)) throw new InvalidOperationException(
                $"No database for identifier {tenantId: N}.");

            return dictionary[tenantId];
        }

        private async Task<Dictionary<Guid, IOrmService>> CreateServiceDictionaryAsync()
        {
            var dbNameResult = await _securityOrmServiceProvider.GetService()
                .Agent.FetchTableAsync("FetchDatabaseNameDictionary", null);

            var result = new Dictionary<Guid, IOrmService>();
            foreach (var row in dbNameResult.Rows)
            {
                var id = row.GetGuid(0);

                var dbName = row.GetString(1);
                if (dbName.IsNullOrWhiteSpace()) throw new InvalidOperationException(
                    $"Database name is null for identifier {id: N} (security database content error).");

                var connString = _configuration.ConnStrings.ContainsKey(dbName) ?
                    _configuration.ConnStrings[dbName]
                    : _configuration.ConnStrings[_configuration.BaseConnStringName];

                if (_configuration.SqlServerIsFileBased)
                    dbName = System.IO.Path.Combine(_configuration.DatabaseFilesFolderPath, $"{dbName}.db");

                var service = _configuration.CreateSqlAgent(connString, dbName, 
                        _sqlDictionaryProvider.GetSqlDictionary())
                    .GetDefaultOrmService(CustomsMaps.CustomMaps);

                result.Add(id, service);
            }

            return result;
        }

    }
}
