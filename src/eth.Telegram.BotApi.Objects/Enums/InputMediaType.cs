using System.Runtime.Serialization;

namespace eth.Telegram.BotApi.Objects.Enums
{
    public enum InputMediaType
    {
        Invalid = 0,

        [EnumMember(Value = "photo")]
        Photo,

        [EnumMember(Value = "video")]
        Video
    }
}
