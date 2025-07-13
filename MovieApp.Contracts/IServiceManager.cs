namespace MovieApp.Contracts;

public interface IServiceManager
{
    IReviewService ReviewService { get; }
    IRoleService RoleService { get; }
    IActorService ActorService { get; }
    IMovieService MovieService { get; }
    IGenreService GenreService { get; }
}