using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using A5Soft.DAL.Core.SqlDictionary;

namespace A5Soft.A5App.Repositories
{
    /// <summary>
    /// default ORM service provider per tenant
    /// </summary>
    /// <typeparam name="TProvider">a type that implements ISqlAgent</typeparam>
    public class OrmServiceProvider<TProvider> : IOrmServiceProvider where TProvider : ISqlAgent
    {
        private readonly string _baseConnStringName;
        private readonly string _baseDbStructureFilePath;
        private readonly string _securityDbStructureFilePath;
        private readonly Dictionary<string, string> _connStrings;
        private readonly ICacheProvider _cache;
        private readonly IPluginProvider _pluginProvider;

        private readonly ISqlDictionary _sqlDictionary;
        private readonly IOrmService _ormServiceForSecurity;


        /// <summary>
        /// creates a new instance of default ORM service provider per tenant
        /// </summary>
        /// <param name="securityDbName">a name of the security database</param>
        /// <param name="baseConnStringName">a base conn string (key) name within the app config
        /// if same conn string is used for more than one database</param>
        /// <param name="baseSqlRepositoryPath">a folder path where base SQL repositories are stored</param>
        /// <param name="baseDbStructureFilePath">a file that contains base tenant database structure</param>
        /// <param name="securityDbStructureFilePath">a file that contains security database structure</param>
        /// <param name="connStrings">a dictionary of conn strings within the app config</param>
        /// <param name="cache">cache provider</param>
        /// <param name="pluginProvider">plugin provider</param>
        public OrmServiceProvider(string securityDbName, string baseConnStringName, string baseSqlRepositoryPath,
            string baseDbStructureFilePath, string securityDbStructureFilePath, Dictionary<string, string> connStrings,
            ICacheProvider cache, IPluginProvider pluginProvider)
        {
            if (securityDbName.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(securityDbName));
            if (baseSqlRepositoryPath.IsNullOrWhiteSpace()) 
                throw new ArgumentNullException(nameof(baseSqlRepositoryPath));
            if (baseDbStructureFilePath.IsNullOrWhiteSpace()) 
                throw new ArgumentNullException(nameof(baseDbStructureFilePath));
            if (securityDbStructureFilePath.IsNullOrWhiteSpace()) 
                throw new ArgumentNullException(nameof(securityDbStructureFilePath));
            if (null == connStrings) throw new ArgumentNullException(nameof(connStrings));

            if (connStrings.Count < 1) throw new InvalidOperationException(
                "No conn strings configured.");
            if (!connStrings.ContainsKey(securityDbName) && (baseConnStringName.IsNullOrWhiteSpace()
                || !connStrings.ContainsKey(baseConnStringName))) throw new InvalidOperationException(
                    "No base conn string configured (no conn string for security database as well).");

            _baseConnStringName = baseConnStringName;
            _baseDbStructureFilePath = baseDbStructureFilePath;
            _securityDbStructureFilePath = securityDbStructureFilePath;
            _connStrings = connStrings;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _pluginProvider = pluginProvider ?? throw new ArgumentNullException(nameof(pluginProvider));

            var sqlDictionaryPaths = _pluginProvider.GetSqlRepositoryPaths()
                .Select(kv => kv.Value).ToList();
            sqlDictionaryPaths.Insert(0, baseSqlRepositoryPath);
            _sqlDictionary = new XmlFolderSqlDictionary(sqlDictionaryPaths, true);

            string connString = _connStrings.ContainsKey(securityDbName) ?
                _connStrings[securityDbName]
                : _connStrings[_baseConnStringName];
            var result = (ISqlAgent)Activator.CreateInstance(typeof(TProvider),
                new object[] { connString, securityDbName, _sqlDictionary });
            _ormServiceForSecurity= result.GetDefaultOrmService(CustomsMaps.CustomMaps);
        }


        /// <inheritdoc cref="IOrmServiceProvider.GetServiceForSecurity"/>
        public IOrmService GetServiceForSecurity()
        {
            return _ormServiceForSecurity;
        }

        /// <inheritdoc cref="IOrmServiceProvider.GetServiceAsync"/>
        public async Task<IOrmService> GetServiceAsync(string tenantId)
        {
            if (tenantId.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(tenantId));

            return await _cache.GetOrCreate(tenantId, async () => await CreateServiceAsync(tenantId));
        }


        private async Task<IOrmService> CreateServiceAsync(string tenantId)
        {
            var dbNameResult = await _ormServiceForSecurity.Agent.FetchTableAsync(
                "FetchDatabaseName", new SqlParam[]
                    {SqlParam.Create("CD", tenantId.Trim())});

            var dbName = string.Empty;
            if (dbNameResult.Rows.Count > 0) 
                dbName = dbNameResult.Rows[0].GetStringOrDefault(0, string.Empty);

            if (dbName.IsNullOrWhiteSpace()) throw new InvalidOperationException(
                $"No database for identifier {tenantId}.");

            string connString = _connStrings.ContainsKey(dbName) ?
                _connStrings[dbName]
                : _connStrings[_baseConnStringName];
            var result = (ISqlAgent)Activator.CreateInstance(typeof(TProvider),
                new object[] { connString, dbName, _sqlDictionary });
            return result.GetDefaultOrmService(CustomsMaps.CustomMaps);
        }

    }
}
