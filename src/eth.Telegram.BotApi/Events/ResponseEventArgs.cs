using eth.Telegram.BotApi.Internal.Serialization;
using System;
using System.Collections.Generic;

namespace eth.Telegram.BotApi.Events
{
    public class ResponseEventArgs
    {
        public object ApiOwner { get; }

        public ApiMethod Method { get; }
        public IReadOnlyCollection<ApiArgument> Arguments { get; }

        public object Response { get; }
        public Exception Exception { get; }

        internal ResponseEventArgs(ApiMethod method, ApiArgs args, object response, Exception exception, object apiOwner)
        {
            ApiOwner = apiOwner;

            Method = method;
            Arguments = args;

            Response = response;
            Exception = exception;
        }
    }
}