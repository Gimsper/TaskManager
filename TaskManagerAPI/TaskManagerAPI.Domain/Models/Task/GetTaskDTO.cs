using TaskManagerAPI.Domain.Models.State;

namespace TaskManagerAPI.Domain.Models.Task
{
    public class GetTaskDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public GetStateDTO State { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
