using System.Collections.Generic;
using System.Threading.Tasks;
using eth.Telegram.BotApi.Objects;

namespace eth.Telegram.BotApi
{
    interface ITelegramBotApi
    {
        Task<List<Update>> GetUpdatesAsync(int offset, int limit, int timeout);

        Task<User> GetMeAsync();

        Task<Message> SendMessageAsync(int chatId, string text);
        Task<Message> SendMessageAsync(string channelusername, string text);

        Task<Message> SendSticker(int chatId, string sticker);
        Task<Message> SendSticker(string channelusername, string sticker);

        //        Use this method to send.webp stickers. On success, the sent Message is returned.
        //Parameters Type    Required Description
        //chat_id Integer or String   Yes Unique identifier for the target chat or username of the target channel (in the format @channelusername)
        //sticker InputFile or String Yes Sticker to send. You can either pass a file_id as String to resend a sticker that is already on the Telegram servers, or upload a new sticker using multipart/form-data.
        //reply_to_message_id Integer     Optional If the message is a reply, ID of the original message
        //reply_markup ReplyKeyboardMarkup or ReplyKeyboardHide or ForceReply  Optional Additional interface options. A JSON-serialized object for a custom reply keyboard, instructions to hide keyboard or to force a reply from the user.
    }
}
