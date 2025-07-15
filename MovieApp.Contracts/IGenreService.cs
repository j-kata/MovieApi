using MovieApp.Core.Dtos.Genre;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Contracts;

public interface IGenreService
{
    public Task<PagedResult<GenreDto>> GetGenresAsync(PageParameters parameters);
}