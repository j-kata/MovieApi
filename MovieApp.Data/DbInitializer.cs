using Bogus;
using MovieApp.Core.Entities;

namespace MovieApp.Data;

public static class DbInitializer
{
    private static readonly string[] _genreNames = ["Action", "Comedy", "Drama", "Horror", "Sci-Fi"];

    public static async Task Seed(MovieContext context)
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

        var actors = GenerateActors(100);
        await context.AddRangeAsync(actors);

        var roles = GenerateRoles(500, movies, actors);
        await context.AddRangeAsync(roles);

        await context.SaveChangesAsync();
    }

    private static IEnumerable<Genre> GenerateGenres(int count) =>
        new Faker<Genre>()
            .RuleFor(o => o.Name, f => _genreNames[f.IndexVariable++])
            .Generate(count);

    private static MovieDetail GenerateMovieDetails() =>
        new Faker<MovieDetail>()
            .RuleFor(o => o.Synopsis, f => f.Lorem.Paragraph(2))
            .RuleFor(o => o.Language, f => f.Lorem.Word())
            .RuleFor(o => o.Budget, f => (int)f.Finance.Amount(100000, 500000000))
            .Generate();

    private static IEnumerable<Movie> GenerateMovies(int count, IEnumerable<Genre> genres) =>
        new Faker<Movie>()
            .RuleFor(o => o.Genre, f => f.PickRandom(genres))
            .RuleFor(o => o.Title, f => $"{f.Lorem.Sentence(1, 5)} {f.UniqueIndex}")
            .RuleFor(o => o.Year, f => f.Date.Past(40).Year)
            .RuleFor(o => o.Duration, f => f.Date.Timespan(new TimeSpan(4, 0, 0)))
            .RuleFor(o => o.MovieDetail, f => GenerateMovieDetails())
            .FinishWith((f, o) => o.MovieDetail.Movie = o)
            .Generate(count);

    private static IEnumerable<Review> GenerateReviews(int count, IEnumerable<Movie> movies) =>
        new Faker<Review>()
            .RuleFor(o => o.Movie, f => f.PickRandom(movies))
            .RuleFor(o => o.ReviewerName, f => f.Name.FullName())
            .RuleFor(o => o.Comment, f => f.Lorem.Paragraphs(2))
            .RuleFor(o => o.Rating, f => f.Random.Number(1, 5))
            .Generate(count);

    private static IEnumerable<Actor> GenerateActors(int count) =>
        new Faker<Actor>()
            .RuleFor(o => o.Name, f => f.Name.FullName())
            .RuleFor(o => o.BirthYear, f => f.Date.Past(100, DateTime.Now.AddYears(-2)).Year)
            .Generate(count);

    private static IEnumerable<Role> GenerateRoles(int count, IEnumerable<Movie> movies, IEnumerable<Actor> actors)
    {
        HashSet<(Movie, Actor)> uniquePairs = [];
        Faker faker = new();
        List<Role> roles = [];

        for (int i = 0; i < count; i++)
        {
            var movie = faker.PickRandom(movies);
            var actor = faker.PickRandom(actors);
            var pair = (movie, actor);

            if (uniquePairs.Contains(pair))
                continue;

            uniquePairs.Add(pair);
            roles.Add(new Role
            {
                Title = faker.Name.JobTitle(),
                Movie = movie,
                Actor = actor
            });
        }
        return roles;
    }
}
