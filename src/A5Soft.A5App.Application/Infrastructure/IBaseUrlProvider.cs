using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// base interface for base url provider (provides web app base url for password reset confirmation etc.)
    /// </summary>
    [Service(ServiceLifetime.Transient)]
    public interface IBaseUrlProvider
    {
        /// <summary>
        /// Gets a web app base url for password reset confirmation etc.
        /// </summary>
        string BaseUrl { get; set; }
    }
}
