namespace MovieApi.Models.Entities
{
    public class Role
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string Title { get; set; } = null!;
        public Movie Movie { get; set; } = null!;
        public Actor Actor { get; set; } = null!;
    }
}