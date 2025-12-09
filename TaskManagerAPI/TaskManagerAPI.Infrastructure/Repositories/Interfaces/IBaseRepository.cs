namespace TaskManagerAPI.Infrastructure.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        List<T> GetAll();
        T? GetById(int id);
        List<T> GetBy(Func<T, bool> func);
        List<T> GetAllInclude(string includePath);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
