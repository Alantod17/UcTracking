using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;

namespace AwesomeDi.Api.Service
{
    public interface ISharesiesService
    {
        Task<SharesiesInstrumentsResult> GetInstruments(int page = 1, int perPage = 500, string sort = "marketCap", string priceChangeTime = "1y", string query ="");
        Task<List<SharesiesInstrumentsPriceHistoryResult>> GetInstrumentPriceHistory(string instrumentId);
        Task<LoginResult> GetUserInfo();
    }

    public class SharesiesService : ISharesiesService
    {
        private readonly _DbContext.AwesomeDiContext _db;
        private readonly HttpClient _client;
        private string UserId { get; set; }
        private string Token { get; set; }
        private Boolean IsGettingToken { get; set; }

        public SharesiesService(_DbContext.AwesomeDiContext db, IHttpClientFactory clientFactory)
        {
            _db = db;
            _client = clientFactory.CreateClient();
            UserId = null;
            Token = null;
            IsGettingToken = false;
        }

        private async Task<(string userId, string accessToken)> GetToken()
        {
            Token = null;
            UserId = null;
            while (IsGettingToken)
            {
                Thread.Sleep(1000);
            }

            if (Token != null && UserId != null)
            {
                return (UserId, Token);
            }

            IsGettingToken = true;
            try
            {
                var config = _db.SharesiesConfiguration.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(config?.UserName) || string.IsNullOrWhiteSpace(config?.Password))
                    throw new Exception("SharesiesConfiguration is in valid");
                var pass = HelperString.Encrypt("Alanalan17!");
                var postData = new
                {
                    email = config.UserName,
                    password = HelperString.Decrypt(config.Password),
                    remember = true
                };
                var request = new HttpRequestMessage(HttpMethod.Post, "https://app.sharesies.nz/api/identity/login");
                request.Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8,
                    "application/json");

                var response = await _client.SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    Token = data.distill_token;
                    UserId = data.user.id;
                    IsGettingToken = false;
                    return (data.user.id, data.distill_token);
                }
                else
                {
                    throw new HttpRequestException(responseContent, null, response.StatusCode);
                }

            }
            catch (Exception)
            {
                IsGettingToken = false;
                throw;
            }
        }

        private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
        {
            if (string.IsNullOrWhiteSpace(Token)) await GetToken();

            var retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(response => response.StatusCode == HttpStatusCode.Forbidden)
                .RetryAsync(1, async (exception, retryCount) =>
                {

                    var responseContent = await exception.Result.Content.ReadAsStringAsync();
                    if (responseContent.Trim() == "Auth expired")
                        await GetToken();
                });

            var result = await retryPolicy.ExecuteAsync(() =>
            {
                var newRequest = new HttpRequestMessage(request.Method, request.RequestUri);
                newRequest.Content = request.Content;
                if (!string.IsNullOrWhiteSpace(Token))
                    newRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                return _client.SendAsync(newRequest);
            });

            return result;
        }

        public async Task<SharesiesInstrumentsResult> GetInstruments(int page = 1, int perPage = 500, string sort = "marketCap", string priceChangeTime = "1y", string query ="")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://data.sharesies.nz/api/v1/instruments?Page={page}&PerPage={perPage}&Sort={sort}&PriceChangeTime={priceChangeTime}&Query={query}");
            var response = await SendRequest(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<SharesiesInstrumentsResult>(responseContent);
            }
            else
            {
                throw new HttpRequestException(responseContent, null, response.StatusCode);
            }
        }

        public async Task<LoginResult> GetUserInfo()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://app.sharesies.com/api/identity/check");
            var response = await SendRequest(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            LoginResult res = null;
            if (response.IsSuccessStatusCode)
            {
                res = JsonConvert.DeserializeObject<LoginResult>(responseContent);
            }
            return res;
        }
            public async Task<List<SharesiesInstrumentsPriceHistoryResult>> GetInstrumentPriceHistory(string instrumentId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://data.sharesies.nz/api/v1/instruments/{instrumentId}/pricehistory");
            var response = await SendRequest(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var resList = new List<SharesiesInstrumentsPriceHistoryResult>();
                JObject res = JObject.Parse(responseContent);
                foreach (var dayPrice in res["dayPrices"])
                {
                    JProperty jProperty = dayPrice.ToObject<JProperty>();
                    var date = jProperty.Name;
                    var value = (decimal)jProperty.Value;
                    var result = new SharesiesInstrumentsPriceHistoryResult();
                    result.Date = DateTime.Parse(date);
                    result.Price = value;
                    resList.Add(result);
                }

                return resList;
            }
            else
            {
                throw new HttpRequestException(responseContent, null, response.StatusCode);
            }
        }
    }

   
}
