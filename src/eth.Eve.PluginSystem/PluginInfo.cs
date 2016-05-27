using System;

namespace eth.Eve.PluginSystem
{
    public class PluginInfo
    {
        public Guid Guid { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Version { get; private set; }

        public PluginInfo(Guid guid, string name, string description, string version)
        {
            Guid = guid;
            Name = name;
            Description = description;
            Version = version;
        }
    }
}
