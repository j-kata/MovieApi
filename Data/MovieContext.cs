using Microsoft.EntityFrameworkCore;

namespace MovieApi.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }
    }
}
