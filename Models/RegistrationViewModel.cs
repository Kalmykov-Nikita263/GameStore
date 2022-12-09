using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "�����")]
        public string UserLogin { get; set; }

        [Required]
        [Display(Name = "��. �����")]
        public string Email { get; set; }

        [Required]
        [UIHint("Password")]
        [Display(Name = "������")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "������ �� ���������")]
        public string PasswordConfirmation { get; set; }
    }
}