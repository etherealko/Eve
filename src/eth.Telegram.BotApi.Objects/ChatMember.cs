using eth.Telegram.BotApi.Objects.Enums;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object contains information about one member of a chat.
    /// </summary>
    public class ChatMember
    {
        /// <summary>
        /// Information about the user
        /// </summary>
        [JsonProperty("user", Required = Required.Always)]
        public User User { get; set; }

        /// <summary>
        /// The member's status in the chat. Can be “creator”, “administrator”, “member”, “restricted”, “left” or “kicked”
        /// </summary>
        [JsonProperty("status", Required = Required.Always)]
        public ChatMemberStatus Status { get; set; }

        /// <summary>
        /// Optional. Restictred and kicked only. Date when restrictions will be lifted for this user, unix time
        /// </summary>
        [JsonProperty("until_date", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? UntilDate { get; set; }

        /// <summary>
        /// Optional. Administrators only. True, if the bot is allowed to edit administrator privileges of that user
        /// </summary>
        [JsonProperty("can_be_edited", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanBeEdited { get; set; }

        /// <summary>
        /// Optional. Administrators only. True, if the administrator can change the chat title, photo and other settings
        /// </summary>
        [JsonProperty("can_change_info", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanChangeInfo { get; set; }

        /// <summary>
        /// Optional. Administrators only. True, if the administrator can post in the channel, channels only
        /// </summary>
        [JsonProperty("can_post_messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanPostMessages { get; set; }

        /// <summary>
        /// Optional. Administrators only. True, if the administrator can edit messages of other users, channels only
        /// </summary>
        [JsonProperty("can_edit_messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanEditMessages { get; set; }

        /// <summary>
        /// Optional. Administrators only. True, if the administrator can delete messages of other users
        /// </summary>
        [JsonProperty("can_delete_messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanDeleteMessages { get; set; }

        /// <summary>
        /// Optional. Administrators only. True, if the administrator can invite new users to the chat
        /// </summary>
        [JsonProperty("can_invite_users", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanInviteUsers { get; set; }

        /// <summary>
        /// Optional. Administrators only. True, if the administrator can restrict, ban or unban chat members
        /// </summary>
        [JsonProperty("can_restrict_members", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanRestrictMembers { get; set; }

        /// <summary>
        /// Optional. Administrators only. True, if the administrator can pin messages, supergroups only
        /// </summary>
        [JsonProperty("can_pin_messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanPinMessages { get; set; }

        /// <summary>
        ///  	Optional. Administrators only. True, if the administrator can add new administrators with a subset of his own privileges or demote administrators that he has promoted, directly or indirectly (promoted by administrators that were appointed by the user)
        /// </summary>
        [JsonProperty("can_promote_members", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanPromoteMembers { get; set; }

        /// <summary>
        /// Optional. Restricted only. True, if the user can send text messages, contacts, locations and venues
        /// </summary>
        [JsonProperty("can_send_messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanSendMessages { get; set; }

        /// <summary>
        /// Optional. Restricted only. True, if the user can send audios, documents, photos, videos, video notes and voice notes, implies can_send_messages
        /// </summary>
        [JsonProperty("can_send_media_messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanSendMediaMessages { get; set; }

        /// <summary>
        /// Optional. Restricted only. True, if the user can send animations, games, stickers and use inline bots, implies can_send_media_messages
        /// </summary>
        [JsonProperty("can_send_other_messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanSendOtherMessages { get; set; }

        /// <summary>
        /// Optional. Restricted only. True, if user may add web page previews to his messages, implies can_send_media_messages
        /// </summary>
        [JsonProperty("can_add_web_page_previews", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanAddWebPagePreviews { get; set; }
    }
}
