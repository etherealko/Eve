using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Common.JetBrains.Annotations;
using eth.Telegram.BotApi.Internal;
using eth.Telegram.BotApi.Objects;

namespace eth.Telegram.BotApi
{
    public class TelegramBotApi : ITelegramBotApi
    {
        private readonly HttpApiClient _api;

        public TelegramBotApi([NotNull] string token, string apiBase = "https://api.telegram.org/")
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrEmpty(apiBase))
                throw new ArgumentNullException(nameof(apiBase));
            
            _api = new HttpApiClient(new Uri(apiBase), token);
        }

        public async Task<List<Update>> GetUpdatesAsync(int offset, int limit, int timeout)
        {
            var args = new
            {
                offset = offset,
                limit = limit,
                timeout = timeout
            };

            return await _api.CallAsync<List<Update>>(ApiMethodPaths.GetUpdates, args)
                .ConfigureAwait(false);
        }

        public async Task<User> GetMeAsync()
        {
            return await _api.CallAsync<User>(ApiMethodPaths.GetMe)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendMessageAsync(int chatId, string text)
        {
            var args = new
            {
                chat_id = chatId,
                text = text,
            };

            return await _api.CallAsync<Message>(ApiMethodPaths.SendMessage, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendMessageAsync(string channelusername, string text)
        {
            var args = new
            {
                chat_id = channelusername,
                text = text,
            };

            return await _api.CallAsync<Message>(ApiMethodPaths.SendMessage, args)
                .ConfigureAwait(false);
        }

        public async Task<Message> SendStickerAsync(int chatId, string sticker)
        {
            var args = new
            {
                chat_id = chatId,
                sticker = sticker,
            };

            return await _api.CallAsync<Message>(ApiMethodPaths.SendSticker, args)
                .ConfigureAwait(false);
        }

        public Task<Message> SendStickerAsync(string channelusername, string sticker)
        {
            throw new NotImplementedException();
        }
    }
}
