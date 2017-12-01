﻿using Newtonsoft.Json;

namespace eth.TestApp.FancyPlugins.HogwartsPlugin
{
    public class HogwartsMember
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty("patronus", Required = Required.Always)]
        public HogwartsPatronus Patronus { get; set; }
        [JsonProperty("isAdmin", Required = Required.Always)]
        public bool IsAdmin { get; set; }
        [JsonProperty("stats", Required = Required.Always)]
        public HogwartsStats Stats { get; set; }
    }
}
