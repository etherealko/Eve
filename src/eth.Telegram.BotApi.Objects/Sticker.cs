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
        /// Sticker width
        /// </summary>
        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }

        /// <summary>
        /// Sticker height
        /// </summary>
        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }

        /// <summary>
        /// Optional. Sticker thumbnail in the .webp or .jpg format
        /// </summary>
        [JsonProperty("thumb", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Optional. Emoji associated with the sticker
        /// </summary>
        [JsonProperty("emoji", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Emoji { get; set; }

        /// <summary>
        /// Optional. Name of the sticker set to which the sticker belongs
        /// </summary>
        [JsonProperty("set_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SetName { get; set; }

        /// <summary>
        /// Optional. For mask stickers, the position where the mask should be placed
        /// </summary>
        [JsonProperty("mask_position", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MaskPosition MaskPosition { get; set; }

        /// <summary>
        /// Optional. File size
        /// </summary>
        [JsonProperty("file_size", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FileSize { get; set; }
    }
}