using System.Collections.Generic;

namespace eth.Eve.Storage.Model
{
    public class EveSpace
    {
        public long Id { get; set; }

        public bool IsActive { get; set; }
        public string BotApiAccessToken { get; set; }

        public virtual ICollection<PluginStoreString> PluginStoreStrings { get; set; }
        public virtual ICollection<PluginStoreBinary> PluginStoreBinaries { get; set; }
    }
}
