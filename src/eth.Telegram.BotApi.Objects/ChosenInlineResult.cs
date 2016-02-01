using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents a result of an inline query that was chosen by the user and sent to their chat partner. 
    /// </summary>
    public class ChosenInlineResult
    {
        /// <summary>
        /// The unique identifier for the result that was chosen.
        /// </summary>
        [JsonProperty("result_id", Required = Required.Always)]
        public int ResultId { get; set; }

        /// <summary>
        /// The user that chose the result.
        /// </summary>
        [JsonProperty("from", Required = Required.Always)]
        public User From { get; set; }


        /// <summary>
        /// The query that was used to obtain the result.
        /// </summary>
        [JsonProperty("query", Required = Required.Always)]
        public string Query { get; set; }
    }
}