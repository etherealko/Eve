﻿using Newtonsoft.Json;
using eth.Telegram.BotApi.Objects.Base;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will hide the current custom keyboard and display the default letter-keyboard.
    /// By default, custom keyboards are displayed until a new keyboard is sent by a bot. An exception is made for one-time keyboards that are hidden immediately after the user presses a button (see ReplyKeyboardMarkup).
    /// </summary>
    public class ReplyKeyboardHide : KeyboardMarkupReply
    {
        /// <summary>
        /// Requests clients to hide the custom keyboard
        /// </summary>
        [JsonProperty("hide_keyboard", Required = Required.Always)]
        public bool HideKeyboard { get; set; } = true;

        /// <summary>
        /// Optional. Use this parameter if you want to hide keyboard for specific users only. Targets: 1) users that are @mentioned in the text of the Message object; 2) if the bot's message is a reply (has reply_to_message_id), sender of the original message.
        /// <example>
        /// Example: A user votes in a poll, bot returns confirmation message in reply to the vote and hides keyboard for that user, while still showing the keyboard with poll options to users who haven't voted yet.
        /// </example>
        /// </summary>
        [JsonProperty("selective", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Selective { get; set; }
    }
}