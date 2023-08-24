using System.ComponentModel.DataAnnotations;

namespace BlogAPI.ViewModels.Accounts
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {1} é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo {1} está em formato inválido.")]
        public string Email { get; set; }
    }
}