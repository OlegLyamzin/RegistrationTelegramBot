﻿namespace RegistrationTelegramBot.DL.Models
{
    public class CategoryType
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public ICollection<Category>? Categories { get; set; }
    }
}
