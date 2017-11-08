using System.Runtime.Serialization;

namespace eth.Telegram.BotApi
{
    public enum ApiMethod
    {
        [EnumMember(Value = "getUpdates")]
        GetUpdates,

        [EnumMember(Value = "getMe")]
        GetMe,

        [EnumMember(Value = "sendMessage")]
        SendMessage,

        [EnumMember(Value = "forwardMessage")]
        ForwardMessage,

        [EnumMember(Value = "sendPhoto")]
        SendPhoto,

        [EnumMember(Value = "sendAudio")]
        SendAudio,

        [EnumMember(Value = "sendDocument")]
        SendDocument,

        [EnumMember(Value = "sendSticker")]
        SendSticker,

        [EnumMember(Value = "sendVideo")]
        SendVideo,

        [EnumMember(Value = "sendVoice")]
        SendVoice,

        [EnumMember(Value = "sendLocation")]
        SendLocation,

        [EnumMember(Value = "sendVenue")]
        SendVenue,

        [EnumMember(Value = "sendContact")]
        SendContact,

        [EnumMember(Value = "sendChatAction")]
        SendChatAction,

        [EnumMember(Value = "getUserProfilePhotos")]
        GetUserProfilePhotos,

        [EnumMember(Value = "setWebhook")]
        SetWebhook,

        [EnumMember(Value = "getFile")]
        GetFile,

        [EnumMember(Value = "kickChatMember")]
        KickChatMember,

        [EnumMember(Value = "leaveChat")]
        LeaveChat,

        [EnumMember(Value = "unbanChatMember")]
        UnbanChatMember,

        [EnumMember(Value = "getChat")]
        GetChat,

        [EnumMember(Value = "getChatAdministrators")]
        GetChatAdmins,

        [EnumMember(Value = "getChatMembersCount")]
        GetChatMembersCount,

        [EnumMember(Value = "getChatMember")]
        GetChatMember,

        [EnumMember(Value = "editMessageText")]
        EditMessageText,

        [EnumMember(Value = "editMessageCaption")]
        EditMessageCaption,

        [EnumMember(Value = "editMessageReplyMarkup")]
        EditMessageReplyMarkup,

        [EnumMember(Value = "deleteMessage")]
        DeleteMessage,

    }
}