using System.Linq.Expressions;

namespace MovieApp.Core.Contracts;

public interface IBaseRepository<T> where T : IEntity
{
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    public Task<bool> AnyByIdAsync(int id);
    void Attach(T entity);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}