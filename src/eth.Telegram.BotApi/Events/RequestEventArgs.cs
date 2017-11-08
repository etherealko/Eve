using eth.Telegram.BotApi.Internal.Serialization;
using System;
using System.Collections.Generic;

namespace eth.Telegram.BotApi.Events
{
    public class RequestEventArgs
    {
        public ApiMethod Method { get; }
        public List<ApiArgument> Arguments { get; }

        public bool MultipartRequired { get; private set; }

        public bool ResponseIsForced { get; private set; }
        public object ForcedResponse { get; private set; }

        protected RequestEventArgs(ApiMethod method, List<ApiArgument> arguments, bool multipartRequired)
        {
            Method = method;
            Arguments = arguments;

            MultipartRequired = multipartRequired;
        }

        public void RequireMultipart()
        {
            MultipartRequired = true;
        }

        public virtual void ForceResponse(object response)
        {
            if (ResponseIsForced)
                throw new InvalidOperationException("response is already forced");

            ResponseIsForced = true;
            ForcedResponse = response;
        }
    }

    internal sealed class RequestEventArgs<TResponse> : RequestEventArgs
    {
        public new TResponse ForcedResponse { get; private set; }

        internal RequestEventArgs(ApiMethod method, ApiArgs args, bool multipartRequired) : base(method, args, multipartRequired) { }

        public override void ForceResponse(object response)
        {
            if (!(response is TResponse))
                throw new ArgumentException($"forced response is not of type {typeof(TResponse)}");

            base.ForceResponse(response);

            ForcedResponse = (TResponse)response;
        }
    }
}