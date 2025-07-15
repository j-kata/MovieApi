using System.Linq.Expressions;

namespace MovieApp.Core.Contracts;

public interface IQueryableRepository<T> where T : IEntity
{
    IQueryable<T> FindBy(Expression<Func<T, bool>>? expression = null, bool trackChanges = false);
    public IQueryable<T> FindAll(bool trackChanges = false);
}
