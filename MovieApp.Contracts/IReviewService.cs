using MovieApp.Core.Dtos.Review;

namespace MovieApp.Contracts;

public interface IReviewService
{
    public Task<IEnumerable<ReviewDto>> GetReviewsAsync(int movieId);
    public Task<ReviewDto> PostReviewAsync(int movieId, ReviewCreateDto createDto);
    public Task DeleteReviewAsync(int id);
}
