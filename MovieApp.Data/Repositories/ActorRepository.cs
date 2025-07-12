using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class ActorRepository(MovieContext context)
    : BaseRepository<Actor>(context), IActorRepository
{
    public async Task<IEnumerable<Actor>> GetActorsAsync(string? name, bool trackChanges = false)
    {
        var result = name is null
            ? FindAll(trackChanges: trackChanges)
            : FindBy(a => EF.Functions.Like(a.Name, $"%{name}%"), trackChanges);

        return await result.ToListAsync();
    }
}
