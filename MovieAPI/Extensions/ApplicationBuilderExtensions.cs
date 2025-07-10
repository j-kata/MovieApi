using MovieAPI.Data;

namespace MovieAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

            // if (env.IsDevelopment())
            // {
            //     await context.Database.EnsureDeletedAsync();
            //     await context.Database.MigrateAsync();
            // }

            try
            {
                await DbInitializer.Seed(context, env);
            }
            catch (Exception ex)
            {
                var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred initializing the DB.");
            }
        }
    }
}