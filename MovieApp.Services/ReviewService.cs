using MovieApp.Contracts;
using MovieApp.Core.Contracts;

namespace MovieApp.Services;

public class ReviewService(IUnitOfWork uow) : IReviewService
{
    public IUnitOfWork Uow { get; } = uow;
}