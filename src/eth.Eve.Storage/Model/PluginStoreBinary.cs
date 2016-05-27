using System;

namespace eth.Eve.Storage.Model
{
    public class PluginStoreBinary
    {
        public Guid PluginGuid { get; set; }
        public string PluginVersion { get; set; }

        public string Key { get; set; }
        public string Metadata { get; set; }
        public byte[] Value { get; set; }
    }
}
