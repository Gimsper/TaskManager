using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Domain.Models.State
{
    public class UpdateStateDTO
    {
        [Required(ErrorMessage = "The field Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "The field Name is required.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
