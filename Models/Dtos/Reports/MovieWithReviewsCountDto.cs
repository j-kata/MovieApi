namespace MovieApi.Models.Dtos.Reports
{
    public class MovieWithReviewsCountDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ReviewsCount { get; set; }
    }
}