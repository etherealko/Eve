using eth.Telegram.BotApi.Objects;

namespace eth.Telegram.BotApi.Internal
{
    internal static class ApiExtensions
    {
        public static string ProperName(this ChatAction action)
        {
            switch (action)
            {
                case ChatAction.Typing:
                    return ApiConstants.ChatActionsProperNames.Typing;
                case ChatAction.UploadingPhoto:
                    return ApiConstants.ChatActionsProperNames.UploadingPhoto;
                case ChatAction.UploadingVideo:
                    return ApiConstants.ChatActionsProperNames.UploadingVideo;
                case ChatAction.UploadingDocument:
                    return ApiConstants.ChatActionsProperNames.UploadingDocument;
                case ChatAction.UploadingAudio:
                    return ApiConstants.ChatActionsProperNames.UploadingAudio;
                case ChatAction.RecordingAudio:
                    return ApiConstants.ChatActionsProperNames.RecordingAudio;
                case ChatAction.RecordingVideo:
                    return ApiConstants.ChatActionsProperNames.UploadingVideo;
                case ChatAction.FindingLocation:
                    return ApiConstants.ChatActionsProperNames.FindingLocation;
                default:
                    return ApiConstants.ChatActionsProperNames.Typing;                   
            }
        }
    }
}
