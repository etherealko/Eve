using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents a group chat.
    /// </summary>
    public class Chat
    {
        /// <summary>
        /// Unique identifier for this chat, not exceeding 1e13 by absolute value
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public long Id { get; set; }
        
        /// <summary>
        /// Type of chat, can be either “private”, “group”, “supergroup” or “channel”
        /// </summary>
        [JsonProperty("type", ItemConverterType = typeof(StringEnumConverter), DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ChatType Type { get; set; }

        /// <summary>
        /// Optional. Title, for channels and group chats
        /// </summary>
        [JsonProperty("title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. First name of the other party in a private chat
        /// </summary>
        [JsonProperty("first_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>
        /// Optional. Last name of the other party in a private chat
        /// </summary>
        [JsonProperty("last_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>
        /// Optional. Username, for private chats and channels if available
        /// </summary>
        [JsonProperty("username", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Username { get; set; }
    }
}