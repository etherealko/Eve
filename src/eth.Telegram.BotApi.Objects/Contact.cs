using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents a phone contact.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Contact's phone number
        /// </summary>
        [JsonProperty("phone_number", Required = Required.Always)]
        public string Username { get; set; }

        /// <summary>
        /// Contact's first name
        /// </summary>
        [JsonProperty("first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        /// <summary>
        /// Optional. Contact's last name
        /// </summary>
        [JsonProperty("last_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>
        /// Optional. Contact's user identifier in Telegram
        /// </summary>
        [JsonProperty("user_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int UserId { get; set; }
    }
}
