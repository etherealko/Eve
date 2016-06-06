using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eth.Eve.Internal;
using eth.Eve.Storage;
using eth.Eve.Storage.Model;
using eth.Eve.TempForTesting;
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

        private readonly EveSpace _currentSpace;

        //temp
        private readonly PluginOne _pluginOne = new PluginOne();

        public EveBot()
        {
            var db = new EveDb();

            _currentSpace = db.EveSpaces.Single(s => s.IsActive); //only single space is supported now

            _updater = new BotUpdatePoller(_currentSpace.BotApiAccessToken);
            _outgoingApi = new TelegramBotApi(_currentSpace.BotApiAccessToken);

            _mainThread = new Thread(UpdateProc);
        }

        public void Start()
        {
            if (_shutdown)
                throw new InvalidOperationException("_shutdown == true");

            _mainThread.Start();

            _pluginOne.Initialize(new PluginContext(_pluginOne.Info, _outgoingApi));
        }

        public void Stop()
        {
            _pluginOne.Teardown();

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
                    //todo: add logging
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
            
            _pluginOne.Handle(new MessageContext { Update = update });
        }

        private void HandleOld(List<Update> updates)
        {
            Debug.Assert(updates != null);
            
            Console.WriteLine($"{updates.Count} message(s)");

            foreach (var update in updates)
                _pluginOne.Handle(new MessageContext { IsInitiallyPolled = true, Update = update });
        }

        public void Dispose()
        {
            _shutdown = true;

            _updater.Dispose();
            _outgoingApi.Dispose();
            _pluginOne.Dispose();
        }
    }
}
