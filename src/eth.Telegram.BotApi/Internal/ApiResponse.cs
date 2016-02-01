using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Internal
{
    internal class ApiResponse<T>
    {
        [JsonProperty("ok", Required = Required.Always)]
        public bool IsOk { get; set; }

        [JsonProperty("result")]
        public T Result { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("error_code")]
        public int? ErrorCode { get; set; }
    }
}
