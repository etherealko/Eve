using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Telegram.BotApi.Objects;
using eth.Telegram.BotApi.Objects.Enums;
using eth.Telegram.BotApi.Objects.Base;

namespace eth.Telegram.BotApi
{
    //Telegram Bot API v2.1
    //for details: https://core.telegram.org/bots/api
    //UPD: the api is currently somewhere between 2.1 and 3.4, keep it in mind and check online if in doubt
    public interface ITelegramBotApi
    {
        Task<List<Update>> GetUpdatesAsync(int offset, int limit, int timeoutSeconds);

        Task<User> GetMeAsync();

        Task<Message> SendMessageAsync(ChatIdOrUsername chatId, string text, 
            ParseMode parseMode = ParseMode.None, 
            bool? disableWebPagePreview = null, 
            bool? disableNotification = null, 
            long? replyToMessageId = null,
            KeyboardMarkupReply replyMarkup = null);

        Task<Message> SendStickerAsync(ChatIdOrUsername chatId, string sticker);

        Task<Message> ForwardMessageAsync(ChatIdOrUsername chatId, ChatIdOrUsername fromChatId, int messageId);

        Task<Message> SendPhotoAsync(ChatIdOrUsername chatId, File photo);

        Task<Message> SendAudioAsync(ChatIdOrUsername chatId, Audio audio);

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
    }
}
