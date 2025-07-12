
using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;

namespace MovieApp.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IReviewService> reviewService;
    public IReviewService ReviewService => reviewService.Value;

    private readonly Lazy<IRoleService> roleService;
    public IRoleService RoleService => roleService.Value;

    public ServiceManager(IUnitOfWork uow, IMapper mapper)
    {
        reviewService = new Lazy<IReviewService>(() => new ReviewService(uow, mapper));
        roleService = new Lazy<IRoleService>(() => new RoleService(uow, mapper));
    }
}
