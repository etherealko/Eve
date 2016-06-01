using eth.Common.JetBrains.Annotations;

namespace eth.Eve.PluginSystem.Storage
{
    public class PluginStoreBinary
    {
        public string PluginVersion { get; set; }

        public string Metadata { get; set; }

        [NotNull]
        public byte[] Value { get; set; }
    }
}
