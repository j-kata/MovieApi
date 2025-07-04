using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Entities
{
    public class MovieDetails
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string? Synopsis { get; set; }
        [MaxLength(100)]
        public string? Language { get; set; }
        public int? Budget { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
    }
}
