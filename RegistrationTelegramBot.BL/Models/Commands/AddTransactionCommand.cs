using Telegram.Bot.Types;
using Telegram.Bot;
using RegistrationTelegramBot.DL.Models;
using UnidecodeSharpFork;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.VariantTypes;

namespace RegistrationTelegramBot.BL.Models.Commands
{
    public class AddTransactionCommand : BuisnessLogicCommand, ICommand, IListener
    {

        public CommandExecutor Executor { get; set; }
        private double _amount;
        private int _categoryId;
        private int _accountId;
        private int _currencyId;
        private string _accountName;
        private CategoryType _categoryType;

        public AddTransactionCommand(Bot bot, CommandExecutor executor, DataBaseConnector dataBaseConnector) : base(bot, dataBaseConnector)
        {
            Executor = executor;
        }

        public override List<string> Name => new List<string> { "+", "Добавить доход (+)", "-", "Добавить расход (-)", "=", "Добавить накопления (=)" };


        public override async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            string text = update.Message.Text;
            if (text.Contains("+"))
                _categoryType = CategoryType.Income;
            if (text.Contains("-"))
                _categoryType = CategoryType.Expense;
            if (text.Contains("="))
                _categoryType = CategoryType.Savings;
            Executor.StartListen(this); //говорим, что теперь нам надо отправлять апдейты
            await Client.SendTextMessageAsync(chatId, "Введите сумму (для отмены нажмите /exit)");
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
                    return;
                }
                var user = await DataBaseConnector.UserService.GetUserByTgIdAsync(chatId.ToString());
                if (user == null)
                {
                    user = await DataBaseConnector.UserService.CreateUserAsync(new DL.Models.User()
                    {
                        TgId = chatId.ToString(),
                        Username = update.Message.Chat.Username,
                        Name = update.Message.Chat.FirstName,
                        Lastname = update.Message.Chat.LastName,
                        CreatedOn = DateTime.Now
                    });
                }
                var categoryType = await DataBaseConnector.CategoryTypeService.GetCategoryTypeByIdAsync(((int)_categoryType));
                if (_amount == 0)
                {
                    Executor.StartListen(this);
                    try
                    {
                        _amount = _categoryType == CategoryType.Expense ? -Convert.ToDouble(text) : Convert.ToDouble(text);

                    }
                    catch (Exception ex)
                    {
                        await Client.SendTextMessageAsync(chatId, "Неправильно введена сумма");
                        await Client.SendTextMessageAsync(chatId, "Введите сумму (для отмены нажмите /exit)");
                        return;
                    }
                    await Client.SendTextMessageAsync(chatId, "Введите категорию (для отмены нажмите /exit)");
                    var categories = await DataBaseConnector.CategoryService.GetAllCategoriesByUserIdAsync((int)user.Id);
                    if (categories != null)
                    {
                        categories = categories.FindAll(cat => cat.TypeId == (int)_categoryType);
                        if (categories.Count > 0)
                        {
                            string existCategories = "Ранее введенные категории" + System.Environment.NewLine;
                            foreach (var category in categories)
                            {
                                existCategories = $"{existCategories}/{Regex.Replace(category.Name, "[ьЬъЪ]|[^a-zA-Zа-яёА-ЯЁ]", "").Unidecode()} ({category.Name}){System.Environment.NewLine}";
                            }
                            await Client.SendTextMessageAsync(chatId, existCategories);
                        }
                    }
                    return;

                }
                if (_categoryId == 0)
                {

                    Executor.StartListen(this);
                    text = text.Replace("/", "");
                    var categories = await DataBaseConnector.CategoryService.GetAllCategoriesByUserIdAsync((int)user.Id);
                    foreach (var category in categories)
                    {
                        if (EqualInUnidcode(category.Name, text))
                        {
                            _categoryId = (int)category.Id;
                        }
                    }
                    if (_categoryId == 0)
                    {
                        var category = await DataBaseConnector.CategoryService.CreateCategoryAsync(new Category { Name = text, UserId = user.Id, TypeId = categoryType.Id });
                        _categoryId = (int)category.Id;
                    }
                    await Client.SendTextMessageAsync(chatId, "Введите название счета (для отмены нажмите /exit)");
                    var accounts = await DataBaseConnector.AccountService.GetAllAccountsByUserIdAsync((int)user.Id);
                    if (accounts != null && accounts.Count > 0)
                    {
                        string existaccounts = "Ранее введенные счета" + System.Environment.NewLine;
                        foreach (var account in accounts)
                        {
                            existaccounts = $"{existaccounts}/{Regex.Replace(account.Name, "[ьЬъЪ]|[^a-zA-Zа-яёА-ЯЁ]", "").Unidecode()} ({account.Name}){System.Environment.NewLine}";
                        }
                        await Client.SendTextMessageAsync(chatId, existaccounts);
                    }
                    return;
                }
                if (_accountId == 0)
                {
                    text = text.Replace("/", "");
                    _accountName = text;
                    var accounts = await DataBaseConnector.AccountService.GetAllAccountsByUserIdAsync((int)user.Id);
                    foreach (var account in accounts)
                    {
                        if (EqualInUnidcode(account.Name, text))
                        {
                            _accountId = (int)account.Id;
                            _currencyId = (int)account.Currency;
                        }
                    }
                    if (_accountId == 0)
                    {
                        Executor.StartListen(this);
                        await Client.SendTextMessageAsync(chatId, "Введите название валюты (для отмены нажмите /exit)");
                        var currencies = await DataBaseConnector.CurrencyService.GetAllCurrenciesByUserIdAsync((int)user.Id);
                        if (currencies != null && currencies.Count > 0)
                        {
                            string existCurrency = "Ранее введенные валюты" + System.Environment.NewLine;
                            foreach (var currency in currencies)
                            {
                                existCurrency = $"{existCurrency}/{Regex.Replace(currency.Name, "[ьЬъЪ]|[^a-zA-Zа-яёА-ЯЁ]", "").Unidecode()} ({currency.Name}){System.Environment.NewLine}";
                            }
                            await Client.SendTextMessageAsync(chatId, existCurrency);
                        }
                        _accountId = -1;
                        return;
                    }
                }
                if (_currencyId == 0)
                {
                    text = text.Replace("/", "");
                    var currencies = await DataBaseConnector.CurrencyService.GetAllCurrenciesByUserIdAsync((int)user.Id);
                    foreach (var currency in currencies)
                    {
                        if (EqualInUnidcode(text, currency.Name))
                        {
                            _currencyId = (int)currency.Id;
                        }
                    }
                    if (_currencyId == 0)
                    {
                        var currency = await DataBaseConnector.CurrencyService.CreateCurrencyAsync(new Currency { Name = text, UserId = user.Id });
                        _currencyId = (int)currency.Id;
                    }
                    var account = await DataBaseConnector.AccountService.CreateAccountAsync(new Account { Name = _accountName, UserId = user.Id, Currency = _currencyId });
                    _accountId = (int)account.Id;
                }

