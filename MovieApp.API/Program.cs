using MovieApp.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSqlLite(builder.Configuration);
builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithXml();

builder.Services.AddApiServices();
builder.Services.AddApiRepositories();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

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
