using Telegram.Bot.Types;
using Telegram.Bot;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class DepositCommand : Command, ICommand
    {
        public DepositCommand(Bot bot) : base(bot)
        {
        }

        public override List<string> Name => new List<string> { "Пополнить 💵" };


        public override async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            //await Client.SendTextMessageAsync(chatId, "Способ пополнения:", replyMarkup: Keyboards.GetDepositOppinionsBoard());
            await Client.SendTextMessageAsync(chatId, "");
            
        }

    }
}