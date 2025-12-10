using TaskManagerAPI.Domain.Models.Dto;

namespace TaskManagerAPI.Application.Services.Interfaces
{
    public interface IBaseService<T> where T : class
    {        
        /// <summary>
        /// Obtiene todos los registros de la entidad de tipo T
        /// </summary>
        /// <returns>Registros de la entidad tipo T</returns>
        ResultOperation<T> GetAll();

        /// <summary>
        /// Obtiene un registro de la entidad de tipo T, filtrando por medio del identificador suministrado
        /// </summary>
        /// <param name="id">Identificador del registro</param>
        /// <returns>Registro de la entidad tipo T</returns>
        ResultOperation<T> GetById(int id);

        /// Obtiene todos los registros de la entidad de tipo T, que cumplan con la condición del callback suministrado
        /// </summary>
        /// <param name="filter">Condición para filtrar registros (Callback)</param>
        /// <returns>Registros filtrados de la entidad tipo T</returns>
        ResultOperation<T> GetBy(Func<T, bool> filter);

        /// <summary>
        /// Obtiene todos los registros de la entidad tipo T, incluyendo el objeto de la propiedad de navegación suministrada
        /// </summary>
        /// <param name="includePath">Ruta de la propiedad de navegación a incluir</param>
        /// <returns>Registros de la entidad tipo T</returns>
        ResultOperation<T> GetAllInclude(string includePath);

        /// <summary>
        /// Crea un nuevo registro de la entidad de tipo T, por medio de la información suministrada
        /// </summary>
        /// <param name="entity">Información del nuevo registro</param>
        /// <returns>Confirmación de creación (bool)</returns>
        ResultOperation Add(T entity);

        /// <summary>
        /// Edita un registro de la entidad existente de tipo T, por medio de la información suministrada
        /// </summary>
        /// <param name="entity">Información nueva para el registro</param>
        /// <returns>Confirmación de actualización (bool)</returns>
        ResultOperation Update(T entity);

        /// <summary>
        /// Elimina un registro de la entidad existente de tipo T, por medio de su identificador
        /// </summary>
        /// <param name="id">Identificador del registro a eliminar</param>
        /// <returns>Confirmación de eliminación (bool)</returns>
        ResultOperation Delete(int id);
    }
}
