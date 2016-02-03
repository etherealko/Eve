namespace eth.Telegram.BotApi.Internal
{
    internal static class ApiConstants
    {
        internal static class ApiMethodPaths
        {
            public const string GetUpdates = "getUpdates";
            public const string GetMe = "getMe";
            public const string SendMessage = "sendMessage";
            public const string ForwardMessage = "forwardMessage";
            public const string SendPhoto = "sendPhoto";
            public const string SendAudio = "sendAudio";
            public const string SendDocument = "sendDocument";
            public const string SendSticker = "sendSticker";
            public const string SendVideo = "sendVideo";
            public const string SendVoice = "sendVoice";
            public const string SendLocation = "sendLocation";
            public const string SendChatAction = "sendChatAction";
            public const string GetUserProfilePhotos = "getUserProfilePhotos";
            public const string SetWebhook = "setWebhook";
            public const string GetFile = "getFile";
        }

        internal static class ChatActionsProperNames
        {
            public const string Typing = "typing";
            public const string UploadingPhoto = "upload_photo";
            public const string RecordingVideo = "record_video";
            public const string UploadingVideo = "upload_video";
            public const string RecordingAudio = "record_audio";
            public const string UploadingAudio = "upload_audio";
            public const string UploadingDocument = "upload_document";
            public const string FindingLocation = "find_location";
        }
    }
}
