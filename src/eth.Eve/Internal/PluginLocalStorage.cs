using eth.Common.JetBrains.Annotations;
using eth.Eve.PluginSystem.Storage;
using eth.Eve.PluginSystem;
using eth.Eve.Storage;
using System.Linq;
using DbModel = eth.Eve.Storage.Model;

namespace eth.Eve.Internal
{
    internal sealed class PluginLocalStorage : IPluginLocalStorage
    {
        private readonly PluginInfo _pluginInfo;
        private readonly EveBotSpace _space;
        private readonly EveDb _dbContext;

        public PluginLocalStorage(PluginInfo pluginInfo, EveBotSpace space)
        {
            _pluginInfo = pluginInfo;
            _space = space;
            _dbContext = space.GetDbContext();
        }

        public bool RemoveBinary(string key)
        {
            var entry = _dbContext.PluginStoreBinaries.SingleOrDefault(b => b.SpaceId == _space.SpaceId &&
                b.PluginGuid == _pluginInfo.Guid &&
                b.Key == key);

            if (entry == null)
                return false;

            _dbContext.PluginStoreBinaries.Remove(entry);
            var count = _dbContext.SaveChanges();

            return count > 0;
        }

        public bool RemoveString(string key)
        {
            var entry = _dbContext.PluginStoreStrings.SingleOrDefault(b => b.SpaceId == _space.SpaceId &&
                b.PluginGuid == _pluginInfo.Guid &&
                b.Key == key);

            if (entry == null)
                return false;

            _dbContext.PluginStoreStrings.Remove(entry);
            var count = _dbContext.SaveChanges();

            return count > 0;
        }

        public void SetBinary(string key, [NotNull] byte[] value, string metadata = null)
        {
            var entry = _dbContext.PluginStoreBinaries.SingleOrDefault(b => b.SpaceId == _space.SpaceId &&
                b.PluginGuid == _pluginInfo.Guid &&
                b.Key == key);

            if (entry != null)
            {
                entry.Value = value;
                entry.Metadata = metadata;
                entry.PluginVersion = _pluginInfo.Version;
            }
            else
            {
                entry = new DbModel.PluginStoreBinary
                {
                    SpaceId = _space.SpaceId,
                    PluginGuid = _pluginInfo.Guid,
                    PluginVersion = _pluginInfo.Version,

                    Key = key,
                    Value = value,
                    Metadata = metadata
                };

                _dbContext.PluginStoreBinaries.Add(entry);
            }

            _dbContext.SaveChanges();
        }

        public void SetString(string key, string value)
        {
            var entry = _dbContext.PluginStoreStrings.SingleOrDefault(b => b.SpaceId == _space.SpaceId &&
                b.PluginGuid == _pluginInfo.Guid &&
                b.Key == key);

            if (entry != null)
            {
                entry.Value = value;
                entry.PluginVersion = _pluginInfo.Version;
            }
            else
            {
                entry = new DbModel.PluginStoreString
                {
                    SpaceId = _space.SpaceId,
                    PluginGuid = _pluginInfo.Guid,
                    PluginVersion = _pluginInfo.Version,

                    Key = key,
                    Value = value,
                };

                _dbContext.PluginStoreStrings.Add(entry);
            }

            _dbContext.SaveChanges();
        }

        public bool TryGetBinary(string key, out PluginStoreBinary value)
        {
            var dbValue = _dbContext.PluginStoreBinaries.SingleOrDefault(b => b.SpaceId == _space.SpaceId && 
                b.PluginGuid == _pluginInfo.Guid && 
                b.Key == key);

            if (dbValue == null)
            {
                value = null;
                return false;
            }

            value = new PluginStoreBinary
            {
                PluginVersion = dbValue.PluginVersion,
                Value = dbValue.Value,
                Metadata = dbValue.Metadata
            };

            return true;
        }

        public bool TryGetString(string key, out PluginStoreString value)
        {
            var dbValue = _dbContext.PluginStoreStrings.SingleOrDefault(b => b.SpaceId == _space.SpaceId &&
                b.PluginGuid == _pluginInfo.Guid &&
                b.Key == key);

            if (dbValue == null)
            {
                value = null;
                return false;
            }

            value = new PluginStoreString
            {
                PluginVersion = dbValue.PluginVersion,
                Value = dbValue.Value
            };

            return true;
        }
        
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
