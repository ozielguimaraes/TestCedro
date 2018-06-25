using System.ComponentModel.DataAnnotations;

namespace TestCedro.Service.WebAPI.ViewModels
{
    public class RegisterVm
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Endereço de email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Password", ErrorMessage = "As senhas não conferem")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório")]
        [Display(Name = "Nome")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "O {0} é obrigatório")]
        [Display(Name = "Sobrenome")]
        public string LastName { get; set; }
    }
}