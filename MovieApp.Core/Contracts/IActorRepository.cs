using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Core.Contracts;

public interface IActorRepository : IBaseRepositoryWithId<Actor>
{
    public Task<PagedResult<Actor>> GetActorsAsync(
        PageParameters parameters,
        string? name,
        bool trackChanges = false);
}