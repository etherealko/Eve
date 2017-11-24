using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Common.JetBrains.Annotations;
using eth.Telegram.BotApi.Internal;
using eth.Telegram.BotApi.Objects;
using eth.Telegram.BotApi.Objects.Enums;
using eth.Telegram.BotApi.Internal.Serialization;
using eth.Telegram.BotApi.Objects.Base;
using Stream = System.IO.Stream;
using eth.Telegram.BotApi.Events;

namespace eth.Telegram.BotApi
{
    public class TelegramBotApi : ITelegramBotApiWithTimeout, IDisposable
    {
        private readonly HttpApiClient _api;

        public TimeSpan HttpClientTimeout
        {
            get { return _api.Timeout; }
            set { _api.Timeout = value; }
        }

        public object Owner { get; }

        public event RequestEventHandler Request;
        public event ResponseEventHandler Response;

        public TelegramBotApi([NotNull] string token, object owner = null, string apiBase = "https://api.telegram.org/")
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrEmpty(apiBase))
                throw new ArgumentNullException(nameof(apiBase));

            Owner = owner;

            _api = new HttpApiClient(new Uri(apiBase), token);
        }

        public async Task<List<Update>> GetUpdatesAsync(int offset, int limit, int timeoutSeconds)
        {
            // not a part of user api 

            var args = new ApiArgs
            {
                { "offset", offset, nameof(offset) },
                { "limit", limit, nameof(limit) },
                { "timeout", timeoutSeconds, nameof(timeoutSeconds) }
            };

            return await _api.CallJsonAsync<List<Update>>(ApiMethod.GetUpdates, args)
                .ConfigureAwait(false);
        }

        #region api

        public async Task<User> GetMeAsync()
        {
            return await CallApiAsync<User>(ApiMethod.GetMe, null)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendMessageAsync(ChatIdOrUsername chatId, string text,
            ParseMode parseMode = ParseMode.None,
            bool? disableWebPagePreview = null,
            bool? disableNotification = null,
            long? replyToMessageId = null,
            KeyboardMarkupReply replyMarkup = null)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "text", text, nameof(text) },
                { "parse_mode", parseMode, nameof(parseMode), ApiArgumentRequired.Optional },
                { "disable_web_page_preview", disableWebPagePreview, nameof(disableWebPagePreview), ApiArgumentRequired.Optional },
                { "disable_notification", disableNotification, nameof(disableNotification), ApiArgumentRequired.Optional },
                { "reply_to_message_id", replyToMessageId, nameof(replyToMessageId), ApiArgumentRequired.Optional },
                { "reply_markup", replyMarkup, nameof(replyMarkup), ApiArgumentRequired.Optional }
            };
            
            return await CallApiAsync<Message>(ApiMethod.SendMessage, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendStickerAsync(ChatIdOrUsername chatId, string sticker,
            bool? disableNotification = null,
            long? replyToMessageId = null,
            KeyboardMarkupReply replyMarkup = null)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "sticker", sticker, nameof(sticker) },
                { "disable_notification", disableNotification, nameof(disableNotification), ApiArgumentRequired.Optional },
                { "reply_to_message_id", replyToMessageId, nameof(replyToMessageId), ApiArgumentRequired.Optional },
                { "reply_markup", replyMarkup, nameof(replyMarkup), ApiArgumentRequired.Optional }
            };

            return await CallApiAsync<Message>(ApiMethod.SendSticker, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> ForwardMessageAsync(ChatIdOrUsername chatId, ChatIdOrUsername fromChatId, int messageId,
            bool? disableNotification = null)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "from_chat_id", fromChatId, nameof(fromChatId) },
                { "message_id", messageId, nameof(messageId) },
                { "disable_notification", disableNotification, nameof(disableNotification), ApiArgumentRequired.Optional }
            };

            return await CallApiAsync<Message>(ApiMethod.ForwardMessage, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendPhotoAsync(ChatIdOrUsername chatId, InputFile photo,
            string caption = null,
            bool? disableNotification = null,
            long? replyToMessageId = null,
            KeyboardMarkupReply replyMarkup = null)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "photo", photo, nameof(photo) },
                { "caption", caption, nameof(caption), ApiArgumentRequired.Optional },
                { "disable_notification", disableNotification, nameof(disableNotification), ApiArgumentRequired.Optional },
                { "reply_to_message_id", replyToMessageId, nameof(replyToMessageId), ApiArgumentRequired.Optional },
                { "reply_markup", replyMarkup, nameof(replyMarkup), ApiArgumentRequired.Optional }
            };

            return await CallApiAsync<Message>(ApiMethod.SendPhoto, args, true)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendAudioAsync(ChatIdOrUsername chatId, InputFile audio, 
            string caption = null, 
            int? duration = null, 
            string performer = null, 
            string title = null, 
            bool? disableNotification = null, 
            int? replyToMessageId = null, 
            KeyboardMarkupReply replyMarkup = null)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "audio", audio, nameof(audio) },
                { "caption", caption, nameof(caption), ApiArgumentRequired.Optional },
                { "duration", duration, nameof(duration), ApiArgumentRequired.Optional },
                { "performer", performer, nameof(performer), ApiArgumentRequired.Optional },
                { "title", title, nameof(title), ApiArgumentRequired.Optional },
                { "disable_notification", disableNotification, nameof(disableNotification), ApiArgumentRequired.Optional },
                { "reply_to_message_id", replyToMessageId, nameof(replyToMessageId), ApiArgumentRequired.Optional },
                { "reply_markup", replyMarkup, nameof(replyMarkup), ApiArgumentRequired.Optional }
            };

            return await CallApiAsync<Message>(ApiMethod.SendAudio, args, true)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendDocumentAsync(ChatIdOrUsername chatId, Document document)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "document", document, nameof(document) }
            };

            return await CallApiAsync<Message>(ApiMethod.SendDocument, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendVideoAsync(ChatIdOrUsername chatId, Video video)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "video", video, nameof(video) }
            };

            return await CallApiAsync<Message>(ApiMethod.SendVideo, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendVoiceAsync(ChatIdOrUsername chatId, Voice voice)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "voice", voice, nameof(voice) }
            };

            return await CallApiAsync<Message>(ApiMethod.SendVoice, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendLocationAsync(ChatIdOrUsername chatId, float latitude, float longitude)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "latitude", latitude, nameof(latitude) },
                { "longitude", longitude, nameof(longitude) }
            };

            return await CallApiAsync<Message>(ApiMethod.SendLocation, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> SendVenueAsync(ChatIdOrUsername chatId, float latitude, float longitude, string title, string address)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "latitude", latitude, nameof(latitude) },
                { "longitude", longitude, nameof(longitude) },
                { "title", title, nameof(title) },
                { "address", address, nameof(address) }
            };

            return await CallApiAsync<Message>(ApiMethod.SendVenue, args)
               .ConfigureAwait(false);
        }
        
        public async Task<Message> SendContactAsync(ChatIdOrUsername chatId, string phoneNumber, string firstName, string lastName)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "phone_number", phoneNumber, nameof(phoneNumber) },
                { "first_name", firstName, nameof(firstName) },
                { "last_name", lastName, nameof(lastName) }
            };

            return await CallApiAsync<Message>(ApiMethod.SendContact, args)
                .ConfigureAwait(false);
        }
        
        public async Task<bool> SendChatActionAsync(ChatIdOrUsername chatId, ChatAction action)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "action", action, nameof(action) }
            };

            return await CallApiAsync<bool>(ApiMethod.SendChatAction, args)
                .ConfigureAwait(false);
        }
        
        public async Task<bool> KickChatMemberAsync(ChatIdOrUsername chatId, ChatIdOrUsername userId)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "user_id", userId, nameof(userId) }
            };

            return await CallApiAsync<bool>(ApiMethod.KickChatMember, args)
                .ConfigureAwait(false);
        }
        
        public async Task<bool> LeaveChatAsync(ChatIdOrUsername chatId)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) }
            };

            return await CallApiAsync<bool>(ApiMethod.LeaveChat, args)
                .ConfigureAwait(false);
        }
        
        public async Task<bool> UnbanChatMemberAsync(ChatIdOrUsername chatId, ChatIdOrUsername userId)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "user_id", userId, nameof(userId) }
            };

            return await CallApiAsync<bool>(ApiMethod.UnbanChatMember, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Chat> GetChatAsync(ChatIdOrUsername chatId)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) }
            };

            return await CallApiAsync<Chat>(ApiMethod.GetChat, args)
                .ConfigureAwait(false);
        }
        
        public async Task<List<ChatMember>> GetChatAdminsAsync(ChatIdOrUsername chatId)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) }
            };

            return await CallApiAsync<List<ChatMember>>(ApiMethod.GetChatAdmins, args)
                .ConfigureAwait(false);
        }
        
        public async Task<int> GetChatMembersCountAsync(ChatIdOrUsername chatId)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) }
            };

            return await CallApiAsync<int>(ApiMethod.GetChatMembersCount, args).ConfigureAwait(false);
        }
        
        public async Task<ChatMember> GetChatMemberAsync(ChatIdOrUsername chatId, ChatIdOrUsername userId)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "user_id", userId, nameof(userId) }
            };

            return await CallApiAsync<ChatMember>(ApiMethod.GetChatMember, args).ConfigureAwait(false);
        }
        
        public async Task<UserProfilePhotos> GetUserProfilePhotoAsync(ChatIdOrUsername userId)
        {
            var args = new ApiArgs
            {
                { "user_id", userId, nameof(userId) }
            };

            return await CallApiAsync<UserProfilePhotos>(ApiMethod.GetUserProfilePhotos, args)
                .ConfigureAwait(false);
        }

        public async Task<File> GetFileInfoAsync(string fileId)
        {
            var args = new ApiArgs
            {
                { "file_id", fileId, nameof(fileId) }
            };

            return await CallApiAsync<File>(ApiMethod.GetFile, args)
                .ConfigureAwait(false);
        }
        
        public async Task<Message> EditMessageText(string text, 
            ChatIdOrUsername chatId = null, 
            int? messageId = null, 
            string inlineMessageId = null, 
            ParseMode parseMode = ParseMode.None, 
            bool? disableWebPagePreview = null, 
            KeyboardMarkupReply replyMarkup = null)
        {
            var args = new ApiArgs
            {
                { "text", text, nameof(text) },
                { "chat_id", chatId, nameof(chatId), ApiArgumentRequired.Optional },
                { "message_id", messageId, nameof(messageId), ApiArgumentRequired.Optional },
                { "inline_message_id", inlineMessageId, nameof(inlineMessageId), ApiArgumentRequired.Optional },
                { "parse_mode", parseMode, nameof(parseMode), ApiArgumentRequired.Optional },
                { "disable_web_page_preview", disableWebPagePreview, nameof(disableWebPagePreview), ApiArgumentRequired.Optional },
                { "reply_markup", replyMarkup, nameof(replyMarkup), ApiArgumentRequired.Optional }
            };

            return await CallApiAsync<Message>(ApiMethod.EditMessageText, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> EditMessageCaption(string caption = null, 
            ChatIdOrUsername chatId = null, 
            int? messageId = null, 
            string inlineMessageId = null, 
            KeyboardMarkupReply replyMarkup = null)
        {
            var args = new ApiArgs
            {
                { "caption", caption, nameof(caption), ApiArgumentRequired.Optional },
                { "chat_id", chatId, nameof(chatId), ApiArgumentRequired.Optional },
                { "message_id", messageId, nameof(messageId), ApiArgumentRequired.Optional },
                { "inline_message_id", inlineMessageId, nameof(inlineMessageId), ApiArgumentRequired.Optional },
                { "reply_markup", replyMarkup, nameof(replyMarkup), ApiArgumentRequired.Optional }
            };

            return await CallApiAsync<Message>(ApiMethod.EditMessageCaption, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> EditMessageReplyMarkup(ChatIdOrUsername chatId = null, 
            int? messageId = null, 
            string inlineMessageId = null, 
            KeyboardMarkupReply replyMarkup = null)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId), ApiArgumentRequired.Optional },
                { "message_id", messageId, nameof(messageId), ApiArgumentRequired.Optional },
                { "inline_message_id", inlineMessageId, nameof(inlineMessageId), ApiArgumentRequired.Optional },
                { "reply_markup", replyMarkup, nameof(replyMarkup), ApiArgumentRequired.Optional }
            };

            return await CallApiAsync<Message>(ApiMethod.EditMessageReplyMarkup, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> DeleteMessage(ChatIdOrUsername chatId, int messageId)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "message_id", messageId, nameof(messageId) }
            };

            return await CallApiAsync<Message>(ApiMethod.DeleteMessage, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> PinChatMessage(ChatIdOrUsername chatId, int messageId, 
            bool? disableNotification = null)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) },
                { "message_id", messageId, nameof(messageId) },
                { "disable_notification", disableNotification, nameof(disableNotification), ApiArgumentRequired.Optional }
            };

            return await CallApiAsync<bool>(ApiMethod.PinChatMessage, args)
                .ConfigureAwait(false);
        }

        public async Task<bool> UnpinChatMessage(ChatIdOrUsername chatId)
        {
            var args = new ApiArgs
            {
                { "chat_id", chatId, nameof(chatId) }
            };

            return await CallApiAsync<bool>(ApiMethod.UnpinChatMessage, args)
                .ConfigureAwait(false);
        }

        #region file download

        public async Task<Stream> GetFileStreamAsync(string filePath)
        {
            return await _api.GetFileStream(filePath)
                .ConfigureAwait(false);
        }

        public async Task<byte[]> GetFileBytesAsync(string filePath)
        {
            return await _api.GetFileBytes(filePath)
                .ConfigureAwait(false);
        }

        #endregion file download

        #endregion api

        private async Task<T> CallApiAsync<T>(ApiMethod method, ApiArgs args, bool multipartRequired = false)
        {
            if (OnRequest(method, args, ref multipartRequired, out T response))
                return response;

            try
            {
                if (multipartRequired)
                    response = await _api.CallMultipartAsync<T>(method, args).ConfigureAwait(false);
                else
                    response = await _api.CallJsonAsync<T>(method, args).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                OnResponse(method, args, null, ex);

                throw;
            }

            OnResponse(method, args, response, null);

            return response;
        }

        private bool OnRequest<T>(ApiMethod method, ApiArgs args, ref bool multipartRequired, out T response)
        {
            response = default(T);

            var e = Request;

            if (e == null)
                return false;

            var eventArgs = new RequestEventArgs<T>(method, args, multipartRequired, Owner);

            e(this, eventArgs);

            if (eventArgs.ResponseIsForced)
            {
                response = eventArgs.ForcedResponse;
                return true;
            }

            multipartRequired = eventArgs.MultipartRequired;

            return false;
        }

        private void OnResponse(ApiMethod method, ApiArgs args, object response, Exception ex)
        {
            var e = Response;

            if (e == null)
                return;

            var eventArgs = new ResponseEventArgs(method, args, response, ex, Owner);

            e(this, eventArgs);
        }

        public void Dispose()
        {
            _api.Dispose();
        }
    }
}
