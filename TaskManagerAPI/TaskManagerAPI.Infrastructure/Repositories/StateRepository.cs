using TaskManagerAPI.Domain.Context;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Repositories.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories
{
    public class StateRepository : BaseRepository<State>, IStateRepository
    {      
        public StateRepository(AppDbContext context) : base(context) { }
    }
}
