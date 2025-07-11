using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class MovieRepository(MovieContext context) : BaseRepository<Movie>(context), IMovieRepository
{
}