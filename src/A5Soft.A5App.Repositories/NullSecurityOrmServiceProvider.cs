using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using System;
using System.Threading.Tasks;

namespace A5Soft.A5App.Repositories
{
    /// <summary>
    /// security database ORM service provider for remote client
    /// </summary>
    /// <remarks>used to enable instantiation of security use cases which in turn use security repositories
    /// on local (desktop) client; throws NotSupported exception if actually invoked</remarks> 
    [DefaultServiceImplementation(typeof(ISecurityOrmServiceProvider), BuildConfiguration.Client)]
    public class NullSecurityOrmServiceProvider : ISecurityOrmServiceProvider
    {
        /// <summary>
        /// throws NotSupported exception if actually invoked
        /// </summary>
        public IOrmService GetService()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// throws NotSupported exception if actually invoked
        /// </summary>
        public Task InitializeAsync()
        {
            throw new NotSupportedException();
        }
    }
}
