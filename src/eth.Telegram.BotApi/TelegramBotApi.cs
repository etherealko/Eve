//Telegram Bot API v2.1
//for details: https://core.telegram.org/bots/api
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

        #region api

        public async Task<List<Update>> GetUpdatesAsync(int offset, int limit, int timeoutSeconds)
        {
            var args = new
            {
                offset = offset,
                limit = limit,
                timeout = timeoutSeconds
            };

            return await _api.CallAsync<List<Update>>(ApiMethod.GetUpdates, args)
                .ConfigureAwait(false);
        }

        public async Task<User> GetMeAsync()
        {
            return await _api.CallAsync<User>(ApiMethod.GetMe)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendMessageAsync(long chatId, string text)
        {
            var args = new
            {
                chat_id = chatId,
                text = text
            };

            return await _api.CallAsync<Message>(ApiMethod.SendMessage, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendMessageAsync(string channelUserName, string text)
        {
            var args = new
            {
                chat_id = channelUserName,
                text = text
            };

            return await _api.CallAsync<Message>(ApiMethod.SendMessage, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendStickerAsync(long chatId, string sticker)
        {
            var args = new
            {
                chat_id = chatId,
                sticker = sticker
            };

            return await _api.CallAsync<Message>(ApiMethod.SendSticker, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendStickerAsync(string channelUserName, string sticker)
        {
            var args = new
            {
                chat_id = channelUserName,
                sticker = sticker
            };

            return await _api.CallAsync<Message>(ApiMethod.SendSticker, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> ForwardMessageAsync(long chatId, int fromChatId, int messageId)
        {
            var args = new
            {
                chat_id = chatId,
                from_chat_id = fromChatId,
                message_id = messageId
            };

            return await _api.CallAsync<Message>(ApiMethod.ForwardMessage, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> ForwardMessageAsync(string channelUserName, string fromChannelUserName, int messageId)
        {
            var args = new
            {
                chat_id = channelUserName,
                from_chat_id = fromChannelUserName,
                message_id = messageId
            };

            return await _api.CallAsync<Message>(ApiMethod.ForwardMessage, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendPhotoAsync(long chatId, File photo)
        {
            var args = new
            {
                chat_id = chatId,
                photo = photo
            };

            return await _api.CallAsync<Message>(ApiMethod.SendPhoto, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendPhotoAsync(string channelUserName, string fileIdToResend)
        {
            var args = new
            {
                chat_id = channelUserName,
                photo = fileIdToResend
            };

            return await _api.CallAsync<Message>(ApiMethod.SendPhoto, args)
                .ConfigureAwait(false);           
        }

        public async Task<Message> SendAudioAsync(long chatId, Audio audio)
        {
            var args = new
            {
                chat_id = chatId,
                audio = audio
            };

            return await _api.CallAsync<Message>(ApiMethod.SendAudio, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendAudioAsync(string channelUserName, string fileIdToResend)
        {
            var args = new
            {
                chat_id = channelUserName,
                audio = fileIdToResend
            };

            return await _api.CallAsync<Message>(ApiMethod.SendAudio, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendDocumentAsync(long chatId, Document document)
        {
            var args = new
            {
                chat_id = chatId,
                document = document
            };

            return await _api.CallAsync<Message>(ApiMethod.SendDocument, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendDocumentAsync(string channelUserName, string fileIdToResend)
        {
            var args = new
            {
                chat_id = channelUserName,
                document = fileIdToResend
            };

            return await _api.CallAsync<Message>(ApiMethod.SendDocument, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendVideoAsync(long chatId, Video video)
        {
            var args = new
            {
                chat_id = chatId,
                video = video
            };

            return await _api.CallAsync<Message>(ApiMethod.SendVideo, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendVideoAsync(string channelUserName, string fileIdToResend)
        {
            var args = new
            {
                chat_id = channelUserName,
                video = fileIdToResend
            };

            return await _api.CallAsync<Message>(ApiMethod.SendVideo, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendVoiceAsync(long chatId, Voice voice)
        {
            var args = new
            {
                chat_id = chatId,
                voice = voice
            };

            return await _api.CallAsync<Message>(ApiMethod.SendVoice, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendVoiceAsync(string channelUserName, string fileIdToResend)
        {
            var args = new
            {
                chat_id = channelUserName,
                voice = fileIdToResend
            };

            return await _api.CallAsync<Message>(ApiMethod.SendVoice, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendLocationAsync(long chatId, float latitude, float longitude)
        {
            var args = new
            {
                chat_id = chatId,
                latitude = latitude,
                longitude = longitude
            };

            return await _api.CallAsync<Message>(ApiMethod.SendLocation, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendLocationAsync(string channelUserName, float latitude, float longitude)
        {
            var args = new
            {
                chat_id = channelUserName,
                latitude = latitude,
                longitude = longitude
            };

            return await _api.CallAsync<Message>(ApiMethod.SendLocation, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendVenueAsync(long chatId, float latitude, float longitude, string title, string address)
        {
            var args = new
            {
                chat_id = chatId,
                latitude = latitude,
                longitude = longitude,
                title = title,
                address = address
            };

            return await _api.CallAsync<Message>(ApiMethod.SendVenue, args)
               .ConfigureAwait(false);
        }

        public async Task<Message> SendVenueAsync(string channelUserName, float latitude, float longitude, string title, string address)
        {
            var args = new
            {
                chat_id = channelUserName,
                latitude = latitude,
                longitude = longitude,
                title = title,
                address = address
            };

            return await _api.CallAsync<Message>(ApiMethod.SendVenue, args)
              .ConfigureAwait(false);
        }

        public async Task<Message> SendContactAsync(long chatId, string phoneNumber, string firstName, string lastName)
        {
            var args = new
            {
                chat_id = chatId,
                phone_number = phoneNumber,
                first_name = firstName,
                last_name = lastName
            };

            return await _api.CallAsync<Message>(ApiMethod.SendContact, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendContactAsync(string channelUserName, string phoneNumber, string firstName, string lastName)
        {
            var args = new
            {
                chat_id = channelUserName,
                phone_number = phoneNumber,
                first_name = firstName,
                last_name = lastName
            };

            return await _api.CallAsync<Message>(ApiMethod.SendContact, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> SendChatActionAsync(long chatId, ChatAction action)
        {
            var args = new
            {
                chat_id = chatId,
                action = action
            };

            return await _api.CallAsync<bool>(ApiMethod.SendChatAction, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> SendChatActionAsync(string channelUserName, ChatAction action)
        {
            var args = new
            {
                chat_id = channelUserName,
                action = action
            };

            return await _api.CallAsync<bool>(ApiMethod.SendChatAction, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> KickChatMemberAsync(long chatId, int userId)
        {
            var args = new
            {
                chat_id = chatId,
                user_id = userId
            };

            return await _api.CallAsync<bool>(ApiMethod.KickChatMember, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> KickChatMemberAsync(string channelUserName, int userId)
        {
            var args = new
            {
                chat_id = channelUserName,
                user_id = userId
            };

            return await _api.CallAsync<bool>(ApiMethod.KickChatMember, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> LeaveChatAsync(long chatId)
        {
            var args = new
            {
                chat_id = chatId
            };

            return await _api.CallAsync<bool>(ApiMethod.LeaveChat, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> LeaveChatAsync(string channelUserName)
        {
            var args = new
            {
                chat_id = channelUserName
            };

            return await _api.CallAsync<bool>(ApiMethod.LeaveChat, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> UnbanChatMemberAsync(long chatId, int userId)
        {
            var args = new
            {
                chat_id = chatId,
                user_id = userId
            };

            return await _api.CallAsync<bool>(ApiMethod.UnbanChatMember, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> UnbanChatMemberAsync(string channelUserName, int userId)
        {
            var args = new
            {
                chat_id = channelUserName,
                user_id = userId
            };

            return await _api.CallAsync<bool>(ApiMethod.UnbanChatMember, args)
                .ConfigureAwait(false);
        }

        public async Task<Chat> GetChatAsync(long chatId)
        {
            var args = new
            {
                chat_id = chatId
            };

            return await _api.CallAsync<Chat>(ApiMethod.GetChat, args)
                .ConfigureAwait(false);
        }

        public async Task<Chat> GetChatAsync(string channelUserName)
        {
            var args = new
            {
                chat_id = channelUserName
            };

            return await _api.CallAsync<Chat>(ApiMethod.GetChat, args)
                .ConfigureAwait(false);
        }

        public async Task<List<ChatMember>> GetChatAdminsAsync(long chatId)
        {
            var args = new
            {
                chat_id = chatId
            };

            return await _api.CallAsync<List<ChatMember>>(ApiMethod.GetChatAdmins, args)
                .ConfigureAwait(false);
        }

        public async Task<List<ChatMember>> GetChatAdminsAsync(string channelUserName)
        {
            var args = new
            {
                chat_id = channelUserName
            };

            return await _api.CallAsync<List<ChatMember>>(ApiMethod.GetChatAdmins, args)
                 .ConfigureAwait(false);
        }

        public async Task<int> GetChatMembersCountAsync(long chatId)
        {
            var args = new
            {
                chat_id = chatId
            };

            return await _api.CallAsync<int>(ApiMethod.GetChatMembersCount, args).ConfigureAwait(false);
        }

        public async Task<int> GetChatMembersCountAsync(string channelUserName)
        {
            var args = new
            {
                chat_id = channelUserName
            };

            return await _api.CallAsync<int>(ApiMethod.GetChatMembersCount, args).ConfigureAwait(false);
        }

        public async Task<ChatMember> GetChatMemberAsync(long chatId, int userId)
        {
            var args = new
            {
                chat_id = chatId,
                user_id = userId
            };

            return await _api.CallAsync<ChatMember>(ApiMethod.GetChatMember, args).ConfigureAwait(false);
        }

        public async Task<ChatMember> GetChatMemberAsync(string channelUserName, int userId)
        {
            var args = new
            {
                chat_id = channelUserName,
                user_id = userId
            };

            return await _api.CallAsync<ChatMember>(ApiMethod.GetChatMember, args).ConfigureAwait(false);
        }

        public async Task<UserProfilePhotos> GetUserProfilePhotoAsync(int userId)
        {
            var args = new
            {
                user_id = userId
            };

            return await _api.CallAsync<UserProfilePhotos>(ApiMethod.GetUserProfilePhotos, args)
                .ConfigureAwait(false);
        }

        public async Task<UserProfilePhotos> GetFileAsync(string fileId)
        {
            var args = new
            {
                file_id = fileId
            };

            return await _api.CallAsync<UserProfilePhotos>(ApiMethod.GetFile, args)
                .ConfigureAwait(false);
        }

        #endregion api

        public void Dispose()
        {
            _api.Dispose();
        }
    }
}
