using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;

namespace A5Soft.A5App.Repositories
{
    /// <summary>
    /// security database ORM service provider for app server
    /// </summary>
    [DefaultServiceImplementation(typeof(ISecurityOrmServiceProvider), BuildConfiguration.Server)]
    public class SecurityOrmServiceProvider : ISecurityOrmServiceProvider
    {
        private readonly IDatabaseConfiguration _configuration;
        private readonly IPluginProvider _pluginProvider;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISecureRandomProvider _randomProvider;
        private readonly IEmailProvider _emailProvider;

        private readonly IOrmService _ormServiceForSecurity;
                        
        
        /// <summary>
        /// creates a new instance of default ORM service provider per tenant
        /// </summary>
        /// <param name="configuration">database configuration data</param>
        /// <param name="cache">cache provider</param>
        /// <param name="pluginProvider">plugin provider</param>
        /// <param name="passwordHasher">password hasher</param>
        /// <param name="randomProvider">secure random provider</param>
        /// <param name="emailProvider">email provider</param>
        public SecurityOrmServiceProvider(IDatabaseConfiguration configuration, ICacheProvider cache,
            IPluginProvider pluginProvider, ISqlDictionaryProvider sqlDictionaryProvider, 
            IPasswordHasher passwordHasher, ISecureRandomProvider randomProvider, IEmailProvider emailProvider)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _pluginProvider = pluginProvider ?? throw new ArgumentNullException(nameof(pluginProvider));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _randomProvider = randomProvider ?? throw new ArgumentNullException(nameof(randomProvider));
            _emailProvider = emailProvider ?? throw new ArgumentNullException(nameof(emailProvider));
            if (null == sqlDictionaryProvider) throw new ArgumentNullException(nameof(sqlDictionaryProvider));

            if (_configuration.SecurityDbName.IsNullOrWhiteSpace()) throw new ArgumentException(
                "Security database name is not configured.", nameof(configuration));
            if (_configuration.SecurityDbStructureFilePath.IsNullOrWhiteSpace()) throw new ArgumentException(
                "Path for security database base schema file is not configured.", nameof(configuration));
            if (_configuration.SqlServerIsFileBased && _configuration.DatabaseFilesFolderPath.IsNullOrWhiteSpace())
                throw new ArgumentException("Databases folder path is not configured.", nameof(configuration));
            if (null == _configuration.ConnStrings || !_configuration.ConnStrings.Any()) throw new ArgumentException(
                "Conn strings are not configured.", nameof(configuration));
            if (!_configuration.ConnStrings.ContainsKey(_configuration.SecurityDbName) &&
                (_configuration.BaseConnStringName.IsNullOrWhiteSpace()
                || !_configuration.ConnStrings.ContainsKey(_configuration.BaseConnStringName)))
                throw new ArgumentException(
                    "No base conn string configured (no conn string for security database as well).",
                    nameof(configuration));

            string connString = _configuration.ConnStrings.ContainsKey(_configuration.SecurityDbName) ?
                _configuration.ConnStrings[_configuration.SecurityDbName]
                : _configuration.ConnStrings[_configuration.BaseConnStringName];

            var securityDatabaseName = _configuration.SqlServerIsFileBased
                ? System.IO.Path.Combine(_configuration.DatabaseFilesFolderPath, $"{_configuration.SecurityDbName}.db")
                : _configuration.SecurityDbName;

            _ormServiceForSecurity = _configuration.CreateSqlAgent(connString, securityDatabaseName, 
                    sqlDictionaryProvider.GetSqlDictionary())
                .GetDefaultOrmService(CustomsMaps.CustomMaps);
        }


        /// <inheritdoc cref="ISecurityOrmServiceProvider.GetService"/>
        public IOrmService GetService()
        {
            return _ormServiceForSecurity;
        }
         
        /// <inheritdoc cref="ISecurityOrmServiceProvider.InitializeAsync"/>
        public async Task InitializeAsync()
        {
            var schemaPaths = _pluginProvider.GetSecurityDbStructurePaths()
                .Select(v => v.Value).ToList();
            schemaPaths.Add(_configuration.SecurityDbStructureFilePath);

            var schema = DAL.Core.Serialization.Extensions.ReadAggregateDbSchema(schemaPaths);

            await _ormServiceForSecurity.Agent.GetDefaultSchemaManager().InitDatabaseAsync(schema);

            var usrCount = await _ormServiceForSecurity.Agent.FetchTableAsync(
                "FetchUserCount", null);
            if (!usrCount.Rows.Any() || usrCount.Rows[0].GetInt32OrDefault(0, 0) < 1)
            {
                var firstUserPassword = _randomProvider.CreateNewPassword(8);
                var firstUser = new User.UserDto()
                {
                    Email = _configuration.FirstUserEmail,
                    RolesForTenants = new List<UserTenant.UserTenantDto>(),
                    AdminRole = AdministrativeRole.Admin,
                    Id = new GuidDomainEntityIdentity(typeof(User)),
                    Phone = string.Empty,
                    HashedPassword = _passwordHasher.HashPassword(firstUserPassword),
                    Name = "Admin",
                    InsertedAt = DateTime.UtcNow,
                    InsertedBy = "init",
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = "init"
                };

                await _ormServiceForSecurity.ExecuteInsertAsync(firstUser, "init");

                var message = new EmailMessage()
                {
                    Email = _configuration.FirstUserEmail,
                    IsHtml = false,
                    Subject = "A5 init password",
                    Content = $"Your password is {firstUserPassword}"
                };

                await _emailProvider.SendAsync(message);
            }
        }
        
    }
}
