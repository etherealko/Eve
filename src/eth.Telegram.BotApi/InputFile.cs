using System;
using System.IO;

namespace eth.Telegram.BotApi
{
    public class InputFile
    {
        public string FileIdOrUrl { get; }

        public Stream Stream { get; }
        public string FileName { get; }
        
        public InputFile(string fileIdOrUrl)
        {
            if (string.IsNullOrWhiteSpace(fileIdOrUrl))
                throw new ArgumentNullException(nameof(fileIdOrUrl));

            FileIdOrUrl = fileIdOrUrl;
        }

        public InputFile(Stream stream, string fileName)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            Stream = stream;
            FileName = fileName;
        }

        public InputFile(byte[] bytes, string fileName)
            : this(new MemoryStream(bytes ?? throw new ArgumentNullException(nameof(bytes))), fileName) { }

        public static implicit operator InputFile(string fileIdOrUrl)
        {
            return new InputFile(fileIdOrUrl);
        }

        public override string ToString()
        {
            return FileIdOrUrl ?? $"({Stream}, '{FileName}')";
        }
    }
}
