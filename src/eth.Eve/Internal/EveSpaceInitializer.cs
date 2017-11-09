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

        public Queue<IRequestInterceptor> RequestInterceptors { get; } = new Queue<IRequestInterceptor>();

        public Queue<IResponseInterceptor> ResponseInterceptors { get; } = new Queue<IResponseInterceptor>();

        public EveSpaceInitializer(EveSpace space)
        {
            EveSpace = space;
            SpaceId = space.Id;
        }
    }
}
