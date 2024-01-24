using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace IntegrationTests
{
    public class HttpClientFixture : IDisposable
    {
        public AccessToken? Token { get; set; } = null;
        private HttpClient Client { get; } = new HttpClient();
        public string BaseUrl { get; set; }
        public string Scope { get; set; }
        public string CreatedId { get; set; }


        public HttpClientFixture()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var apiConfig = configuration.Get<apiconfig>()!;
            BaseUrl = apiConfig.BaseUrl;
            Scope = apiConfig.Scope;
            CreatedId = ""; // initially empty string, to be populated later
        }

        internal async Task<HttpClient> GetAuthorizedHttpClientAsync()
        {
            if (Token is null || Token.Value.ExpiresOn <= DateTime.Now)
            {
                var credential = new DefaultAzureCredential();
                Token = await credential.GetTokenAsync(new TokenRequestContext(new[]
                {
                    Scope
                }));
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.Value.Token);
            }

            return Client;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Client.Dispose();
        }
    }
}
