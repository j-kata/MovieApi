using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Genre;

namespace MovieApp.Services;

public class GenreService(IUnitOfWork uow, IMapper mapper) : IGenreService
{
    public async Task<IEnumerable<GenreDto>> GetGenresAsync(bool trackChanges = false)
    {
        var genres = await uow.Genres.GetGenresAsync(trackChanges);
        return mapper.Map<IEnumerable<GenreDto>>(genres);
    }
}
