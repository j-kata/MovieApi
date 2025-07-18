using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Data.Extensions;

namespace MovieApp.Data.Repositories;

public class ActorRepository(MovieContext context)
    : BaseRepositoryWithId<Actor>(context), IActorRepository
{
    public async Task<PagedResult<Actor>> GetActorsAsync(
        ActorParameters parameters,
        bool trackChanges = false)
    {
        var query = parameters.Name is null
            ? FindAll(trackChanges: trackChanges)
            : FindBy(a => EF.Functions.Like(a.Name, $"%{parameters.Name}%"), trackChanges);

        return await query.ToPagedResultAsync(parameters.PageSize, parameters.PageIndex);
    }
}
