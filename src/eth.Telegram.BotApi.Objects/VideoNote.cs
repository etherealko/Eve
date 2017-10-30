using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents a video message (available in Telegram apps as of v.4.0).
    /// </summary>
    public class VideoNote
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Video width and height as defined by sender
        /// </summary>
        [JsonProperty("length", Required = Required.Always)]
        public int Length { get; set; }

        /// <summary>
        /// Duration of the video in seconds as defined by sender
        /// </summary>
        [JsonProperty("duration", Required = Required.Always)]
        public int Duration { get; set; }

        /// <summary>
        /// Optional. Video thumbnail
        /// </summary>
        [JsonProperty("thumb", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Optional. File size
        /// </summary>
        [JsonProperty("file_size", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FileSize { get; set; }
    }
}