using MovieApp.Core.Contracts;

namespace MovieApp.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MovieContext _context;
    private readonly Lazy<IReviewRepository> _reviewRepository;
    public IReviewRepository Reviews => _reviewRepository.Value;

    public UnitOfWork(MovieContext context)
    {
        _context = context;
        _reviewRepository = new Lazy<IReviewRepository>(() => new ReviewRepository(_context));

    }

    public async Task CompleteAsync() => await _context.SaveChangesAsync();
}