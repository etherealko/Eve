using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// Inline query result base class
    /// </summary>
    public class InlineQueryResultBase
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
        /// String Title of the result
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in your bot's message.
        /// </summary>
        [JsonProperty("parse_mode", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ParseMode { get; set; }

        /// <summary>
        /// Optional. Disables link previews for links in the sent message
        /// </summary>
        [JsonProperty("disable_web_page_preview", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool DisableWebPagePreview { get; set; }

        /// <summary>
        /// Optional. Short description of the result
        /// </summary>
        [JsonProperty("description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Optional. Url of the thumbnail for the result
        /// </summary>
        [JsonProperty("thumb_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ThumbUrl { get; set; }
    }
}
