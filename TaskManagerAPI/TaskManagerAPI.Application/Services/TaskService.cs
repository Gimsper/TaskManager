using Models.Dto;
using TaskManagerAPI.Application.Services.Interfaces;
using TaskManagerAPI.Domain.Models.Dto;
using TaskManagerAPI.Domain.Models.State;
using TaskManagerAPI.Domain.Models.Task;
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

        /// <summary>
        /// Obtiene la información de la tarea registrada por medio de su identificador,
        /// incluyendo los estados correspondientes a la misma.
        /// </summary>
        /// <param name="id">Identificador de la tarea</param>
        /// <returns>Información de la tarea</returns>
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

        /// <summary>
        /// Obtiene la información paginada de todos las tareas registradas,
        /// incluyendo los estados correspondientes a cada una.
        /// </summary>
        /// <param name="query">Objeto que contiene el número de página al que se quiere navegar</param>
        /// <returns>Lista paginada de tareas registradas</returns>
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

                var taskDto = tasks.Select(e =>
                {
                    return new GetTaskDTO
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Description = e.Description,
                        State = new GetStateDTO
                        {
                            Id = e.State.Id,
                            Name = e.State.Name
                        },
                        DueDate = e.DueDate,
                        CreatedAt = e.CreatedAt,
                        UpdatedAt = e.UpdatedAt
                    };
                });

                var response = PagedList<Domain.Entities.Task>.Create(tasks, query.PageNumber, 5);
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
