using MovieApp.Core.Contracts;

namespace MovieApp.Data.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WithOffset<T>(this IQueryable<T> dbset, int size, int index)
        where T : class, IEntity =>
        dbset.Skip((index - 1) * size).Take(size);
}
