using A5Soft.A5App.Application.Infrastructure;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.WebApp
{
    /// <inheritdoc cref="IBaseUrlProvider"/>
    [DefaultServiceImplementation(typeof(IBaseUrlProvider))]
    public class BaseUrlProvider : IBaseUrlProvider
    {
        
        public string BaseUrl { get; set; }

    }
}
