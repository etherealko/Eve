using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Telegram.BotApi.Objects;
using eth.Telegram.BotApi.Objects.Enums;
using eth.Telegram.BotApi.Objects.Base;
using Stream = System.IO.Stream;

namespace eth.Telegram.BotApi
{
    /// <summary>
    /// Telegram Bot API v2.1
    /// for details: https://core.telegram.org/bots/api
    /// UPD: the api is currently somewhere between 2.1 and 3.4, keep it in mind and check online if in doubt
    /// </summary>
    public interface ITelegramBotApi
    {
        Task<User> GetMeAsync();

        Task<Message> SendMessageAsync(ChatIdOrUsername chatId, string text, 
            ParseMode parseMode = ParseMode.None, 
            bool? disableWebPagePreview = null, 
            bool? disableNotification = null, 
            long? replyToMessageId = null,
            KeyboardMarkupReply replyMarkup = null);

        Task<Message> SendStickerAsync(ChatIdOrUsername chatId, string sticker,
            bool? disableNotification = null,
            long? replyToMessageId = null,
            KeyboardMarkupReply replyMarkup = null);

        Task<Message> ForwardMessageAsync(ChatIdOrUsername chatId, ChatIdOrUsername fromChatId, int messageId,
            bool? disableNotification = null);

        Task<Message> SendPhotoAsync(ChatIdOrUsername chatId, InputFile photo,
            string caption = null,
            bool? disableNotification = null,
            long? replyToMessageId = null,
            KeyboardMarkupReply replyMarkup = null);

        Task<Message> SendAudioAsync(ChatIdOrUsername chatId, InputFile audio, 
            string caption = null, 
            int? duration = null, 
            string performer = null, 
            string title = null, 
            bool? disableNotification = null, 
            int? replyToMessageId = null, 
            KeyboardMarkupReply replyMarkup = null);

        Task<Message> SendDocumentAsync(ChatIdOrUsername chatId, Document document);

        Task<Message> SendVideoAsync(ChatIdOrUsername chatId, Video video);

        Task<Message> SendVoiceAsync(ChatIdOrUsername chatId, Voice voice);

        Task<Message> SendLocationAsync(ChatIdOrUsername chatId, float latitude, float longitude);

        Task<Message> SendVenueAsync(ChatIdOrUsername chatId, float latitude, float longitude, string title, string address);

        Task<Message> SendContactAsync(ChatIdOrUsername chatId, string phoneNumber, string firstName, string lastName);

        Task<bool> SendChatActionAsync(ChatIdOrUsername chatId, ChatAction action);

        Task<UserProfilePhotos> GetUserProfilePhotoAsync(ChatIdOrUsername userId);

        Task<bool> KickChatMemberAsync(ChatIdOrUsername chatId, ChatIdOrUsername userId);

        Task<bool> LeaveChatAsync(ChatIdOrUsername chatId);

        Task<bool> UnbanChatMemberAsync(ChatIdOrUsername chatId, ChatIdOrUsername userId);

        Task<Chat> GetChatAsync(ChatIdOrUsername chatId);

        Task<List<ChatMember>> GetChatAdminsAsync(ChatIdOrUsername chatId);

        Task<int> GetChatMembersCountAsync(ChatIdOrUsername chatId);

        Task<ChatMember> GetChatMemberAsync(ChatIdOrUsername chatId, ChatIdOrUsername userId);

        Task<File> GetFileInfoAsync(string fileId);

        Task<Message> EditMessageTextAsync(string text,
            ChatIdOrUsername chatId = null,
            int? messageId = null,
            string inlineMessageId = null, 
            ParseMode parseMode = ParseMode.None,
            bool? disableWebPagePreview = null,
            KeyboardMarkupReply replyMarkup = null);

        Task<Message> EditMessageCaptionAsync(string caption = null,
            ChatIdOrUsername chatId = null,
            int? messageId = null,
            string inlineMessageId = null,
            KeyboardMarkupReply replyMarkup = null);

        Task<Message> EditMessageReplyMarkupAsync(ChatIdOrUsername chatId = null,
            int? messageId = null,
            string inlineMessageId = null,
            KeyboardMarkupReply replyMarkup = null);

        Task<Message> DeleteMessageAsync(ChatIdOrUsername chatId, int messageId);

        Task<bool> PinChatMessageAsync(ChatIdOrUsername chatId, int messageId,
            bool? disableNotification = null);
        
        Task<bool> UnpinChatMessageAsync(ChatIdOrUsername chatId);

        Task<List<Message>> SendMediaGroupAsync(ChatIdOrUsername chatId, List<InputMedia> media, 
            bool? disableNotification = null, 
            int? replyToMessageId = null);
        
        #region stickers

        Task<StickerSet> GetStickerSetAsync(string name);

        Task<File> UploadStickerFileAsync(string userId, InputFile pngSticker);

        Task<bool> CreateNewStickerSetAsync(int userId, string name, string title, InputFile pngSticker, string emojis,
            bool? containsMasks = null, 
            MaskPosition maskPosition = null);

        Task<bool> AddStickerToSetAsync(int userId, string name, InputFile pngSticker, string emojis, 
            MaskPosition maskPosition = null);

        Task<bool> SetStickerPositionInSetAsync(string sticker, int position);

        Task<bool> DeleteStickerFromSetAsync(string sticker);

        #endregion

        #region file download

        Task<Stream> GetFileStreamAsync(string filePath);

        Task<byte[]> GetFileBytesAsync(string filePath);

        #endregion
    }
}
