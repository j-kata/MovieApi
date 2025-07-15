using MovieApp.Core.Dtos.Review;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Contracts;

public interface IReviewService
{
    public Task<PagedResult<ReviewDto>> GetReviewsAsync(int movieId, PageParameters parameters);
    public Task<ReviewDto> PostReviewAsync(int movieId, ReviewCreateDto createDto);
    public Task DeleteReviewAsync(int id);
}
