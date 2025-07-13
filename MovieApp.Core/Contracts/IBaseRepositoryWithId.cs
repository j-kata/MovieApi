namespace MovieApp.Core.Contracts;

public interface IBaseRepositoryWithId<T> : IBaseRepository<T> where T : IHasId
{
    public Task<bool> AnyByIdAsync(int id);
    Task<T?> GetByIdAsync(int id, bool trackChanges = false);
    void RemoveById(int id);
}
