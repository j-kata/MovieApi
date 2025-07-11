using MovieApp.Core.Contracts;

namespace MovieApp.Core.Entities;

public class Role : IEntity
{
    public int MovieId { get; set; }
    public int ActorId { get; set; }
    public string Title { get; set; } = null!;
    public Movie Movie { get; set; } = null!;
    public Actor Actor { get; set; } = null!;
}
