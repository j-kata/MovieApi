using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class ReviewRepository(MovieContext context) : IReviewRepository
{
    private readonly MovieContext context = context;

    public Task<bool> AnyAsync(int id) =>
        context.Reviews.AnyAsync(r => r.Id == id);

    public void Attach(Review review) =>
        context.Reviews.Attach(review);

    public void Delete(Review review) =>
        context.Remove(review);
}