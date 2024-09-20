using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class AdminRegComand : BuisnessLogicCommand, ICommand, IListener
    {

        private RegCommand _regCommand;

        public override List<string> Name => new List<string> { "/regSecret" };

        public CommandExecutor Executor { get; set; }
        public AdminRegComand(Bot bot, CommandExecutor executor, DataBaseConnector dataBaseConnector) : base(bot, dataBaseConnector)
        {
            Executor = executor;
            _regCommand = new RegCommand(bot, executor, dataBaseConnector) { IsForced = true, IsAdminReg = true };
        }


        public override async Task Execute(Update update)
        {
            await _regCommand.Execute(update);
        }

        public Task GetUpdate(Update update)
        {
            throw new NotImplementedException();
        }
    }
}
