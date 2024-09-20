using RegistrationTelegramBot.BL.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class SendMessageToUserCommand : Command, ICommand, IListener
    {
        public override List<string> Name => new List<string> { "Написать пользователю ✍️", "Ответить на сообщение" };

        private string reciever;

        public CommandExecutor Executor { get; }

        public SendMessageToUserCommand(Bot bot, CommandExecutor executor) : base(bot)
        {
            Executor = executor;
        }

        public async override Task Execute(Update update)
        {
            long chatId = update.Message == null ? update.CallbackQuery.Message.Chat.Id : update.Message.Chat.Id;

            if (!Bot.IsAdmin(chatId.ToString()))
            {
                await Client.SendTextMessageAsync(chatId, "У вас нет доступа");
                return;
            }
            if (update.Message == null)
            {
                //foreach (var item in update.CallbackQuery.Message.ReplyMarkup.InlineKeyboard)
                //{
                //    foreach (var itemInner in item)
                //    {
                //        if (!string.IsNullOrEmpty(itemInner.Text))
                //        {
                //            reciever = itemInner.Text;
                //        }
                //    }
                //}
                reciever = update.CallbackQuery.Data;
            }
            Executor.StartListen(this); //говорим, что теперь нам надо отправлять апдейты
            string messageTxt = string.IsNullOrEmpty(reciever) ? "Кому ID (для отмены нажмите /exit)" : "Введите сообщение (для отмены нажмите /exit)";
            await Client.SendTextMessageAsync(Bot.GetMainAdmin(), messageTxt);

        }

        public async Task GetUpdate(Update update)
        {
            long chatId = update.Message.Chat.Id;
            Executor.StopListen();
            if (update.Message.Text != null && update.Message.Text == "/exit") //Проверочка{
            {
                reciever = null;
                return;
            }
            if (string.IsNullOrEmpty(reciever))
            {
                reciever = update.Message.Text;
                await Client.SendTextMessageAsync(chatId, "Введите сообщение (для отмены нажмите /exit)");
                Executor.StartListen(this);
                return;
            }

            switch (update.Message.Type)
            {
                case Telegram.Bot.Types.Enums.MessageType.Text:
                    await Client.SendTextMessageAsync(reciever, update.Message.Text);
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Sticker:
                    await Client.SendStickerAsync(reciever, sticker: new InputFileId(update.Message.Sticker.FileId));
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Photo:

                    await Client.SendPhotoAsync(reciever, new InputFileId(update.Message.Photo.Last().FileId));
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Video:
                    await Client.SendVideoAsync(reciever, new InputFileId(update.Message.Video.FileId));
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Voice:
                    await Client.SendVoiceAsync(reciever, new InputFileId(update.Message.Voice.FileId));
                    break;
            }
            reciever = null;

        }
    }
}
