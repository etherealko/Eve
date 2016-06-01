using System;
using eth.Common.JetBrains.Annotations;
using eth.Eve.PluginSystem.Storage;
using eth.Eve.PluginSystem;
using eth.Eve.Storage;

namespace eth.Eve.Internal
{
    internal sealed class PluginLocalStorage : IPluginLocalStorage
    {
        private readonly PluginInfo _pluginInfo;
        private readonly EveDb _dbContext;

        public PluginLocalStorage(PluginInfo pluginInfo)
        {
            _pluginInfo = pluginInfo;
            _dbContext = new EveDb();
        }

        public bool RemoveBinary(string key)
        {
            throw new NotImplementedException();
        }

        public bool RemoveString(string key)
        {
            throw new NotImplementedException();
        }

        public void SetBinary(string key, [NotNull] byte[] value, string metaData = null)
        {
            throw new NotImplementedException();
        }

        public void SetString(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetBinary(string key, out PluginStoreBinary value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetString(string key, out PluginStoreString value)
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
