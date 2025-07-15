using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Review;
using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Services;

public class ReviewService(IUnitOfWork uow, IMapper mapper) : IReviewService
{
    public async Task<PagedResult<ReviewDto>> GetReviewsAsync(int movieId, PageParameters parameters)
    {
        if (!await uow.Movies.AnyByIdAsync(movieId))
            return null!; // TODO: throw exception

        var result = await uow.Reviews.GetMovieReviewsAsync(parameters, movieId);

        return new PagedResult<ReviewDto>(
            items: mapper.Map<IEnumerable<ReviewDto>>(result.Items),
            details: result.Details
        );
    }

    public async Task<ReviewDto> PostReviewAsync(int movieId, ReviewCreateDto createDto)
    {
        var movie = await uow.Movies.GetByIdAsync(movieId, trackChanges: true);
        if (movie is null)
            return null!; // TODO: throw exception

        var review = mapper.Map<Review>(createDto);
        movie.Reviews.Add(review);

        await uow.CompleteAsync();

        return mapper.Map<ReviewDto>(review);
    }

    public async Task DeleteReviewAsync(int id)
    {
        if (!await uow.Reviews.AnyByIdAsync(id))
            return;// TODO: throw exception

        uow.Reviews.RemoveById(id);
        await uow.CompleteAsync();
    }
}
