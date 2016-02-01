using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents a point on the map.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Longitude as defined by sender
        /// </summary>
        [JsonProperty("longitude", Required = Required.Always)]
        public double Longitude { get; set; }
        
        /// <summary>
        /// Latitude as defined by sender
        /// </summary>
        [JsonProperty("latitude", Required = Required.Always)]
        public double Latitude { get; set; }
    }
}