using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Domain.Entities
{
    public class State
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
