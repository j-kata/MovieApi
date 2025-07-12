using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Review;
using MovieApp.Core.Entities;

namespace MovieApp.Services;

public class ReviewService(IUnitOfWork uow, IMapper mapper) : IReviewService
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ReviewDto>> GetReviews(int movieId)
    {
        if (!await _uow.Movies.AnyByIdAsync(movieId))
            return null!; // TODO: throw exception

        return _mapper.Map<IEnumerable<ReviewDto>>(
            await _uow.Reviews.GetMovieReviewsAsync(movieId));
    }

    public async Task<ReviewDto> PostReview(int movieId, ReviewCreateDto createDto)
    {
        var movie = await _uow.Movies.GetByIdAsync(movieId, trackChanges: true);
        if (movie is null)
            return null!; // TODO: throw exception

        var review = _mapper.Map<Review>(createDto);
        movie.Reviews.Add(review);

        await _uow.CompleteAsync();

        return _mapper.Map<ReviewDto>(review);
    }

    public async Task DeleteReview(int id)
    {
        if (!await _uow.Reviews.AnyByIdAsync(id))
        { } // TODO: throw exception

        _uow.Reviews.RemoveById(id);
        await _uow.CompleteAsync();
    }
}
