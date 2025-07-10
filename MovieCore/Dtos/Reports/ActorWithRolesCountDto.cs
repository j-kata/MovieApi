namespace MovieCore.Dtos.Reports
{
    public class ActorWithRolesCountDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RolesCount { get; set; }
    }
}