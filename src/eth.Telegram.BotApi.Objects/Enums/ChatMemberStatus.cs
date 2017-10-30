using System.Runtime.Serialization;

namespace eth.Telegram.BotApi.Objects.Enums
{
    public enum ChatMemberStatus
    {
        Invalid = 0,

        [EnumMember(Value = "creator")]
        Creator,

        [EnumMember(Value = "administrator")]
        Administrator,

        [EnumMember(Value = "member")]
        Member,

        [EnumMember(Value = "left")]
        Left,

        [EnumMember(Value = "kicked")]
        Kicked,

        [EnumMember(Value = "restricted")]
        Restricted
    }
}
