using System;

namespace eth.Eve.Storage.Model
{
    public class PluginStoreString
    {
        public long SpaceId { get; set; }

        public Guid PluginGuid { get; set; }
        public string PluginVersion { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
