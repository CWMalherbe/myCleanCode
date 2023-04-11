using Application.Bases;
using Domain.Bases;

namespace Application.Interfaces;

/// <summary>
/// Generic validator interface
/// </summary>
/// <typeparam name="T">Dto object as BaseEntityDto</typeparam>
/// <typeparam name="U">Domain object as BaseEntity</typeparam>
public interface IValidator<T, U>
    where T : BaseEntityDTO
    where U : BaseEntity
{
    /// <summary>
    /// Validates Dto
    /// </summary>
    /// <param name="DtoEntity"></param>
    void ValidateDtoEntity(T DtoEntity);
    /// <summary>
    /// Validates Entity
    /// </summary>
    /// <param name="entity"></param>
    void ValidateEntity(U entity);
}