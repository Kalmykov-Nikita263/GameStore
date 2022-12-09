using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "�����")]
        public string UserLogin { get; set; }

        [Required]
        [UIHint("Password")]
        [Display(Name = "������")]
        public string Password { get; set; }

        [Display(Name = "��������� ����?")]
        public bool RememberMe { get; set; }
    }
}