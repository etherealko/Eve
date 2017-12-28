using eth.Telegram.BotApi.Objects.Enums;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects.Base
{
    /// <summary>
    /// This object represents the content of a media message to be sent. It should be one of
    /// InputMediaPhoto
    /// InputMediaVideo
    /// </summary>
    public abstract class InputMedia
    {
        /// <summary>
        /// Type of the result
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public InputMediaType Type { get; set; }
    }
}
