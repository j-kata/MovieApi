namespace MovieApp.Core.Shared;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public PaginationMeta Details { get; set; }

    public PagedResult(IEnumerable<T> items, PaginationMeta details)
    {
        Items = items;
        Details = details;
    }

    public PagedResult(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        Details = new PaginationMeta
        {
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}