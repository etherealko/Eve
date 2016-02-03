using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using eth.Eve.Internal;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;

#pragma warning disable 4014

namespace eth.Eve
{
    public class EveBot : IDisposable
    {
        private readonly BotUpdatePoller _updater;
        private readonly TelegramBotApi _outgoingApi;

        private readonly Thread _mainThread;

        private volatile bool _shutdown;

        public EveBot(string token)
        {
            _updater = new BotUpdatePoller(token);
            _outgoingApi = new TelegramBotApi(token);

            _mainThread = new Thread(UpdateProc);
        }

        public void Start()
        {
            if (_shutdown)
                throw new InvalidOperationException("_shutdown == true");

            _mainThread.Start();
        }

        public void Stop()
        {
            Dispose();
        }

        private async void UpdateProc()
        {
            var updates = await _updater.PollInitialUpdates();
            HandleOld(updates);

            while (!_shutdown)
            {
                try
                {
                    updates = await _updater.PollUpdates();
                    foreach (var update in updates)
                        HandleNew(update);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        private void HandleNew(Update update)
        {
            Debug.Assert(update != null);

            if (update.Message == null)
                return;

            if (update.Message.Chat.Id == -49047577 && 
                (update.Message.Text ?? "").ToLowerInvariant().Contains("ева"))
                _outgoingApi.SendMessageAsync(-49047577, "чо");
        }

        private void HandleOld(List<Update> updates)
        {
            Debug.Assert(updates != null);

            _outgoingApi.SendMessageAsync(-49047577, $"Юля сгорела {updates.Count} раз.");
        }

        public void Dispose()
        {
            _shutdown = true;

            _updater.Dispose();
            _outgoingApi.Dispose();
        }
    }
}
