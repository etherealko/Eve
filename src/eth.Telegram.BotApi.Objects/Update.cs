using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents an incoming update.
    /// At most one of the optional parameters can be present in any given update.
    /// </summary>
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
        /// Optional. New version of a message that is known to the bot and was edited
        /// </summary>
        [JsonProperty("channel_post", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message EditedMessage { get; set; }

        /// <summary>
        /// Optional. New version of a message that is known to the bot and was edited
        /// </summary>
        [JsonProperty("edited_channel_post", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message ChannelPost { get; set; }

        /// <summary>
        /// Optional. New version of a message that is known to the bot and was edited
        /// </summary>
        [JsonProperty("edited_message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message EditedChannelPost { get; set; }

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

        /// <summary>
        /// Optional. New incoming callback query
        /// </summary>
        [JsonProperty("callback_query", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CallbackQuery CallbackQuery { get; set; }

        /// <summary>
        /// Optional. New incoming shipping query. Only for invoices with flexible price
        /// </summary>
        [JsonProperty("shipping_query", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ShippingQuery ShippingQuery { get; set; }

        /// <summary>
        /// Optional. New incoming pre-checkout query. Contains full information about checkout
        /// </summary>
        [JsonProperty("pre_checkout_query", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PreCheckoutQuery PreCheckoutQuery { get; set; }
    }
}