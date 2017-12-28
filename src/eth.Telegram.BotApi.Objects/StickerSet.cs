using Newtonsoft.Json;
using System.Collections.Generic;

namespace eth.Telegram.BotApi.Objects
{
    /// <summary>
    /// This object represents a sticker set.
    /// </summary>
    public class StickerSet
    {
        /// <summary>
        /// Sticker set name
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Sticker set title
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// True, if the sticker set contains masks
        /// </summary>
        [JsonProperty("contains_masks", Required = Required.Always)]
        public bool ContainsMasks { get; set; }

        /// <summary>
        /// List of all set stickers
        /// </summary>
        [JsonProperty("stickers", Required = Required.Always)]
        public List<Sticker> Stickers { get; set; }
    }
}