using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;
using MovieApp.Data.Extensions;

namespace MovieApp.Data.Repositories;

public class ActorRepository(MovieContext context)
    : BaseRepositoryWithId<Actor>(context), IActorRepository
{
    public async Task<IEnumerable<Actor>> GetActorsAsync(PageParameters parameters, string? name, bool trackChanges = false)
    {
        var result = name is null
            ? FindAll(trackChanges: trackChanges)
            : FindBy(a => EF.Functions.Like(a.Name, $"%{name}%"), trackChanges);

        return await result.WithOffset(parameters.PageSize, parameters.PageIndex).ToListAsync();
    }
}
