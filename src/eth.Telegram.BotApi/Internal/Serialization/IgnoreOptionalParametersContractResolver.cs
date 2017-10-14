using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace eth.Telegram.BotApi.Internal.Serialization
{
    internal class IgnoreOptionalParametersContractResolver : DefaultContractResolver
    {
        private readonly Type _type;
        private readonly HashSet<string> _ignoredProperties;

        public IgnoreOptionalParametersContractResolver(Type type, string[] ignoredProperties)
        {
            _type = type;
            _ignoredProperties = new HashSet<string>(ignoredProperties, StringComparer.OrdinalIgnoreCase);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == _type && _ignoredProperties.Contains(property.PropertyName))
                property.Ignored = true;

            return property;
        }
    }
}

