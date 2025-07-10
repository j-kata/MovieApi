namespace MovieAPI.Models.Dtos.Reports
{
    public class TopMoviesByGenreDto
    {
        public string Genre { get; set; } = string.Empty;
        public IEnumerable<MovieWithRatingDto> Movies { get; set; } = [];
    }
}