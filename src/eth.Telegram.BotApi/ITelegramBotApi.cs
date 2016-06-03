using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Telegram.BotApi.Objects;

namespace eth.Telegram.BotApi
{
    public interface ITelegramBotApi
    {
        Task<List<Update>> GetUpdatesAsync(int offset, int limit, int timeoutSeconds);

        Task<User> GetMeAsync();

        Task<Message> SendMessageAsync(int chatId, string text);
        Task<Message> SendMessageAsync(string channelUserName, string text);

        Task<Message> SendStickerAsync(int chatId, string sticker);
        Task<Message> SendStickerAsync(string channelUserName, string sticker);

        Task<Message> ForwardMessageAsync(int chatId, int fromChatId, int messageId);
        Task<Message> ForwardMessageAsync(string channelUserName, string fromChannelUserName, int messageId);

        Task<Message> SendPhotoAsync(int chatId, File photo);
        Task<Message> SendPhotoAsync(string channelUserName, string fileIdToResend);

        Task<Message> SendAudioAsync(int chatId, Audio audio);
        Task<Message> SendAudioAsync(string channelUserName, string fileIdToResend);

        Task<Message> SendDocumentAsync(int chatId, Document document);
        Task<Message> SendDocumentAsync(string channelUserName, string fileIdToResend);

        Task<Message> SendVideoAsync(int chatId, Video video);
        Task<Message> SendVideoAsync(string channelUserName, string fileIdToResend);

        Task<Message> SendVoiceAsync(int chatId, Voice voice);
        Task<Message> SendVoiceAsync(string channelUserName, string fileIdToResend);

        Task<Message> SendLocationAsync(int chatId, float latitude, float longitude);
        Task<Message> SendLocationAsync(string channelUserName, float latitude, float longitude);

        Task<Message> SendVenueAsync(int chatId, float latitude, float longitude, string title, string address);
        Task<Message> SendVenueAsync(string channelUserName, float latitude, float longitude, string title, string address);

        Task<Message> SendContactAsync(int chatId, string phoneNumber, string firstName, string lastName);
        Task<Message> SendContactAsync(string channelUserName, string phoneNumber, string firstName, string lastName);

        Task<bool> SendChatActionAsync(int chatId, ChatAction action);
        Task<bool> SendChatActionAsync(string channelUserName, ChatAction action);

        Task<UserProfilePhotos> GetUserProfilePhotoAsync(int userId);
        Task<UserProfilePhotos> GetFileAsync(string fileId);

        Task<bool> KickChatMemberAsync(int chatId, int userId);
        Task<bool> KickChatMemberAsync(string channelUserName, int userId);

        Task<bool> LeaveChatAsync(int chatId);
        Task<bool> LeaveChatAsync(string channelUserName);

        Task<bool> UnbanChatMemberAsync(int chatId, int userId);
        Task<bool> UnbanChatMemberAsync(string channelUserName, int userId);

        Task<Chat> GetChatAsync(int chatId);
        Task<Chat> GetChatAsync(string channelUserName);

        Task<List<ChatMember>> GetChatAdminsAsync(int chatId);
        Task<List<ChatMember>> GetChatAdminsAsync(string channelUserName);

        Task<int> GetChatMembersCountAsync(int chatId);
        Task<int> GetChatMembersCountAsync(string channelUserName);

        Task<ChatMember> GetChatMemberAsync(int chatId, int userId);
        Task<ChatMember> GetChatMemberAsync(string channelUserName, int userId);
    }
}
