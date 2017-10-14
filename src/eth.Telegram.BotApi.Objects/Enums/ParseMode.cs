using System.Runtime.Serialization;

namespace eth.Telegram.BotApi.Objects.Enums
{
    public enum ParseMode
    {
        None = 0,

        [EnumMember(Value = "Markdown")]
        Markdown,

        [EnumMember(Value = "HTML")]
        HTML
    }
}
