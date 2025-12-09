using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Domain.Models.State
{
    public class CreateStateDTO
    {
        [Required(ErrorMessage = "The field Name is required")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
