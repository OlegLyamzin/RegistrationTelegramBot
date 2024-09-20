namespace RegistrationTelegramBot.DL.Models
{
    public class Transaction
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public double? Amount { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CategoryId { get; set; }
        public int? AccountId { get; set; }

        public User? User { get; set; }
        public Category? Category { get; set; }
        public Account? Account { get; set; }
    }
}
