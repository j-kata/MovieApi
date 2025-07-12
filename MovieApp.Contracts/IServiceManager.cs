namespace MovieApp.Contracts;

public interface IServiceManager
{
    IReviewService ReviewService { get; }
    IRoleService RoleService { get; }
}