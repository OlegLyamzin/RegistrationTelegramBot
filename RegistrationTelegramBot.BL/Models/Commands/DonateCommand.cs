
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class DonateCommand : Command, ICommand
    {
        public DonateCommand(Bot bot) : base(bot)
        {
        }

        public override List<string> Name => new List<string> { "Пожертвовать ❤️" };


        public override async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            await Client.SendTextMessageAsync(Bot.GetMainAdmin(), $"{update.Message.Chat.Id} - {update.Message.Chat.Username} - {update.Message.Chat.FirstName} - {update.Message.Chat.LastName} нажал пожертвовать");
            await Client.SendTextMessageAsync(chatId, @"Надеюсь, что это письмо найдет вас в благополучии и высоких скоростях интернета! 💌🙏🌐

Благодаря вашей поддержке, мы можем сохранить наш сервис бесплатным для всех. 🤝💰🆓 Мы постарались обеспечить вам самое высокое качество, доступность и скорость подключения, независимо от вашего местоположения, чтобы вы все могли наслаждаться безопасным и защищенным интернетом. 🔝

Однако, поддерживать такой сервис требует затрат.  Мы постоянно обновляем нашу инфраструктуру, улучшаем сервера и развиваемся, чтобы соответствовать всем вашим потребностям. К сожалению, финансовые ресурсы становятся ограниченными. 📉💰

В связи с вышесказанным, я обращаюсь к вам с просьбой пожертвовать любую сумму, которую вы считаете возможной. 💲🙏

Ссылки для пожертвований 👇 ", replyMarkup: Keyboards.GetDonateInfoBoard());
        }

    }
}