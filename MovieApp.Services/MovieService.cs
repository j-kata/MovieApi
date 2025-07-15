using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Movie;
using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Services.Extensions;
using MovieApp.Core.Exceptions;

namespace MovieApp.Services;

public class MovieService(IUnitOfWork uow, IMapper mapper) : IMovieService
{
    public async Task<PagedResult<MovieDto>> GetMoviesAsync(MovieParameters parameters)
    {
        var result = await uow.Movies.GetMoviesAsync(parameters);
        return result.Map<Movie, MovieDto>(mapper);
    }

    public async Task<MovieDto> GetMovieAsync(int id, bool withActors = false)
    {
        var movie = await uow.Movies.GetMovieAsync(id, includeActors: withActors)
            ?? throw new NotFoundException<Movie>(id);

        // TODO: replace with general one?
        return withActors
            ? mapper.Map<MovieWithActorsDto>(movie)
            : mapper.Map<MovieDto>(movie);
    }

    public async Task<MovieDetailDto> GetMovieDetailedAsync(int id)
    {
        var movie = await uow.Movies
            .GetMovieAsync(id, includeActors: true, includeReviews: true, includeDetails: true)
            ?? throw new NotFoundException<Movie>(id);

        return mapper.Map<MovieDetailDto>(movie);
    }

    public async Task UpdateMovieAsync(int id, MovieUpdateDto updateDto)
    {
        if (!await uow.Genres.AnyByIdAsync(updateDto.GenreId))
            throw new NotFoundException<Genre>(updateDto.GenreId);

        if (id != updateDto.Id)
            return; // TODO: throw exception

        var movie = await uow.Movies.GetMovieAsync(id, includeDetails: true, trackChanges: true)
            ?? throw new NotFoundException<Movie>(id);

        mapper.Map(updateDto, movie);
        await uow.CompleteAsync();
    }

    public async Task<MovieUpdateDto> GetMovieForPatchAsync(int id)
    {
        var movie = await uow.Movies.GetMovieAsync(id, includeDetails: true)
            ?? throw new NotFoundException<Movie>(id);

        return mapper.Map<MovieUpdateDto>(movie);
    }

    public async Task<MovieDto> PostMovieAsync(MovieCreateDto createDto)
    {
        if (!await uow.Genres.AnyByIdAsync(createDto.GenreId))
            throw new NotFoundException<Genre>(createDto.GenreId);

        var movie = mapper.Map<Movie>(createDto);

        uow.Movies.Add(movie);
        await uow.CompleteAsync();

        return mapper.Map<MovieDto>(movie);
    }

    public async Task DeleteMovieAsync(int id)
    {
        if (!await uow.Movies.AnyByIdAsync(id))
            throw new NotFoundException<Movie>(id);

        uow.Movies.RemoveById(id);
        await uow.CompleteAsync();
    }
}