
using MovieApp.Contracts;

namespace MovieApp.Services;

public class ServiceManager(
    Lazy<IReviewService> reviewService,
    Lazy<IRoleService> roleService,
    Lazy<IActorService> actorService,
    Lazy<IMovieService> movieService,
    Lazy<IGenreService> genreService) : IServiceManager
{
    public IReviewService ReviewService => reviewService.Value;
    public IRoleService RoleService => roleService.Value;
    public IActorService ActorService => actorService.Value;
    public IMovieService MovieService => movieService.Value;
    public IGenreService GenreService => genreService.Value;
}
