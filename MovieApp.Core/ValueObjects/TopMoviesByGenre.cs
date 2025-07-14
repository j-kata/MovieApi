namespace MovieApp.Core.ValueObjects;

public class TopMoviesByGenre
{
    public string Genre { get; set; } = string.Empty;
    public IEnumerable<MovieWithRating> Movies { get; set; } = [];
}

