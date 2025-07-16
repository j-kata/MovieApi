using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Review;
using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Services.Extensions;
using MovieApp.Core.Exceptions;

namespace MovieApp.Services;

public class ReviewService(IUnitOfWork uow, IMapper mapper) : IReviewService
{
    private const int DefaultMaxCount = 10;
    private const int OldMovieMaxCount = 5;
    private const int OldMovieAge = 20;

    public async Task<PagedResult<ReviewDto>> GetReviewsAsync(int movieId, PageParameters parameters)
    {
        if (!await uow.Movies.AnyByIdAsync(movieId))
            throw new NotFoundException<Movie>(movieId);

        var result = await uow.Reviews.GetMovieReviewsAsync(parameters, movieId);
        return result.Map<Review, ReviewDto>(mapper);
    }

    public async Task<ReviewDto> PostReviewAsync(int movieId, ReviewCreateDto createDto)
    {
        var movie = await uow.Movies.GetByIdAsync(movieId, trackChanges: true)
            ?? throw new NotFoundException<Movie>(movieId);

        var reviewsCount = await uow.Reviews.GetMovieReviewsCountAsync(movieId);

        // rule for old movies
        if (movie.Age > OldMovieAge && reviewsCount >= OldMovieMaxCount)
            throw new ConflictException($"A {OldMovieAge}yo movie cannot have more than {OldMovieMaxCount} reviews");

        // rule for all movies
        if (reviewsCount >= DefaultMaxCount)
            throw new ConflictException($"A movie cannot have more than {DefaultMaxCount} reviews");

        var review = mapper.Map<Review>(createDto);
        movie.Reviews.Add(review);

        await uow.CompleteAsync();

        return mapper.Map<ReviewDto>(review);
    }

    public async Task DeleteReviewAsync(int id)
    {
        if (!await uow.Reviews.AnyByIdAsync(id))
            throw new NotFoundException<Review>(id);

        uow.Reviews.RemoveById(id);
        await uow.CompleteAsync();
    }
}
