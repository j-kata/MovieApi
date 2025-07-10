namespace MovieApp.Core.Dtos.Reports
{
    public class MovieWithRatingDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Rating { get; set; }
    }
}