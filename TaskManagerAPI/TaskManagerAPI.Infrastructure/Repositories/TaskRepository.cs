using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Domain.Context;
using TaskManagerAPI.Infrastructure.Repositories.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories
{
    public class TaskRepository : BaseRepository<Domain.Entities.Task>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context) { }

        public Domain.Entities.Task? GetByIdInclude(int id)
        {
            return _dbSet.Include(e => e.State).FirstOrDefault(e => e.Id == id);
        }
    }
}
