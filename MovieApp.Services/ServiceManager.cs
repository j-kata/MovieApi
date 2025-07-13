
using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;

namespace MovieApp.Services;

public class ServiceManager(
    Lazy<IReviewService> reviewService,
    Lazy<IRoleService> roleService,
    Lazy<IActorService> actorService,
    Lazy<IMovieService> movieService) : IServiceManager
{
    public IReviewService ReviewService => reviewService.Value;
    public IRoleService RoleService => roleService.Value;
    public IActorService ActorService => actorService.Value;
    public IMovieService MovieService => movieService.Value;
}
