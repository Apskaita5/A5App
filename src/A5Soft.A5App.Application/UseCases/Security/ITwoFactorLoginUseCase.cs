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
    /// A use case for two factor authentication login (second step).
    /// </summary>  
    [UseCase(ServiceLifetime.Singleton)]
    public interface ITwoFactorLoginUseCase
    {
        /// <summary>
        /// Completes two factor authentication (login) by providing a token.
        /// </summary>
        /// <param name="token">a two factor authentication token</param>
        /// <param name="ct">a cancellation token (if any)</param>
        [RemoteMethod]
        Task<LoginResponse> InvokeAsync(TwoFactorLoginRequest token, CancellationToken ct);

        /// <summary>
        /// Gets a metadata for query criteria, i.e. two factor authentication token. 
        /// </summary>
        IEntityMetadata GetCriteriaMetadata();

        /// <summary>
        /// Validates query criteria, i.e. two factor authentication token. 
        /// </summary>
        BrokenRulesCollection Validate(TwoFactorLoginRequest criteria);
    }
}