using System.Collections.Generic;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represent a user's profile pictures.
    /// </summary>
    public class UserProfilePhotos
    {
        /// <summary>
        /// Total number of profile pictures the target user has
        /// </summary>
        [JsonProperty("total_count", Required = Required.Always)]
        public int TotalCount { get; set; }
        
        /// <summary>
        /// Requested profile pictures (in up to 4 sizes each)
        /// </summary>
        [JsonProperty("photos", Required = Required.Always)]
        public List<PhotoSize> Photos { get; set; }
    }
}