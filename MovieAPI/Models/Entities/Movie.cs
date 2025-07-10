namespace MovieAPI.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public TimeSpan Duration { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public MovieDetail MovieDetail { get; set; } = null!;
        public ICollection<Role> Roles { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];
    }
}
