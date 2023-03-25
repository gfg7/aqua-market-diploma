using System.ComponentModel.DataAnnotations;

namespace AquaMarket_DTO
{
    public class Profile
    {
        [Required(ErrorMessage = "Заполните графу Email.")]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Заполните графу номера телефона.")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Заполните графу имени.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Заполните графу фамилии.")]
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
    }
}
