namespace MovieAPI.Models.Dtos.Reports
{
    public class GenreWithMovieCountDto
    {
        public string Name { get; set; } = string.Empty;
        public int MoviesCount { get; set; }
    }
}