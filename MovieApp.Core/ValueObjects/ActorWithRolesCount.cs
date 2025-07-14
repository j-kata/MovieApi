namespace MovieApp.Core.ValueObjects;

public class ActorWithRolesCount
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RolesCount { get; set; }
}
