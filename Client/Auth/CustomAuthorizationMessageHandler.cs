using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorApp.Client.Auth
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager, BaseAddress baseAddress)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { baseAddress.Url });
        }
    }
}
