namespace TaskManagerAPI.Infrastructure.Repositories.Interfaces
{
    public interface ITaskRepository : IBaseRepository<Domain.Entities.Task>
    {
        Domain.Entities.Task? GetByIdInclude(int id);
    }
}
