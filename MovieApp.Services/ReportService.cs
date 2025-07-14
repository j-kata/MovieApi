using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Reports;

namespace MovieApp.Services;

public class ReportService(IUnitOfWork uow, IMapper mapper) : IReportService
{
    public async Task<IEnumerable<TopMoviesByGenreDto>> GetTopMoviesByGenreAsync(int topCount)
    {
        var topMovies = await uow.Reports.GetTopMoviesByGenreAsync(topCount);
        return mapper.Map<IEnumerable<TopMoviesByGenreDto>>(topMovies);
    }

    public async Task<IEnumerable<GenreWithRatingDto>> GetGenresAvgRatingAsync()
    {
        var genres = await uow.Reports.GetGenresAvgRatingAsync();
        return mapper.Map<IEnumerable<GenreWithRatingDto>>(genres);
    }

    public async Task<ActorWithRolesCountDto> GetActorWithMostRolesAsync()
    {
        var actor = await uow.Reports.GetActorWithMostRolesAsync();
        if (actor is null)
            return null!; // TODO: throw exception

        return mapper.Map<ActorWithRolesCountDto>(actor);
    }

    public async Task<MovieWithReviewsCountDto> GetFilmWithMostReviewsAsync()
    {
        var movie = await uow.Reports.GetFilmWithMostReviewsAsync();
        if (movie is null)
            return null!; // TODO: throw exception

        return mapper.Map<MovieWithReviewsCountDto>(movie);
    }

    public async Task<GenreWithMoviesCountDto> GetGenreWithMostMoviesAsync()
    {
        var genre = await uow.Reports.GetGenreWithMostMoviesAsync();
        if (genre is null)
            return null!; // TODO: throw exception

        return mapper.Map<GenreWithMoviesCountDto>(genre);
    }
}

