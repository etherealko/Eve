using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Common;
using eth.Common.JetBrains.Annotations;
using eth.Telegram.BotApi.Internal;
using eth.Telegram.BotApi.Objects;

namespace eth.Telegram.BotApi
{
    public class TelegramBotApi : ITelegramBotApi, IHttpClientTimeout, IDisposable
    {
        private readonly HttpApiClient _api;

        public TimeSpan HttpClientTimeout
        {
            get { return _api.Timeout; }
            set { _api.Timeout = value; }
        }

        public TelegramBotApi([NotNull] string token, string apiBase = "https://api.telegram.org/")
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrEmpty(apiBase))
                throw new ArgumentNullException(nameof(apiBase));
            
            _api = new HttpApiClient(new Uri(apiBase), token);
        }

        public async Task<List<Update>> GetUpdatesAsync(int offset, int limit, int timeoutSeconds)
        {
            var args = new
            {
                offset = offset,
                limit = limit,
                timeout = timeoutSeconds
            };

            return await _api.GetAsync<List<Update>>(ApiMethodPaths.GetUpdates, args)
                .ConfigureAwait(false);
        }

        public async Task<User> GetMeAsync()
        {
            return await _api.GetAsync<User>(ApiMethodPaths.GetMe)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendMessageAsync(int chatId, string text)
        {
            var args = new
            {
                chat_id = chatId,
                text = text
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendMessage, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendMessageAsync(string channelUserName, string text)
        {
            var args = new
            {
                chat_id = channelUserName,
                text = text
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendMessage, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendStickerAsync(int chatId, string sticker)
        {
            var args = new
            {
                chat_id = chatId,
                sticker = sticker
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendSticker, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendStickerAsync(string channelUserName, string sticker)
        {
            var args = new
            {
                chat_id = channelUserName,
                sticker = sticker
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendSticker, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> ForwardMessage(int chatId, int fromChatId, int messageId)
        {
            var args = new
            {
                chat_id = chatId,
                from_chat_id = fromChatId,
                message_id = messageId
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.ForwardMessage, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> ForwardMessage(string channelUserName, string fromChannelUserName, int messageId)
        {
            var args = new
            {
                chat_id = channelUserName,
                from_chat_id = fromChannelUserName,
                message_id = messageId
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.ForwardMessage, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendPhoto(int chatId, File photo)
        {
            var args = new
            {
                chat_id = chatId,
                photo = photo
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendPhoto, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendPhoto(string channelUserName, string fileIdToResend)
        {
            var args = new
            {
                chat_id = channelUserName,
                photo = fileIdToResend
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendPhoto, args)
                .ConfigureAwait(false);           
        }

        public async Task<Message> SendAudio(int chatId, Audio audio)
        {
            var args = new
            {
                chat_id = chatId,
                audio = audio
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendAudio, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendAudio(string channelUserName, string fileIdToResend)
        {
            var args = new
            {
                chat_id = channelUserName,
                audio = fileIdToResend
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendAudio, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendDocument(int chatId, Document document)
        {
            var args = new
            {
                chat_id = chatId,
                document = document
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendDocument, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendDocument(string channelUserName, string fileIdToResend)
        {
            var args = new
            {
                chat_id = channelUserName,
                document = fileIdToResend
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendDocument, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendVideo(int chatId, Video video)
        {
            var args = new
            {
                chat_id = chatId,
                video = video
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendVideo, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendVideo(string channelUserName, string fileIdToResend)
        {
            var args = new
            {
                chat_id = channelUserName,
                video = fileIdToResend
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendVideo, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendVoice(int chatId, Voice voice)
        {
            var args = new
            {
                chat_id = chatId,
                voice = voice
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendVoice, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendVoice(string channelUserName, string fileIdToResend)
        {
            var args = new
            {
                chat_id = channelUserName,
                voice = fileIdToResend
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendVoice, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendLocation(int chatId, float latitude, float longitude)
        {
            var args = new
            {
                chat_id = chatId,
                latitude = latitude,
                longitude = longitude
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendLocation, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendLocation(string channelUserName, float latitude, float longitude)
        {
            var args = new
            {
                chat_id = channelUserName,
                latitude = latitude,
                longitude = longitude
            };

            return await _api.GetAsync<Message>(ApiMethodPaths.SendLocation, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> SendChatAction(int chatId, ChatAction action)
        {
            var args = new
            {
                chat_id = chatId,
                action = action
            };

            return await _api.GetAsync<bool>(ApiMethodPaths.SendChatAction, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> SendChatAction(string channelUserName, ChatAction action)
        {
            var args = new
            {
                chat_id = channelUserName,
                action = action
            };

            return await _api.GetAsync<bool>(ApiMethodPaths.SendChatAction, args)
                .ConfigureAwait(false);
        }

        public async Task<UserProfilePhotos> GetUserProfilePhoto(int userId)
        {
            var args = new
            {
                user_id = userId             
            };

            return await _api.GetAsync<UserProfilePhotos>(ApiMethodPaths.GetUserProfilePhotos, args)
                .ConfigureAwait(false);
        }

        public async Task<UserProfilePhotos> GetFile(string fileId)
        {
            var args = new
            {
                file_id = fileId
            };

            return await _api.GetAsync<UserProfilePhotos>(ApiMethodPaths.GetFile, args)
                .ConfigureAwait(false);
        }

        public void Dispose()
        {
            _api.Dispose();
        }
    }
}
