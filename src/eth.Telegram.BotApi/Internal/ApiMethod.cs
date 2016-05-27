using System.Runtime.Serialization;

namespace eth.Telegram.BotApi.Internal
{
    internal enum ApiMethod
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

        [EnumMember(Value = "sendChatAction")]
        SendChatAction,

        [EnumMember(Value = "getUserProfilePhotos")]
        GetUserProfilePhotos,

        [EnumMember(Value = "setWebhook")]
        SetWebhook,

        [EnumMember(Value = "getFile")]
        GetFile,
    }
}