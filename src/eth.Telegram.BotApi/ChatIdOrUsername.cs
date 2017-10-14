using System;

namespace eth.Telegram.BotApi
{
    public class ChatIdOrUsername
    {
        public long Id { get; }
        public string Username { get; }

        public ChatIdOrUsername(long id)
        {
            Id = id;
        }

        public ChatIdOrUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            Username = username;
        }
        
        public static implicit operator ChatIdOrUsername(long id)
        {
            return new ChatIdOrUsername(id);
        }

        public static implicit operator ChatIdOrUsername(string username)
        {
            return new ChatIdOrUsername(username);
        }

        public override string ToString()
        {
            return Username ?? Id.ToString();
        }
    }
}
