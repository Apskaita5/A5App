using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using A5Soft.A5App.Application.UseCases.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace A5Soft.A5App.WebApp
{
    public class AppAuthenticationHandler : SignInAuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// A5 cookie authentication schema name
        /// </summary>
        public const string AuthSchemaName = "A5.AuthCookie";

        /// <summary>
        /// a name of the cookie set by the authentication handler
        /// </summary>
        public const string AuthCookieName = "A5.AuthCookie";

        private readonly IValidateUserIdentityUseCase _validateIdentityUseCase;


        /// <inheritdoc />
        public AppAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IValidateUserIdentityUseCase validateIdentityUseCase) 
            : base(options, logger, encoder, clock)
        {
            _validateIdentityUseCase = validateIdentityUseCase ??
                throw new ArgumentNullException(nameof(validateIdentityUseCase));
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Request.Cookies.TryGetValue(AuthCookieName, out string authCookie))
                return AuthenticateResult.NoResult();

            ClaimsIdentity identity;

            try
            {
                identity = authCookie.DeserializeClaimsIdentity();
                if (null == identity) throw new InvalidOperationException(
                        "JsonConvert.DeserializeObject returned null.");

                await _validateIdentityUseCase.InvokeAsync(identity);
            }
            catch (UnauthenticatedException e)
            {
                return AuthenticateResult.Fail(e.Message);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Invalid auth cookie format: {ex.Message}");
            }

            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity),
                AuthSchemaName));
        }

        protected override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            if (null == user) throw new ArgumentNullException(nameof(user));

            var options = new CookieOptions()
            {
                Expires = new DateTimeOffset(DateTime.UtcNow.AddDays(30)),
                IsEssential = true,
                HttpOnly = true,
                MaxAge = TimeSpan.FromDays(30)
            };

            var cookieValue = ((ClaimsIdentity)user.Identity).Serialize();

            Response.Cookies.Append(AuthCookieName, cookieValue, options);

            return Task.CompletedTask;
        }

        protected override Task HandleSignOutAsync(AuthenticationProperties properties)
        {
            Response.Cookies.Delete(AuthCookieName);
            return Task.CompletedTask;
        }

    }
}
