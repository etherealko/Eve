using eth.Telegram.BotApi.Objects.Base;
using eth.Telegram.BotApi.Objects.Enums;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// Represents a link to a photo. By default, this photo will be sent by the user with optional caption. Alternatively, you can provide message_text to send it instead of photo.
    /// </summary>
    public class InlineQueryResultPhoto : InlineQueryResultBase
    {
        /// <summary>
        /// A valid URL of the photo. Photo must be in jpeg format. Photo size must not exceed 5MB
        /// </summary>
        [JsonProperty("photo_url", Required = Required.Always)]
        public string PhotoUrl { get; set; }
        
        /// <summary>
        /// Oprional. Text of the message to be sent, 1-4096 characters
        /// </summary>
        [JsonProperty("message_text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MessageText { get; set; }

        /// <summary>
        /// Optional. Width of the photo
        /// </summary>
        [JsonProperty("photo_width", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int PhotoWidth { get; set; }

        /// <summary>
        /// Optional. Height of the photo
        /// </summary>
        [JsonProperty("photo_height", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int PhotoHeight { get; set; }

        /// <summary>
        /// Optional. Caption of the photo to be sent, 0-200 characters
        /// </summary>
        [JsonProperty("caption", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Caption { get; set; }

        public InlineQueryResultPhoto()
        {
            Type = InlineQueryResultType.Photo;
        }
    }
}
    