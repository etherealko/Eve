using eth.Telegram.BotApi.Objects.Enums;

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
        public InputMediaType Type { get; set; }
    }
}
