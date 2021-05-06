using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using A5Soft.CARMA.Application.DataPortal;
using System.Security.Claims;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.A5App.Domain.Core;
using A5Soft.CARMA.Application;

namespace A5Soft.A5App.WebApp
{
    public class DataPortalMiddleware : ServerDataPortalBase
    {
        private readonly RequestDelegate _next;
        private readonly IValidateUserIdentityUseCase _validateIdentityUseCase;


        /// <inheritdoc />
        public DataPortalMiddleware(RequestDelegate next, ILogger logger, 
            IValidateUserIdentityUseCase validateIdentityUseCase) : base(logger)
        {
            _next = next;
            _validateIdentityUseCase = validateIdentityUseCase ??
                throw new ArgumentNullException(nameof(validateIdentityUseCase));
        }


        public async Task Invoke(HttpContext httpContext)
        {
            string requestJson;
            using (var reader = new StreamReader(httpContext.Request.Body))
            {
                requestJson = await reader.ReadToEndAsync();
            }

            await httpContext.Response.WriteAsync(await HandleRequest(requestJson, 
                (t) => httpContext.RequestServices.GetService(t), 
                u => httpContext.User = new ClaimsPrincipal(u)));
        }


        protected override Exception GetExceptionForUser(Exception actualException)
        {
            if (actualException is BusinessException) return actualException;
            return new InvalidOperationException("Server has encountered an exception (see server logs for details).");
        }

        protected override void SetCulture(CultureInfo culture, CultureInfo uiCulture)
        {
            System.Globalization.CultureInfo.CurrentCulture = culture;
            System.Globalization.CultureInfo.CurrentUICulture = uiCulture;
        }

        protected override Task ValidateIdentity(ClaimsIdentity identity)
        {
            return _validateIdentityUseCase.InvokeAsync(identity);
        }

    }
}
