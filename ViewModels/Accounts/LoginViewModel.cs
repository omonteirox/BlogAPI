using System.ComponentModel.DataAnnotations;

namespace BlogAPI.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o e-mail")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        public string Password { get; set; }
    }
}