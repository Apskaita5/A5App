using System.Collections.Generic;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;

namespace A5Soft.A5App.Repositories
{
    /// <summary>
    /// a container for database configuration options
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public interface IDatabaseConfiguration
    {

        /// <summary>
        /// a name of the security database (required)
        /// </summary>
        string SecurityDbName { get; }

        /// <summary>
        /// a base conn string (key) name within the app config if same conn string is used
        /// for more than one database
        /// </summary>
        string BaseConnStringName { get; }

        /// <summary>
        /// a folder path where base SQL repositories are stored (required)
        /// </summary>
        string BaseSqlRepositoryPath { get; }

        /// <summary>
        /// a file that contains base tenant database structure (required) 
        /// </summary>
        string BaseDbStructureFilePath { get; }

        /// <summary>
        /// a file that contains a base security database structure (required)
        /// </summary>
        string SecurityDbStructureFilePath { get; }

        /// <summary>
        /// a value indicating whether the SQL server in use is file based
        /// </summary>
        bool SqlServerIsFileBased { get; }

        /// <summary>
        /// a folder where the database files are stored if the SQL server is file based (required)
        /// </summary>
        string DatabaseFilesFolderPath { get; }

        /// <summary>
        /// a dictionary of conn strings within the app config (required)
        /// </summary>
        IDictionary<string, string> ConnStrings { get; }

        /// <summary>
        /// an email of the first app user, used to init database on first use (required)
        /// </summary>
        string FirstUserEmail { get; }

        /// <summary>
        /// a database name prefix to use if the database creation is managed by the app
        /// </summary>
        string DatabaseNamePrefix { get; }

        /// <summary>
        /// a factory method for creating ISqlAgent instances based on app configuration 
        /// </summary>
        /// <param name="connString">a conn string to use</param>
        /// <param name="dbName">a database name</param>
        /// <param name="sqlDictionary">an SQL dictionary</param>
        ISqlAgent CreateSqlAgent(string connString, string dbName, ISqlDictionary sqlDictionary);

    }
}
