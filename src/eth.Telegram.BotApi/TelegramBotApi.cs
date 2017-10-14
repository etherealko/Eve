using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Common;
using eth.Common.JetBrains.Annotations;
using eth.Telegram.BotApi.Internal;
using eth.Telegram.BotApi.Objects;
using eth.Telegram.BotApi.Objects.Enums;
using Newtonsoft.Json.Linq;
using eth.Telegram.BotApi.Internal.Serialization;

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

        public async Task<Message> SendMessageAsync(ChatIdOrUsername chatId, string text,
            ParseMode parseMode = ParseMode.None,
            bool? disableWebPagePreview = null,
            bool? disableNotification = null,
            long? replyToMessageId = null,
            object replyMarkup = null)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId },
                { "text", text },
                { "parse_mode", parseMode, true },
                { "disable_web_page_preview", disableWebPagePreview, true },
                { "disable_notification", disableNotification, true },
                { "reply_to_message_id", replyToMessageId, true },
                { "reply_markup", replyMarkup, true }
            };
            
            return await _api.CallAsync<Message>(ApiMethod.SendMessage, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendStickerAsync(ChatIdOrUsername chatId, string sticker)
        {
            var args = new
            {
                chat_id = chatId,
                sticker = sticker
            };

            return await _api.CallAsync<Message>(ApiMethod.SendSticker, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> ForwardMessageAsync(ChatIdOrUsername chatId, ChatIdOrUsername fromChatId, int messageId)
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
        
        public async Task<Message> SendPhotoAsync(ChatIdOrUsername chatId, File photo)
        {
            var args = new
            {
                chat_id = chatId,
                photo = photo
            };

            return await _api.CallAsync<Message>(ApiMethod.SendPhoto, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendAudioAsync(ChatIdOrUsername chatId, Audio audio)
        {
            var args = new
            {
                chat_id = chatId,
                audio = audio
            };

            return await _api.CallAsync<Message>(ApiMethod.SendAudio, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendDocumentAsync(ChatIdOrUsername chatId, Document document)
        {
            var args = new
            {
                chat_id = chatId,
                document = document
            };

            return await _api.CallAsync<Message>(ApiMethod.SendDocument, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendVideoAsync(ChatIdOrUsername chatId, Video video)
        {
            var args = new
            {
                chat_id = chatId,
                video = video
            };

            return await _api.CallAsync<Message>(ApiMethod.SendVideo, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendVoiceAsync(ChatIdOrUsername chatId, Voice voice)
        {
            var args = new
            {
                chat_id = chatId,
                voice = voice
            };

            return await _api.CallAsync<Message>(ApiMethod.SendVoice, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendLocationAsync(ChatIdOrUsername chatId, float latitude, float longitude)
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
        
        public async Task<Message> SendVenueAsync(ChatIdOrUsername chatId, float latitude, float longitude, string title, string address)
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
        
        public async Task<Message> SendContactAsync(ChatIdOrUsername chatId, string phoneNumber, string firstName, string lastName)
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
        
        public async Task<bool> SendChatActionAsync(ChatIdOrUsername chatId, ChatAction action)
        {
            var args = new
            {
                chat_id = chatId,
                action = action
            };

            return await _api.CallAsync<bool>(ApiMethod.SendChatAction, args)
                .ConfigureAwait(false);
        }
        
        public async Task<bool> KickChatMemberAsync(ChatIdOrUsername chatId, ChatIdOrUsername userId)
        {
            var args = new
            {
                chat_id = chatId,
                user_id = userId
            };

            return await _api.CallAsync<bool>(ApiMethod.KickChatMember, args)
                .ConfigureAwait(false);
        }
        
        public async Task<bool> LeaveChatAsync(ChatIdOrUsername chatId)
        {
            var args = new
            {
                chat_id = chatId
            };

            return await _api.CallAsync<bool>(ApiMethod.LeaveChat, args)
                .ConfigureAwait(false);
        }
        
        public async Task<bool> UnbanChatMemberAsync(ChatIdOrUsername chatId, ChatIdOrUsername userId)
        {
            var args = new
            {
                chat_id = chatId,
                user_id = userId
            };

            return await _api.CallAsync<bool>(ApiMethod.UnbanChatMember, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Chat> GetChatAsync(ChatIdOrUsername chatId)
        {
            var args = new
            {
                chat_id = chatId
            };

            return await _api.CallAsync<Chat>(ApiMethod.GetChat, args)
                .ConfigureAwait(false);
        }
        
        public async Task<List<ChatMember>> GetChatAdminsAsync(ChatIdOrUsername chatId)
        {
            var args = new
            {
                chat_id = chatId
            };

            return await _api.CallAsync<List<ChatMember>>(ApiMethod.GetChatAdmins, args)
                .ConfigureAwait(false);
        }
        
        public async Task<int> GetChatMembersCountAsync(ChatIdOrUsername chatId)
        {
            var args = new
            {
                chat_id = chatId
            };

            return await _api.CallAsync<int>(ApiMethod.GetChatMembersCount, args).ConfigureAwait(false);
        }
        
        public async Task<ChatMember> GetChatMemberAsync(ChatIdOrUsername chatId, ChatIdOrUsername userId)
        {
            var args = new
            {
                chat_id = chatId,
                user_id = userId
            };

            return await _api.CallAsync<ChatMember>(ApiMethod.GetChatMember, args).ConfigureAwait(false);
        }
        
        public async Task<UserProfilePhotos> GetUserProfilePhotoAsync(ChatIdOrUsername userId)
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
