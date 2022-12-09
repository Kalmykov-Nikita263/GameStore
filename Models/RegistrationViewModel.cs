using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "логин")]
        public string UserLogin { get; set; }

        [Required]
        [Display(Name = "эл. почта")]
        public string Email { get; set; }

        [Required]
        [UIHint("Password")]
        [Display(Name = "пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "пароли не совпадают")]
        public string PasswordConfirmation { get; set; }
    }
}