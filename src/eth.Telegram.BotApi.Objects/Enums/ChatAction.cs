using System.Runtime.Serialization;

namespace eth.Telegram.BotApi.Objects.Enums
{
    public enum ChatAction
    {
        Invalid = 0,

        [EnumMember(Value = "typing")]
        Typing,

        [EnumMember(Value = "upload_photo")]
        UploadingPhoto,

        [EnumMember(Value = "record_video")]
        RecordingVideo,

        [EnumMember(Value = "upload_video")]
        UploadingVideo,

        [EnumMember(Value = "record_audio")]
        RecordingAudio,

        [EnumMember(Value = "upload_audio")]
        UploadingAudio,

        [EnumMember(Value = "upload_document")]
        UploadingDocument,

        [EnumMember(Value = "find_location")]
        FindingLocation
    }
}
