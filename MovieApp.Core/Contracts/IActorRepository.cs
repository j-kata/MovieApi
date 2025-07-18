using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Core.Contracts;

public interface IActorRepository : IBaseRepositoryWithId<Actor>
{
    public Task<PagedResult<Actor>> GetActorsAsync(
        ActorParameters parameters,
        bool trackChanges = false);
}