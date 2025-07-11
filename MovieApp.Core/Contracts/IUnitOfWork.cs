namespace MovieApp.Core.Contracts;

public interface IUnitOfWork
{
    public IReviewRepository Reviews { get; }
    public Task CompleteAsync();
}