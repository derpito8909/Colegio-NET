namespace Colegio.Web.Application.Services;

public sealed class PagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalItems { get; init; }

    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    public bool HasPrev => Page > 1;
    public bool HasNext => Page < TotalPages;

    public static PagedResult<T> From(IReadOnlyList<T> all, int page, int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var total = all.Count;
        var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResult<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = total
        };
    }
}