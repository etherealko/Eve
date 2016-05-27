using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
                }
                catch (Exception ex)
                {
                    
                }
            }

            Debug.WriteLine("Update thread finished");
        }

        private void HandleNew(Update update)
        {
            Debug.Assert(update != null);

            if (update.Message == null)
                return;

            Console.WriteLine($" >{update.Message?.From.FirstName ?? "<no fname>"}: {update.Message.Text}");

            if (update.Message.Chat.Id == -49047577)
            {
                var eva = new Regex(@"\b[eе]+[vв]+[aа]+\b", RegexOptions.IgnoreCase);

                if (eva.IsMatch(update.Message.Text ?? ""))
                {
                    _outgoingApi.SendMessageAsync(-49047577, "чо");
                    return;
                }

                if (update.Message.From.Id == 146268050)
                {
                    var roll = new Random().Next(10);
                    if (roll == 7)
                        _outgoingApi.SendMessageAsync(-49047577, "юль, ну впиши по-братски");
                    return;
                }
            }
        }

        private void HandleOld(List<Update> updates)
        {
            Debug.Assert(updates != null);

            //_outgoingApi.SendMessageAsync(-49047577, $"Юля сгорела {updates.Count} раз.");
            Console.WriteLine($"{updates.Count} message(s)");
        }

        public void Dispose()
        {
            _shutdown = true;

            _updater.Dispose();
            _outgoingApi.Dispose();
        }
    }
}
