using System.ComponentModel.DataAnnotations;

namespace TestCedro.Service.WebAPI.ViewModels
{
    public class LoginVm
    {
        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar seu acesso?")]
        public bool RememberMe { get; set; }
    }
}