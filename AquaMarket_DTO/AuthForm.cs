using System.ComponentModel.DataAnnotations;

namespace AquaMarket_DTO
{
    public class AuthForm
    {
        [Required(ErrorMessage = "Заполните графу Email.")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Заполните графу пароля.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
