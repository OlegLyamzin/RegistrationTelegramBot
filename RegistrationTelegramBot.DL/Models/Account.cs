namespace RegistrationTelegramBot.DL.Models
{
    public class Account
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? Currency { get; set; }
        public int? UserId { get; set; }

        public User? User { get; set; }
        public Currency? CurrencyInfo { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
