using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Reports;
using MovieApp.Core.Entities;
using MovieApp.Core.Exceptions;

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
        var actor = await uow.Reports.GetActorWithMostRolesAsync()
            ?? throw new NotFoundException<Actor>("No data available to generate this report.");

        return mapper.Map<ActorWithRolesCountDto>(actor);
    }

    public async Task<MovieWithReviewsCountDto> GetFilmWithMostReviewsAsync()
    {
        var movie = await uow.Reports.GetFilmWithMostReviewsAsync()
            ?? throw new NotFoundException<Movie>("No data available to generate this report.");

        return mapper.Map<MovieWithReviewsCountDto>(movie);
    }

    public async Task<GenreWithMoviesCountDto> GetGenreWithMostMoviesAsync()
    {
        var genre = await uow.Reports.GetGenreWithMostMoviesAsync()
            ?? throw new NotFoundException<Genre>("No data available to generate this report.");

        return mapper.Map<GenreWithMoviesCountDto>(genre);
    }
}

