using AutoMapper;
using MovieApp.Core.Shared;

namespace MovieApp.Services.Extensions;

public static class PagedResultExtensions
{
    public static PagedResult<TDest> Map<TSrc, TDest>(this PagedResult<TSrc> source, IMapper mapper) =>
        new(
            items: mapper.Map<IEnumerable<TDest>>(source.Items),
            details: source.Details
        );
}