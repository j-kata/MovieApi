namespace MovieApp.Core.ValueObjects;

public class MovieWithReviewsCount
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ReviewsCount { get; set; }
}
