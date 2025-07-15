namespace MovieApp.Core.Shared;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public PaginationMeta Meta { get; set; }

    public PagedResult(IEnumerable<T> items, PaginationMeta meta)
    {
        Items = items;
        Meta = meta;
    }

    public PagedResult(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        Meta = new PaginationMeta
        {
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}