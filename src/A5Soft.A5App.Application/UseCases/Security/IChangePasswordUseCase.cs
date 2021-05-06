using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case for changing the user password.
    /// </summary> 
    [UseCase(ServiceLifetime.Transient)]
    [AuthenticatedAuthorization]
    public interface IChangePasswordUseCase : IAuthorizedUseCase
    {
        /// <summary>
        /// Change the current user password.
        /// </summary>
        /// <param name="request">a request to change the current user password</param>
        [RemoteMethod]
        Task InvokeAsync(ChangePasswordRequest request);

        /// <summary>
        /// Gets a metadata for a request to change the current user password. 
        /// </summary>
        IEntityMetadata GetParameterMetadata();

        /// <summary>
        /// Validates a request to change the current user password.
        /// </summary>
        /// <param name="parameter">a request to change the current user password</param>
        BrokenRulesCollection Validate(ChangePasswordRequest parameter);

    }
}