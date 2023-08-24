using System.ComponentModel.DataAnnotations;

namespace BlogAPI.ViewModels.Categories
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "Campo Nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo Slug é obrigatório")]
        public string Slug { get; set; }
    }
}