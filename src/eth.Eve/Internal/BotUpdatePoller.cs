using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;

namespace eth.Eve.Internal
{
    internal class BotUpdatePoller : IDisposable
    {
        private readonly TelegramBotApi _api;

        private bool _preStartUpdatesPolled;
        private int _lastUpdate;

        public BotUpdatePoller(TelegramBotApi api)
        {
            _api = api;
        }

        public async Task<List<Update>> PollInitialUpdates()
        {
            var updates = new List<Update>();

            do
            {
                var newUpdates = await _api.GetUpdatesAsync(_lastUpdate, 100, 0)
                    .ConfigureAwait(false);

                if (newUpdates.Count <= 0)
                    break;

                _lastUpdate = newUpdates[newUpdates.Count - 1].UpdateId + 1;
                updates.AddRange(newUpdates);
            } while (updates.Count > 0);

            _preStartUpdatesPolled = true;
            return updates;
        }

        public async Task<List<Update>> PollUpdates(uint pollTimeoutMs = 30000)
        {
            if (!_preStartUpdatesPolled)
                await PollInitialUpdates();

            var newUpdates = await _api.GetUpdatesAsync(_lastUpdate, 100, (int)pollTimeoutMs/1000)
                .ConfigureAwait(false);

            if (newUpdates.Count > 0)
                _lastUpdate = newUpdates[newUpdates.Count - 1].UpdateId + 1;

            return newUpdates;
        }

        public void Dispose()
        {
            _api.Dispose();
        }
    }
}
