using System;
using System.Net.Http;
using System.Threading.Tasks;
using blazorwasm_calls_MS_graph.Data;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;

namespace blazorwasm_calls_MS_graph
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddMicrosoftGraphClient("https://graph.microsoft.com/User.Read");

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });
            await builder.Build().RunAsync();
        }
    }
}