                var transaction = await DataBaseConnector.TransactionService.CreateTransactionAsync(new Transaction
                {
                    UserId = user.Id,
                    Amount = _amount,
                    CategoryId = _categoryId,
                    AccountId = _accountId,
                    CreatedOn = DateTime.UtcNow
                });
                string categoryName = (await DataBaseConnector.CategoryService.GetCategoryByIdAsync(_categoryId)).Name;
                string accountName = (await DataBaseConnector.AccountService.GetAccountByIdAsync(_accountId)).Name;
                string currencyName = (await DataBaseConnector.CurrencyService.GetCurrencyByIdAsync(_currencyId)).Name;
                await Client.SendTextMessageAsync(chatId, $"Добавлено {_amount} {categoryName} {accountName} {currencyName}", replyMarkup: Keyboards.GetMainMenuBoard(Bot.IsAdmin(chatId.ToString())));
            }
            catch (Exception ex)
            {
                _amount = 0;
                _categoryId = 0;
                _currencyId = 0;
                _accountName = null;
                _accountId = 0;
                throw ex;
            }
            _amount = 0;
            _categoryId = 0;
            _currencyId = 0;
            _accountName = null;
            _accountId = 0;
        }

        private static bool EqualInUnidcode(string text1, string text2)
        {
            text1 = Regex.Replace(text1, "[ьЬъЪ]|[^a-zA-Zа-яёА-ЯЁ]", "");
            text2 = Regex.Replace(text2, "[ьЬъЪ]|[^a-zA-Zа-яёА-ЯЁ]", "");
            return text1.Unidecode() == text2.Unidecode();
        }
    }
}
