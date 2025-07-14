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
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IReportService, ReportService>();

        services.AddLazyService<IReviewService>();
        services.AddLazyService<IRoleService>();
        services.AddLazyService<IActorService>();
        services.AddLazyService<IMovieService>();
        services.AddLazyService<IGenreService>();
        services.AddLazyService<IReportService>();
    }

    public static void AddApiRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();

        services.AddLazyService<IReviewRepository>();
        services.AddLazyService<IRoleRepository>();
        services.AddLazyService<IActorRepository>();
        services.AddLazyService<IMovieRepository>();
        services.AddLazyService<IGenreRepository>();
        services.AddLazyService<IReportRepository>();
    }

    public static void AddLazyService<T>(this IServiceCollection services) where T : class =>
        services.AddScoped(provider => new Lazy<T>(() => provider.GetRequiredService<T>()));
}