using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Authentication.WebAssembly.Msal.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IAccessTokenProvider =
    Microsoft.AspNetCore.Components.WebAssembly.Authentication.IAccessTokenProvider;

namespace blazorwasm_calls_MS_graph.Data
{
    /// <summary>
    /// Adds services and implements methods to use Microsoft Graph SDK.
    /// </summary>
    internal static class GraphClientExtensions
    {
        public static IServiceCollection AddGraphClient(
                this IServiceCollection services, string baseUrl, List<string> scopes)
        {
            if (string.IsNullOrEmpty(baseUrl) || scopes.IsNullOrEmpty())
            {
                return services;
            }

            services.Configure<RemoteAuthenticationOptions<MsalProviderOptions>>(
                options =>
                {
                    scopes?.ForEach((scope) =>
                    {
                        options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
                    });
                });

            services.AddScoped<IAuthenticationProvider, GraphAuthenticationProvider>();

            services.AddScoped(sp =>
            {
                return new GraphServiceClient(
                    new HttpClient(),
                    sp.GetRequiredService<IAuthenticationProvider>(),
                    baseUrl);
            });

            return services;
        }

        private class GraphAuthenticationProvider : IAuthenticationProvider
        {
            private readonly IConfiguration config;

            public GraphAuthenticationProvider(IAccessTokenProvider tokenProvider,
                IConfiguration config)
            {
                TokenProvider = tokenProvider;
                this.config = config;
            }

            public IAccessTokenProvider TokenProvider { get; }

            public async Task AuthenticateRequestAsync(RequestInformation request,
                Dictionary<string, object> additionalAuthenticationContext = null,
                CancellationToken cancellationToken = default)
            {
                var result = await TokenProvider.RequestAccessToken(
                    new AccessTokenRequestOptions()
                    {
                        Scopes =
                            config.GetSection("MicrosoftGraph:Scopes").Get<string[]>()
                    });

                if (result.TryGetToken(out var token))
                {
                    request.Headers.Add("Authorization",
                        $"{CoreConstants.Headers.Bearer} {token.Value}");
                }
            }
        }
    }
}
