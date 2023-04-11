using Microsoft.EntityFrameworkCore;

namespace Application.Models;


/// <summary>
/// Represents a paginated list of items.
/// </summary>
/// <typeparam name="T">The type of item in the list.</typeparam>
public class PaginatedList<T>
{
    /// <summary>
    /// Gets the list of items on the current page.
    /// </summary>
    public List<T> Items { get; private set; }

    /// <summary>
    /// Gets the page number of the current page.
    /// </summary>
    public int PageNumber { get; private set; }

    /// <summary>
    /// Gets the total number of pages in the paginated list.
    /// </summary>
    public int TotalPages { get; private set; }

    /// <summary>
    /// Gets the total number of items in the paginated list.
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
    /// </summary>
    /// <param name="items">The list of items on the current page.</param>
    /// <param name="totalCount">The total number of items in the paginated list.</param>
    /// <param name="pageNumber">The page number of the current page.</param>
    /// <param name="totalPages">The maximum number of items per page.</param>
    public PaginatedList(List<T> items, int totalCount, int pageNumber, int totalPages)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        TotalPages = totalPages;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaginatedList{T}"/> class asynchronously.
    /// </summary>
    /// <param name="source">The source queryable collection.</param>
    /// <param name="pageNumber">The page number of the current page.</param>
    /// <param name="pageSize">The maximum number of items per page.</param>
    /// <returns>A new instance of the <see cref="PaginatedList{T}"/> class.</returns>
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, (int)Math.Ceiling(count / (double)pageSize));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaginatedList{T}"/> class.
    /// </summary>
    /// <param name="source">The source queryable collection.</param>
    /// <param name="pageNumber">The page number of the current page.</param>
    /// <param name="pageSize">The maximum number of items per page.</param>
    /// <returns>A new instance of the <see cref="PaginatedList{T}"/> class.</returns>
    public static PaginatedList<T> Create(IList<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, count, pageNumber, (int)Math.Ceiling(count / (double)pageSize));
    }
}
