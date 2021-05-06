using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case for user login.
    /// </summary> 
    [UseCase(ServiceLifetime.Singleton)]
    public interface ILoginUseCase : IUseCase
    {
        /// <summary>
        /// Log in.
        /// </summary>
        /// <param name="credentials">user credentials</param>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<LoginResponse> InvokeAsync(LoginRequest credentials, CancellationToken ct);

        /// <summary>
        /// Gets a metadata for query criteria, i.e. user credentials. 
        /// </summary>
        IEntityMetadata GetCriteriaMetadata();

        /// <summary>
        /// Validates query criteria, i.e. user credentials. 
        /// </summary>
        BrokenRulesCollection Validate(LoginRequest criteria);
    }
}