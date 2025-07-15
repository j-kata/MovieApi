using AutoMapper;
using MovieApp.Contracts;
using MovieApp.Core.Contracts;
using MovieApp.Core.Dtos.Genre;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Services;

public class GenreService(IUnitOfWork uow, IMapper mapper) : IGenreService
{
    public async Task<PagedResult<GenreDto>> GetGenresAsync(PageParameters parameters)
    {
        var result = await uow.Genres.GetGenresAsync(parameters);
        return new PagedResult<GenreDto>(
            items: mapper.Map<IEnumerable<GenreDto>>(result.Items),
            details: result.Details
        );
    }
}
