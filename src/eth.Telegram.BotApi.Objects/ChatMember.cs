using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace eth.Telegram.BotApi.Objects
{
    public class ChatMember
    {
        [JsonProperty("user", Required = Required.Always)]
        public User User { get; set; }

        [JsonProperty("status", ItemConverterType = typeof(StringEnumConverter), Required = Required.Always)]
        public ChatMemberStatus Status { get; set; }
    }
}
