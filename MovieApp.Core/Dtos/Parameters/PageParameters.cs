namespace MovieApp.Core.Dtos.Parameters;

public class PageParameters
{
    private const int PageSizeMax = 100;
    private const int PageSizeDefault = 10;

    private int _pageSize = PageSizeDefault;
    public int PageSize
    {
        get => _pageSize;
        set =>
            _pageSize = value > PageSizeMax
            ? PageSizeMax
            : value <= 0 ? PageSizeDefault : value;
    }
    public int PageIndex { get; set; } = 1;
}