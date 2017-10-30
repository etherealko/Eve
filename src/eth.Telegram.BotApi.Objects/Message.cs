using System.Collections.Generic;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents a message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Unique message identifier
        /// </summary>
        [JsonProperty("message_id", Required = Required.Always)]
        public int MessageId { get; set; }

        /// <summary>
        /// Optional. Sender, empty for messages sent to channels
        /// </summary>
        [JsonProperty("from", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public User From { get; set; }
        
        /// <summary>
        /// Date the message was sent in Unix time
        /// </summary>
        [JsonProperty("date", Required = Required.Always)]
        public int Date { get; set; }
        
        /// <summary>
        /// Conversation the message belongs to — user in case of a private message, GroupChat in case of a group
        /// </summary>
        [JsonProperty("chat", Required = Required.Always)]
        public Chat Chat { get; set; }

        /// <summary>
        /// Optional. For forwarded messages, sender of the original message
        /// </summary>
        [JsonProperty("forward_from", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public User ForwardFrom { get; set; }

        /// <summary>
        /// Optional. For messages forwarded from channels, information about the original channel
        /// </summary>
        [JsonProperty("forward_from_chat", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Chat ForwardFromChat { get; set; }

        /// <summary>
        /// Optional. For messages forwarded from channels, identifier of the original message in the channel
        /// </summary>
        /// 
        [JsonProperty("forward_from_message_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ForwardFromMessageId { get; set; }

        /// <summary>
        /// Optional. For messages forwarded from channels, signature of the post author if present
        /// </summary>
        [JsonProperty("forward_signature", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ForwardSignature { get; set; }

        /// <summary>
        /// Optional. For forwarded messages, date the original message was sent in Unix time
        /// </summary>
        [JsonProperty("forward_date", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ForwardDate { get; set; }
        
        /// <summary>
        /// Optional. For replies, the original message. Note that the Message object in this field will not contain further reply_to_message fields even if it itself is a reply.
        /// </summary>
        [JsonProperty("reply_to_message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message ReplyToMessage { get; set; }

        /// <summary>
        /// Date the message was last edited in Unix time
        /// </summary>
        [JsonProperty("edit_date", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? EditDate { get; set; }

        /// <summary>
        /// Optional. Signature of the post author for messages in channels
        /// </summary>
        [JsonProperty("author_signature", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AuthorSignature { get; set; }

        /// <summary>
        /// Optional. For text messages, the actual UTF-8 text of the message, 0-4096 characters.
        /// </summary>
        [JsonProperty("text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Text { get; set; }

        /// <summary>
        /// Optional. For text messages, special entities like usernames, URLs, bot commands, etc. that appear in the text
        /// </summary>
        [JsonProperty("entities", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<MessageEntity> Entities { get; set; }

        /// <summary>
        /// Optional. For messages with a caption, special entities like usernames, URLs, bot commands, etc. that appear in the caption
        /// </summary>
        [JsonProperty("caption_entities", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<MessageEntity> MessageEntities { get; set; }

        /// <summary>
        /// Optional. Message is an audio file, information about the file
        /// </summary>
        [JsonProperty("audio", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Audio Audio { get; set; }
        
        /// <summary>
        /// Optional. Message is a general file, information about the file
        /// </summary>
        [JsonProperty("document", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Document Document { get; set; }

        /// <summary>
        /// Optional. Message is a game, information about the game.
        /// </summary>
        [JsonProperty("game", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Game Game { get; set; }

        /// <summary>
        /// Optional. Message is a photo, available sizes of the photo
        /// </summary>
        [JsonProperty("photo", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<PhotoSize> Photo { get; set; }
        
        /// <summary>
        /// Optional. Message is a sticker, information about the sticker
        /// </summary>
        [JsonProperty("sticker", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Sticker Sticker { get; set; }
        
        /// <summary>
        /// Optional. Message is a video, information about the video
        /// </summary>
        [JsonProperty("video", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Video Video { get; set; }
        
        /// <summary>
        /// Optional. Message is a voice message, information about the file
        /// </summary>
        [JsonProperty("voice", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Voice Voice { get; set; }

        /// <summary>
        /// Optional. Message is a video note, information about the video message
        /// </summary>
        [JsonProperty("video_note", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public VideoNote VideoNote { get; set; }

        /// <summary>
        /// Optional. Caption for the photo or video, 0-200 characters
        /// </summary>
        [JsonProperty("caption", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Caption { get; set; }

        /// <summary>
        /// Optional. Message is a shared contact, information about the contact
        /// </summary>
        [JsonProperty("contact", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Contact Contact { get; set; }
        
        /// <summary>
        /// Optional. Message is a shared location, information about the location
        /// </summary>
        [JsonProperty("location", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Location Location { get; set; }

        /// <summary>
        /// Optional. Message is a venue, information about the venue
        /// </summary>
        [JsonProperty("venue", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Venue Venue { get; set; }

        /// <summary>
        /// Optional. New members that were added to the group or supergroup and information about them (the bot itself may be one of these members)
        /// </summary>
        [JsonProperty("new_chat_members", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<User> NewChatMembers { get; set; }

        /// <summary>
        /// Optional. A member was removed from the group, information about them (this member may be the bot itself)
        /// </summary>
        [JsonProperty("left_chat_member", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public User LeftChatMember { get; set; }
        
        /// <summary>
        /// Optional. A group title was changed to this value
        /// </summary>
        [JsonProperty("new_chat_title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string NewChatTitle { get; set; }
        
        /// <summary>
        /// Optional. A group title was changed to this value
        /// </summary>
        [JsonProperty("new_chat_photo", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<PhotoSize> NewChatPhoto { get; set; }
        
        /// <summary>
        /// Optional. Informs that the group photo was deleted
        /// </summary>
        [JsonProperty("delete_chat_photo", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DeleteChatPhoto { get; set; }
        
        /// <summary>
        /// Optional. Informs that the group has been created
        /// </summary>
        [JsonProperty("group_chat_created", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? GroupChatCreated { get; set; }

        /// <summary>
        /// Optional. Service message: the supergroup has been created
        /// </summary>
        [JsonProperty("supergroup_chat_created", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? SupergroupChatCreated { get; set; }

        /// <summary>
        /// Optional. Service message: the channel has been created
        /// </summary>
        [JsonProperty("channel_chat_created", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ChannelChatCreated { get; set; }

        /// <summary>
        /// Optional. The group has been migrated to a supergroup with the specified identifier, not exceeding 1e13 by absolute value
        /// </summary>
        [JsonProperty("migrate_to_chat_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? MigrateToChatId { get; set; }

        /// <summary>
        /// Optional. The supergroup has been migrated from a group with the specified identifier, not exceeding 1e13 by absolute value
        /// </summary>
        [JsonProperty("migrate_from_chat_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? MigrateFromChatId { get; set; }

        /// <summary>
        /// Optional. Specified message was pinned. Note that the Message object in this field will not contain further reply_to_message fields even if it is itself a reply.
        /// </summary>
        [JsonProperty("pinned_message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message PinnedMessage { get; set; }

        /// <summary>
        /// Optional. Message is an invoice for a payment, information about the invoice.
        /// </summary>
        [JsonProperty("invoice", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Optional. Message is a service message about a successful payment, information about the payment.
        /// </summary>
        [JsonProperty("successful_payment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SuccessfulPayment SuccessfulPayment { get; set; }
    }
}