using eth.Telegram.BotApi.Objects.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace eth.Telegram.BotApi.Objects.Base
{
    /// <summary>
    /// Inline query result base class
    /// </summary>
    public abstract class InlineQueryResultBase
    {
        /// <summary>
        /// Type of the result
        /// </summary>
        [JsonProperty("type", Required = Required.Always, ItemConverterType = typeof(StringEnumConverter))]
        public InlineQueryResultType Type { get; set; }

        /// <summary>
        /// Unique identifier for this result, 1-64 Bytes
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
                
        /// <summary>
        /// Optional. Inline keyboard attached to the message
        /// </summary>
        [JsonProperty("reply_markup", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineKeyboardMarkup ReplyMarkup { get; set; }
    }
}
