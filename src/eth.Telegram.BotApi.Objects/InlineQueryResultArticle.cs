using eth.Telegram.BotApi.Objects.Base;
using eth.Telegram.BotApi.Objects.Enums;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// Represents a link to an article or web page.
    /// </summary>
    public class InlineQueryResultArticle : InlineQueryResultBase
    {
        /// <summary>
        /// String Text of the message to be sent, 1-4096 characters
        /// </summary>
        [JsonProperty("message_text", Required = Required.Always)]
        public string MessageText { get; set; }

        /// <summary>
        /// Optional. URL of the result
        /// </summary>
        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Url { get; set; }

        /// <summary>
        /// Optional. Pass True, if you don't want the URL to be shown in the message
        /// </summary>
        [JsonProperty("hide_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool HideUrl { get; set; }

        /// <summary>
        /// Optional. Thumbnail width
        /// </summary>
        [JsonProperty("thumb_width", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ThumbWidth { get; set; }

        /// <summary>
        /// Optional. Thumbnail height
        /// </summary>
        [JsonProperty("thumb_height", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ThumbHeight { get; set; }

        public InlineQueryResultArticle()
        {
            Type = InlineQueryResultType.Article;
        }
    }
}