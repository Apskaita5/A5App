using A5Soft.A5App.Application.Infrastructure;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace A5Soft.A5App.Repositories
{
    /// <summary>
    /// default ORM service provider per tenant for local (desktop) app
    /// </summary>         
    [DefaultServiceImplementation(typeof(ITenantOrmServiceProvider), BuildConfiguration.Client)]
    public class LocalTenantOrmServiceProvider : ITenantOrmServiceProvider
    {
        private readonly IDatabaseConfiguration _configuration;
        private readonly ISqlDictionaryProvider _sqlDictionaryProvider;
        private readonly ICacheProvider _cache;
        private readonly IPluginProvider _pluginProvider;


        public LocalTenantOrmServiceProvider(IDatabaseConfiguration configuration,
            ISqlDictionaryProvider sqlDictionaryProvider, ICacheProvider cache, IPluginProvider pluginProvider)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _sqlDictionaryProvider =
                sqlDictionaryProvider ?? throw new ArgumentNullException(nameof(sqlDictionaryProvider));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _pluginProvider = pluginProvider ?? throw new ArgumentNullException(nameof(pluginProvider));

            if (_configuration.SqlServerIsFileBased && _configuration.DatabaseFilesFolderPath.IsNullOrWhiteSpace())
                throw new ArgumentException("Databases folder path is not configured.", nameof(configuration));
            if (_configuration.BaseConnStringName.IsNullOrWhiteSpace()) throw new ArgumentException(
                "Base conn string key name is not configured.", nameof(configuration));
            if (null == _configuration.ConnStrings || !_configuration.ConnStrings
                .ContainsKey(_configuration.BaseConnStringName)) throw new ArgumentException(
                "Conn string is not configured.", nameof(configuration));
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
            if (_configuration.SqlServerIsFileBased)
            {
                return Directory.EnumerateFiles(_configuration.DatabaseFilesFolderPath,
                        "*.db", SearchOption.TopDirectoryOnly)
                    .ToDictionary(k => new Guid(Path.GetFileNameWithoutExtension(k).CreateMD5()), 
                        v => CreateService(v));
            }

            var agent = _configuration.CreateSqlAgent(
                _configuration.ConnStrings[_configuration.BaseConnStringName],
                string.Empty, _sqlDictionaryProvider.GetSqlDictionary());
            var result = await agent.FetchDatabasesAsync(
                _configuration.DatabaseNamePrefix + agent.Wildcart);

            return result.ToDictionary(k => new Guid(k.CreateMD5()),
                v => CreateService(v));
        }

        private IOrmService CreateService(string database)
        {
            return _configuration.CreateSqlAgent(_configuration.ConnStrings[_configuration.BaseConnStringName],
                    database, _sqlDictionaryProvider.GetSqlDictionary())
                .GetDefaultOrmService(CustomsMaps.CustomMaps);
        }

    }
}
