using Models.Dto;
using TaskManagerAPI.Application.Services.Interfaces;
using TaskManagerAPI.Domain.Models.Dto;
using TaskManagerAPI.Infrastructure.Repositories.Interfaces;

namespace TaskManagerAPI.Application.Services
{
    public class TaskService : BaseService<Domain.Entities.Task>, ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository repo) : base(repo)
        {
            _taskRepository = repo;
        }

        public ResultOperation<Domain.Entities.Task> GetByIdInclude(int id)
        {
            var result = new ResultOperation<Domain.Entities.Task>();
            try
            {
                result.Result = _taskRepository.GetByIdInclude(id);
                result.stateOperation = true;
            }
            catch (Exception ex)
            {
                result.stateOperation = false;
                result.MessageExceptionTechnical = ex.Message + Environment.NewLine + ex.StackTrace;
            }
            return result;
        }

        public ResultOperation<PagedList<Domain.Entities.Task>> GetAllInclude(QueryFilter query)
        {
            var result = new ResultOperation<PagedList<Domain.Entities.Task>>();
            try
            {
                var tasks = new List<Domain.Entities.Task>();

                tasks = _taskRepository.GetAllInclude("State");
                if (tasks.Count == 0)
                {
                    result.MessageResult = "No hay datos";
                }

                var response = PagedList<Domain.Entities.Task>.Create(tasks, query.PageNumber, 10);
                result.Result = response;
                result.stateOperation = true;
            }
            catch (Exception ex)
            {
                result.stateOperation = false;
                result.MessageExceptionTechnical = ex.Message + Environment.NewLine + ex.StackTrace;
            }
            return result;
        }
    }
}
