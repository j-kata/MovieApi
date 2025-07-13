namespace MovieApp.Core.Dtos.Movie;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Duration { get; set; } = string.Empty;
    public int GenreId { get; set; }
}
