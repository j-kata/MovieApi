using MovieApp.Core.Contracts;

namespace MovieApp.Data.Repositories;

public class UnitOfWork(
        MovieContext context,
        Lazy<IReviewRepository> reviewRepository,
        Lazy<IMovieRepository> movieRepository,
        Lazy<IActorRepository> actorRepository,
        Lazy<IRoleRepository> roleRepository,
        Lazy<IGenreRepository> genreRepository
    ) : IUnitOfWork
{
    public IReviewRepository Reviews => reviewRepository.Value;
    public IMovieRepository Movies => movieRepository.Value;
    public IActorRepository Actors => actorRepository.Value;
    public IRoleRepository Roles => roleRepository.Value;
    public IGenreRepository Genres => genreRepository.Value;

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
