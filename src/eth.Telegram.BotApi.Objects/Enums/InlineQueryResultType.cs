using System.Runtime.Serialization;

namespace eth.Telegram.BotApi.Objects.Enums
{
    public enum InlineQueryResultType
    {
        Invalid = 0,

        [EnumMember(Value = "article")]
        Article,

        [EnumMember(Value = "photo")]
        Photo,

        [EnumMember(Value = "gif")]
        Gif,

        [EnumMember(Value = "mpeg4_gif")]
        Mpeg4Gif,

        [EnumMember(Value = "video")]
        Video,

        [EnumMember(Value = "audio")]
        Audio,

        [EnumMember(Value = "voice")]
        Voice,

        [EnumMember(Value = "document")]
        Document,

        [EnumMember(Value = "location")]
        Location,

        [EnumMember(Value = "venue")]
        Venue,

        [EnumMember(Value = "contact")]
        Contact,

        [EnumMember(Value = "game")]
        Game,

        [EnumMember(Value = "sticker")]
        Sticker,
    }
}