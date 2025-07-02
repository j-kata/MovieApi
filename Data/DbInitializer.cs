using Bogus;
using MovieApi.Models.Entities;

namespace MovieApi.Data
{
    public static class DbInitializer
    {
        private static readonly string[] _genreNames = ["Action", "Comedy", "Drama", "Horror", "Sci-Fi"];

        public static async Task Seed(MovieContext context, IWebHostEnvironment? env)
        {
            context.Database.EnsureCreated();

            if (context.Movies.Any())
                return;

            var genres = GenerateGenres(_genreNames.Length);
            await context.AddRangeAsync(genres);

            var movies = GenerateMovies(100, genres);
            await context.AddRangeAsync(movies);

            var reviews = GenerateReviews(100, movies);
            await context.AddRangeAsync(reviews);

            var actors = GenerateActors(100, movies);
            await context.AddRangeAsync(actors);

            await context.SaveChangesAsync();
        }

        private static IEnumerable<Genre> GenerateGenres(int count) =>
            new Faker<Genre>()
                .RuleFor(o => o.Name, f => _genreNames[f.IndexVariable++])
                .Generate(count);

        private static MovieDetails GenerateMovieDetails() =>
            new Faker<MovieDetails>()
                    .RuleFor(o => o.Synopsis, f => f.Lorem.Paragraph(2))
                    .RuleFor(o => o.Language, f => f.Lorem.Word())
                    .RuleFor(o => o.Budget, f => (int)f.Finance.Amount(100000, 500000000))
                    .Generate();

        private static IEnumerable<Movie> GenerateMovies(int count, IEnumerable<Genre> genres) =>
            new Faker<Movie>()
                .RuleFor(o => o.Genre, f => f.PickRandom(genres))
                .RuleFor(o => o.Title, f => f.Lorem.Sentence(1, 5))
                .RuleFor(o => o.Year, f => f.Date.Past(40).Year)
                .RuleFor(o => o.Duration, f => f.Date.Timespan(new TimeSpan(4, 0, 0)))
                .RuleFor(o => o.MovieDetails, f => GenerateMovieDetails())
                .FinishWith((f, o) => o.MovieDetails.Movie = o)
                .Generate(count);

        private static IEnumerable<Review> GenerateReviews(int count, IEnumerable<Movie> movies) =>
            new Faker<Review>()
                .RuleFor(o => o.Movie, f => f.PickRandom(movies))
                .RuleFor(o => o.ReviewerName, f => f.Name.FullName())
                .RuleFor(o => o.Comment, f => f.Lorem.Paragraphs(2))
                .RuleFor(o => o.Rating, f => f.Random.Number(1, 5))
                .Generate(count);

        private static IEnumerable<Actor> GenerateActors(int count, IEnumerable<Movie> movies) =>
            new Faker<Actor>()
                .RuleFor(o => o.Name, f => f.Name.FullName())
                .RuleFor(o => o.BirthYear, f => f.Date.Past(100, DateTime.Now.AddYears(-2)).Year)
                .RuleFor(o => o.Movies, f => f.PickRandom(movies.ToList(), 5).ToList())
                .Generate(count);

    }
}