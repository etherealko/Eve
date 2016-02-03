using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Telegram.BotApi.Internal;
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

        Task<Message> ForwardMessage(int chatId, int fromChatId, int messageId);
        Task<Message> ForwardMessage(string channelUserName, string fromChannelUserName, int messageId);

        Task<Message> SendPhoto(int chatId, File photo);
        Task<Message> SendPhoto(string channelUserName, string fileIdToResend);

        Task<Message> SendAudio(int chatId, Audio audio);
        Task<Message> SendAudio(string channelUserName, string fileIdToResend);

        Task<Message> SendDocument(int chatId, Document document);
        Task<Message> SendDocument(string channelUserName, string fileIdToResend);

        Task<Message> SendVideo(int chatId, Video video);
        Task<Message> SendVideo(string channelUserName, string fileIdToResend);

        Task<Message> SendVoice(int chatId, Voice voice);
        Task<Message> SendVoice(string channelUserName, string fileIdToResend);

        Task<Message> SendLocation(int chatId, float latitude, float longitude);
        Task<Message> SendLocation(string channelUserName, float latitude, float longitude);

        Task<bool> SendChatAction(int chatId, ChatAction action);
        Task<bool> SendChatAction(string channelUserName, ChatAction action);

        Task<UserProfilePhotos> GetUserProfilePhoto(int userId);
        Task<UserProfilePhotos> GetFile(string fileId);
    }
}
