using System.Linq.Expressions;
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
using Services.Tests.Helpers;

namespace Services.Tests;

public class MovieServiceTests
{
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
        var movieCount = 10;
        var movies = GenerateMovies(movieCount);
        var parameters = new MovieParameters();
        var pageMeta = new PaginationMeta { PageIndex = 1, PageSize = 10, TotalCount = movieCount };
        var pagedMovies = new PagedResult<Movie>(items: movies, details: pageMeta);

        uow.Setup(u => u.Movies.GetMoviesAsync(parameters, false))
            .ReturnsAsync(pagedMovies);

        var result = await service.GetMoviesAsync(parameters);

        uow.Verify(u => u.Movies.GetMoviesAsync(parameters, false), Times.Once);
        Assert.IsType<PagedResult<MovieDto>>(result);
        Assert.Equivalent(pageMeta, result.Details);
        Assert.Equal(movieCount, result.Items.Count());
        Assert.Equal(movies.First().Id, result.Items.First().Id);
    }

    [Fact]
    public async Task GetMovieAsync_ThrowsException_WhenMovieDoesNotExist()
    {
        SetupMovieFetch(null);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.GetMovieAsync(MovieId));
    }

    [Fact]
    public async Task GetMovieAsync_ReturnsMovieDto_WhenWithActorsIsFalse()
    {
        var movie = GenerateMovie();
        SetupMovieFetch(movie);

        var result = await service.GetMovieAsync(MovieId);

        Assert.IsType<MovieDto>(result);
        AssertDtoMatchesEntity(movie, result);
    }

    [Fact]
    public async Task GetMovieAsync_ReturnsMovieWithActorsDto_WhenWithActorsIsTrue()
    {
        var movie = GenerateMovie();
        SetupMovieFetch(movie, includeActors: true);

        var result = await service.GetMovieAsync(MovieId, withActors: true);

        Assert.IsType<MovieWithActorsDto>(result);
        AssertDtoMatchesEntity(movie, result);
    }

    [Fact]
    public async Task GetMovieDetailedAsync_ThrowsException_WhenMovieDoesNotExists()
    {
        SetupMovieFetch(null, includeActors: true, includeDetails: true, includeReviews: true);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.GetMovieDetailedAsync(MovieId));
    }

    [Fact]
    public async Task GetMovieDetailedAsync_ReturnsMovieDetailDto_WhenMovieExists()
    {
        var movie = GenerateMovie();
        SetupMovieFetch(movie, includeActors: true, includeDetails: true, includeReviews: true);

        var result = await service.GetMovieDetailedAsync(MovieId);

        Assert.IsType<MovieDetailDto>(result);
        AssertDtoMatchesEntity(movie, result);
    }

    [Fact]
    public async Task GetMovieForPatchAsync_ThrowsException_WhenMovieDoesNotExist()
    {
        SetupMovieFetch(null, includeDetails: true);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.GetMovieForPatchAsync(MovieId));
    }

    [Fact]
    public async Task GetMovieForPatchAsync_ReturnsMovieUpdateDto_WhenMovieExists()
    {
        var movie = GenerateMovie();
        SetupMovieFetch(movie, includeDetails: true);

        var result = await service.GetMovieForPatchAsync(MovieId);

        Assert.IsType<MovieUpdateDto>(result);
        Assert.Equal(movie.Id, result.Id);
        Assert.Equal(movie.Title, result.Title);
    }

    [Fact]
    public async Task DeleteMovieAsync_ThrowsException_WhenMovieDoesNotExist()
    {
        SetupMoviePresence(false);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.DeleteMovieAsync(MovieId));
    }

    [Fact]
    public async Task DeleteMovieAsync_DeletesMovieAndCompletes_WhenMovieExists()
    {
        SetupMoviePresence(true);

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
        SetupMovieFetch(null, includeDetails: true);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.UpdateMovieAsync(MovieId, updateDto));
    }

    [Fact]
    public async Task UpdateMovieAsync_ThrowsNotFound_WhenGenreDoesNotExist()
    {
        var updateDto = GenerateUpdateDto();

        SetupMovieFetch(GenerateMovie(), includeDetails: true, trackChanges: true);
        SetupGenreFetch(null);

        await Assert.ThrowsAsync<NotFoundException<Genre>>(
            () => service.UpdateMovieAsync(MovieId, updateDto));
    }

    [Fact]
    public async Task UpdateMovieAsync_ThrowsConflict_WhenDocumentaryHasBudgetOver1M()
    {
        var updateDto = GenerateUpdateDto();

        SetupMovieFetch(GenerateMovie(), includeDetails: true, trackChanges: true);
        SetupGenreFetch(GenerateGenre(name: Documentary));

        await Assert.ThrowsAsync<ConflictException>(
            () => service.UpdateMovieAsync(MovieId, updateDto));
    }

    [Fact]
    public async Task UpdateMovieAsync_ThrowsConflict_WhenMovieNameIsDuplicated()
    {
        var updateDto = GenerateUpdateDto();

        SetupMovieFetch(GenerateMovie(), includeDetails: true, trackChanges: true);
        SetupGenreFetch(GenerateGenre());
        SetupMovieDuplicate(true);

        await Assert.ThrowsAsync<ConflictException>(
            () => service.UpdateMovieAsync(MovieId, updateDto));
    }

    [Theory]
    [InlineData(Documentary, SmallBudget)]
    [InlineData(Horror, BigBudget)]
    public async Task UpdateMovieAsync_UpdatesMovieAndCompletes_WhenDataIsValid(string genreName, int budget)
    {
        var updateDto = GenerateUpdateDto(budget: budget);

        SetupMovieFetch(GenerateMovie(), includeDetails: true, trackChanges: true);
        SetupGenreFetch(GenerateGenre(name: genreName));
        SetupMovieDuplicate(false);

        await service.UpdateMovieAsync(MovieId, updateDto);

        uow.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task PostMovieAsync_ThrowsNotFound_WhenGenreDoesNotExist()
    {
        var createDto = GenerateCreateDto();
        SetupGenreFetch(null);

        await Assert.ThrowsAsync<NotFoundException<Genre>>(
            () => service.PostMovieAsync(createDto));
    }

    [Fact]
    public async Task PostMovieAsync_ThrowsConflict_WhenDocumentaryHasBudgetOver1M()
    {
        var createDto = GenerateCreateDto();
        SetupGenreFetch(GenerateGenre(name: Documentary));

        await Assert.ThrowsAsync<ConflictException>(
            () => service.PostMovieAsync(createDto));
    }

    [Fact]
    public async Task PostMovieAsync_ThrowsConflict_WhenMovieNameIsDuplicated()
    {
        var createDto = GenerateCreateDto();

        SetupGenreFetch(GenerateGenre());
        SetupMovieDuplicate(true);

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

        SetupGenreFetch(GenerateGenre(name: genreName));
        SetupMovieDuplicate(false);
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

    private void SetupMovieFetch(
        Movie? movie,
        bool includeActors = false,
        bool includeDetails = false,
        bool includeReviews = false,
        bool trackChanges = false) =>
            uow.Setup(u => u.Movies.GetMovieAsync(
                It.IsAny<int>(),
                includeActors,
                includeDetails,
                includeReviews,
                trackChanges))
            .ReturnsAsync(movie);

    private void SetupGenreFetch(Genre? genre) =>
        uow.Setup(u => u.Genres.GetByIdAsync(It.IsAny<int>(), false))
            .ReturnsAsync(genre);

    private void SetupMovieDuplicate(bool duplicate) =>
        uow.Setup(u => u.Movies.AnyAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync(duplicate);

    private void SetupMoviePresence(bool isPresent) =>
        uow.Setup(u => u.Movies.AnyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(isPresent);

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
