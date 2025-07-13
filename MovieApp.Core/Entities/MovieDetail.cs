using MovieApp.Core.Contracts;

namespace MovieApp.Core.Entities
{
    public class MovieDetail : IHasId
    {
        public int Id { get; set; }
        public string? Synopsis { get; set; }
        public string? Language { get; set; }
        public int? Budget { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
    }
}
