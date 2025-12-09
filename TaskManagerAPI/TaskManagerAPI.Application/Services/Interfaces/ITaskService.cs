using Models.Dto;
using TaskManagerAPI.Domain.Models.Dto;

namespace TaskManagerAPI.Application.Services.Interfaces
{
    public interface ITaskService : IBaseService<Domain.Entities.Task> 
    {
        ResultOperation<Domain.Entities.Task> GetByIdInclude(int id);
        ResultOperation<PagedList<Domain.Entities.Task>> GetAllInclude(QueryFilter query);
    }
}
