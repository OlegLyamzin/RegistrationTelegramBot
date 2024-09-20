using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationTelegramBot.DL.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string? TgId { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public DateTime CreatedOn { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Account>? Accounts { get; set; }
    }
}
