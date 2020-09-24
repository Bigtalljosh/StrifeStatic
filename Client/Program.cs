using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorApp.Client.Auth;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace BlazorApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var baseAddress = builder.Configuration["BaseAddress"] ?? builder.HostEnvironment.BaseAddress;
            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(baseAddress) });

            var appConfig = new BaseAddress(baseAddress);
            builder.Services.AddSingleton(appConfig);

            var auth0Config = builder.Configuration.GetSection("Auth0").Get<Auth0Config>();
            builder.Services.AddSingleton(auth0Config);

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
            builder.Services.AddHttpClient("ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                                            .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddOidcAuthentication(options =>
            {
                options.ProviderOptions.Authority = auth0Config.Authority;
                options.ProviderOptions.ClientId = auth0Config.ClientId;
                options.ProviderOptions.ResponseType = "token id_token";
                options.ProviderOptions.PostLogoutRedirectUri = "/index";
            });

            builder.Services.AddAuthorizationCore();
            builder.Services.AddHeadElementHelper();
            
            await builder.Build().RunAsync();
        }
    }
}
