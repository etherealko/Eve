using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents one button of the reply keyboard. Optional fields are mutually exclusive.
    /// </summary>
    public class KeyboardButton
    {
        /// <summary>
        /// Label text on the button
        /// </summary>
        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }

        /// <summary>
        /// Optional. If True, the user's phone number will be sent as a contact when the button is pressed. Available in private chats only
        /// </summary>
        [JsonProperty("request_contact", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool RequestContact { get; set; }

        /// <summary>
        /// Optional. If True, the user's current location will be sent when the button is pressed. Available in private chats only
        /// </summary>
        [JsonProperty("request_location", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool RequestLocation { get; set; }
    }
}