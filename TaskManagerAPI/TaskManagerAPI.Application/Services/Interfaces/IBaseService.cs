using TaskManagerAPI.Domain.Models.Dto;

namespace TaskManagerAPI.Application.Services.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        ResultOperation<T> GetAll();
        ResultOperation<T> GetById(int id);
        ResultOperation<T> GetBy(Func<T, bool> filter);
        ResultOperation<T> GetAllInclude(string includePath);
        ResultOperation Add(T entity);
        ResultOperation Update(T entity);
        ResultOperation Delete(int id);
    }
}
