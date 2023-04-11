using Application.Interfaces;
using Domain.Bases;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts.DatabaseContext;
/// <summary>
/// Implementation of the generic IDatabaseRepository.
/// Please see the IDatabaseRepository comments to see my 
/// thoughts on how to expand to a DatabaseRepositoryManager 
/// for multiple DB's.
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class DatabaseRepository<T> : IDatabaseRepository<T> where T : BaseEntity
{
    #region Fields
    private readonly DbContext _databaseContext;
    private DbSet<T>? _databaseSet;
    #endregion

    #region Constructor
    /// <summary>
    /// Constrcuts a new instance of a data repository
    /// </summary>
    /// <param name="databaseContext"></param>
    public DatabaseRepository(DbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    #endregion

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="id">The id of the entity</param>
    /// <param name="token"></param>
    /// <returns>Returns an entity or null"/></returns>
    public virtual async Task<T?> GetByIdAsync(object id, CancellationToken token)
    {
        return await Entities.FindAsync(id, token);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    public virtual async Task InsertAsync(T entity, CancellationToken token)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        await Entities.AddAsync(entity, token);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual async Task SaveChangesAsync(CancellationToken token)
    {
        await _databaseContext.SaveChangesAsync(token);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    public virtual async Task UpdateAsync(T entity, CancellationToken token)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var e = await FindEntityAsync(entity, token);

        //The entity is not in the object context, attach it
        if (e == null)
        {
            Entities.Attach(entity);
            _databaseContext.Entry(entity).State = EntityState.Modified;
        }

        //The entity is already in the object context, set the values for it
        else
        {
            _databaseContext.Entry(e).CurrentValues.SetValues(entity);
        }

        //Attempt to set the ModifiedOnUtc value of the entity
        var modifiedOn = typeof(T).GetProperty("ModifiedOnUtc");
        modifiedOn?.SetValue(entity, DateTime.UtcNow, null);
    }

    /// <summary>
    ///<inheritdoc/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<T?> FindEntityAsync(T entity, CancellationToken token)
    {
        //Attempt to get the primary key from the object
        var pkName = $"{typeof(T).Name}Id";
        var pkProperty = typeof(T).GetProperty(pkName);
        var pkValue = pkProperty?.GetValue(entity);
        if (pkValue != null)
        {
            return await Entities.FindAsync(pkValue, token);
        }

        //No entity found
        return null;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="token"></param>
    public virtual Task DeleteAsync(T entity, CancellationToken token)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (Entities.Local.FirstOrDefault(e => e == entity) == null)
            Entities.Attach(entity);

        //Delete the entity
        Entities.Remove(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public virtual async Task<IQueryable<T>> QuerySQL(string sql, params object[] parameters)
    {
        return await Task.FromResult(_databaseContext.Database.SqlQueryRaw<T>(sql, parameters));
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual async Task ExecuteSQL(string sql, params object[] parameters)
    {
        //NOT SURE IF WE NEED TO OPEN TRANCTIONS AND CLOSE
        //WILL TEST IT LATER
        await _databaseContext.Database.ExecuteSqlRawAsync(sql, parameters);
    }

    /// <summary>
    /// <inheritdoc/>
    /// I haven't figured out how to make this generic, but there should be a way.
    /// I have added code to focus figuring it out, but have not gotten there yet.
    /// </summary>
    /// <param name="id">The id of the entity</param>
    /// <param name="includes"></param>
    /// <param name="token"></param>
    /// <returns>Returns an entity or null"/></returns>
    public async Task<T?> GetByIdAsyncWithIncludes(long id, string[] includes, CancellationToken token)
    {
        /*
        var keyProperty = context.Model.FindEntityType(typeof(Blog)).FindPrimaryKey().Properties[0];
        var entity = context.Blogs
            .Include(e => e.Posts)
            .FirstOrDefault(e => EF.Property<int>(e, keyProperty.Name) == id);
        */
        var table = this.Table;
        foreach (string include in includes)
        {
            table = table.Include(include);
        }
        return await table.FirstOrDefaultAsync<T>(x => x.Id == id, token);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<IList<T>> GetAllEntitiesAsync(CancellationToken token)
    {
        return await this.Entities.ToListAsync(token);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual IQueryable<T> Table
    {
        get
        {
            return this.Entities;
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual IQueryable<T> TableNoTracking
    {
        get
        {
            return this.Entities.AsNoTracking();
        }
    }

    /// <summary>
    /// Access All the entities.
    /// </summary>
    protected virtual DbSet<T> Entities
    {
        get
        {
            if (_databaseSet == null)
            {
                _databaseSet = _databaseContext.Set<T>();
            }
            return _databaseSet;
        }
    }
}