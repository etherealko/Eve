using System;

namespace eth.Telegram.BotApi
{
    public class InputFile
    {
        //todo: from file constructor for post upload

        public string FileIdOrUrl { get; }

        public InputFile(string fileIdOrUrl)
        {
            if (string.IsNullOrWhiteSpace(fileIdOrUrl))
                throw new ArgumentNullException(nameof(fileIdOrUrl));

            FileIdOrUrl = fileIdOrUrl;
        }
        
        public static implicit operator InputFile(string fileIdOrUrl)
        {
            return new InputFile(fileIdOrUrl);
        }

        public override string ToString()
        {
            return FileIdOrUrl;
        }
    }
}
