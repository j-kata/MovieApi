using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class ReviewRepository(MovieContext context)
    : BaseRepository<Review>(context), IReviewRepository
{
    // public IQueryable<Review> Query(Expression<Func<Review, bool>> expression) =>
    //     context.Reviews.Where(expression);

    public void RemoveById(int id)
    {
        var review = new Review { Id = id };
        Attach(review);
        Remove(review);
    }
}