namespace MovieApp.Core.ValueObjects;

public class GenreWithMoviesCount
{
    public string Name { get; set; } = string.Empty;
    public int MoviesCount { get; set; }
}