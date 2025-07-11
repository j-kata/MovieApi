using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.API.Extensions;
using MovieApp.Data.Profiles;
using MovieApp.Core.Contracts;
using MovieApp.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MovieContext>(dbContextOptions =>
    dbContextOptions.UseSqlite(builder.Configuration.GetConnectionString("MovieContext")
    ?? throw new InvalidOperationException("Connection string 'MovieContext' not found")));
builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    setup.IncludeXmlComments(xmlCommentsFullPath);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// TODO: load all at once automatically?
builder.Services.AddAutoMapper(opt => opt.AddProfiles([
    new MovieProfile(),
    new ReviewProfile(),
    new ActorProfile(),
    new RoleProfile()
]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1")
    );

    app.SeedData().Wait();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
