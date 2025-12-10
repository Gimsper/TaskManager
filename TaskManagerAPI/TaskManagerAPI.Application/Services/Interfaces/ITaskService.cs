using Models.Dto;
using TaskManagerAPI.Domain.Models.Dto;

namespace TaskManagerAPI.Application.Services.Interfaces
{
    public interface ITaskService : IBaseService<Domain.Entities.Task> 
    {
        /// <summary>
        /// Obtiene la información de la tarea registrada por medio de su identificador,
        /// incluyendo los estados correspondientes a la misma.
        /// </summary>
        /// <param name="id">Identificador de la tarea</param>
        /// <returns>Información de la tarea</returns>
        ResultOperation<Domain.Entities.Task> GetByIdInclude(int id);

        /// <summary>
        /// Obtiene la información paginada de todos las tareas registradas,
        /// incluyendo los estados correspondientes a cada una.
        /// </summary>
        /// <param name="query">Objeto que contiene el número de página al que se quiere navegar</param>
        /// <returns>Lista paginada de tareas registradas</returns>
        ResultOperation<PagedList<Domain.Entities.Task>> GetAllInclude(QueryFilter query);
    }
}
