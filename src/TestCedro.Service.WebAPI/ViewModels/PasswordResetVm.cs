using System.ComponentModel.DataAnnotations;

namespace TestCedro.Service.WebAPI.ViewModels
{
    public class PasswordResetVm
    {
        [Required]
        [Display(Name = "Reset Token")]
        public string ResetToken { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ser pelo menos {2} characteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("NewPassword", ErrorMessage = "A senha e confirmação não conferem.")]
        public string ConfirmPassword { get; set; }
    }
}