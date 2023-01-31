using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LiteraturePlatformClient.Models
{
    public class RegisterModel
    {
        public int UserId { get; set; }

        [MinLength(3, ErrorMessage = "Min Login length should be 3 symbols")]
        [MaxLength(30, ErrorMessage = "Max Login length should be 30 symbols")]
        public string Login { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+=?^_`{|}~-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-]+$", ErrorMessage = "Email is not valid ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MinLength(8, ErrorMessage = "Min Password length should be 8 symbols")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
