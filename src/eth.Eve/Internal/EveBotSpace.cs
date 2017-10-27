using eth.Eve.PluginSystem;
using eth.Eve.Storage.Model;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace eth.Eve.Internal
{
    internal class EveBotSpace : IDisposable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private volatile bool _shutdown;

        private readonly long _spaceId;
        private List<IPlugin> _plugins;

        private readonly BotUpdatePoller _updater;
        private readonly TelegramBotApi _outgoingApi;
        private readonly Thread _mainThread;

        public EveBotSpace(EveSpace space, List<IPlugin> plugins)
        {
            _spaceId = space.Id;
            _plugins = plugins;
            
            _updater = new BotUpdatePoller(space.BotApiAccessToken);
            _outgoingApi = new TelegramBotApi(space.BotApiAccessToken);
            _mainThread = new Thread(UpdateProc);
        }

        public void Start()
        {
            foreach (var plugin in _plugins)
                plugin.Initialize(new PluginContext(plugin.Info, _spaceId, _outgoingApi));

            foreach (var plugin in _plugins)
                plugin.Initialized();
            
            _mainThread.Start();
        }

        public void Stop()
        {
            Dispose();
        }

        private async void UpdateProc()
        {
            List<Update> updates;

            try
            {
                updates = await _updater.PollInitialUpdates();

                HandleOld(updates);
            }
            catch (Exception ex)
            {
                //so what?
            }

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

        private void HandleOld(List<Update> updates)
        {
            Debug.Assert(updates != null);

            Debug.WriteLine($"{_spaceId}: {updates.Count} message(s)");

            foreach (var update in updates)
            {
                var message = new UpdateContext { IsInitiallyPolled = true, Update = update };

                foreach (var plugin in _plugins)
                {
                    var result = plugin.Handle(message);

                    if (result == HandleResult.HandledCompletely)
                        break;
                }
            }
        }

        private void HandleNew(Update update)
        {
            Debug.Assert(update != null);
            
            var message = new UpdateContext { Update = update };

            foreach (var plugin in _plugins)
            {
                var result = plugin.Handle(message);

                if (result == HandleResult.HandledCompletely)
                    break;
            }
        }

        public void Dispose()
        {
            _shutdown = true;

            _updater.Dispose();
            _outgoingApi.Dispose();

            foreach (var plugin in _plugins)
                plugin.Teardown();
        }
    }
}
