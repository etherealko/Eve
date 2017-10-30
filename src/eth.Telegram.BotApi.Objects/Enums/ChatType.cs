using System.Runtime.Serialization;

namespace eth.Telegram.BotApi.Objects.Enums
{
    public enum ChatType
    {
        Invalid = 0,

        [EnumMember(Value = "private")]
        Private,

        [EnumMember(Value = "group")]
        Group,

        [EnumMember(Value = "supergroup")]
        Supergroup,

        [EnumMember(Value = "channel")]
        Channel
    }
}