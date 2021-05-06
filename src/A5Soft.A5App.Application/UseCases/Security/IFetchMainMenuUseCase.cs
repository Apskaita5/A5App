using System.Threading.Tasks;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Navigation;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// a use case to fetch the application main menu
    /// </summary>
    [AuthenticatedAuthorization]
    [UseCase(ServiceLifetime.Transient)]
    public interface IFetchMainMenuUseCase
    {
        /// <summary>
        /// fetches the application main menu
        /// </summary>
        [RemoteMethod]
        Task<MainMenu> FetchAsync();
    }
}