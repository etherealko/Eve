using Newtonsoft.Json;
using System;

namespace eth.Telegram.BotApi.Internal.Serialization
{
    internal class InputFileConverter : JsonConverter
    {
        public override bool CanRead { get { return false; } }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(InputFile);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var inputFile = (InputFile)value;

            if (inputFile == null)
                writer.WriteToken(JsonToken.Null);
            
            writer.WriteToken(JsonToken.String, inputFile.ToString());
        }
    }
}
