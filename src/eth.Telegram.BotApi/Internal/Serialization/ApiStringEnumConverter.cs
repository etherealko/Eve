using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Internal.Serialization
{
    internal class ApiStringEnumConverter : StringEnumConverter
    {
        public bool AllowUnknownEnumValues { get; set; }

        public ApiStringEnumConverter()
        {
            AllowIntegerValues = false;
            AllowUnknownEnumValues = true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if ((int)value == 0)
                base.WriteJson(writer, null, serializer);
            else
                base.WriteJson(writer, value, serializer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (JsonSerializationException ex)
            {
                if (!AllowUnknownEnumValues)
                    throw;

                HttpApiClient.Log.Warn(ex, "Enum deserialization failed, most likely a new enum value was introduced.");
                return existingValue;
            }
        }
    }
}
