using RegistrationTelegramBot.BL;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class SendMessageToAdminCommand : Command, ICommand, IListener
    {
        public override List<string> Name => new List<string> { "Написать админу 🤡" };


        public CommandExecutor Executor { get; }

        public SendMessageToAdminCommand(Bot bot, CommandExecutor executor) : base(bot)
        {
            Executor = executor;
        }


        public async override Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            Executor.StartListen(this); //говорим, что теперь нам надо отправлять апдейты
            await Client.SendTextMessageAsync(chatId, "Введите сообщение (для отмены нажмите /exit)");

        }

        public async Task GetUpdate(Update update)
        {
            Executor.StopListen();
            try
            {
                long chatId = update.Message.Chat.Id;
                if (update.Message.Text != null && update.Message.Text == "/exit") //Проверочка{
                {
                    return;
                }

                await Client.SendTextMessageAsync(Bot.GetMainAdmin(), $" {update.Message.Chat.Id} - @{update.Message.Chat.Username} - {update.Message.Chat.FirstName} - {update.Message.Chat.LastName} - Пишет: {update.Message.Text} " , replyMarkup: Keyboards.GetAdminResponseMenu(chatId));
                
                switch(update.Message.Type)
                {
                    case Telegram.Bot.Types.Enums.MessageType.Sticker:
                        await Client.SendStickerAsync(Bot.GetMainAdmin(), sticker: new InputFileId(update.Message.Sticker.FileId));
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Photo:
                        await Client.SendPhotoAsync(Bot.GetMainAdmin(), new InputFileId(update.Message.Photo.Last().FileId));                        
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Video:
                        await Client.SendVideoAsync(Bot.GetMainAdmin(), new InputFileId(update.Message.Video.FileId));
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Voice:
                        await Client.SendVoiceAsync(Bot.GetMainAdmin(), new InputFileId(update.Message.Voice.FileId));
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Text:
                        break;
                    default:
                        await Client.SendTextMessageAsync(Bot.GetMainAdmin(), "Другой формат сообщения " + update.Message.Type.ToString());
                        break;
                }

            }
            catch
            {
                
            }

        }
    }
}
