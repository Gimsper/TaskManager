using TaskManagerAPI.Application.Services.Interfaces;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Repositories.Interfaces;

namespace TaskManagerAPI.Application.Services
{
    public class StateService : BaseService<State>, IStateService
    {
        public StateService(IStateRepository repo) : base(repo) { }
    }
}
