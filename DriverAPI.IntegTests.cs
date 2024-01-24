using IntegrationTests.Ordering;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace IntegrationTests
{
    [TestCaseOrderer("IntegrationTests.Ordering.PriorityOrderer", "Integration.Tests")]
    public class IntegTests : IClassFixture<HttpClientFixture>
    {
        private readonly HttpClientFixture _httpClientFixture;

        public IntegTests(HttpClientFixture httpClassFixture)
        {
            _httpClientFixture = httpClassFixture;
        }

        [Fact, TestPriority(0)]
        public async Task GetRead()
        {
            // Arrange
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://staging-driverapi.loadrpm.com/"),
            };

            //var authToken = TestHelper.GetAccessToken(client);
            var jsonContent = TestHelper.GetHttpContentFromJson("C:\\integrationrepos\\DriverApiIntegrationTests\\SampleData\\auth-prep.json");
            MultipartFormDataContent getbodydata = new MultipartFormDataContent();
            getbodydata.Add(jsonContent);

            // We need to make this call 1st - in order for the 2nd call to retrieve an auth token
            string UriPrep = $"{client.BaseAddress}/api/auth/verification-code";
            var response1 = await client.PostAsync(UriPrep, getbodydata);

            // Get the auth token
            var jsonContent2 = TestHelper.GetHttpContentFromJson("C:\\integrationrepos\\DriverApiIntegrationTests\\SampleData\\auth-getdata.json");
            MultipartFormDataContent getbodydata2 = new MultipartFormDataContent();
            getbodydata2.Add(jsonContent2);

            string UriAuthToken = $"{client.BaseAddress}/api/auth/validate-verification-code";
            var response2 = await client.PostAsync(UriAuthToken, getbodydata2);

            var finalresponse = response2;

            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "eyJhbGciOiJSUzI1NiIsImtpZCI6IjU0RTA1M0JEQ0EzMDFEQjQ3MDYyOEVERTlCMTE4MTVDM0I1RjFEMDIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJWT0JUdmNvd0hiUndZbzdlbXhHQlhEdGZIUUkifQ.eyJuYmYiOjE3MDYxMTM3MzIsImV4cCI6MTcwNjExNzMzMiwiaXNzIjoiaHR0cHM6Ly9hdXRoLmxvYWRycG0uY29tIiwiYXVkIjoiaHR0cHM6Ly9hdXRoLmxvYWRycG0uY29tL3Jlc291cmNlcyIsImNsaWVudF9pZCI6IkRyaXZlckFwaS1zdGFnaW5nIiwiY2xpZW50X2VudiI6InN0YWdpbmciLCJzdWIiOiI3NGVlZjAyYy0xZmU1LTQ5OWYtOGZlMC04ZmU2NjQ1MWZjNDciLCJhdXRoX3RpbWUiOjE3MDYxMTM3MzEsImlkcCI6ImxvY2FsIiwibmFtZSI6Ik1lbmxvIHRlc3QgdXNlciIsInJvbGVzIjoiRHJpdmVyQXBpOkRyaXZlcixEcml2ZXJBcGlFVVN0YWdpbmc6RHJpdmVyLERyaXZlckFwaUVVOkRyaXZlcixScG1BcGktZGV2OkRyaXZlcixBcHBOYXZEZXY6UkVBRE9OTFlFTVBMT1lFRSxEcml2ZXJBcGktc3RhZ2luZzpEcml2ZXIsRHJpdmVyQXBpLWRldjpEcml2ZXIiLCJjbGllbnRzIjoiRHJpdmVyQXBpLERyaXZlckFwaUVVU3RhZ2luZyxEcml2ZXJBcGlFVSxScG1BcGktZGV2LEFwcE5hdkRldixEcml2ZXJBcGktc3RhZ2luZyxEcml2ZXJBcGktZGV2IiwicGhvbmVfbnVtYmVyIjoiMTczNDY2NTE4NDciLCJwaG9uZSI6IjczNDY2NTE4NDciLCJwaG9uZV9jb3VudHJ5X2NvZGUiOiIxIiwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJzbXMiXX0.foLR2dgVtX1UUWvYqrQqqFn3qMJ3xddTV09PFMCzu2lYagRFbO9yc7PA_ai8aj2LiAeAunU3fcPeurIOg85P7NP0k9jF4UtqtaEh_sVGGLmyG1BWpEESNIXzKn_qjFa2R4SRfIK5uElamvPLUbMaNY-_ABZVXaDJPhXoWBYP35iACyPVYUeY58IW73MJDaH1Ysfdx6jMmiQTOPcqz71CSoautwMiOiZBJ_956g0GW0TeungbppDGFqA05kHrxtDCw7soE6TFUC4QBgxeIJ-75uRdxhb_WJdFqCMUKkOC7TrN0NsKYujj3SSrT2qbF2pU_in2-zQJcD66xIHkt9aqkw");

            string getUri = $"{_httpClientFixture.BaseUrl}/api/countries/";

            // Act
            var response = await client.GetAsync(getUri);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(1)]
        public async Task VerifyDocument()
        {
            // Arrange
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://staging-driverapi.loadrpm.com/"),
            };

            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "eyJhbGciOiJSUzI1NiIsImtpZCI6IjU0RTA1M0JEQ0EzMDFEQjQ3MDYyOEVERTlCMTE4MTVDM0I1RjFEMDIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJWT0JUdmNvd0hiUndZbzdlbXhHQlhEdGZIUUkifQ.eyJuYmYiOjE3MDYxMTM3MzIsImV4cCI6MTcwNjExNzMzMiwiaXNzIjoiaHR0cHM6Ly9hdXRoLmxvYWRycG0uY29tIiwiYXVkIjoiaHR0cHM6Ly9hdXRoLmxvYWRycG0uY29tL3Jlc291cmNlcyIsImNsaWVudF9pZCI6IkRyaXZlckFwaS1zdGFnaW5nIiwiY2xpZW50X2VudiI6InN0YWdpbmciLCJzdWIiOiI3NGVlZjAyYy0xZmU1LTQ5OWYtOGZlMC04ZmU2NjQ1MWZjNDciLCJhdXRoX3RpbWUiOjE3MDYxMTM3MzEsImlkcCI6ImxvY2FsIiwibmFtZSI6Ik1lbmxvIHRlc3QgdXNlciIsInJvbGVzIjoiRHJpdmVyQXBpOkRyaXZlcixEcml2ZXJBcGlFVVN0YWdpbmc6RHJpdmVyLERyaXZlckFwaUVVOkRyaXZlcixScG1BcGktZGV2OkRyaXZlcixBcHBOYXZEZXY6UkVBRE9OTFlFTVBMT1lFRSxEcml2ZXJBcGktc3RhZ2luZzpEcml2ZXIsRHJpdmVyQXBpLWRldjpEcml2ZXIiLCJjbGllbnRzIjoiRHJpdmVyQXBpLERyaXZlckFwaUVVU3RhZ2luZyxEcml2ZXJBcGlFVSxScG1BcGktZGV2LEFwcE5hdkRldixEcml2ZXJBcGktc3RhZ2luZyxEcml2ZXJBcGktZGV2IiwicGhvbmVfbnVtYmVyIjoiMTczNDY2NTE4NDciLCJwaG9uZSI6IjczNDY2NTE4NDciLCJwaG9uZV9jb3VudHJ5X2NvZGUiOiIxIiwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJzbXMiXX0.foLR2dgVtX1UUWvYqrQqqFn3qMJ3xddTV09PFMCzu2lYagRFbO9yc7PA_ai8aj2LiAeAunU3fcPeurIOg85P7NP0k9jF4UtqtaEh_sVGGLmyG1BWpEESNIXzKn_qjFa2R4SRfIK5uElamvPLUbMaNY-_ABZVXaDJPhXoWBYP35iACyPVYUeY58IW73MJDaH1Ysfdx6jMmiQTOPcqz71CSoautwMiOiZBJ_956g0GW0TeungbppDGFqA05kHrxtDCw7soE6TFUC4QBgxeIJ-75uRdxhb_WJdFqCMUKkOC7TrN0NsKYujj3SSrT2qbF2pU_in2-zQJcD66xIHkt9aqkw");

            string getUri = $"{_httpClientFixture.BaseUrl}/api/countries/";

            // Act
            var response = await client.GetAsync(getUri);

            string stringResponse = await response.Content.ReadAsStringAsync();
            string countriesList = File.ReadAllText($"SampleData/Countries.json"); //TestHelper.GetHttpContentFromSampleData("Countries.json");

            // Assert
            Assert.Equal(stringResponse, countriesList.ToString());
        }

        [Fact, TestPriority(2)]
        public async Task PosXXXXtUpdate()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://staging-driverapi.loadrpm.com/"),
            };

            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "eyJhbGciOiJSUzI1NiIsImtpZCI6IjU0RTA1M0JEQ0EzMDFEQjQ3MDYyOEVERTlCMTE4MTVDM0I1RjFEMDIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJWT0JUdmNvd0hiUndZbzdlbXhHQlhEdGZIUUkifQ.eyJuYmYiOjE3MDYxMTM3MzIsImV4cCI6MTcwNjExNzMzMiwiaXNzIjoiaHR0cHM6Ly9hdXRoLmxvYWRycG0uY29tIiwiYXVkIjoiaHR0cHM6Ly9hdXRoLmxvYWRycG0uY29tL3Jlc291cmNlcyIsImNsaWVudF9pZCI6IkRyaXZlckFwaS1zdGFnaW5nIiwiY2xpZW50X2VudiI6InN0YWdpbmciLCJzdWIiOiI3NGVlZjAyYy0xZmU1LTQ5OWYtOGZlMC04ZmU2NjQ1MWZjNDciLCJhdXRoX3RpbWUiOjE3MDYxMTM3MzEsImlkcCI6ImxvY2FsIiwibmFtZSI6Ik1lbmxvIHRlc3QgdXNlciIsInJvbGVzIjoiRHJpdmVyQXBpOkRyaXZlcixEcml2ZXJBcGlFVVN0YWdpbmc6RHJpdmVyLERyaXZlckFwaUVVOkRyaXZlcixScG1BcGktZGV2OkRyaXZlcixBcHBOYXZEZXY6UkVBRE9OTFlFTVBMT1lFRSxEcml2ZXJBcGktc3RhZ2luZzpEcml2ZXIsRHJpdmVyQXBpLWRldjpEcml2ZXIiLCJjbGllbnRzIjoiRHJpdmVyQXBpLERyaXZlckFwaUVVU3RhZ2luZyxEcml2ZXJBcGlFVSxScG1BcGktZGV2LEFwcE5hdkRldixEcml2ZXJBcGktc3RhZ2luZyxEcml2ZXJBcGktZGV2IiwicGhvbmVfbnVtYmVyIjoiMTczNDY2NTE4NDciLCJwaG9uZSI6IjczNDY2NTE4NDciLCJwaG9uZV9jb3VudHJ5X2NvZGUiOiIxIiwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJzbXMiXX0.foLR2dgVtX1UUWvYqrQqqFn3qMJ3xddTV09PFMCzu2lYagRFbO9yc7PA_ai8aj2LiAeAunU3fcPeurIOg85P7NP0k9jF4UtqtaEh_sVGGLmyG1BWpEESNIXzKn_qjFa2R4SRfIK5uElamvPLUbMaNY-_ABZVXaDJPhXoWBYP35iACyPVYUeY58IW73MJDaH1Ysfdx6jMmiQTOPcqz71CSoautwMiOiZBJ_956g0GW0TeungbppDGFqA05kHrxtDCw7soE6TFUC4QBgxeIJ-75uRdxhb_WJdFqCMUKkOC7TrN0NsKYujj3SSrT2qbF2pU_in2-zQJcD66xIHkt9aqkw");

            string getUri = $"{_httpClientFixture.BaseUrl}/api/countries/";

            // Act
            var response = await client.GetAsync(getUri);

            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}