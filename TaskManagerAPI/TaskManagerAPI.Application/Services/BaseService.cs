using TaskManagerAPI.Application.Services.Interfaces;
using TaskManagerAPI.Domain.Models.Dto;
using TaskManagerAPI.Infrastructure.Repositories.Interfaces;

namespace TaskManagerAPI.Application.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly IBaseRepository<T> _repo;

        public BaseService(IBaseRepository<T> repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Obtiene todos los registros de la entidad de tipo T
        /// </summary>
        /// <returns>Registros de la entidad tipo T</returns>
        public ResultOperation<T> GetAll()
        {
            var result = new ResultOperation<T>();
            try
            {
                result.Results = _repo.GetAll();
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
        /// Obtiene un registro de la entidad de tipo T, filtrando por medio del identificador suministrado
        /// </summary>
        /// <param name="id">Identificador del registro</param>
        /// <returns>Registro de la entidad tipo T</returns>
        public ResultOperation<T> GetById(int id)
        {
            var result = new ResultOperation<T>();
            try
            {
                result.Result = _repo.GetById(id);
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
        /// Obtiene todos los registros de la entidad de tipo T, que cumplan con la condición del callback suministrado
        /// </summary>
        /// <param name="filter">Condición para filtrar registros (Callback)</param>
        /// <returns>Registros filtrados de la entidad tipo T</returns>
        public ResultOperation<T> GetBy(Func<T, bool> filter)
        {
            var result = new ResultOperation<T>();
            try
            {
                result.Results = _repo.GetBy(filter);
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
        /// Obtiene todos los registros de la entidad tipo T, incluyendo el objeto de la propiedad de navegación suministrada
        /// </summary>
        /// <param name="includePath">Ruta de la propiedad de navegación a incluir</param>
        /// <returns>Registros de la entidad tipo T</returns>
        public ResultOperation<T> GetAllInclude(string includePath)
        {
            var result = new ResultOperation<T>();
            try
            {
                result.Results = _repo.GetAllInclude(includePath);
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
        /// Crea un nuevo registro de la entidad de tipo T, por medio de la información suministrada
        /// </summary>
        /// <param name="entity">Información del nuevo registro</param>
        /// <returns>Confirmación de creación (bool)</returns>
        public ResultOperation Add(T entity)
        {
            var result = new ResultOperation();
            try
            {
                _repo.Add(entity);
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
        /// Edita un registro de la entidad existente de tipo T, por medio de la información suministrada
        /// </summary>
        /// <param name="entity">Información nueva para el registro</param>
        /// <returns>Confirmación de actualización (bool)</returns>
        public ResultOperation Update(T entity)
        {
            var result = new ResultOperation();
            try
            {
                _repo.Update(entity);
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
        /// Elimina un registro de la entidad existente de tipo T, por medio de su identificador
        /// </summary>
        /// <param name="id">Identificador del registro a eliminar</param>
        /// <returns>Confirmación de eliminación (bool)</returns>
        public ResultOperation Delete(int id)
        {
            var result = new ResultOperation();
            try
            {
                _repo.Delete(id);
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
