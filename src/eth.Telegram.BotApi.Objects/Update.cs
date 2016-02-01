using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    public class Update
    {
        /// <summary>
        /// The update‘s unique identifier. Update identifiers start from a certain positive number and increase sequentially. 
        /// This ID becomes especially handy if you’re using Webhooks, since it allows you to ignore repeated updates or to restore the correct update sequence, should they get out of order.
        /// </summary>
        [JsonProperty("update_id", Required = Required.Always)]
        public int UpdateId { get; set; }

        /// <summary>
        /// Optional. New incoming message of any kind — text, photo, sticker, etc.
        /// </summary>
        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message Message { get; set; }

        /// <summary>
        /// Optional. New incoming inline query
        /// </summary>
        [JsonProperty("inline_query", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineQuery InlineQuery { get; set; }

        /// <summary>
        /// Optional. The result of an inline query that was chosen by a user and sent to their chat partner.
        /// </summary>
        [JsonProperty("chosen_inline_result", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ChosenInlineResult ChosenInlineResult { get; set; }
    }
}