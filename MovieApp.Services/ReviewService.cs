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
