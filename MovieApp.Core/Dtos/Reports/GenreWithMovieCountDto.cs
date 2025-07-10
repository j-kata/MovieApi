namespace MovieApp.Core.Dtos.Reports
{
    public class GenreWithMovieCountDto
    {
        public string Name { get; set; } = string.Empty;
        public int MoviesCount { get; set; }
    }
}