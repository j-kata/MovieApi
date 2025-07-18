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
using Services.Tests.Helpers;

namespace Services.Tests;

public class ReviewServiceTests
{
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
        SetupMoviePresence(false);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.GetReviewsAsync(MovieId, new PageParameters()));
    }

    [Fact]
    public async Task GetReviewsAsync_ReturnsPagedResultOfDtos_WhenMovieExists()
    {
        var reviewsCount = 5;
        var reviews = GenerateReviews(reviewsCount);
        var parameters = new PageParameters();
        var pageMeta = new PaginationMeta { PageIndex = 1, PageSize = 10, TotalCount = reviewsCount };
        var pagedReviews = new PagedResult<Review>(items: reviews, details: pageMeta);

        SetupMoviePresence(true);
        uow.Setup(u => u.Reviews.GetMovieReviewsAsync(parameters, MovieId, true, false))
            .ReturnsAsync(pagedReviews);

        var result = await service.GetReviewsAsync(MovieId, parameters);

        uow.Verify(u => u.Reviews.GetMovieReviewsAsync(parameters, MovieId, true, false), Times.Once);
        Assert.IsType<PagedResult<ReviewDto>>(result);
        Assert.Equivalent(pageMeta, result.Details);
        Assert.Equal(reviewsCount, result.Items.Count());
        Assert.Equal(reviews.First().Id, result.Items.First().Id);
    }

    [Fact]
    public async Task DeleteReviewAsync_ThrowsException_WhenReviewDoesNotExist()
    {
        SetupReviewPresence(false);

        await Assert.ThrowsAsync<NotFoundException<Review>>(
            () => service.DeleteReviewAsync(ReviewId));
    }

    [Fact]
    public async Task DeleteReviewAsync_DeletesAndCompletes_WhenReviewExists()
    {
        SetupReviewPresence(true);

        await service.DeleteReviewAsync(ReviewId);

        uow.Verify(u => u.Reviews.RemoveById(ReviewId), Times.Once());
        uow.Verify(u => u.CompleteAsync(), Times.Once());
    }

    [Fact]
    public async Task PostReviewAsync_ThrowsNotFound_WhenMovieDoesNotExist()
    {
        var createDto = GenerateCreateDto();
        SetupMovieFetch(null, trackChanges: true);

        await Assert.ThrowsAsync<NotFoundException<Movie>>(
            () => service.PostReviewAsync(MovieId, createDto));
    }

    [Fact]
    public async Task PostReviewAsync_ThrowsConflict_WhenOldMovieGetsTooManyReviews()
    {
        var createDto = GenerateCreateDto();
        SetupMovieFetch(GenerateMovie(year: CurrentYear - OldMovieAge - 1), trackChanges: true);
        SetupReviewsCount(OldMovieReviewMaxCount);

        await Assert.ThrowsAsync<ConflictException>(
            () => service.PostReviewAsync(MovieId, createDto));
    }

    [Fact]
    public async Task PostReviewAsync_ThrowsConflict_WhenNewMovieGetsTooManyReviews()
    {
        var createDto = GenerateCreateDto();
        SetupMovieFetch(GenerateMovie(year: CurrentYear), trackChanges: true);
        SetupReviewsCount(DefaultReviewMaxCount);

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

        SetupMovieFetch(movie, trackChanges: true);
        SetupReviewsCount(count);

        var result = await service.PostReviewAsync(MovieId, createDto);

        uow.Verify(u => u.CompleteAsync(), Times.Once);
        Assert.IsType<ReviewDto>(result);
        Assert.Single(movie.Reviews);
        Assert.Equal(createDto.ReviewerName, result.ReviewerName);
    }

    private void SetupMoviePresence(bool isPresent) =>
        uow.Setup(u => u.Movies.AnyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(isPresent);

    private void SetupReviewPresence(bool isPresent) =>
        uow.Setup(u => u.Reviews.AnyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(isPresent);

    private void SetupMovieFetch(
        Movie? movie,
        bool trackChanges = false) =>
            uow.Setup(u => u.Movies.GetByIdAsync(
                It.IsAny<int>(),
                trackChanges))
            .ReturnsAsync(movie);

    private void SetupReviewsCount(int count) =>
        uow.Setup(u => u.Reviews.GetMovieReviewsCountAsync(It.IsAny<int>()))
            .ReturnsAsync(count);

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