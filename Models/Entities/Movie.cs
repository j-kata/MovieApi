using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public TimeSpan Duration { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public MovieDetails MovieDetails { get; set; } = null!;
        public ICollection<Actor> Actors { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];
    }
}
