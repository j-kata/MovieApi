using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;

namespace MovieApp.Data.Extensions
{
    public static class DbSetExtensions
    {
        public static IQueryable<T> WithTracking<T>(this DbSet<T> dbset, bool trackChanges = false)
            where T : class, IEntity =>
            trackChanges ? dbset : dbset.AsNoTracking();
    }
}