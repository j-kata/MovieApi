using AutoMapper;
using Moq;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Movie;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Entities;
using MovieApp.Core.Exceptions;
using MovieApp.Core.Shared;
using MovieApp.Data;
using MovieApp.Data.Profiles;
using MovieApp.Services;
using Services.Tests.Extenstions;
using Services.Tests.Helpers;


namespace Services.Tests;

public class MovieServiceTests
{
    private const int MoviesCount = 10;
    private const int MovieId = 1;
    private const string MovieTitle = "Title";
    private const string Documentary = "Documentary";
    private const string Horror = "Horror";
    private const int BigBudget = 1_000_001;
    private const int SmallBudget = 500_000;

    private readonly Mock<IUnitOfWork> uow;
    private readonly IMapper mapper;
    private readonly IMovieService service;

    public MovieServiceTests()
    {
        uow = new Mock<IUnitOfWork>();
        mapper = MapperFactory.Create<MovieProfile>();
        service = new MovieService(uow.Object, mapper);
    }

    [Fact]
    public async Task GetMoviesAsync_ReturnsPagedResultOfDtos()
    {
        var movies = GenerateMovies(MoviesCount);
        var parameters = new MovieParameters();

        uow.SetupMoviesFetch(PagedResultFactory.CreatePagedResult(movies));

        var result = await service.GetMoviesAsync(parameters);

        uow.Verify(u => u.Movies.GetMoviesAsync(parameters, false), Times.Once);
        Assert.IsType<PagedResult<MovieDto>>(result);
        Assert.Equal(MoviesCount, result.Items.Count());
        Assert.Equal(movies.First().Id, result.Items.First().Id);
    }

