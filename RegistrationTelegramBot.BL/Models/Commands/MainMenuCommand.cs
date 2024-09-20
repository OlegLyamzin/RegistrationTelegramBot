using RegistrationTelegramBot.BL;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class MainMenuCommand : BuisnessLogicCommand, ICommand
    {
        public MainMenuCommand(Bot bot, DataBaseConnector serverConnector) : base(bot, serverConnector)
        {
        }

        public override List<string> Name => new List<string> { "Главное меню  ↩️", "/start" };


        public override async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            string username = update.Message.Chat.Username == null ? update.Message.Chat.Id + "_" + update.Message.Chat.FirstName+"_"+update.Message.Chat.LastName : update.Message.Chat.Username;

                await Client.SendTextMessageAsync(Bot.GetMainAdmin(), $" {chatId} - @{update.Message.Chat.Username} - {update.Message.Chat.FirstName} - {update.Message.Chat.LastName} - Попытка нового подключения");
            


            await Client.SendTextMessageAsync(chatId, "Добро пожаловать в бот регистрации на Вечер Хвалы в Доме Евангелия. Для регистрации нажмите /reg");
        }

    }
}
