using MovieApp.Core.Dtos.Review;

namespace MovieApp.Contracts;

public interface IReviewService
{
    public Task<IEnumerable<ReviewDto>> GetReviews(int movieId);
    public Task<ReviewDto> PostReview(int movieId, ReviewCreateDto createDto);
    public Task DeleteReview(int id);
}
