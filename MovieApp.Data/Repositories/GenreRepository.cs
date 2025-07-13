using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;
using MovieApp.Data;

namespace MovieApp.Data.Repositories;

public class GenreRepository(MovieContext context)
    : BaseRepositoryWithId<Genre>(context), IGenreRepository
{
}
