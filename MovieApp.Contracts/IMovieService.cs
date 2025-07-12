using MovieApp.Core.Dtos.Movie;
using MovieApp.Core.Parameters;

namespace MovieApp.Contracts;

public interface IMovieService
{
    public Task<IEnumerable<MovieDto>> GetMoviesAsync(MovieParameters? parameters);
    public Task<MovieDto> GetMovieAsync(int id, bool withActors = false);
    public Task<MovieDetailDto> GetMovieDetailsAsync(int id);
    public Task PutMovieAsync(int id, MovieUpdateDto updateDto);
    public Task<MovieDto> PostMovieAsync(MovieCreateDto createDto);
    public Task DeleteMovieAsync(int id);
}