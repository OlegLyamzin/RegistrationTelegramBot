
using DocumentFormat.OpenXml.Office2013.Word;
using RegistrationTelegramBot.BL;
using RegistrationTelegramBot.BL.Models;
using RegistrationTelegramBot.Core;
using RegistrationTelegramBot.DL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Runtime;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RegistrationTelegramBot.API.Controllers
{
    [ApiController]
    [Route("/")]
    public class BotController : ControllerBase
    {
        private Bot _bot;
        private UpdateDistributor _updateDistributor;
        private AppSettings _appSettings;
        private DataBaseConnector _dataBaseConnector;

        public BotController( Bot bot, DataBaseConnector dataBaseConnector, UpdateDistributor updateDistributor, IOptions<AppSettings> options) {

            _bot = bot;
            _updateDistributor = updateDistributor;
            _appSettings = options.Value;
            _dataBaseConnector = dataBaseConnector;
        }
        [HttpPost]
        public async void Post(Update update) //Сюда будут приходить апдейты
        {
            try
            {       
                await _updateDistributor.GetUpdate(update);
            }            
            catch(Exception ex)
            {
                try
                {
                    var Client = _bot.Get();
                    if (update.Message != null)
                    {
                        await Client.SendTextMessageAsync(_bot.GetMainAdmin(), $" {update.Message.Chat.Id} - @{update.Message.Chat.Username} - {update.Message.Chat.FirstName} - {update.Message.Chat.LastName} - {ex.Message}");
                        await Client.SendTextMessageAsync(update.Message.Chat.Id, "Что-то пошло не так");
                    }
                    else if (update.CallbackQuery.Message != null)
                    {
                        await Client.SendTextMessageAsync(_bot.GetMainAdmin(), $" {update.CallbackQuery.Message.Chat.Id} - @{update.CallbackQuery.Message.Chat.Username} - {update.CallbackQuery.Message.Chat.FirstName} - {update.CallbackQuery.Message.Chat.LastName} - {ex.Message}");
                        await Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Что-то пошло не так");
                    }
                    else
                    {
                        await Client.SendTextMessageAsync(_bot.GetMainAdmin(), $"{ex.Message}");
                    }
                }
                catch { }

            }
        }
        [HttpGet]
        public async Task<string> Get()
        {
            //Здесь мы пишем, что будет видно если зайти на адрес,
            //указаную в ngrok и launchSettings
            try
            {
            var Client = _bot.Get();
            var info = await Client.GetWebhookInfoAsync();
            return "Telegram bot was started " + _appSettings.URL + " " + info.Url + " " + info.LastErrorMessage;
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }
        [HttpGet("client/{clientId}")]
        public ContentResult GetClient(int clientId)
        {
            //Здесь мы пишем, что будет видно если зайти на адрес,
            //указаную в ngrok и launchSettings
            try
            {
                var ClientBot = _bot.Get();
                var client = _dataBaseConnector.ClientService.GetClientById(clientId);
                return Content($"<p>Посетитель: {client.Name} - {client.Church} - {(((bool)client.IsPaid) ? "Оплатил" : "Не оплатил")} - {client.CreatedOn}</p>\r\n<form action=\"{client.Id}\" method=\"post\">\r\n    <button name=\"client\" value=\"upvote\">Отправить чек на проверку</button>\r\n</form>", "text/html", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost("client/{clientId}")]
        public async Task<ContentResult> GetClientCheck(int clientId)
        {
            //Здесь мы пишем, что будет видно если зайти на адрес,
            //указаную в ngrok и launchSettings
            try
            {
                var ClientBot = _bot.Get();
                var client = _dataBaseConnector.ClientService.GetClientById(clientId);
                var supervisors = _bot.GetSupervisor();
                foreach (var supervisor in supervisors)
                {

                    await ClientBot.SendTextMessageAsync(supervisor, $"Посетитель: {(string.IsNullOrEmpty(client.Username) ? "" : $"@{client.Username} - ")}{client.Name} - {client.Church} - {(((bool)client.IsPaid) ? "Оплатил" : "Не оплатил")} - {client.CreatedOn}");
                    if (client.FileType == "PHOTO")
                    {
                        await ClientBot.SendPhotoAsync(supervisor, new InputFileId(client.FileIdCheck));
                    }
                    else if (client.FileType == "DOC")
                    {
                        await ClientBot.SendDocumentAsync(supervisor, new InputFileId(client.FileIdCheck));
                    }
                }

                return Content($"<p>Посетитель: {client.Name} - {client.Church} - {(((bool)client.IsPaid) ? "Оплатил" : "Не оплатил")} - {client.CreatedOn}</p>\r\n<form action=\"{client.Id}\" method=\"post\">\r\n    <button name=\"client\" value=\"upvote\">Отправить чек на проверку</button>\r\n</form>", "text/html", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
