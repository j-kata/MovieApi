using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;
using MovieApp.Data;

namespace MovieApp.Data.Repositories;

public class GenreRepository(MovieContext context)
    : BaseRepository<Genre>(context), IGenreRepository
{
}
