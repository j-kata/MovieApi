using MovieApp.Core.Dtos.Movie;
using MovieApp.Core.Parameters;

namespace MovieApp.Contracts;

public interface IMovieService
{
    public Task<IEnumerable<MovieDto>> GetMoviesAsync(MovieParameters? parameters);
    public Task<MovieDto> GetMovieAsync(int id, bool withActors = false);
    public Task<MovieDetailDto> GetMovieDetailedAsync(int id);
    public Task UpdateMovieAsync(int id, MovieUpdateDto updateDto);
    public Task<MovieUpdateDto> GetMovieForPatchAsync(int id);
    public Task<MovieDto> PostMovieAsync(MovieCreateDto createDto);
    public Task DeleteMovieAsync(int id);
}