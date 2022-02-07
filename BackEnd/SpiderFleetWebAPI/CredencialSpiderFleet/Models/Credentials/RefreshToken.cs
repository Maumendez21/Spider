using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace CredencialSpiderFleet.Models.Credentials
{
    public class RefreshToken
    {
        public async System.Threading.Tasks.Task<TokenCredentials> GetNewToken(string refresh_token)
        {
            HttpClient httpClient = new HttpClient();

            //var responseMessage = await httpClient.PostAsync("http://spiderfleetapi.azurewebsites.net/token", new FormUrlEncodedContent(
            var responseMessage = await httpClient.PostAsync("https://spiderv3.azurewebsites.net/token", new FormUrlEncodedContent(
            //var responseMessage = await httpClient.PostAsync("https://localhost:44379/token", new FormUrlEncodedContent(
            new[]
                {
                        new KeyValuePair<string, string>("grant_type", "refresh_token"),
                        new KeyValuePair<string, string>("refresh_token", refresh_token),
                }
                ));

            var tokenModel = await responseMessage.Content.ReadAsAsync<TokenCredentials>();

            return tokenModel;
            
        }
    }
}