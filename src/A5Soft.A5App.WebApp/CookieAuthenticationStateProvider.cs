using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Domain;
using Microsoft.AspNetCore.Http;
using AuthenticationStateProvider = Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider;

namespace A5Soft.A5App.WebApp
{
    public class CookieAuthenticationStateProvider : AuthenticationStateProvider, 
        IAuthenticationStateProvider, IDisposable
    {
        private static readonly TimeSpan RevalidationInterval = TimeSpan.FromMinutes(15);

        private HttpContext _context;
        private readonly IValidateUserIdentityUseCase _validateIdentityUseCase;

        private Task<AuthenticationState> _currentState = null;
        private CancellationTokenSource _loopCancellationTokenSource = null;


        /// <inheritdoc />
        public CookieAuthenticationStateProvider(IHttpContextAccessor context,
            IBaseUrlProvider urlProvider, IValidateUserIdentityUseCase validateIdentityUseCase)
        {
            _context = context?.HttpContext;
            _validateIdentityUseCase = validateIdentityUseCase
                ?? throw new ArgumentNullException(nameof(validateIdentityUseCase));

            if (null == urlProvider) throw new ArgumentNullException(nameof(urlProvider));
            if (null != _context?.Request)
            {
                urlProvider.BaseUrl = $"{_context.Request.Scheme}://{_context.Request.Host}" +
                    $"{_context.Request.PathBase}";
            }
        }


        /// <inheritdoc cref="IAuthenticationStateProvider.IdentityChanged"/>
        public event IdentityChangedHandler IdentityChanged;

        /// <inheritdoc cref="AuthenticationStateProvider.GetAuthenticationStateAsync"/>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (null == _currentState)
            {
                var identity = TryGetIdentityFromEnvironment();
                      
                var result = new ClaimsPrincipal(identity ?? new ClaimsIdentity());

                _currentState = Task.FromResult(new AuthenticationState(result));

                if (identity?.IsAuthenticated ?? false)
                {
                    _loopCancellationTokenSource?.Cancel();
                    _loopCancellationTokenSource = new CancellationTokenSource();
                    _ = RevalidationLoop(_currentState, _loopCancellationTokenSource.Token);
                }
            }

            return _currentState;
        }

        /// <inheritdoc cref="IAuthenticationStateProvider.GetIdentityAsync"/>
        public async Task<ClaimsIdentity> GetIdentityAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            return (ClaimsIdentity)authState.User.Identity;
        }
        
        /// <inheritdoc cref="IAuthenticationStateProvider.NotifyIdentityChanged"/>
        public void NotifyIdentityChanged(ClaimsIdentity updatedIdentity)
        {
            if (null == updatedIdentity) throw new ArgumentNullException(nameof(updatedIdentity));

            _currentState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(updatedIdentity)));

            NotifyAuthenticationStateChanged(_currentState);

            IdentityChanged?.Invoke(Task.FromResult(updatedIdentity));

            _loopCancellationTokenSource?.Cancel();
            _loopCancellationTokenSource = new CancellationTokenSource();
            _ = RevalidationLoop(_currentState, _loopCancellationTokenSource.Token);
        }

        public void SetContext(HttpContext httpContext)
        {
            _context = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        }


        private ClaimsIdentity TryGetIdentityFromEnvironment()
        {
            if (null != _context?.Request && _context.Request.Cookies.TryGetValue(
                AppAuthenticationHandler.AuthCookieName, out string identityCookie) &&
                !identityCookie.IsNullOrWhiteSpace())
            {
                return identityCookie.DeserializeClaimsIdentity();
            }

            return null;
        }

        private async Task RevalidationLoop(Task<AuthenticationState> authenticationStateTask, CancellationToken ct)
        {
            try
            {
                var identity = await authenticationStateTask;
                if (identity.User.Identity.IsAuthenticated)
                {
                    while (!ct.IsCancellationRequested)
                    {
                        try
                        {
                            await Task.Delay(RevalidationInterval, ct);
                            await _validateIdentityUseCase.InvokeAsync((ClaimsIdentity)identity.User.Identity);
                        }
                        catch (TaskCanceledException tce)
                        {
                            // If it was our cancellation token, then this revalidation loop gracefully completes
                            // Otherwise, treat it like any other failure
                            if (tce.CancellationToken == ct)
                            {
                                break;
                            }
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                NotifyIdentityChanged(new ClaimsIdentity());
            }
        }

        void IDisposable.Dispose()
        {
            _loopCancellationTokenSource?.Cancel();
            Dispose(disposing: true);
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
        }

    }
}
