using Azure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    internal class TestHelper
    {

        public static HttpContent GetHttpContentFromSampleData(string jsonFileName)
        {
            string json = File.ReadAllText($"SampleData/{jsonFileName}");
            var postContent = GetHttpContentFromJson(json);

            return postContent;
        }

        public static HttpContent GetHttpContentFromJson(string json)
        {
            var postContent = new StringContent(json)
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            };
            return postContent;
        }

        public static async Task<dynamic> GetContentFromHttpResponse(HttpResponseMessage responseMessage)
        {
            string responseContent = await responseMessage.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject<dynamic>(responseContent);
            return content;
        }

        public static async Task<dynamic> GetAccessToken(HttpClient Client)
        {
            var jsonContent = TestHelper.GetHttpContentFromJson("auth-prep.json");
            MultipartFormDataContent getbodydata = new MultipartFormDataContent();
            getbodydata.Add(jsonContent);

            // We need to make this call 1st - in order for the 2nd call to retrieve an auth token
            string UriPrep = $"{Client.BaseAddress}\api\auth\verification-code";
            var response1 = await Client.PostAsync(UriPrep, getbodydata);

            // Get the auth token
            var jsonContent2 = TestHelper.GetHttpContentFromJson("auth-getdata.json");
            MultipartFormDataContent getbodydata2 = new MultipartFormDataContent();
            getbodydata2.Add(jsonContent2);

            string UriAuthToken = $"{Client.BaseAddress}\api\auth\validate-verification-code";
            var response2 = await Client.PostAsync(UriAuthToken, getbodydata2);

            var finalresponse = JsonConvert.DeserializeObject<dynamic>(response2.ToString());

            return finalresponse;
        }

    }
}

