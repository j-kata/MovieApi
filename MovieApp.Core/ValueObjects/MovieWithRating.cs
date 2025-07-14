namespace MovieApp.Core.ValueObjects;

public class MovieWithRating
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public double Rating { get; set; }
}
