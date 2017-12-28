using eth.Telegram.BotApi.Objects.Base;
using eth.Telegram.BotApi.Objects.Enums;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// Represents link to a page containing an embedded video player or a video file.
    /// </summary>
    public class InlineQueryResultVideo : InlineQueryResult
    {
        /// <summary>
        /// A valid URL for the embedded video player or video file
        /// </summary>
        [JsonProperty("video_url", Required = Required.Always)]
        public string VideoUrl { get; set; }

        /// <summary>
        /// Text of the message to be sent with the video, 1-4096 characters
        /// </summary>
        [JsonProperty("message_text", Required = Required.Always)]
        public string MessageText { get; set; }

        /// <summary>
        /// Optional. Video width
        /// </summary>
        [JsonProperty("video_width", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int VideoWidth { get; set; }

        /// <summary>
        /// Optional. Video height
        /// </summary>
        [JsonProperty("video_height", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int VideoHeight { get; set; }

        /// <summary>
        /// Optional. Video duration in seconds
        /// </summary>
        [JsonProperty("video_duration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int VideoDuration { get; set; }

        public InlineQueryResultVideo()
        {
            Type = InlineQueryResultType.Video;
        }
    }
}