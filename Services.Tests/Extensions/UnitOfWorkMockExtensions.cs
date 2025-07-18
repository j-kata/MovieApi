using System.Linq.Expressions;
using Moq;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Entities;
using MovieApp.Core.Shared;

namespace Services.Tests.Extenstions;

public static class UnitOfWorkMockExtenstions
{
    public static void SetupMoviesFetch(
        this Mock<IUnitOfWork> uow, PagedResult<Movie> movies, bool trackChanges = false) =>
            uow.Setup(u => u.Movies.GetMoviesAsync(It.IsAny<MovieParameters>(), trackChanges))
                .ReturnsAsync(movies);

    public static void SetupActorsFetch(
        this Mock<IUnitOfWork> uow, PagedResult<Actor> actors, bool trackChanges = false) =>
            uow.Setup(u => u.Actors.GetActorsAsync(It.IsAny<ActorParameters>(), trackChanges))
                .ReturnsAsync(actors);

    public static void SetupReviewsFetch(
        this Mock<IUnitOfWork> uow, PagedResult<Review> reviews,
        bool latestFirst = true, bool trackChanges = false) =>
            uow.Setup(u => u.Reviews.GetMovieReviewsAsync(It.IsAny<PageParameters>(), It.IsAny<int>(), latestFirst, trackChanges))
                .ReturnsAsync(reviews);

    public static void SetupActorFetch(this Mock<IUnitOfWork> uow, Actor? actor, bool trackChanges = false) =>
        uow.Setup(u => u.Actors.GetByIdAsync(It.IsAny<int>(), trackChanges))
            .ReturnsAsync(actor);

    public static void SetupGenreFetch(this Mock<IUnitOfWork> uow, Genre? genre) =>
        uow.Setup(u => u.Genres.GetByIdAsync(It.IsAny<int>(), false))
            .ReturnsAsync(genre);

    public static void SetupMovieFetch(this Mock<IUnitOfWork> uow, Movie? movie,
        bool includeActors = false, bool includeDetails = false,
        bool includeReviews = false, bool trackChanges = false) =>
            uow.Setup(u => u.Movies.GetMovieAsync(
                It.IsAny<int>(),
                includeActors,
                includeDetails,
                includeReviews,
                trackChanges))
            .ReturnsAsync(movie);

    public static void SetupMovieByIdFetch(this Mock<IUnitOfWork> uow, Movie? movie, bool trackChanges = false) =>
        uow.Setup(u => u.Movies.GetByIdAsync(It.IsAny<int>(), trackChanges))
            .ReturnsAsync(movie);

    public static void SetupMoviePresence(this Mock<IUnitOfWork> uow, bool isPresent) =>
        uow.Setup(u => u.Movies.AnyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(isPresent);

    public static void SetupReviewPresence(this Mock<IUnitOfWork> uow, bool isPresent) =>
        uow.Setup(u => u.Reviews.AnyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(isPresent);

    public static void SetupReviewsCount(this Mock<IUnitOfWork> uow, int count) =>
        uow.Setup(u => u.Reviews.GetMovieReviewsCountAsync(It.IsAny<int>()))
            .ReturnsAsync(count);

    public static void SetupMovieDuplicate(this Mock<IUnitOfWork> uow, bool duplicate) =>
        uow.Setup(u => u.Movies.AnyAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync(duplicate);

}