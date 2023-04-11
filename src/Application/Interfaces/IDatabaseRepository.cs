using Domain.Bases;

namespace Application.Interfaces;
/// <summary>
/// Repository to handle database interactions.
/// Note that should more than one dbcontext be needed, 
/// we need to implement a DatabaseRepository Manager. 
/// Won't we needed in all cases, but hell, why not.
/// Good example: https://code-maze.com/aspnetcore-multiple-databases-efcore/
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDatabaseRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Acces the entire entity table
    /// </summary>
    IQueryable<T> Table { get; }
    /// <summary>
    /// Acces the entire entity table
    /// </summary>
    IQueryable<T> TableNoTracking { get; }
    /// <summary>
    /// Deletes an existing entity from the repository. 
    /// Should add another section to see if more than one item exists, 
    /// then throw all the exceptions.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task DeleteAsync(T entity, CancellationToken token);
    /// <summary>
    /// Fetch an entity from the repository by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<T?> GetByIdAsync(object id, CancellationToken token);
    /// <summary>
    /// Fetch an entity from the repository by id with inlcudes
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includes"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<T?> GetByIdAsyncWithIncludes(long id, string[] includes, CancellationToken token);
    /// <summary>
    /// Insert a new entity into the repository
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task InsertAsync(T entity, CancellationToken token);
    /// <summary>
    /// Update an existing entity in the repository. 
    /// Currently the working system adds the value as modified
    /// if the object is not found.
    /// Should return a result. 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task UpdateAsync(T entity, CancellationToken token);
    /// <summary>
    /// Saves all the changes made to the database repository
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken token);

    /// <summary>
    /// Query with raw sql command or a stor proc
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns>IQueryable of base Entity Type</returns>
    Task<IQueryable<T>> QuerySQL (string sql, params object[] parameters);

    /// <summary>
    /// Executes raw sql, does not return anything
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task ExecuteSQL(string sql, params object[] parameters);
    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <returns>An list of type T</returns>
    Task<IList<T>> GetAllEntitiesAsync(CancellationToken token);
}