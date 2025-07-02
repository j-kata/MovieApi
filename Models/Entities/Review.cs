using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string ReviewerName { get; set; } = null!;
        [MaxLength(1500)]
        public string Comment { get; set; } = null!;
        [Range(1, 5)]
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
    }
}
