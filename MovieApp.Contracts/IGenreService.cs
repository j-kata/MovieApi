using MovieApp.Core.Dtos.Genre;

namespace MovieApp.Contracts;

public interface IGenreService
{
    public Task<IEnumerable<GenreDto>> GetGenresAsync();
}