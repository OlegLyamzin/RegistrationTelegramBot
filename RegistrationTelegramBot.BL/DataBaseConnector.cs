using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using RegistrationTelegramBot.DL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationTelegramBot.BL
{
    public class DataBaseConnector
    {
        public AccountService AccountService;
        public CategoryService CategoryService;
        public CategoryTypeService CategoryTypeService;
        public CurrencyService CurrencyService;
        public TransactionService TransactionService;
        public UserService UserService;
        public ClientService ClientService;

        public DataBaseConnector(AccountService accountService,
            CategoryService categoryService,
            CategoryTypeService categoryTypeService,
            CurrencyService currencyService,
            TransactionService transactionService,
            UserService userService,
            ClientService clientService) 
        {
            AccountService = accountService;
            CategoryService = categoryService;
            CategoryTypeService = categoryTypeService;
            CurrencyService = currencyService;
            TransactionService = transactionService;
            UserService = userService;
            ClientService = clientService;
            InitDataBase();
        }

        public async Task InitDataBase()
        {
            foreach(int type in Enum.GetValues(typeof(CategoryType)))
            {
                var categoty = await CategoryTypeService.GetCategoryTypeByIdAsync(type);
                if (categoty == null) {
                    await CategoryTypeService.CreateCategoryTypeAsync(new DL.Models.CategoryType
                    {
                        Id = type,
                        Name = Enum.GetName(typeof(CategoryType), type)
                    });
                }
            }
        }
    }

    public enum CategoryType
    {
        Income = 1,Expense = 2,Savings = 3
    }
}
