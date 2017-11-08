namespace eth.Telegram.BotApi
{
    public class ApiArgument
    {
        public string ArgumentName { get; }
        public string SerializationArgumentName { get; }

        public object Value { get; set; }

        public ApiArgument(string argumentName, string serializationArgumentName, object value)
        {
            ArgumentName = argumentName;
            SerializationArgumentName = serializationArgumentName;
            Value = value;
        }
    }
}
