using Application.Interfaces;
using Domain.Bases;
using Infrastructure.Contexts.DatabaseContext.ContextImplementation;

namespace Infrastructure.Contexts.DatabaseContext;
/// <summary>
/// <inheritdoc/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class DatabaseManager<T> : IDatabaseManager<T> where T : BaseEntity
{
    private readonly IDatabaseRepository<T> _applicationRepository;
    private readonly IDatabaseRepository<T> _auditRepository;
    private readonly IDatabaseRepository<T> _authenticationRepository;

    /// <summary>
    /// Editable constructor used for the creation of different repositories
    /// </summary>
    /// <param name="applicationContext"></param>
    /// <param name="auditContext"></param>
    /// <param name="authenticationContext"></param>
    public DatabaseManager(ApplicationContext applicationContext, AuditContext auditContext, AuthenticationContext authenticationContext)
    {
        _applicationRepository = new DatabaseRepository<T>(applicationContext);
        _auditRepository = new DatabaseRepository<T>(auditContext);
        _authenticationRepository = new DatabaseRepository<T>(authenticationContext);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IDatabaseRepository<T> ApplicationRepository
    {
        get
        {
            return _applicationRepository;
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IDatabaseRepository<T> AuditRepository
    {
        get
        {
            return _auditRepository;
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IDatabaseRepository<T> AuthenticationRepository
    {
        get
        {
            return _authenticationRepository;
        }
    }
}
