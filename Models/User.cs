using System.ComponentModel.DataAnnotations;

namespace LiteraturePlatform.Models
{
    public class User
    {
        public int UserId { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string Login { get; set; }

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Password { get; set; }

    }
}
