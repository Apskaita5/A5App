using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Infrastructure
{
    /// <summary>
    /// <see cref="ISecurityTokenProvider"/> implementation using a service secret
    /// (random string stored in the app config) to sign values.
    /// </summary>
    [DefaultServiceImplementation(typeof(ISecurityTokenProvider))]
    public class ServerSecretSecurityTokenProvider : ISecurityTokenProvider
    {
        private readonly ISecurityPolicy _securityPolicy;


        public ServerSecretSecurityTokenProvider(ISecurityPolicy securityPolicy)
        {
            _securityPolicy = securityPolicy ?? throw new ArgumentNullException(nameof(securityPolicy));
        }


        /// <inheritdoc cref="ISecurityTokenProvider.CreateTokenAsync"/>
        public Task<string> CreateTokenAsync(string forValue, CancellationToken ct = default)
        {
            return Task.FromResult(ComputeSha256Hash($"{_securityPolicy.ServerSecret}|{forValue}"));
        }

        /// <inheritdoc cref="ISecurityTokenProvider.ValidateTokenAsync"/>
        public Task<bool> ValidateTokenAsync(string token, string forValue, CancellationToken ct = default)
        {
            if (token.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(token));
            if (forValue.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(forValue));

            var expectedToken = ComputeSha256Hash($"{_securityPolicy.ServerSecret}|{forValue}");

            return Task.FromResult(expectedToken == token);
        }


        private static string ComputeSha256Hash(string rawData)
        {
            if (rawData.IsNullOrWhiteSpace()) return string.Empty;
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
