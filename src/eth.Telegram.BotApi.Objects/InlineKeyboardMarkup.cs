using eth.Telegram.BotApi.Objects.Base;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents an inline keyboard that appears right next to the message it belongs to.
    /// </summary>
    public class InlineKeyboardMarkup : KeyboardMarkupReply
    {
        /// <summary>
        /// Array of button rows, each represented by an Array of InlineKeyboardButton objects
        /// </summary>
        [JsonProperty("inline_keyboard", Required = Required.Always)]
        public List<List<InlineKeyboardButton>> InlineKeyboard { get; set; }
    }
}
