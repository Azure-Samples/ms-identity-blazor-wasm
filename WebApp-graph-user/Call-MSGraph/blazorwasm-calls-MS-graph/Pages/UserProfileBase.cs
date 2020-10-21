using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
//using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace blazorwasm_calls_MS_graph.Pages
{
    public class UserProfileBase : ComponentBase
    {
        [Inject]
        GraphServiceClient GraphClient { get; set; }

        protected User _user = new User();
        protected override async Task OnInitializedAsync()
        {
            await GetUserProfile();
        }

        /// <summary>
        /// Retrieves user information from Microsoft Graph /me endpoint.
        /// </summary>
        /// <returns></returns>
        private async Task GetUserProfile()
        {
            try
            {
                var request = GraphClient.Me.Request();
                _user = await request.GetAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

