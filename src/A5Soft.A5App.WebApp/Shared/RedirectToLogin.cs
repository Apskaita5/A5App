using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace A5Soft.A5App.WebApp.Shared
{
    public partial class RedirectToLogin : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject]
        public SmartNavigationManager Navigation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateTask!;

            if (authenticationState?.User?.Identity is null || !authenticationState.User.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo("Account/Login");
            }
        }
    }
}
