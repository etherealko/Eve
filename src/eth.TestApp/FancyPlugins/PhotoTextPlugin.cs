using eth.PluginSamples;
using System;
using System.Linq;
using System.Threading.Tasks;
using eth.Eve.PluginSystem;
using eth.Telegram.BotApi;
using Newtonsoft.Json;
using System.IO;
using eth.Telegram.BotApi.Objects.Enums;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WSize = System.Windows.Size;
using System.Threading;
using System.Collections.Generic;
using eth.Telegram.BotApi.Objects;

#pragma warning disable CS4014 //missing await

namespace eth.TestApp.FancyPlugins
{
    public class PhotoTextPlugin : PluginBase
    {
        public override PluginInfo Info { get; } = new PluginInfo(new Guid("9B8117B8-D0FA-4655-86A1-D3652905B97B"),
                                                         "PhotoTextPlugin",
                                                         "Brand new Eve plugin",
                                                         "0.0.0.1");
        
        public override HandleResult Handle(IUpdateContext c)
        {
            if (c.IsInitiallyPolled)
                return HandleResult.Ignored;

            var update = c.Update;

            if (update.Message == null)
                return HandleResult.Ignored;
            
            List<PhotoSize> photos;
            string text;

            if (update.Message.Photo != null)
            {
                photos = update.Message.Photo;
                text = update.Message.Caption;
            }
            else if (update.Message.ReplyToMessage?.Photo != null)
            {
                photos = update.Message.ReplyToMessage.Photo;
                text = update.Message.Text;
            }
            else
                return HandleResult.Ignored;

            if (photos.Count == 0 || 
                text == null ||
                text.Length < 4 ||
                text.Substring(0, 4) != "echo")
                return HandleResult.Ignored;
            
            var photo = (from p in photos
                         where p.FileSize < 20_000_000
                         orderby p.Height * p.Width descending
                         select p).FirstOrDefault();

            if (photo == null)
                return HandleResult.Ignored;

            Task.Factory.StartNew(async () =>
            {
                text = text.Substring(4);

                if (text.Length < 2)
                    text = "ахахахахахахах)))" + Environment.NewLine +
                        "ОРУ" + Environment.NewLine +
                        "ОРУНЬКАЮ" + Environment.NewLine +
                        "ОРУЛИРУЮ))";
                else
                    text = text.Substring(1);

                _ctx.BotApi.SendChatActionAsync(update.Message.Chat.Id, ChatAction.UploadingPhoto);

                var fileInfo = await _ctx.BotApi.GetFileInfoAsync(photo.FileId);
                var fileBytes = await _ctx.BotApi.GetFileBytesAsync(fileInfo.FilePath);

                var processedBytes = ProcessPhoto(fileBytes, text);

                _ctx.BotApi.SendPhotoAsync(chatId: update.Message.Chat.Id,
                    photo: new InputFile(processedBytes, Path.GetFileName(fileInfo.FilePath)),
                    replyToMessageId: update.Message.MessageId);
            }, TaskCreationOptions.LongRunning);

            return HandleResult.HandledCompletely;
        }

        private byte[] ProcessPhoto(byte[] input, string text)
        {
            using (var ms = new MemoryStream())
            {
                var staThread = new Thread(() =>
                {
                    var renderControl = new PhotoTextRenderControl(input, text);

                    var width = renderControl.PhotoBitmap.Width;
                    var height = renderControl.PhotoBitmap.Height;

                    var size = new WSize(width, height);

                    renderControl.Measure(size);
                    renderControl.Arrange(new Rect(size));

                    RenderTargetBitmap bmp = new RenderTargetBitmap(
                        renderControl.PhotoBitmap.PixelWidth, 
                        renderControl.PhotoBitmap.PixelHeight, 
                        renderControl.PhotoBitmap.DpiX, 
                        renderControl.PhotoBitmap.DpiY, PixelFormats.Pbgra32);

                    bmp.Render(renderControl);

                    var encoder = new JpegBitmapEncoder { QualityLevel = 90 };

                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    
                    encoder.Save(ms);
                }) { IsBackground = true };

                staThread.SetApartmentState(ApartmentState.STA);

                staThread.Start();
                staThread.Join();

                return ms.ToArray();
            }
        }
    }
}
