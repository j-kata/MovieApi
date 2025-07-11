namespace MovieApp.Core.Contracts;

public interface IBaseRepository<T> where T : IEntity
{
    void Attach(T entity);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}