using Telegram.Bot.Types;
using Telegram.Bot;
using RegistrationTelegramBot.DL.Models;
using UnidecodeSharpFork;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net;
using System;
using System.IO;
using IronQr;
using QRCoder;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class RegCommand : BuisnessLogicCommand, ICommand, IListener
    {

        public CommandExecutor Executor { get; set; }
        private string _name;
        private string _email;
        private string _church;
        private byte[] _check;
        public bool IsForced { get; set; }
        public bool IsAdminReg { get; set; }

        public RegCommand(Bot bot, CommandExecutor executor, DataBaseConnector dataBaseConnector) : base(bot, dataBaseConnector)
        {
            Executor = executor;
        }

        public override List<string> Name => new List<string> { "/reg" };


        public override async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            string text = update.Message.Text;

            var clients = await DataBaseConnector.ClientService.GetAllClients();
            if(clients.Count > 150 && !IsAdminReg)
            {
                await Client.SendTextMessageAsync(chatId, @"К сожалению все места заняты, регистрация закрыта. Следите за новостями https://t.me/PG_msc");
                return;
            }

            var user = await DataBaseConnector.ClientService.GetClientByTgIdAsync(chatId.ToString());
            if (user != null && (bool)user.IsPaid && !IsForced)
            {
                await Client.SendTextMessageAsync(chatId, @"Вы уже зарегестрированы. Следите за новостями https://t.me/PG_msc 
Если вы хотите зарегестрировать еще кого-то, то нажмите /reg2");
                await Client.SendTextMessageAsync(chatId, "Если у вас возникли проблемы, то опишите вашу ситуацию");
                return;
            }
            Executor.StartListen(this); //говорим, что теперь нам надо отправлять апдейты
            await Client.SendTextMessageAsync(chatId, "Введите ваше ФИО (для отмены нажмите /exit)");
        }

        public async Task GetUpdate(Update update)
        {
            try
            {
                Executor.StopListen();
                string text = update.Message.Text;
                long chatId = update.Message.Chat.Id;

                if (text == "/exit")
                {
                    _name = null;
                    _church = null;
                    _email = null;
                    _check = null;
                    return;
                }
                if (string.IsNullOrEmpty(_name))
                {
                    Executor.StartListen(this);
                    _name = text;

                }
                else if (string.IsNullOrEmpty(_email))
                {

                    Executor.StartListen(this);
                    _email = text;
                }
                else if (string.IsNullOrEmpty(_church))
                {
                    Executor.StartListen(this);
                    _church = text;
                } else if (_check == null)
                {
                    string fileId = null;
                    string filetype = "DOC";
                    switch (update.Message.Type)
                    {
                        case Telegram.Bot.Types.Enums.MessageType.Photo:
                            fileId = update.Message.Photo.Last().FileId;
                            filetype = "PHOTO";
                            break;
                        case Telegram.Bot.Types.Enums.MessageType.Document:
                            fileId = update.Message.Document.FileId;
                            break;
                    }
                    if (fileId != null)
                    {
                        var file = await Client.GetFileAsync(fileId);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await Client.DownloadFileAsync(file.FilePath, ms);
                            _check = ms.ToArray();
                        }

                        var client = new Client();
                        client.TgId = chatId.ToString();
                        client.Username = update.Message.Chat.Username;
                        client.Name = _name;
                        client.Email = _email;
                        client.Church = _church;
                        client.Check = _check;
                        client.IsPaid = true;
                        client.FileIdCheck = fileId;
                        client.FileType = filetype;
                        client = await DataBaseConnector.ClientService.AddClient(client);
                        var supervisors = Bot.GetSupervisor();
                        foreach (var supervisor in supervisors)
                        {
                            await Client.SendTextMessageAsync(supervisor, $"Отправлен чек при регистрации {_name} - {_email} - {_church}");
                            if (filetype == "PHOTO")
                            {
                                await Client.SendPhotoAsync(supervisor, new InputFileId(fileId), replyMarkup: Keyboards.GetCheckBlockMenu(client.Id));
                            }
                            else if (filetype == "DOC")
                            {
                                await Client.SendDocumentAsync(supervisor, new InputFileId(fileId), replyMarkup: Keyboards.GetCheckBlockMenu(client.Id));
                            }
                        }
                        _name = null;
                        _email = null;
                        _church = null;
                        _check = null;
                        await Client.SendTextMessageAsync(chatId, "Регистрация завершена! Ждем вас на вечере хвалы. Следите за новостями https://t.me/PG_msc");

                        byte[] qrCodeImage = null;
                        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(Bot.GetURL() + "/client/" + client.Id, QRCodeGenerator.ECCLevel.Q))
                        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                        {
                            qrCodeImage = qrCode.GetGraphic(20);
                        }
                        if (qrCodeImage != null)
                        {
                            using (MemoryStream ms = new MemoryStream(qrCodeImage))
                            {
                                InputFile iof = new InputFileStream(ms, "bilet.bmp");

                                await Client.SendTextMessageAsync(chatId, "Ваш билет, сохраните и предъявите при входе на мероприятие:");
                                var send = await Client.SendPhotoAsync(chatId, iof);
                            }
                        }
                        return;
                    }
                    else
                    {
                        Executor.StartListen(this);
                    }
                }

                if (string.IsNullOrEmpty(_name))
                {
                    await Client.SendTextMessageAsync(chatId, "Введите ваше ФИО (для отмены нажмите /exit)");
                }
                else if (string.IsNullOrEmpty(_email))
                {
                    await Client.SendTextMessageAsync(chatId, "Введите ваш email (для отмены нажмите /exit)");
                } 
                else if (string.IsNullOrEmpty(_church))
                {
                    await Client.SendTextMessageAsync(chatId, "Введите название вашей церкви (для отмены нажмите /exit)");
                }
                else if (_check == null)
                {
                    await Client.SendTextMessageAsync(chatId, "Это благотворительное мероприятие и минимальная сумма для регистрации составляет 500₽❗️Для лиц младше 18 лет - 250₽\r\n\r\nЕсли вы хотите пожертвовать больше, то можете жертвовать больше указанной суммы:)\r\n\r\nПросим ответным сообщением выслать нам чек об оплате\r\n\r\nЖдём встречи с вами 05.10.2024 в 18:00. Вас встретят и проводят до нужного зала!\U0001faf1🏼‍\U0001faf2🏽\r\n\r\nЕсли захотите поддержать цвет вечера, то предлагаем одеться в черно-белый стиль:) \r\nНо это не обязательно, главное - прославить Бога и прикоснуться к истории!⛪️\r\n\r\nЕсли возникнут сложности при оплате, пишите @katherinefk \r\n\r\n*все сборы и пожертвования будут переданы на восстановление Дома Евангелия ");
                    using (WebClient client = new WebClient())
                    {
                        var img = client.DownloadData("https://createqr.ru/qr_gen?data=ST00012%7CName%3D%D0%A6%D0%A0%D0%9E+%22%D0%9E%D0%91%D0%AA%D0%95%D0%94%D0%98%D0%9D%D0%95%D0%9D%D0%98%D0%95+%D0%A6%D0%95%D0%A0%D0%9A%D0%92%D0%95%D0%99+%D0%95%D0%92%D0%90%D0%9D%D0%93%D0%95%D0%9B%D0%AC%D0%A1%D0%9A%D0%98%D0%A5+%D0%A5%D0%A0%D0%98%D0%A1%D0%A2%D0%98%D0%90%D0%9D-%D0%91%D0%90%D0%9F%D0%A2%D0%98%D0%A1%D0%A2%D0%9E%D0%92+%D0%9F%D0%9E+%D0%A1%D0%9F%D0%91+%D0%98+%D0%9B%D0%95%D0%9D.+%D0%9E%D0%91%D0%9B%D0%90%D0%A1%D0%A2%D0%98%22%7CPersonalAcc%3D40703810020000002769%7CBankName%3D%D0%9E%D0%9E%D0%9E+%22%D0%91%D0%B0%D0%BD%D0%BA+%D0%A2%D0%BE%D1%87%D0%BA%D0%B0%22%7CBIC%3D044525104%7CCorrespAcc%3D30101810745374525104%7CSum%3D50000%7CSumRub%3D500%7CPurpose%3D%D0%9D%D0%B0+%D0%94%D0%BE%D0%BC+%D0%95%D0%B2%D0%B0%D0%BD%D0%B3%D0%B5%D0%BB%D0%B8%D1%8F+%D1%81%D0%BE%D0%B3%D0%BB%D0%B0%D1%81%D0%BD%D0%BE+%D0%BE%D1%84%D0%B5%D1%80%D1%82%D0%B5+%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%BD%D0%BE%D0%B9+%D0%B2+%D0%98%D0%A2%D0%A1+%D0%98%D0%BD%D1%82%D0%B5%D1%80%D0%BD%D0%B5%D1%82+%D0%BF%D0%BE+%D0%B0%D0%B4%D1%80%D0%B5%D1%81%D1%83%3A++https%3A%2F%2F%D0%B4%D0%BE%D0%BC%D0%B5%D0%B2%D0%B0%D0%BD%D0%B3%D0%B5%D0%BB%D0%B8%D1%8F.%D1%80%D1%84%2F%D0%BF%D0%BE%D0%B6%D0%B5%D1%80%D1%82%D0%B2%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5%7CPayeeINN%3D7802131258%7CKPP%3D780201001");
                        using (MemoryStream stream = new MemoryStream(img))
                        {
                            InputFile iof = new InputFileStream(stream, "qrcode.png");
                            await Client.SendPhotoAsync(chatId, iof, replyMarkup: Keyboards.GetInstructionsBoard());
                        }
                    }
                    await Client.SendTextMessageAsync(chatId, "Приложите чек об оплате в виде документа или картинки (для отмены нажмите /exit)");
                }
            }
            catch (Exception ex)
            {
                _name = null;
                _church = null;
                _email = null;
                _check = null;
                throw ex;
            }
        }

        private static bool EqualInUnidcode(string text1, string text2)
        {
            text1 = Regex.Replace(text1, "[ьЬъЪ]|[^a-zA-Zа-яёА-ЯЁ]", "");
            text2 = Regex.Replace(text2, "[ьЬъЪ]|[^a-zA-Zа-яёА-ЯЁ]", "");
            return text1.Unidecode() == text2.Unidecode();
        }
    }
}
