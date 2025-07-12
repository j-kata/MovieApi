using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MovieContext _context;

    private readonly Lazy<IReviewRepository> _reviewRepository;
    public IReviewRepository Reviews => _reviewRepository.Value;

    private readonly Lazy<IMovieRepository> _movieRepository;
    public IMovieRepository Movies => _movieRepository.Value;

    private readonly Lazy<IActorRepository> _actorRepository;
    public IActorRepository Actors => _actorRepository.Value;

    private readonly Lazy<IRoleRepository> _roleRepository;
    public IRoleRepository Roles => _roleRepository.Value;

    private readonly Lazy<IGenreRepository> _genreRepository;
    public IGenreRepository Genres => _genreRepository.Value;

    public UnitOfWork(MovieContext context)
    {
        _context = context;
        _reviewRepository = new Lazy<IReviewRepository>(() => new ReviewRepository(_context));
        _movieRepository = new Lazy<IMovieRepository>(() => new MovieRepository(_context));
        _actorRepository = new Lazy<IActorRepository>(() => new ActorRepository(_context));
        _roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(_context));
        _genreRepository = new Lazy<IGenreRepository>(() => new GenreRepository(_context));
    }

    public async Task CompleteAsync() => await _context.SaveChangesAsync();
}