    [Fact]
    public async Task GetMovieAsync_ThrowsException_WhenMovieDoesNotExist()
    {
        uow.SetupMovieFetch(null);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.GetMovieAsync(MovieId));
    }

    [Fact]
    public async Task GetMovieAsync_ReturnsMovieDto_WhenWithActorsIsFalse()
    {
        var movie = GenerateMovie();
        uow.SetupMovieFetch(movie);

        var result = await service.GetMovieAsync(MovieId);

        Assert.IsType<MovieDto>(result);
        AssertDtoMatchesEntity(movie, result);
    }

    [Fact]
    public async Task GetMovieAsync_ReturnsMovieWithActorsDto_WhenWithActorsIsTrue()
    {
        var movie = GenerateMovie();
        uow.SetupMovieFetch(movie, includeActors: true);

        var result = await service.GetMovieAsync(MovieId, withActors: true);

        Assert.IsType<MovieWithActorsDto>(result);
        AssertDtoMatchesEntity(movie, result);
    }

    [Fact]
    public async Task GetMovieDetailedAsync_ThrowsException_WhenMovieDoesNotExists()
    {
        uow.SetupMovieFetch(null, includeActors: true, includeDetails: true, includeReviews: true);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.GetMovieDetailedAsync(MovieId));
    }

    [Fact]
    public async Task GetMovieDetailedAsync_ReturnsMovieDetailDto_WhenMovieExists()
    {
        var movie = GenerateMovie();
        uow.SetupMovieFetch(movie, includeActors: true, includeDetails: true, includeReviews: true);

        var result = await service.GetMovieDetailedAsync(MovieId);

        Assert.IsType<MovieDetailDto>(result);
        AssertDtoMatchesEntity(movie, result);
    }

    [Fact]
    public async Task GetMovieForPatchAsync_ThrowsException_WhenMovieDoesNotExist()
    {
        uow.SetupMovieFetch(null, includeDetails: true);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.GetMovieForPatchAsync(MovieId));
    }

    [Fact]
    public async Task GetMovieForPatchAsync_ReturnsMovieUpdateDto_WhenMovieExists()
    {
        var movie = GenerateMovie();
        uow.SetupMovieFetch(movie, includeDetails: true);

        var result = await service.GetMovieForPatchAsync(MovieId);

        Assert.IsType<MovieUpdateDto>(result);
        Assert.Equal(movie.Id, result.Id);
        Assert.Equal(movie.Title, result.Title);
    }

    [Fact]
    public async Task DeleteMovieAsync_ThrowsException_WhenMovieDoesNotExist()
    {
        uow.SetupMoviePresence(false);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.DeleteMovieAsync(MovieId));
    }

    [Fact]
    public async Task DeleteMovieAsync_DeletesMovieAndCompletes_WhenMovieExists()
    {
        uow.SetupMoviePresence(true);

        await service.DeleteMovieAsync(MovieId);

        uow.Verify(u => u.Movies.RemoveById(MovieId), Times.Once());
        uow.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Fact]
    public async Task UpdateMovieAsync_ThrowsBadRequest_WhenIdsDontMatch()
    {
        var updateDto = GenerateUpdateDto();

        await Assert.ThrowsAsync<BadRequestException>(
            () => service.UpdateMovieAsync(MovieId + 1, updateDto));
    }

    [Fact]
    public async Task UpdateMovieAsync_ThrowsNotFound_WhenMovieDoesNotExist()
    {
        var updateDto = GenerateUpdateDto();
        uow.SetupMovieFetch(null, includeDetails: true);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.UpdateMovieAsync(MovieId, updateDto));
    }

    [Fact]
    public async Task UpdateMovieAsync_ThrowsNotFound_WhenGenreDoesNotExist()
    {
        var updateDto = GenerateUpdateDto();

        uow.SetupMovieFetch(GenerateMovie(), includeDetails: true, trackChanges: true);
        uow.SetupGenreFetch(null);

        await Assert.ThrowsAsync<NotFoundException<Genre>>(
            () => service.UpdateMovieAsync(MovieId, updateDto));
    }

    [Fact]
    public async Task UpdateMovieAsync_ThrowsConflict_WhenDocumentaryHasBudgetOver1M()
    {
        var updateDto = GenerateUpdateDto();

        uow.SetupMovieFetch(GenerateMovie(), includeDetails: true, trackChanges: true);
        uow.SetupGenreFetch(GenerateGenre(name: Documentary));

        await Assert.ThrowsAsync<ConflictException>(
            () => service.UpdateMovieAsync(MovieId, updateDto));
    }

    // TODO: check with ids (same object och another one)
    [Fact]
    public async Task UpdateMovieAsync_ThrowsConflict_WhenMovieNameIsDuplicated()
    {
        var updateDto = GenerateUpdateDto();

        uow.SetupMovieFetch(GenerateMovie(), includeDetails: true, trackChanges: true);
        uow.SetupGenreFetch(GenerateGenre());
        uow.SetupMovieDuplicate(true);

        await Assert.ThrowsAsync<ConflictException>(
            () => service.UpdateMovieAsync(MovieId, updateDto));
    }

    [Theory]
    [InlineData(Documentary, SmallBudget)]
    [InlineData(Horror, BigBudget)]
    public async Task UpdateMovieAsync_UpdatesMovieAndCompletes_WhenDataIsValid(string genreName, int budget)
    {
        var updateDto = GenerateUpdateDto(budget: budget);

        uow.SetupMovieFetch(GenerateMovie(), includeDetails: true, trackChanges: true);
        uow.SetupGenreFetch(GenerateGenre(name: genreName));
        uow.SetupMovieDuplicate(false);

        await service.UpdateMovieAsync(MovieId, updateDto);

        uow.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task PostMovieAsync_ThrowsNotFound_WhenGenreDoesNotExist()
    {
        var createDto = GenerateCreateDto();
        uow.SetupGenreFetch(null);

        await Assert.ThrowsAsync<NotFoundException<Genre>>(
            () => service.PostMovieAsync(createDto));
    }

    [Fact]
    public async Task PostMovieAsync_ThrowsConflict_WhenDocumentaryHasBudgetOver1M()
    {
        var createDto = GenerateCreateDto();
        uow.SetupGenreFetch(GenerateGenre(name: Documentary));

        await Assert.ThrowsAsync<ConflictException>(
            () => service.PostMovieAsync(createDto));
    }

    [Fact]
    public async Task PostMovieAsync_ThrowsConflict_WhenMovieNameIsDuplicated()
    {
        var createDto = GenerateCreateDto();

        uow.SetupGenreFetch(GenerateGenre());
        uow.SetupMovieDuplicate(true);

        await Assert.ThrowsAsync<ConflictException>(
            () => service.PostMovieAsync(createDto));
    }

    [Theory]
    [InlineData(Documentary, SmallBudget)]
    [InlineData(Horror, BigBudget)]
    public async Task PostMovieAsync_CreatesMovieAndCompletes_WhenDataIsValid(string genreName, int budget)
    {
        var createDto = GenerateCreateDto(budget: budget);
        Movie? createdMovie = null;

        uow.SetupGenreFetch(GenerateGenre(name: genreName));
        uow.SetupMovieDuplicate(false);
        uow.Setup(u => u.Movies.Add(It.IsAny<Movie>()))
            .Callback<Movie>(m => createdMovie = m);

        var result = await service.PostMovieAsync(createDto);

        uow.Verify(u => u.Movies.Add(It.IsAny<Movie>()), Times.Once);
        uow.Verify(u => u.CompleteAsync(), Times.Once);
        Assert.NotNull(createdMovie);
        Assert.IsType<MovieDto>(result);
        Assert.Equal(createDto.Title, result.Title);
        Assert.Equal(createDto.Title, createdMovie.Title);
    }

    private static Movie GenerateMovie(int id = MovieId, string title = MovieTitle) =>
        new() { Id = id, Title = title };

    private static Genre GenerateGenre(int id = 1, string name = Horror) =>
        new() { Id = id, Name = name };

    private static MovieUpdateDto GenerateUpdateDto(int id = MovieId, string title = MovieTitle, int budget = BigBudget) =>
        new() { Id = id, Title = title, Budget = budget };

    private static MovieCreateDto GenerateCreateDto(string title = MovieTitle, int budget = BigBudget) =>
        new() { Title = title, Budget = budget };

    private static IEnumerable<Movie> GenerateMovies(int count)
    {
        var genres = DbInitializer.GenerateGenres(5);
        return DbInitializer.GenerateMovies(count, genres);
    }

    private static void AssertDtoMatchesEntity(Movie movie, MovieDto dto)
    {
        Assert.Equal(movie.Id, dto.Id);
        Assert.Equal(movie.Title, dto.Title);
        Assert.Equal(movie.Year, dto.Year);
    }
}
