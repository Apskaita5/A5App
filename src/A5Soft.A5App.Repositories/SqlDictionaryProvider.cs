using System;
using System.Linq;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using A5Soft.DAL.Core.SqlDictionary;

namespace A5Soft.A5App.Repositories
{
    /// <summary>
    /// default implementation of <see cref="ISqlDictionaryProvider"/>
    /// </summary>
    [DefaultServiceImplementation(typeof(ISqlDictionaryProvider))]
    public class SqlDictionaryProvider : ISqlDictionaryProvider
    {
        private readonly IDatabaseConfiguration _configuration;
        private readonly IPluginProvider _pluginProvider;

        private readonly ISqlDictionary _sqlDictionary;


        public SqlDictionaryProvider(IDatabaseConfiguration configuration, IPluginProvider pluginProvider)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _pluginProvider = pluginProvider ?? throw new ArgumentNullException(nameof(pluginProvider));

            if (_configuration.BaseSqlRepositoryPath.IsNullOrWhiteSpace()) throw new ArgumentException(
                "Base SQL repository folder path is not configured.", nameof(configuration));

            var sqlDictionaryPaths = _pluginProvider.GetSqlRepositoryPaths()
                .Select(kv => kv.Value).ToList();
            sqlDictionaryPaths.Insert(0, _configuration.BaseSqlRepositoryPath);
            _sqlDictionary = new XmlFolderSqlDictionary(sqlDictionaryPaths, true);
        }


        /// <inheritdoc cref="ISqlDictionaryProvider.GetSqlDictionary"/>
        public ISqlDictionary GetSqlDictionary()
        {
            return _sqlDictionary;
        }
    }
}
