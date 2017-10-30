using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents a venue.
    /// </summary>
    public class Venue
    {
        /// <summary>
        /// Venue location
        /// </summary>
        [JsonProperty("location", Required = Required.Always)]
        public Location Location { get; set; }

        /// <summary>
        /// Name of the venue
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Address of the venue
        /// </summary>
        [JsonProperty("address", Required = Required.Always)]
        public string Address { get; set; }

        /// <summary>
        /// Optional. Foursquare identifier of the venue
        /// </summary>
        [JsonProperty("foursquare_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FoursquareId { get; set; }
    }
}