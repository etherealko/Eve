using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using eth.Common.JetBrains.Annotations;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Internal
{
    internal class HttpApiClient
    {
        private readonly string _token;
        private readonly HttpClient _client;

        public HttpApiClient([NotNull] Uri baseUri, [NotNull] string token)
        {
            Debug.Assert(baseUri != null);
            Debug.Assert(!string.IsNullOrEmpty(token));
            
            _token = token;

            _client = new HttpClient
            {
                BaseAddress = baseUri,
                Timeout = TimeSpan.FromSeconds(3)
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Connection.Add("Keep-Alive");
        }

        public async Task<T> CallAsync<T>([NotNull] string method, [CanBeNull] object args = null)
        {
            Debug.Assert(!string.IsNullOrEmpty(method));
            
            var content = new StringContent(JsonConvert.SerializeObject(args), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"/bot{_token}/{method}", content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new TelegramBotApiException
                {
                    HttpStatusCode = response.StatusCode,
                    HttpStatusMessage = response.ReasonPhrase
                };

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseDeserialized = JsonConvert.DeserializeObject<ApiResponse<T>>(responseString);

            if (!responseDeserialized.IsOk)
                throw new TelegramBotApiException
                {
                    TelegramDescription = responseDeserialized.Description,
                    TelegramErrorCode = responseDeserialized.ErrorCode
                };
            
            return responseDeserialized.Result;
        }
    }
}
