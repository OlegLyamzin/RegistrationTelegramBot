using Telegram.Bot.Types;
using Telegram.Bot;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class BalanceCommand : Command, ICommand
    {
        public BalanceCommand(Bot bot) : base(bot)
        {
        }

        public override List<string> Name => new List<string> { "Баланс 💳"};


        public override async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            await Client.SendTextMessageAsync(chatId, "Баланс 🤑", replyMarkup: Keyboards.GetBalanceBoard());
        }

    }
}