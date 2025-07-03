namespace MovieApi.Models.Dtos.Movie
{
    public class MovieDetailsDto : MovieDto
    {
        public string Synopsis { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Budget { get; set; }
        // add reviews and actors 
    }
}