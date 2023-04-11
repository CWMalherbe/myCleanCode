using Application.Bases;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Extentions;
/// <summary>
/// Usefull functions to alter mapping queries
/// </summary>
public static class MappingExtensions
{
    /// <summary>
    /// Returns paginated list of item T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable">Queryable Dataset</param>
    /// <param name="pageNumber">Defaults to 1</param>
    /// <param name="pageSize">Defaults to 10</param>
    /// <returns>paginatedList object containing a list of items T</returns>
    public static async Task<PaginatedList<T>> PaginatedListAsync<T>(this IQueryable<T> queryable, int pageNumber, int pageSize) where T : BaseEntityDTO
    {
        return await PaginatedList<T>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
    }

    /// <summary>
    /// Returns the queryable item to a list.
    /// Not entiryly ueful, but can be used to perform the needed tests.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable">Queryable Dataset</param>
    /// <returns>Returns a dataset to list of objects</returns>
    public static Task<List<T>> ProjectToListAsync<T>(this IQueryable<T> queryable) where T : BaseEntityDTO
    {
        return queryable.ToListAsync();
    }
}
