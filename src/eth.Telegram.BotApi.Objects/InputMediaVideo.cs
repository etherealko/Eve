using eth.Telegram.BotApi.Objects.Base;
using eth.Telegram.BotApi.Objects.Enums;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// Represents a video to be sent.
    /// </summary>
    public class InputMediaVideo : InputMedia
    {
        //
        
        public InputMediaVideo()
        {
            Type = InputMediaType.Video;
        }
    }
}
