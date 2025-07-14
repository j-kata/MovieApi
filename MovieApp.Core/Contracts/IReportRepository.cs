using MovieApp.Core.ValueObjects;

namespace MovieApp.Core.Contracts;

public interface IReportRepository
{
    public Task<IEnumerable<TopMoviesByGenre>> GetTopMoviesByGenreAsync(int topCount, bool trackChanges = false);
    public Task<IEnumerable<GenreWithRating>> GetGenresAvgRatingAsync(bool trackChanges = false);
    public Task<ActorWithRolesCount?> GetActorWithMostRolesAsync(bool trackChanges = false);
    public Task<MovieWithReviewsCount?> GetFilmWithMostReviewsAsync(bool trackChanges = false);
    public Task<GenreWithMoviesCount?> GetGenreWithMostMoviesAsync(bool trackChanges = false);
}