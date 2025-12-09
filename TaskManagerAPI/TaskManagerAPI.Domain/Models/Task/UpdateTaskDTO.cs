using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Domain.Models.Task
{
    public class UpdateTaskDTO
    {
        [Required(ErrorMessage = "The field Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "The field Title is required.")]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "The field StateId is required.")]
        public int StateId { get; set; }
    }
}
