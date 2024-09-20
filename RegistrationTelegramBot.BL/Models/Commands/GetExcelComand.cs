using RegistrationTelegramBot.BL.Models;
using RegistrationTelegramBot.BL;
using Telegram.Bot.Types;
using Telegram.Bot;
using ClosedXML.Excel;
using RegistrationTelegramBot.DL.Models;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class GetExcelComand : BuisnessLogicCommand, ICommand
    {
        public GetExcelComand(Bot bot, DataBaseConnector dataBaseConnector) : base(bot, dataBaseConnector)
        {
        }

        public override List<string> Name => new List<string> { "/excel", "Получить Эксель" };


        public override async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            string[] supervisors = Bot.GetSupervisor();
            if (!supervisors.Contains(chatId.ToString()))
            {
                await Client.SendTextMessageAsync(chatId.ToString(), $"У вас нет доступа");
                return;
            }
            var filePath = @"/root/" + chatId + ".xlsx";

            var clients = await DataBaseConnector.ClientService.GetAllClients();
            using (var workbook = System.IO.File.Exists(filePath) ? new XLWorkbook(filePath) : new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Count > 0 ? workbook.Worksheet(1) : workbook.AddWorksheet("Sheet1");

                if (worksheet.Row(1).Cell(1).IsEmpty())
                {
                    worksheet.Cell(1, 1).Value = "ФИО";
                    worksheet.Cell(1, 2).Value = "Email";
                    worksheet.Cell(1, 3).Value = "Церковь";
                    worksheet.Cell(1, 4).Value = "Дата оплаты";
                    worksheet.Cell(1, 5).Value = "Ссылка на проверку чека";
                    worksheet.Cell(1, 6).Value = "Ссылка на телеграм зарегестрившего (при наличии)";
                }
                foreach (var client in clients)
                {
                    var nextRow = worksheet.LastRowUsed().RowNumber() + 1;

                    worksheet.Cell(nextRow, 1).Value = client.Name;
                    worksheet.Cell(nextRow, 2).Value = client.Email;
                    worksheet.Cell(nextRow, 3).Value = client.Church;
                    worksheet.Cell(nextRow, 4).Value = client.CreatedOn;
                    worksheet.Cell(nextRow, 5).Value = Bot.GetURL() + "/client/" + client.Id;
                    if (!string.IsNullOrEmpty(client.Username))
                        worksheet.Cell(nextRow, 6).Value = "https://t.me/" + client.Username;

                }
                workbook.SaveAs(filePath);
            }
            using (var stream = System.IO.File.OpenRead(filePath))
            {
                InputFile iof = new InputFileStream(stream, "clients.xlsx");
                var send = await Client.SendDocumentAsync(chatId, iof);
            }
            System.IO.File.Delete(filePath);
        }

    }
}

