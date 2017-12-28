using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// Contains information about why a request was unsuccessful.
    /// </summary>
    public class ResponseParameters
    {
        /// <summary>
        /// Optional. The group has been migrated to a supergroup with the specified identifier. 
        /// This number may be greater than 32 bits and some programming languages may have difficulty/silent defects in interpreting it. 
        /// But it is smaller than 52 bits, so a signed 64 bit integer or double-precision float type are safe for storing this identifier.
        /// </summary>
        [JsonProperty("migrate_to_chat_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long MigrateToChatId { get; set; }

        /// <summary>
        /// Optional. In case of exceeding flood control, the number of seconds left to wait before the request can be repeated
        /// </summary>
        [JsonProperty("retry_after", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RetryAfter { get; set; }
    }
}
