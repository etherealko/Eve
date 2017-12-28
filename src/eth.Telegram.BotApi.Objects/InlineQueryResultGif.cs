using eth.Telegram.BotApi.Objects.Base;
using eth.Telegram.BotApi.Objects.Enums;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// Represents a link to an animated GIF file. 
    /// By default, this animated GIF file will be sent by the user with optional caption. Alternatively, you can provide message_text to send it instead of the animation.
    /// </summary>
    public class InlineQueryResultGif : InlineQueryResult
    {
        /// <summary>
        /// A valid URL for the GIF file.File size must not exceed 1MB
        /// </summary>
        [JsonProperty("gif_url", Required = Required.Always)]
        public string GifUrl { get; set; }

        /// <summary>
        /// Optional. Width of the GIF
        /// </summary>
        [JsonProperty("gif_width", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int GifWidth { get; set; }

        /// <summary>
        /// Optional. Height of the GIF
        /// </summary>
        [JsonProperty("gif_height", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int GifHeight { get; set; }

        /// <summary>
        /// Optional. Caption of the GIF file to be sent, 0-200 characters
        /// </summary>
        [JsonProperty("caption", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Caption { get; set; }

        /// <summary>
        /// Oprional. Text of a message to be sent instead of the animation, 1-4096 characters
        /// </summary>
        [JsonProperty("message_text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MessageText { get; set; }

        public InlineQueryResultGif()
        {
            Type = InlineQueryResultType.Gif;
        }
    }
}