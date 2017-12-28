using eth.Telegram.BotApi.Objects.Base;
using eth.Telegram.BotApi.Objects.Enums;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// Represents a photo to be sent.
    /// </summary>
    public class InputMediaPhoto : InputMedia
    {
        //

        public InputMediaPhoto()
        {
            Type = InputMediaType.Photo;
        }
    }
}
