using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Telegram.BotApi.Objects;

namespace eth.Telegram.BotApi
{
    public interface ITelegramBotApi
    {
        Task<List<Update>> GetUpdatesAsync(int offset, int limit, int timeoutSeconds);

        Task<User> GetMeAsync();

        Task<Message> SendMessageAsync(long chatId, string text);
        Task<Message> SendMessageAsync(string channelUserName, string text);

        Task<Message> SendStickerAsync(long chatId, string sticker);
        Task<Message> SendStickerAsync(string channelUserName, string sticker);

        Task<Message> ForwardMessageAsync(long chatId, int fromChatId, int messageId);
        Task<Message> ForwardMessageAsync(string channelUserName, string fromChannelUserName, int messageId);

        Task<Message> SendPhotoAsync(long chatId, File photo);
        Task<Message> SendPhotoAsync(string channelUserName, string fileIdToResend);

        Task<Message> SendAudioAsync(long chatId, Audio audio);
        Task<Message> SendAudioAsync(string channelUserName, string fileIdToResend);

        Task<Message> SendDocumentAsync(long chatId, Document document);
        Task<Message> SendDocumentAsync(string channelUserName, string fileIdToResend);

        Task<Message> SendVideoAsync(long chatId, Video video);
        Task<Message> SendVideoAsync(string channelUserName, string fileIdToResend);

        Task<Message> SendVoiceAsync(long chatId, Voice voice);
        Task<Message> SendVoiceAsync(string channelUserName, string fileIdToResend);

        Task<Message> SendLocationAsync(long chatId, float latitude, float longitude);
        Task<Message> SendLocationAsync(string channelUserName, float latitude, float longitude);

        Task<Message> SendVenueAsync(long chatId, float latitude, float longitude, string title, string address);
        Task<Message> SendVenueAsync(string channelUserName, float latitude, float longitude, string title, string address);

        Task<Message> SendContactAsync(long chatId, string phoneNumber, string firstName, string lastName);
        Task<Message> SendContactAsync(string channelUserName, string phoneNumber, string firstName, string lastName);

        Task<bool> SendChatActionAsync(long chatId, ChatAction action);
        Task<bool> SendChatActionAsync(string channelUserName, ChatAction action);

        Task<UserProfilePhotos> GetUserProfilePhotoAsync(int userId);
        Task<UserProfilePhotos> GetFileAsync(string fileId);

        Task<bool> KickChatMemberAsync(long chatId, int userId);
        Task<bool> KickChatMemberAsync(string channelUserName, int userId);

        Task<bool> LeaveChatAsync(long chatId);
        Task<bool> LeaveChatAsync(string channelUserName);

        Task<bool> UnbanChatMemberAsync(long chatId, int userId);
        Task<bool> UnbanChatMemberAsync(string channelUserName, int userId);

        Task<Chat> GetChatAsync(long chatId);
        Task<Chat> GetChatAsync(string channelUserName);

        Task<List<ChatMember>> GetChatAdminsAsync(long chatId);
        Task<List<ChatMember>> GetChatAdminsAsync(string channelUserName);

        Task<int> GetChatMembersCountAsync(long chatId);
        Task<int> GetChatMembersCountAsync(string channelUserName);

        Task<ChatMember> GetChatMemberAsync(long chatId, int userId);
        Task<ChatMember> GetChatMemberAsync(string channelUserName, int userId);
    }
}
