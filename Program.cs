using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MovieContext>(dbContextOptions =>
    dbContextOptions.UseSqlite(builder.Configuration.GetConnectionString("MovieContext")
    ?? throw new InvalidOperationException("Connection string 'MovieContext' not found")));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "v1")
    );

    app.SeedData().Wait();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
