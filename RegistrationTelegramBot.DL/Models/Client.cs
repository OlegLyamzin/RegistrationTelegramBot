using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationTelegramBot.DL.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string? Username { get; set; }

        [MaxLength(30)]
        public string? TgId { get; set; }

        [MaxLength(1000)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Email { get; set; }

        [MaxLength(1000)]
        public string? Church { get; set; }

        public bool? IsPaid { get; set; }

        public byte[]? Check { get; set; }

        public DateTime? CreatedOn { get; set; }
        [MaxLength(255)]
        public string? FileIdCheck { get; set; }
        [MaxLength(45)]
        public string? FileType { get; set; }
    }
}
