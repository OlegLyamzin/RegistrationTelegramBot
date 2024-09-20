using RegistrationTelegramBot.BL;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class CheckBlockCommand : BuisnessLogicCommand, ICommand
    {
        public CheckBlockCommand(Bot bot, DataBaseConnector serverConnector) : base(bot, serverConnector)
        {
        }

        public override List<string> Name => new List<string> { "Чек не подходит" };


        public override async Task Execute(Update update)
        {
            long chatId = update.CallbackQuery.Message.Chat.Id;
            string[] supervisors = Bot.GetSupervisor();
            if (!supervisors.Contains(chatId.ToString()))
            {
                await Client.SendTextMessageAsync(chatId.ToString(), $"У вас нет доступа");
                return;
            }
            int clientId  = Convert.ToInt32( update.CallbackQuery.Data);
            var client = DataBaseConnector.ClientService.GetClientById(clientId);

            await Client.SendTextMessageAsync(client.TgId, @$"Ваша регистрация на имя {client.Name} была удалена по причине: Чек об оплате не подходит.
Пройдите регистрацию заново или напишите @katherinefk");

            foreach (var supervisor in supervisors)
            {
                await Client.SendTextMessageAsync(supervisor, $" {client.Name} - {client.Email} - {client.Church} - регистрация была удалена");
            }

            DataBaseConnector.ClientService.DeleteClient(clientId);

        }

    }
}
