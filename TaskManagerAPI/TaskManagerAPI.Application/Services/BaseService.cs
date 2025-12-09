using TaskManagerAPI.Application.Services.Interfaces;
using TaskManagerAPI.Domain.Models.Dto;
using TaskManagerAPI.Infrastructure.Repositories.Interfaces;
using static Dapper.SqlMapper;

namespace TaskManagerAPI.Application.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly IBaseRepository<T> _repo;

        public BaseService(IBaseRepository<T> repo)
        {
            _repo = repo;
        }

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
