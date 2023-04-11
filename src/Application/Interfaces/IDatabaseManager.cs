using Domain.Bases;

namespace Application.Interfaces;
/// <summary>
/// Manages all the database Repositories
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDatabaseManager<T> where T : BaseEntity
{
    /// <summary>
    /// Where all the application specific stuur is stored
    /// </summary>
    public IDatabaseRepository<T> ApplicationRepository { get; }
    /// <summary>
    /// Where all the audit information is stored
    /// </summary>
    public IDatabaseRepository<T> AuditRepository { get; }
    /// <summary>
    /// Where all the authentication information is stored
    /// </summary>
    public IDatabaseRepository<T> AuthenticationRepository { get; }
}
