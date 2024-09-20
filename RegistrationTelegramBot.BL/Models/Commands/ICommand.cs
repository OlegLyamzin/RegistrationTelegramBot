using Telegram.Bot.Types;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public interface ICommand
    {
        public Task Execute(Update update);
        public bool Contains(string command);
    }
}
