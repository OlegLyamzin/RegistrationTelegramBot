namespace RegistrationTelegramBot.DL.Models
{
    public class Category
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? UserId { get; set; }
        public int? TypeId { get; set; }

        public User? User { get; set; }
        public CategoryType? CategoryType { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
