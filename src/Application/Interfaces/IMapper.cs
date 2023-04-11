using Application.Bases;
using Domain.Bases;

namespace Application.Interfaces;

/// <summary>
/// Generic mapper interface
/// </summary>
/// <typeparam name="T">Dto object as BaseEntityDto</typeparam>
/// <typeparam name="U">Domain object as BaseEntity</typeparam>
public interface IMapper<T, U>
    where T : BaseEntityDTO
    where U : BaseEntity
{
    /// <summary>
    /// Converts Entity to Dto
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    T MapEntityToDto(U entity);
    /// <summary>
    /// Converts Dto to Entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    U MapDtoToEntity(T entity);
}