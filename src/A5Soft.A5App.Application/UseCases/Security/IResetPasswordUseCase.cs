using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case for requesting user password reset.
    /// </summary>  
    [UseCase(ServiceLifetime.Singleton)]
    public interface IResetPasswordUseCase
    {
        /// <summary>
        /// Request user password reset. 
        /// </summary>
        /// <param name="request">a request to reset user password (basically an email)</param>
        [RemoteMethod]
        Task InvokeAsync(ResetPasswordRequest request);

        /// <summary>
        /// Gets a metadata for query criteria, i.e. a request to reset user password (basically an email). 
        /// </summary>
        IEntityMetadata GetParameterMetadata();

        /// <summary>
        /// Validates query criteria, i.e. a request to reset user password (basically an email). 
        /// </summary>
        BrokenRulesCollection Validate(ResetPasswordRequest parameter);
    }
}