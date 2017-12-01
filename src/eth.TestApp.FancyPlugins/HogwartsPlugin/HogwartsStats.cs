using Newtonsoft.Json;

namespace eth.TestApp.FancyPlugins.HogwartsPlugin
{
    public class HogwartsStats
    {
        [JsonProperty("snitchSuccess", Required = Required.Always)]
        public int SnitchSuccess { get; set; }
        [JsonProperty("snitchTries", Required = Required.Always)]
        public int SnitchTries { get; set; }
        [JsonProperty("bludgerSuccess", Required = Required.Always)]
        public int BludgerSuccess { get; set; }
        [JsonProperty("bludgerTries", Required = Required.Always)]
        public int BludgerTries { get; set; }
        [JsonProperty("dodgeSuccess", Required = Required.Always)]
        public int DodgeSuccess { get; set; }
        [JsonProperty("dodgeTries", Required = Required.Always)]
        public int DodgeTries { get; set; }
    }
}
