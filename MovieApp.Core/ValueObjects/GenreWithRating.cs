namespace MovieApp.Core.ValueObjects;

public class GenreWithRating
{
    public int Id { get; set; }
    public string Genre { get; set; } = string.Empty;
    public double Rating { get; set; }
}
