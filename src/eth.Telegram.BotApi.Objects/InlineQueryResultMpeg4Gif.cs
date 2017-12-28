using eth.Telegram.BotApi.Objects.Base;
using eth.Telegram.BotApi.Objects.Enums;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// Represents a link to a video animation (H.264/MPEG-4 AVC video without sound). 
    /// By default, this animated MPEG-4 file will be sent by the user with optional caption. Alternatively, you can provide message_text to send it instead of the animation.
    /// </summary>
    public class InlineQueryResultMpeg4Gif : InlineQueryResult
    {
        /// <summary>
        /// A valid URL for the MP4 file.File size must not exceed 1MB
        /// </summary>
        [JsonProperty("mpeg4_url", Required = Required.Always)]
        public string Mpeg4Url { get; set; }

        /// <summary>
        /// Optional. Video width
        /// </summary>
        [JsonProperty("mpeg4_width", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Mpeg4Width { get; set; }

        /// <summary>
        /// Optional. Video height
        /// </summary>
        [JsonProperty("mpeg4_height", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Mpeg4Height { get; set; }

        /// <summary>
        /// Optional. Caption of the MPEG-4 file to be sent, 0-200 characters
        /// </summary>
        [JsonProperty("caption", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Caption { get; set; }

        /// <summary>
        /// Oprional. Text of a message to be sent instead of the animation, 1-4096 characters
        /// </summary>
        [JsonProperty("message_text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MessageText { get; set; }

        public InlineQueryResultMpeg4Gif()
        {
            Type = InlineQueryResultType.Mpeg4Gif;
        }
    }
}