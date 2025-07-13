using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;

namespace MovieApp.Data.Repositories;

public class MovieRepository(MovieContext context) : BaseRepositoryWithId<Movie>(context), IMovieRepository
{
    public async Task<IEnumerable<Movie>> GetMoviesAsync(MovieParameters? parameters, bool trackChanges = false)
    {
        var query = FindAll(trackChanges: trackChanges);
        if (parameters?.Title is not null)
            query = query.Where(m => EF.Functions.Like(m.Title, $"%{parameters.Title}%"));
        if (parameters?.Year is not null)
            query = query.Where(m => m.Year == parameters.Year);
        if (parameters?.Genre is not null)
            query = query.Where(m => EF.Functions.Like(m.Genre.Name, parameters.Genre));
        if (parameters?.Actor is not null)
            query = query.Where(m => m.Roles.Any(a => EF.Functions.Like(a.Actor.Name, $"%{parameters.Actor}%")));

        return await query.Include(m => m.Genre).ToListAsync();
    }

    public Task<Movie?> GetMovieAsync(
        int id,
        bool includeActors = false,
        bool includeDetails = false,
        bool includeReviews = false,
        bool trackChanges = false)
    {
        var query = FindBy(m => m.Id == id, trackChanges);
        if (includeActors)
            query = query.Include(m => m.Roles).ThenInclude(r => r.Actor);
        if (includeDetails)
            query = query.Include(m => m.MovieDetail);
        if (includeReviews)
            query = query.Include(m => m.Reviews);

        return query.Include(m => m.Genre).FirstOrDefaultAsync();
    }
}
