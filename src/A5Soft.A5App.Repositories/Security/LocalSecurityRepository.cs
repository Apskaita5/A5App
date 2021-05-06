using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using A5Soft.A5App.Repositories.Security.Maps;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;

namespace A5Soft.A5App.Repositories.Security
{
    /// <summary>
    /// a default implementation for <see cref="ILocalSecurityRepository"/>
    /// </summary>
    [DefaultServiceImplementation(typeof(ILocalSecurityRepository), BuildConfiguration.Client)]
    public class LocalSecurityRepository : ILocalSecurityRepository
    {
        private readonly ITenantOrmServiceProvider _ormServiceProvider;
        private readonly ISqlDictionaryProvider _sqlDictionaryProvider;
        private readonly IDatabaseConfiguration _databaseConfiguration;
        private readonly ILogger _logger;


        public LocalSecurityRepository(ITenantOrmServiceProvider ormServiceProvider,
            ISqlDictionaryProvider sqlDictionaryProvider, IDatabaseConfiguration databaseConfiguration, ILogger logger)
        {
            _ormServiceProvider = ormServiceProvider ?? throw new ArgumentNullException(nameof(ormServiceProvider));
            _sqlDictionaryProvider =
                sqlDictionaryProvider ?? throw new ArgumentNullException(nameof(sqlDictionaryProvider));
            _databaseConfiguration =
                databaseConfiguration ?? throw new ArgumentNullException(nameof(databaseConfiguration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <inheritdoc cref="ILocalSecurityRepository.FetchLoginResponseAsync"/>
        public async Task<LocalLoginResponse> FetchLoginResponseAsync(Guid tenantId, string password)
        {
            var service = await _ormServiceProvider.GetServiceAsync(tenantId);
            try
            {
                await service.Agent.FetchTableAsync("FetchTenantNameInTenantDb", null);
            }
            catch (SqlAuthenticationException)
            {
                 return new LocalLoginResponse(true);
            }
            catch (Exception e)
            {
                return new LocalLoginResponse(e.Message);
            }

            return new LocalLoginResponse(false);
        }

        /// <inheritdoc cref="ILocalSecurityRepository.FetchTenantLookupAsync"/>
        public async Task<List<TenantLookup>> FetchTenantLookupAsync()
        {
            var result = new List<TenantLookup>();

            if (_databaseConfiguration.SqlServerIsFileBased)
            {
                foreach (var dbFile in Directory.EnumerateFiles(_databaseConfiguration.DatabaseFilesFolderPath, 
                    "*.db", SearchOption.TopDirectoryOnly))
                {
                    var fileId = new Guid(Path.GetFileNameWithoutExtension(dbFile).CreateMD5());
                    try
                    {
                        var agent = _databaseConfiguration.CreateSqlAgent(
                            _databaseConfiguration.ConnStrings[_databaseConfiguration.BaseConnStringName],
                            dbFile, _sqlDictionaryProvider.GetSqlDictionary());
                        var tenantNameTbl = await agent.FetchTableAsync(
                            "FetchTenantNameInTenantDb", null);
                        if (tenantNameTbl.Rows.Any())
                        {
                            result.Add(new TenantLookupDb(fileId, tenantNameTbl.Rows[0].GetString(0)));
                        }
                        else
                        {
                            _logger.LogError(new InvalidOperationException(
                                $"FetchTenantNameInTenantDb returned null for FetchTenantNameInTenantDb " +
                                $"query on database file {dbFile}."));
                        }
                    }
                    catch (SqlAuthenticationException)
                    {
                        result.Add(new TenantLookupDb(fileId, ResolveTenantNameForFile(
                            Path.GetFileNameWithoutExtension(dbFile))));
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, dbFile);
                    }
                }
            }
            else
            {
                var agent = _databaseConfiguration.CreateSqlAgent(
                    _databaseConfiguration.ConnStrings[_databaseConfiguration.BaseConnStringName],
                    string.Empty, _sqlDictionaryProvider.GetSqlDictionary());

                var databases = await agent.FetchDatabasesAsync(
                    _databaseConfiguration.DatabaseNamePrefix + agent.Wildcart);

                foreach (var database in databases)
                {
                    try
                    {
                        var tenantNameTbl = await agent.FetchTableAsync(
                            "FetchTenantNameInTenantDb", null);
                        if (tenantNameTbl.Rows.Any())
                        {
                            result.Add(new TenantLookupDb(new Guid(database.CreateMD5()),
                                tenantNameTbl.Rows[0].GetString(0)));
                        }
                        else
                        {
                            _logger.LogError(new InvalidOperationException(
                                $"FetchTenantNameInTenantDb returned null for FetchTenantNameInTenantDb " +
                                $"query on database {database}."));
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, database);
                    }
                }
            }

            return result;
        }

        /// <inheritdoc cref="ILocalSecurityRepository.UpdatePassword"/>
        public void UpdatePassword(string newPassword)
        {
            throw new NotImplementedException();
        }


        private string ResolveTenantNameForFile(string fileName)
        {
            string[] words = Regex.Matches(fileName, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+|[0-9]+|[a-z]+)")
                .OfType<Match>()
                .Select(m => m.Value)
                .ToArray();
            return string.Join(" ", words);
        }
    }
}
