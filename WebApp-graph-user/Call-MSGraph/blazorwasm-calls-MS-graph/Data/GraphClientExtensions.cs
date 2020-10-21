using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Authentication.WebAssembly.Msal.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace blazorwasm_calls_MS_graph.Data
{
    internal static class GraphClientExtensions
    {
        public static IServiceCollection AddMicrosoftGraphClient(this IServiceCollection services, params string[] scopes)
        {
            services.Configure<RemoteAuthenticationOptions<MsalProviderOptions>>(options =>
            {
                foreach (var scope in scopes)
                {
                    options.ProviderOptions.AdditionalScopesToConsent.Add(scope);

                }
            });

            services.AddScoped<IAuthenticationProvider, NoOpGraphAuthenticationProvider>();
            services.AddScoped<IHttpProvider, HttpClientHttpProvider>(sp => new HttpClientHttpProvider(new HttpClient()));
            services.AddScoped<GraphServiceClient>();
            return services;
        }

        private class NoOpGraphAuthenticationProvider : IAuthenticationProvider
        {
            public NoOpGraphAuthenticationProvider(IAccessTokenProvider provider)
            {
                Provider = provider;
            }

            public IAccessTokenProvider Provider { get; }

            public async Task AuthenticateRequestAsync(HttpRequestMessage request)
            {
                var result = await Provider.RequestAccessToken(new AccessTokenRequestOptions()
                {
                    Scopes = new[] { "https://graph.microsoft.com/User.Read" }
                });

                if (result.TryGetToken(out var token))
                {
                    request.Headers.Authorization ??= new AuthenticationHeaderValue("Bearer", token.Value);
                }
            }
        }

        private class HttpClientHttpProvider : IHttpProvider
        {
            private readonly HttpClient _client;

            public HttpClientHttpProvider(HttpClient client)
            {
                _client = client;
            }

            public ISerializer Serializer { get; } = new Serializer();

            public TimeSpan OverallTimeout { get; set; } = TimeSpan.FromSeconds(300);

            public void Dispose()
            {
            }

            public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
            {
                return _client.SendAsync(request);
            }

            public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            {
                return _client.SendAsync(request, completionOption, cancellationToken);
            }
        }
    }
}
