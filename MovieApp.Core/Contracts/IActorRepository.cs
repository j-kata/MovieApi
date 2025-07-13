using MovieApp.Core.Entities;

namespace MovieApp.Core.Contracts;

public interface IActorRepository : IBaseRepositoryWithId<Actor>
{
    public Task<IEnumerable<Actor>> GetActorsAsync(string? name, bool trackChanges = false);
}