using Microsoft.EntityFrameworkCore;
using MovieData;

namespace MovieAPI.Extensions
{
    public static class MovieContextExtensions
    {
        public static Task<bool> IsPresentAsync<T>(this MovieContext context, int id) where T : class =>
            context.Set<T>().AnyAsync(c => EF.Property<int>(c, "Id") == id);

        public static IQueryable<T> QueryById<T>(this MovieContext context, int id) where T : class =>
            context.Set<T>().Where(c => EF.Property<int>(c, "Id") == id);

        public static T AttachStubById<T>(this MovieContext context, int id) where T : class, new()
        {
            var obj = new T();
            typeof(T).GetProperty("Id")?.SetValue(obj, id);

            context.Set<T>().Attach(obj);
            return obj;
        }
    }
}