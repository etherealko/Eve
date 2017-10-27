using eth.Common.JetBrains.Annotations;
using System;

namespace eth.Eve.PluginSystem.Storage
{
    public interface IPluginLocalStorage : IDisposable
    {
        bool TryGetString(string key, out PluginStoreString value);
        bool TryGetBinary(string key, out PluginStoreBinary value);

        bool RemoveString(string key);
        bool RemoveBinary(string key);

        void SetString(string key, string value);
        void SetBinary(string key, [NotNull] byte[] value, string metadata = null);

        //todo: introduce 'saveChanges' method
    }
}
