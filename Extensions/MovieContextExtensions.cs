using Microsoft.EntityFrameworkCore;
using MovieApi.Data;

namespace MovieApi.Extensions
{
    public static class MovieContextExtensions
    {
        public static Task<bool> IsMoviePresentAsync(this MovieContext context, int movieId) =>
            context.Movies.AnyAsync(m => m.Id == movieId);

        public static Task<bool> IsReviewPresentAsync(this MovieContext context, int reviewId) =>
            context.Reviews.AnyAsync(r => r.Id == reviewId);

        public static Task<bool> IsActorPresentAsync(this MovieContext context, int actorId) =>
            context.Actors.AnyAsync(a => a.Id == actorId);
    }
}