using System.Collections.Generic;

namespace eth.Telegram.BotApi.Internal.Serialization
{
    internal partial class ApiArgs : List<ApiArgument>
    {
        public void Add<T>(string serializationArgumentName, T value, string argumentName, ApiArgumentRequired required = ApiArgumentRequired.Yes)
        {
            if (required == ApiArgumentRequired.Optional && Equals(value, default(T)))
                return;

            base.Add(new ApiArgument(argumentName, serializationArgumentName, value));
        }
    }
}
