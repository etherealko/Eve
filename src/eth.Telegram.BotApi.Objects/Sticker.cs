using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents a sticker.
    /// </summary>
    public class Sticker
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        ///  Sticker width
        /// </summary>
        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }

        /// <summary>
        ///  Sticker height
        /// </summary>
        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }

        /// <summary>
        /// Optional. Document thumbnail as defined by sender
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