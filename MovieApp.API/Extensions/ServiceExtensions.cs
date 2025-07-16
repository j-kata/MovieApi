using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Data;
using MovieApp.Data.Repositories;
using MovieApp.Services;

namespace MovieApp.API.Extensions;

public static class ServiceExtensions
{
    public static void AddSqlLite(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MovieContext>(dbContextOptions =>
            dbContextOptions.UseSqlite(configuration.GetConnectionString("MovieContext")
            ?? throw new InvalidOperationException("Connection string 'MovieContext' not found")));
    }

    public static void AddSwaggerWithXml(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        {
            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
            setup.IncludeXmlComments(xmlCommentsFullPath);
        });
    }

    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddServiceWithLazy<IReviewService, ReviewService>();
        services.AddServiceWithLazy<IRoleService, RoleService>();
        services.AddServiceWithLazy<IActorService, ActorService>();
        services.AddServiceWithLazy<IMovieService, MovieService>();
        services.AddServiceWithLazy<IGenreService, GenreService>();
        services.AddServiceWithLazy<IReportService, ReportService>();
    }

    public static void AddApiRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddServiceWithLazy<IReviewRepository, ReviewRepository>();
        services.AddServiceWithLazy<IRoleRepository, RoleRepository>();
        services.AddServiceWithLazy<IActorRepository, ActorRepository>();
        services.AddServiceWithLazy<IMovieRepository, MovieRepository>();
        services.AddServiceWithLazy<IGenreRepository, GenreRepository>();
        services.AddServiceWithLazy<IReportRepository, ReportRepository>();
    }

    private static void AddServiceWithLazy<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped<TInterface, TImplementation>();
        services.AddLazyService<TInterface>();
    }

    private static void AddLazyService<T>(this IServiceCollection services) where T : class =>
        services.AddScoped(provider => new Lazy<T>(() => provider.GetRequiredService<T>()));
}