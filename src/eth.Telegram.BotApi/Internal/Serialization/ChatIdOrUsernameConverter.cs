using Newtonsoft.Json;
using System;

namespace eth.Telegram.BotApi.Internal.Serialization
{
    internal class ChatIdOrUsernameConverter : JsonConverter
    {
        public override bool CanRead { get { return false; } }
        
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ChatIdOrUsername);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var chatId = (ChatIdOrUsername)value;

            if (chatId == null)
                writer.WriteToken(JsonToken.Null);

            if (chatId.Username != null)
                writer.WriteToken(JsonToken.String, chatId.Username);
            else
                writer.WriteToken(JsonToken.Integer, chatId.Id);
        }
    }
}
