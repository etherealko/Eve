using eth.Common.Extensions;
using eth.Eve.PluginSystem;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace eth.Eve.Internal
{
    internal class EveBotSpace : IDisposable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private volatile bool _shutdown;

        private readonly List<PluginContext> _pluginContexts;
        private readonly List<IRequestInterceptor> _requestInterceptors;
        private readonly List<IResponseInterceptor> _responseInterceptors;
        private readonly List<IHealthListener> _healthListeners;

        private readonly BotUpdatePoller _updater;
        private readonly Thread _mainThread;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly TelegramBotApi _outgoingApi;

        private volatile User _me;
        
        public long SpaceId { get; }
        public TaskFactory TaskFactory { get; }

        public EveBotSpace(EveSpaceInitializer initializer)
        {
            SpaceId = initializer.EveSpace.Id;

            _requestInterceptors = initializer.RequestInterceptors.ToList();
            _responseInterceptors = initializer.ResponseInterceptors.ToList();
            _healthListeners = initializer.HealthListeners.ToList();

            TaskFactory = new TaskFactory(_cts.Token, 
                TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning, 
                TaskContinuationOptions.None, 
                TaskScheduler.Default);

            _outgoingApi = new TelegramBotApi(initializer.EveSpace.BotApiAccessToken, null) { HttpClientTimeout = TimeSpan.FromSeconds(30) };
            _updater = new BotUpdatePoller(_outgoingApi);
            _mainThread = new Thread(UpdateProc);
            
            _pluginContexts = initializer.Plugins.Select(p =>
            {
                var api = new TelegramBotApi(initializer.EveSpace.BotApiAccessToken, p) { HttpClientTimeout = TimeSpan.FromSeconds(10) };

                api.Request += (o, e) =>
                {
                    foreach (var interceptor in _requestInterceptors)
                    {
                        interceptor.OnRequest(e);

                        if (e.ResponseIsForced)
                            break;
                    }
                };

                api.Response += (o, e) =>
                {
                    foreach (var interceptor in _responseInterceptors)
                        interceptor.OnResponse(e);
                };

                return new PluginContext(p, this, api);
            }).ToList();
        }

        public void Start()
        {
            try
            {
                _me = _outgoingApi.GetMeAsync().Result;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1)
                    ex.ThrowInner();

                throw;
            }

            foreach (var pluginContext in _pluginContexts)            
                pluginContext.Plugin.Initialize(pluginContext);
            
            foreach (var pluginContext in _pluginContexts)
                pluginContext.Plugin.Initialized();
            
            _mainThread.Start();
        }

        public void Stop()
        {
            Dispose();
        }

        public async Task<User> GetMeAsync(bool forceServerQuery = false)
        {
            var me = _me;

            if (me == null || forceServerQuery)
                return _me = await _outgoingApi.GetMeAsync();

            return me;
        }

        private async void UpdateProc()
        {
            List<Update> updates;

            var retryCount = 0;
            var isDisconnected = true;
            var lastPolled = DateTime.MinValue;
            
            while (!_shutdown)
            {
                try
                {
                    updates = await (isDisconnected ? _updater.PollInitialUpdates() : _updater.PollUpdates());

                    retryCount = 0;
                    lastPolled = DateTime.Now;
                }
                catch (TaskCanceledException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    switch (retryCount)
                    {
                        case 0:
                            Thread.Sleep(100);
                            break;
                        case 1:
                            Thread.Sleep(500);
                            break;
                        case 2:
                            Thread.Sleep(1500);
                            break;
                        default:
                            Thread.Sleep(3000);
                            break;
                    }

                    if (DateTime.Now - lastPolled > TimeSpan.FromSeconds(30))
                        isDisconnected = true;

                    ++retryCount;
                    continue;
                }

                try
                {
                    HandleUpdates(updates, isDisconnected);
                }
                catch (Exception ex)
                {
                    Log.Warn(ex, "very very very bad ");
                }

                isDisconnected = false;
            }

            Debug.WriteLine("Update thread finished");
        }

        private void HandleUpdates(List<Update> updates, bool isInitial)
        {
            Debug.Assert(updates != null);

            foreach (var update in updates)
            {
                var message = new UpdateContext { IsInitiallyPolled = isInitial, Update = update };

                foreach (var pluginContext in _pluginContexts)
                    try
                    {
                        message.CurrentContext = pluginContext;

                        var result = pluginContext.Plugin.Handle(message);

                        if (result == HandleResult.HandledCompletely)
                            break;
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(ex, "message handling: plugin has thrown an exception (initial poll) ");

                        foreach (var listener in _healthListeners)
                            listener.OnHandleMessageException(ex, message, pluginContext.Plugin);
                    }
            }
        }

        public void Dispose()
        {
            _shutdown = true;

            _updater.Dispose();
            _outgoingApi.Dispose();

            foreach (var pluginContext in _pluginContexts)
            {
                try //i should not be writing this
                {
                    pluginContext.Dispose();
                }
                catch (Exception ex)
                {

                }
            }

            _cts.Cancel();
        }
    }
}
