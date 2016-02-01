using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents a general file (as opposed to <see cref="PhotoSize">photos</see> and <see cref="Audio">audio</see> audio files). 
    /// Telegram users can send files of any type of up to 1.5 GB in size.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Optional. Document thumbnail as defined by sender
        /// </summary>
        [JsonProperty("thumb", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        ///  Optional. Original filename as defined by sender
        /// </summary>
        [JsonProperty("file_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileName { get; set; }

        /// <summary>
        /// Optional. MIME type of the file as defined by sender
        /// </summary>
        [JsonProperty("mime_type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MimeType { get; set; }

        /// <summary>
        /// Optional. File size
        /// </summary>
        [JsonProperty("file_size", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FileSize { get; set; }
    }
}