using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "логин")]
        public string UserLogin { get; set; }

        [Required]
        [UIHint("Password")]
        [Display(Name = "пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }
}