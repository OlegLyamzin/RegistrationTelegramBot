namespace RegistrationTelegramBot.DL.Models
{
    public class Currency
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public ICollection<Account>? Accounts { get; set; }
        public int? UserId { get; set; }

        public User? User { get; set; }
    }
}
