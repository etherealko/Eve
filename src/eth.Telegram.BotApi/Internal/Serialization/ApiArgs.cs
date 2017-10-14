using System.Collections.Generic;

namespace eth.Telegram.BotApi.Internal.Serialization
{
    internal class ApiArgs : Dictionary<string, object>
    {
        // maybe "isOptional" enum will be better?
        public void Add<T>(string parameterName, T value, bool isOptional = false)
        {
            if (isOptional && Equals(value, default(T)))
                return;

            base.Add(parameterName, value);
        }
    }
}
