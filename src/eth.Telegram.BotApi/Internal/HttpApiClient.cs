using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using eth.Telegram.BotApi.Internal.Serialization;
using NLog;
using Newtonsoft.Json.Linq;
using System.IO;

namespace eth.Telegram.BotApi.Internal
{
    internal class HttpApiClient : IDisposable
    {
        private static readonly JsonConverter[] Converters = {
            new ApiStringEnumConverter(),
            new ChatIdOrUsernameConverter(),
            new InputFileConverter(),
            new ApiArgsConverter()
        };

        internal static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly Formatting LogJsonFormatting;

        private readonly string _token;
        private readonly HttpClient _client;
        
        public TimeSpan Timeout { get => _client.Timeout; set => _client.Timeout = value; }
        
        static HttpApiClient()
        {
            #region log configuration

            if (LogManager.Configuration != null && 
                LogManager.Configuration.Variables.TryGetValue("apiIndentJson", out var v) == true && 
                bool.TryParse(v?.Text, out var b))
                LogJsonFormatting = b ? Formatting.Indented: Formatting.None;
            else
                LogJsonFormatting = Formatting.Indented;

            #endregion
        }

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
        
        public async Task<T> CallJsonAsync<T>(ApiMethod method, ApiArgs args = null)
        {
            var requestSerialized = JsonConvert.SerializeObject(args, Formatting.None, Converters);
            var requestContent = new StringContent(requestSerialized, Encoding.UTF8, "application/json");

            #region trace

            if (Log.IsTraceEnabled)
                Log.Trace($"-> '{method}' via json, " + 
                    (LogJsonFormatting == Formatting.Indented ? Environment.NewLine : null) + 
                    JsonConvert.SerializeObject(args, LogJsonFormatting, Converters) ?? "(null)");

            #endregion

            return await CallInternalAsync<T>(method, requestContent).ConfigureAwait(false);
        }

        public async Task<T> CallMultipartAsync<T>(ApiMethod method, ApiArgs args = null)
        {
            var requestContent = new MultipartFormDataContent();

            if (args != null)
                foreach (var arg in args)
                {
                    switch (arg.Value)
                    {
                        case string s:
                            requestContent.Add(new StringContent(s, Encoding.UTF8), arg.SerializationArgumentName);
                            break;
                        case InputFile file:
                            if (file.FileIdOrUrl != null)
                                requestContent.Add(new StringContent(file.FileIdOrUrl, Encoding.UTF8, "application/json"), arg.SerializationArgumentName);
                            else
                                requestContent.Add(new StreamContent(file.Stream, 4096), arg.SerializationArgumentName, file.FileName);

                            break;
                        default:
                            var json = JsonConvert.SerializeObject(arg.Value, Formatting.None, Converters);
                            requestContent.Add(new StringContent(json, Encoding.UTF8, "application/json"), arg.SerializationArgumentName);
                            break;
                    }
                }

            #region trace

            if (Log.IsTraceEnabled)
                Log.Trace($"-> '{method}' via multipart, " + (LogJsonFormatting == Formatting.Indented ? Environment.NewLine : null) +
                    JsonConvert.SerializeObject(args, LogJsonFormatting, Converters));

            #endregion

            return await CallInternalAsync<T>(method, requestContent).ConfigureAwait(false);
        }

        public async Task<byte[]> GetFileBytes(string filePath)
        {
            var content = await GetFileContent(filePath);

            return await content.ReadAsByteArrayAsync()
                .ConfigureAwait(false);
        }

        public async Task<Stream> GetFileStream(string filePath)
        {
            var content = await GetFileContent(filePath);

            return await content.ReadAsStreamAsync()
                .ConfigureAwait(false);
        }

        private async Task<T> CallInternalAsync<T>(ApiMethod method, HttpContent requestContent)
        {
            var response = await _client.PostAsync($"/bot{_token}/{method}", requestContent).ConfigureAwait(false);
            
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            ApiResponse<T> responseDeserialized;

            #region trace

            if (Log.IsTraceEnabled)
            {
                // pavel durov molodec, sharit v unikode: "text":"\u0433\u043e\u0432\u043d\u043e"

                var json = responseString;

                try { json = JToken.Parse(json).ToString(LogJsonFormatting, Converters); }
                catch { }

                Log.Trace("<-" + (LogJsonFormatting == Formatting.Indented ? Environment.NewLine : null) + json);
            }

            #endregion

            try
            {
                responseDeserialized = JsonConvert.DeserializeObject<ApiResponse<T>>(responseString, Converters);
            } 
            catch
            {
                // a non-success status code response still could be correctly deserialized giving us a detailed telegram api error message
                // if this fails - throw a generic telegramapibotexception or rethrow this (OK status code is unexpected)

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
        
        private async Task<HttpContent> GetFileContent(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            var response = await _client.GetAsync($"/file/bot{_token}/{filePath}", HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);

            //check response headers and shit

            return response.Content;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
