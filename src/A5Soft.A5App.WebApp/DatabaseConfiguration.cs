using System;
using System.Collections.Generic;
using System.Linq;
using A5Soft.A5App.Repositories;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using A5Soft.DAL.MySql;
using A5Soft.DAL.SQLite;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace A5Soft.A5App.WebApp
{
    /// <inheritdoc cref="IDatabaseConfiguration"/>
    [DefaultServiceImplementation(typeof(IDatabaseConfiguration))]
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        public DatabaseConfiguration(IConfiguration config, IWebHostEnvironment environment)
        {
            if (null == config) throw new ArgumentNullException(nameof(config));
            if (null == environment) throw new ArgumentNullException(nameof(environment));

            var usrConfigGroup = nameof(DatabaseConfiguration);
            if (environment.IsDevelopment()) usrConfigGroup = "Dev" + usrConfigGroup;

            SecurityDbName = config[$"{usrConfigGroup}:{nameof(SecurityDbName)}"]?.Trim() ?? string.Empty;
            BaseConnStringName = config[$"{usrConfigGroup}:{nameof(BaseConnStringName)}"]?.Trim() ?? string.Empty;
            FirstUserEmail = config[$"{usrConfigGroup}:{nameof(FirstUserEmail)}"]?.Trim() ?? string.Empty;
            SqlImplementationCode = config[$"{usrConfigGroup}:{nameof(SqlImplementationCode)}"]?.Trim() ?? string.Empty;
            DatabaseNamePrefix = config[$"{nameof(DatabaseConfiguration)}:{nameof(DatabaseNamePrefix)}"]?.Trim() ?? string.Empty;
            if (int.TryParse(config[$"{nameof(DatabaseConfiguration)}:{nameof(SqlQueryTimeout)}"], out int result)) 
                SqlQueryTimeout = result;
            else SqlQueryTimeout = 120000;
            ConnStrings = config.GetSection(environment.IsDevelopment() ? "DevConnectionStrings" : "ConnectionStrings")
                .GetChildren().ToDictionary(
                v => v.Key.Trim(), 
                v => v.Value.Trim(), 
                StringComparer.OrdinalIgnoreCase);

            if (SecurityDbName.IsNullOrWhiteSpace()) throw new ArgumentException(
                "Security database name is not configured.", nameof(config));
            if (FirstUserEmail.IsNullOrWhiteSpace()) throw new ArgumentException(
                "First user email is not configured.", nameof(config));
            if (SqlImplementationCode.IsNullOrWhiteSpace()) throw new ArgumentException(
                "SQL sever type is not configured.", nameof(config));
            if (!SqlImplementationCode.Equals("mysql", StringComparison.OrdinalIgnoreCase)
                && !SqlImplementationCode.Equals("sqlite", StringComparison.OrdinalIgnoreCase))
                throw new NotImplementedException($"SQL server type {SqlImplementationCode} is not implemented.");

            SqlServerIsFileBased = SqlImplementationCode.Equals("sqlite", StringComparison.OrdinalIgnoreCase);
                  
            if (environment.IsDevelopment())
            {
                BaseSqlRepositoryPath = System.IO.Path.Combine(AppContext.BaseDirectory, "SqlRepositories");
                BaseDbStructureFilePath = System.IO.Path.Combine(AppContext.BaseDirectory, "tenantSchema.xml");
                SecurityDbStructureFilePath = System.IO.Path.Combine(AppContext.BaseDirectory, "securitySchema.xml");
                DatabaseFilesFolderPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Data");
            }
            else
            {
                BaseSqlRepositoryPath = System.IO.Path.Combine(environment.ContentRootPath, "SqlRepositories");
                BaseDbStructureFilePath = System.IO.Path.Combine(environment.ContentRootPath, "tenantSchema.xml");
                SecurityDbStructureFilePath = System.IO.Path.Combine(environment.ContentRootPath, "securitySchema.xml");
                DatabaseFilesFolderPath = System.IO.Path.Combine(environment.ContentRootPath, "Data");
            }
        }


        /// <inheritdoc cref="IDatabaseConfiguration.SecurityDbName"/>
        public string SecurityDbName { get; }

        /// <inheritdoc cref="IDatabaseConfiguration.BaseConnStringName"/>
        public string BaseConnStringName { get; }

        /// <inheritdoc cref="IDatabaseConfiguration.BaseSqlRepositoryPath"/>
        public string BaseSqlRepositoryPath { get; }

        /// <inheritdoc cref="IDatabaseConfiguration.BaseDbStructureFilePath"/>
        public string BaseDbStructureFilePath { get; }

        /// <inheritdoc cref="IDatabaseConfiguration.SecurityDbStructureFilePath"/>
        public string SecurityDbStructureFilePath { get; }

        /// <inheritdoc cref="IDatabaseConfiguration.ConnStrings"/>
        public IDictionary<string, string> ConnStrings { get; }

        /// <inheritdoc cref="IDatabaseConfiguration.FirstUserEmail"/>
        public string FirstUserEmail { get; }

        /// <summary>
        /// SQL server type (mysql or sqlite).
        /// </summary>
        public string SqlImplementationCode { get; }

        /// <summary>
        /// Gets configured SQL query/command timeout in ms.
        /// </summary>
        public int SqlQueryTimeout { get; }

        /// <inheritdoc cref="IDatabaseConfiguration.SqlServerIsFileBased"/>
        public bool SqlServerIsFileBased { get; }

        /// <inheritdoc cref="IDatabaseConfiguration.DatabaseFilesFolderPath"/>
        public string DatabaseFilesFolderPath { get; }

        /// <inheritdoc cref="IDatabaseConfiguration.DatabaseNamePrefix"/>
        public string DatabaseNamePrefix { get; }


        /// <inheritdoc cref="IDatabaseConfiguration.CreateSqlAgent"/>
        public ISqlAgent CreateSqlAgent(string connString, string dbName, ISqlDictionary sqlDictionary)
        {
            if (connString.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(connString));
            if (dbName.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(dbName));
            if (null == sqlDictionary) throw new ArgumentNullException(nameof(sqlDictionary));

            if (SqlImplementationCode.IsNullOrWhiteSpace()) throw new InvalidOperationException(
                "SQL server type is not configured.");

            if (SqlImplementationCode.Equals("mysql", StringComparison.OrdinalIgnoreCase))
            {
                return new MySqlAgent(connString, dbName, sqlDictionary)
                {
                    QueryTimeOut = SqlQueryTimeout,
                    BooleanStoredAsTinyInt = true,
                    UseTransactionPerInstance = false,
                    AllSchemaNamesLowerCased = true,
                    GuidStoredAsBlob = false
                };
            }
            if (SqlImplementationCode.Equals("sqlite", StringComparison.OrdinalIgnoreCase))
            {
                return new SqliteAgent(connString, dbName, sqlDictionary)
                {
                    QueryTimeOut = SqlQueryTimeout,
                    BooleanStoredAsTinyInt = true,
                    UseTransactionPerInstance = false,
                    AllSchemaNamesLowerCased = true,
                    GuidStoredAsBlob = false
                };
            }

            throw new NotImplementedException($"SQL server type {SqlImplementationCode} is not implemented.");
        }

    }
}
