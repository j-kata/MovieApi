namespace MovieApp.Core.Contracts;

public interface IUnitOfWork
{
    public IReviewRepository Reviews { get; }
    public IMovieRepository Movies { get; }
    public IRoleRepository Roles { get; }
    public IActorRepository Actors { get; }
    public IGenreRepository Genres { get; }
    public Task CompleteAsync();
}
