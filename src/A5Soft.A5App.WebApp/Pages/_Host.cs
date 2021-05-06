using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace A5Soft.A5App.WebApp.Pages
{
    public class _Host : PageModel
    {
        private CookieAuthenticationStateProvider _authProvider;
        private IBaseUrlProvider _urlProvider;

        /// <inheritdoc />
        public _Host(AuthenticationStateProvider authProvider, IBaseUrlProvider urlProvider)
        {
            _authProvider = authProvider as CookieAuthenticationStateProvider 
                ?? throw new ArgumentNullException(nameof(authProvider));
            _urlProvider = urlProvider ?? throw new ArgumentNullException(nameof(urlProvider));
        }

        public IActionResult OnGetAsync()
        {
            _authProvider.SetContext(this.HttpContext);
            if (null != this.HttpContext?.Request)
            {
                _urlProvider.BaseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host}" +
                    $"{this.HttpContext.Request.PathBase}";
            }
            return Page();
        }

    }
}
