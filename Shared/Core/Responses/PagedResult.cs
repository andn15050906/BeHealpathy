namespace Core.Responses;

public class PagedResult<T>
{
    public long TotalCount { get; init; }
    public short PageIndex { get; init; }
    public byte PageSize { get; init; }
    public int PageCount { get => (int)Math.Ceiling((double)TotalCount / PageSize); }
    public List<T> Items { get; init; }

    public PagedResult(long totalCount, short pageIndex, byte pageSize, List<T> items)
    {
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
        Items = items;
    }

    public PagedResult()
    {
        Items = new List<T>();
    }

    public static PagedResult<T> GetEmpty() => new(0, 0, 1, new List<T>(0));
}