using MovieApp.Core.Shared;

namespace Services.Tests.Helpers;

public static class PagedResultFactory
{
    public static PagedResult<T> CreatePagedResult<T>(IEnumerable<T> items)
      =>
            new(
                items: items,
                pageIndex: 1,
                pageSize: 10,
                totalCount: items.Count()
            );
}