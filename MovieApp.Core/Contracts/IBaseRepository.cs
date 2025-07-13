using System.Linq.Expressions;

namespace MovieApp.Core.Contracts;

public interface IBaseRepository<T> where T : IEntity
{
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    IQueryable<T> FindBy(Expression<Func<T, bool>>? expression = null, bool trackChanges = false);
    public IQueryable<T> FindAll(bool trackChanges = false);

    void Attach(T entity);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}



