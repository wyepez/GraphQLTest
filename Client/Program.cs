using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var oktaConfig = new OktaConfig
            {
                ClientId = "0oahq5fc1jMZR4Z0V356",
                ClientSecret = "HfxiSurzYSJ1zlLFMJtju5eEPkd4Lus79KbbklQV",
                TokenUrl = "https://dev-109229.okta.com/oauth2/default/v1/token"
            };

            var tokenService = new TokenService(oktaConfig);
            var token = await tokenService.GetToken();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var query = @"
                {
                  author(id: 1) {
                    name
                  }
                }";

            var postData = new { Query = query };
            var stringContent = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

            var res = await httpClient.PostAsync("http://localhost:49631/graphql", stringContent);
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();

                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine($"Error occurred... Status code:{res.StatusCode}");
            }
        }
    }
}
