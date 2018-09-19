using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace eth.Telegram.BotApi.Internal.Serialization
{
    internal class ApiArgsConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ApiArgs);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var apiArgs = (ApiArgs)value;

            if (apiArgs == null)
            {
                writer.WriteToken(JsonToken.Null);
                return;
            }

            var jObject = new JObject();

            foreach (var arg in apiArgs)
                jObject.Add(arg.SerializationArgumentName, JToken.FromObject(arg.Value, serializer));

            jObject.WriteTo(writer);
        }
    }
}
