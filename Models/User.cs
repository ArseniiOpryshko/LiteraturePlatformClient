using System.ComponentModel.DataAnnotations;

namespace LiteraturePlatformClient.Models
{
    public class User
    {
        public int UserId { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string Login { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MinLength(8)]
        public string Password { get; set; }

    }
}
