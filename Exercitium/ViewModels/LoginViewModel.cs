using System.ComponentModel.DataAnnotations;

namespace Exercitium.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "E-mail is required")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "E-mail is required")]
        public string Password { get; set; } = string.Empty;
    }
}
