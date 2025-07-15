using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;

namespace MovieApp.Core.Contracts;

public interface IActorRepository : IBaseRepositoryWithId<Actor>
{
    public Task<IEnumerable<Actor>> GetActorsAsync(PageParameters parameters, string? name, bool trackChanges = false);
}