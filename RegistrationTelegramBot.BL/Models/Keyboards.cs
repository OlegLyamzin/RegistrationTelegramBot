using RegistrationTelegramBot.BL.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RegistrationTelegramBot.BL.Models
{
    public static class Keyboards
    {
        public static ReplyKeyboardMarkup GetMainMenuBoard(long expiryTime, bool isAdmin)
        {
            return GetMainMenuBoard(new DateTime((expiryTime * 10000) + 621355968000000000), isAdmin);
        }
        public static ReplyKeyboardMarkup GetMainMenuBoard(DateTime expiryTime, bool isAdmin)
        {
            string emojuExpiry = DateTime.Now < expiryTime ? "✅" : "🔴";
            string adminCommand = isAdmin ? "Админка 🤡": "Написать админу 🤡";
            var kbrd = new ReplyKeyboardMarkup(
                new[] {
                    new[] // row 1
                    {
                        new KeyboardButton(emojuExpiry +" " + expiryTime.ToString("dd.MM.yyyy HH:mm") + " "+emojuExpiry)
                    },
                    new[] // row 2
                    {
                        new KeyboardButton("Пожертвовать ❤️"),
                        new KeyboardButton("Подключить VPN 🛜"),
                    },
                    new[] // row 3
                    {
                        new KeyboardButton("Узнать о VPN 🤔"),
                        new KeyboardButton(adminCommand)
                    }
                    
                }
                )
            {
                ResizeKeyboard = true
            };
            return kbrd;
        }

        public static ReplyKeyboardMarkup GetMainMenuBoard(bool isAdmin)
        {
            string adminCommand = isAdmin ? "Админка 🤡" : "Написать админу 🤡";
            var kbrd = new ReplyKeyboardMarkup(
                new[] {
                    new[]
                    {
                        new KeyboardButton("Добавить доход (+)")
                    },
                    new[] 
                    {
                        new KeyboardButton("Добавить расход (-)")
                    },
                    new[]
                    {
                        new KeyboardButton("Добавить накопления (=)")
                    },
                    new[] 
                    {
                        new KeyboardButton("Получить Эксель")
                    }
                    //new[]
                    //{
                    //    new KeyboardButton(adminCommand)
                    //}

                }
                )
            {
                ResizeKeyboard = true
            };
            return kbrd;
        }
        public static ReplyKeyboardMarkup GetInfoSubscribeBoard(DateTime expiryTime)
        {
            string emojuExpiry = DateTime.Now < expiryTime ? "✅" : "🔴";
            var kbrd = new ReplyKeyboardMarkup(
                new[] {
                    new[] // row 1
                    {
                        new KeyboardButton(emojuExpiry +" " + expiryTime.ToString("dd.MM.yyyy HH:mm") + " "+emojuExpiry)
                    },
                    new[] // row 1
                    {
                        new KeyboardButton("Баланс 💳"),
                        new KeyboardButton("Продлить 🕙"),
                    },
                    //new[] // row 2
                    //{
                    //    new KeyboardButton("Продлить бесплатно 🆓")
                    //},                    
                    new[] // row 3
                    {
                        new KeyboardButton("Главное меню  ↩️")
                    },
                }
                )
            {
                ResizeKeyboard = true
            };
            return kbrd;
        }

        public static ReplyKeyboardMarkup GetBalanceBoard()
        {
            var kbrd = new ReplyKeyboardMarkup(
                new[] {
                    new[] // row 1
                    {
                        new KeyboardButton("🏦 Ваш баланс: 0 ₽ 🏦"),
                        new KeyboardButton("Пополнить 💵"),
                    },
                    new[] // row 2
                    {
                        new KeyboardButton("Назад 📅")
                    },
                }
                )
            {
                ResizeKeyboard = true
            };
            return kbrd;
        }

        public static InlineKeyboardMarkup GetDepositOppinionsBoard()
        {
            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new []  {
                        InlineKeyboardButton.WithCallbackData("Кошелек 👛", "Wallet")
                        }
            });
            return kbrd;
        }

        internal static IReplyMarkup? GetRenewSubscribeBoard()
        {
            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new []  {
                        InlineKeyboardButton.WithCallbackData("1 мес. 📅 - 150 ₽", "1MonthSubscribe")
                        },

                new []  {
                        InlineKeyboardButton.WithCallbackData("3 мес. 📅 - 390 ₽", "3MonthSubscribe")
                        },

                new []  {
                        InlineKeyboardButton.WithCallbackData("6 мес. 📅 - 700 ₽", "6MonthSubscribe")
                        }
            });
            return kbrd;
        }

        internal static IReplyMarkup? GetRenewSubscribeFreeBoard()
        {
            
            var kbrd = new ReplyKeyboardMarkup(
                new[] {
                    new[] // row 1
                    {
                        new KeyboardButton("Начать 🤖"),
                    },
                    new[] // row 2
                    {
                        new KeyboardButton("Назад 📅")
                    },
                }
                )
            {
                ResizeKeyboard = true
            };
            return kbrd;
        }

        internal static IReplyMarkup? GetConnectVPNInfoBoard()
        {
            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new []  {
                        InlineKeyboardButton.WithUrl("Инструкция для IPhone", "https://telegra.ph/Instrukciya-po-podklyucheniyu-VPN-Vless-dlya-IPhone-12-16")
                        },

                new []  {
                        InlineKeyboardButton.WithUrl("Инструкция для Android", "https://telegra.ph/Instrukciya-po-podklyucheniyu-VPN-Vless-i-ShadowSocks-dlya-Android-12-24-2")
                        },

                new []  {
                        InlineKeyboardButton.WithUrl("Инструкция для Windows", "https://telegra.ph/Instrukciya-po-podklyucheniyu-VPN-Vless-i-ShadowSocks-dlya-PK-12-24")
                        },

                new []  {
                        InlineKeyboardButton.WithUrl("Проверить VPN", "https://2ip.ru/")
                        }
            });
            return kbrd;
        }

        internal static IReplyMarkup? GetDonateInfoBoard()
        {
            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new []  {
                        InlineKeyboardButton.WithUrl("Tribute (предпочтительно)", "https://t.me/tribute/app?startapp=d8gh")
                        },

                new []  {
                        InlineKeyboardButton.WithUrl("Donation Alerts", "https://www.donationalerts.com/r/justvpn")
                        }
            });
            return kbrd;
        }

        internal static IReplyMarkup? GetAdminResponseMenu(long chatId)
        {
            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new []  {
                        InlineKeyboardButton.WithCallbackData("Ответить на сообщение", chatId.ToString())
                        },
            });
            return kbrd;
        }

        internal static IReplyMarkup? GetCheckBlockMenu(long chatId)
        {
            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new []  {
                        InlineKeyboardButton.WithCallbackData("Чек не подходит", chatId.ToString())
                        },
            });
            return kbrd;
        }

        internal static IReplyMarkup? GetUnblockMenu(string username, long chatId)
        {
            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new []  {
                        InlineKeyboardButton.WithCallbackData(username, "Unblock")
                        }
            });
            return kbrd;
        }

        internal static IReplyMarkup GetAdminMunu()
        {
            var kbrd = new ReplyKeyboardMarkup(
                new[] {
                    new[] // row 1
                    {
                        new KeyboardButton("Общее сообщение 💬"),
                        new KeyboardButton("Написать пользователю ✍️")
                    },
                    new[] // row 2
                    {
                        new KeyboardButton("Разблокировать/заблокировать 🔐"),
                        new KeyboardButton("Поставить время всем ⏰")
                    },
                    new[] // row 3
                    {
                        new KeyboardButton("Главное меню  ↩️")
                    },
                }
                )
            {
                ResizeKeyboard = true
            };
            return kbrd; ;
        }

        internal static IReplyMarkup? GetInstructionsBoard()
        {
            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new []  {
                        InlineKeyboardButton.WithUrl("Инструкция для T-Bank (Tinkoff)", "https://telegra.ph/Oplata-cherez-T-bank-09-14")
                        },

                new []  {
                        InlineKeyboardButton.WithUrl("Инструкция для Сбер", "https://telegra.ph/Oplata-cherez-Sber-09-14")
                        },

                new []  {
                        InlineKeyboardButton.WithUrl("Инструкция для прочих банков", "https://telegra.ph/Oplata-cherez-prochie-banki-09-14")
                        }
            });
            return kbrd;
        }
    }
}
