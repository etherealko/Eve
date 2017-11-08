using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace eth.Telegram.BotApi.Internal.Serialization
{
    internal class DefaultValueToNullStringEnumConverter : StringEnumConverter
    {
        public DefaultValueToNullStringEnumConverter()
        {
            AllowIntegerValues = false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if ((int)value == 0)
                base.WriteJson(writer, null, serializer);
            else
                base.WriteJson(writer, value, serializer);
        }
    }
}
