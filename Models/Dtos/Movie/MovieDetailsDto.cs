namespace MovieApi.Models.Dtos.Movie
{
    public class MovieDetailsDto : MovieDto
    {
        public string? Synopsis { get; set; }
        public string? Language { get; set; }
        public int? Budget { get; set; }
        public List<ReviewDto> Reviews { get; set; } = [];
        public List<ActorDto> Actors { get; set; } = [];
    }
}