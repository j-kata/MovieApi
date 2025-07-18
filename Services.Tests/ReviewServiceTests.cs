using AutoMapper;
using Moq;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Dtos.Review;
using MovieApp.Core.Entities;
using MovieApp.Core.Exceptions;
using MovieApp.Core.Shared;
using MovieApp.Data;
using MovieApp.Data.Profiles;
using MovieApp.Services;
using Services.Tests.Extenstions;
using Services.Tests.Helpers;

namespace Services.Tests;

public class ReviewServiceTests
{
    private const int ReviewsCount = 5;
    private const int MovieId = 1;
    private const int ReviewId = 1;
    private const string ReviewerName = "Reviewer Name";
    private const int DefaultReviewMaxCount = 10;
    private const int OldMovieReviewMaxCount = 5;
    private const int ValidReviewCount = 3;
    private const int OldMovieAge = 20;

    private int CurrentYear => DateTime.Now.Year;
    private const int AncientYear = 1999;

    private readonly Mock<IUnitOfWork> uow;
    private readonly IMapper mapper;
    private readonly IReviewService service;

    public ReviewServiceTests()
    {
        uow = new Mock<IUnitOfWork>();
        mapper = MapperFactory.Create<ReviewProfile>();
        service = new ReviewService(uow.Object, mapper);
    }

    [Fact]
    public async Task GetReviewsAsync_ThrowsException_WhenMovieDoesNotExist()
    {
        uow.SetupMoviePresence(false);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.GetReviewsAsync(MovieId, new PageParameters()));
    }

    [Fact]
    public async Task GetReviewsAsync_ReturnsPagedResultOfDtos_WhenMovieExists()
    {
        var reviews = GenerateReviews(ReviewsCount);
        var parameters = new PageParameters();

        uow.SetupMoviePresence(true);
        uow.SetupReviewsFetch(PagedResultFactory.CreatePagedResult(reviews));

        var result = await service.GetReviewsAsync(MovieId, parameters);

        uow.Verify(u => u.Reviews.GetMovieReviewsAsync(parameters, MovieId, true, false), Times.Once);
        Assert.IsType<PagedResult<ReviewDto>>(result);
        Assert.Equal(ReviewsCount, result.Items.Count());
        Assert.Equal(reviews.First().Id, result.Items.First().Id);
    }

    [Fact]
    public async Task DeleteReviewAsync_ThrowsException_WhenReviewDoesNotExist()
    {
        uow.SetupReviewPresence(false);

        await Assert.ThrowsAsync<NotFoundException<Review>>(
            () => service.DeleteReviewAsync(ReviewId));
    }

    [Fact]
    public async Task DeleteReviewAsync_DeletesAndCompletes_WhenReviewExists()
    {
        uow.SetupReviewPresence(true);

        await service.DeleteReviewAsync(ReviewId);

        uow.Verify(u => u.Reviews.RemoveById(ReviewId), Times.Once());
        uow.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Fact]
    public async Task PostReviewAsync_ThrowsNotFound_WhenMovieDoesNotExist()
    {
        var createDto = GenerateCreateDto();
        uow.SetupMovieByIdFetch(null, trackChanges: true);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.PostReviewAsync(MovieId, createDto));
    }

    [Fact]
    public async Task PostReviewAsync_ThrowsConflict_WhenOldMovieGetsTooManyReviews()
    {
        var createDto = GenerateCreateDto();
        uow.SetupMovieByIdFetch(GenerateMovie(year: CurrentYear - OldMovieAge - 1), trackChanges: true);
        uow.SetupReviewsCount(OldMovieReviewMaxCount);

        await Assert.ThrowsAsync<ConflictException>(
            () => service.PostReviewAsync(MovieId, createDto));
    }

    [Fact]
    public async Task PostReviewAsync_ThrowsConflict_WhenNewMovieGetsTooManyReviews()
    {
        var createDto = GenerateCreateDto();
        uow.SetupMovieByIdFetch(GenerateMovie(year: CurrentYear), trackChanges: true);
        uow.SetupReviewsCount(DefaultReviewMaxCount);

        await Assert.ThrowsAsync<ConflictException>(
            () => service.PostReviewAsync(MovieId, createDto));
    }

    [Theory]
    [InlineData(2000, ValidReviewCount)]
    [InlineData(2025, ValidReviewCount)]
    public async Task PostReviewAsync_CreatesReview_WhenDataIsValid(int year, int count)
    {
        var createDto = GenerateCreateDto();
        var movie = GenerateMovie(year: year);

        uow.SetupMovieByIdFetch(movie, trackChanges: true);
        uow.SetupReviewsCount(count);

        var result = await service.PostReviewAsync(MovieId, createDto);

        uow.Verify(u => u.CompleteAsync(), Times.Once);
        Assert.IsType<ReviewDto>(result);
        Assert.Single(movie.Reviews);
        Assert.Equal(createDto.ReviewerName, result.ReviewerName);
    }

    private static Movie GenerateMovie(int id = MovieId, int year = 2020) =>
        new() { Id = id, Year = year };

    private static IEnumerable<Review> GenerateReviews(int count)
    {
        var genres = DbInitializer.GenerateGenres(1);
        var movies = DbInitializer.GenerateMovies(1, genres);
        return DbInitializer.GenerateReviews(count, movies);
    }

    private static ReviewCreateDto GenerateCreateDto(int movieId = MovieId, string reviewerName = ReviewerName) =>
        new() { MovieId = movieId, ReviewerName = reviewerName };
}