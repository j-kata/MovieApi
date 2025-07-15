namespace MovieApp.Core.Dtos.Parameters;

public class MovieParameters : PageParameters
{
    public string? Title { get; set; }
    public int? Year { get; set; }
    public string? Actor { get; set; }
    public string? Genre { get; set; }
}