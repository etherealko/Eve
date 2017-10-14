using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using eth.Telegram.BotApi.Internal.Serialization;

namespace eth.Telegram.BotApi.Internal
{
    internal partial class HttpApiClient : IDisposable
    {
        private readonly static JsonConverter[] Converters = new JsonConverter[]
        {
            new DefaultValueToNullStringEnumConverter(),
            new ChatIdOrUsernameConverter()
        };

        private readonly string _token;
        private readonly HttpClient _client;
        
        public TimeSpan Timeout { get { return _client.Timeout; } set { _client.Timeout = value; } }

        public HttpApiClient(Uri baseUri, string token)
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
        
        public async Task<T> CallAsync<T>(ApiMethod method, object args = null)
        {
            var requestSerialized = JsonConvert.SerializeObject(args, Formatting.None, Converters);

            var requestContent = new StringContent(requestSerialized, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"/bot{_token}/{method}", requestContent).ConfigureAwait(false);

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            ApiResponse<T> responseDeserialized;

            try
            {
                responseDeserialized = JsonConvert.DeserializeObject<ApiResponse<T>>(responseString, Converters);
            }
            catch
            {
                // a non-success status code response still could be correctly deserialized giving us a detailed telegram api error message
                // if this fails - throw a generic telegramapibotexception or rethrow this (unexpected for the case when the status code is OK)

                if (!response.IsSuccessStatusCode)
                    throw new TelegramBotApiException
                    {
                        HttpStatusCode = response.StatusCode,
                        HttpReasonPhrase = response.ReasonPhrase,
                        HttpResponseContent = responseString
                    };
                else
                    throw;
            }

            if (!responseDeserialized.IsOk)
                throw new TelegramBotApiException
                {
                    HttpStatusCode = response.StatusCode,
                    HttpReasonPhrase = response.ReasonPhrase,

                    TelegramErrorCode = responseDeserialized.ErrorCode,
                    TelegramDescription = responseDeserialized.Description
                };

            return responseDeserialized.Result;
        }

        //todo: call with multipartformdata file attached

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
