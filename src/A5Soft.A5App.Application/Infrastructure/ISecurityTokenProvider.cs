using System.Threading;
using System.Threading.Tasks;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// Provides content validation service (similar to JWT token)
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public interface ISecurityTokenProvider
    {
        /// <summary>
        /// Creates a new security token for the value specified.
        /// </summary>
        /// <param name="forValue">a value to create a security token for</param>
        /// <param name="ct">a cancellation token (if any)</param>
        /// <returns>a security token that can be used to validate whether
        /// the value has been tampered</returns>
        Task<string> CreateTokenAsync(string forValue, CancellationToken ct = default);

        /// <summary>
        /// Gets a value indicating whether the value specified match the security token
        /// issued by the security token provider.
        /// </summary>
        /// <param name="token">a security token to validate</param>
        /// <param name="forValue">a value for which the token was (presumably) issued</param>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<bool> ValidateTokenAsync(string token, string forValue, CancellationToken ct = default);

    }
}
