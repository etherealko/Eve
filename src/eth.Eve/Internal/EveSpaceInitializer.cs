using System.Collections.Generic;
using eth.Eve.PluginSystem;
using eth.Eve.Storage.Model;

namespace eth.Eve.Internal
{
    internal class EveSpaceInitializer : IEveSpaceInitializer
    {
        public EveSpace EveSpace { get; }

        public long SpaceId { get; }

        public Queue<IPlugin> Plugins { get; } = new Queue<IPlugin>();

        public EveSpaceInitializer(EveSpace space)
        {
            EveSpace = space;
            SpaceId = space.Id;
        }
    }
}
