using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Domain.Models.Task
{
    public class CreateTaskDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public int StateId { get; set; }
    }
}
