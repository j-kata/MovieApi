using MovieApp.Core.Dtos.Reports;

namespace MovieApp.Contracts;

public interface IReportService
{
    public Task<IEnumerable<TopMoviesByGenreDto>> GetTopMoviesByGenreAsync(int topCount);
    public Task<IEnumerable<GenreWithRatingDto>> GetGenresAvgRatingAsync();
    public Task<ActorWithRolesCountDto> GetActorWithMostRolesAsync();
    public Task<MovieWithReviewsCountDto> GetFilmWithMostReviewsAsync();
    public Task<GenreWithMoviesCountDto> GetGenreWithMostMoviesAsync();
}