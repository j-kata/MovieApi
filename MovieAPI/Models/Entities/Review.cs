namespace MovieAPI.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
    }
}
