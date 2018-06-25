using System.ComponentModel.DataAnnotations;

namespace TestCedro.Service.WebAPI.ViewModels
{
    public class ForgotPasswordVm
    {
        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Endereço de email")]
        public string Email { get; set; }
    }
}