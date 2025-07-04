using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Entities
{
    public class MovieDetail
    {
        public int Id { get; set; }
        public string? Synopsis { get; set; }
        public string? Language { get; set; }
        public int? Budget { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
    }
}
