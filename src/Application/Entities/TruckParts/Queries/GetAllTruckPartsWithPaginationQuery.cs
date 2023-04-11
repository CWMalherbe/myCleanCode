using Application.Bases;
using Application.Extentions;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using MediatR;

namespace Application.Entities.TruckParts.Queries;

/// <summary>
/// Represents a request object to get a paginated list of truck parts.
/// Inherits from <see cref="BasePagination"/> and implements <see cref="IRequest{TResponse}"/> interface with a response of <see cref="PaginatedList{T}"/> of <see cref="TruckPartDTO"/>.
/// </summary>
public record GetAllTruckPartsWithPaginationQuery : BasePagination, IRequest<PaginatedList<TruckPartDTO>>
{

}

/// <summary>
/// Represents a handler for the <see cref="GetAllTruckPartsWithPaginationQuery"/> request object. 
/// Implements <see cref="IRequestHandler{TRequest,TResponse}"/> interface with a request of <see cref="GetAllTruckPartsWithPaginationQuery"/> and a response of <see cref="PaginatedList{T}"/> of <see cref="TruckPartDTO"/>.
/// </summary>
public class GetAllTruckPartsWithPaginationQueryHandler : IRequestHandler<GetAllTruckPartsWithPaginationQuery, PaginatedList<TruckPartDTO>>
{
    private readonly IDatabaseManager<TruckPart> _databaseManager;
    private readonly IMapper<TruckPartDTO, TruckPart> _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllTruckPartsWithPaginationQueryHandler"/> class with the specified dependencies.
    /// </summary>
    /// <param name="databaseManager">The <see cref="IDatabaseManager{TEntity}"/> object to use for database operations.</param>
    /// <param name="mapper">The <see cref="IMapper{TSource, TDestination}"/> object to use for mapping between entity and DTO.</param>
    public GetAllTruckPartsWithPaginationQueryHandler(IDatabaseManager<TruckPart> databaseManager, IMapper<TruckPartDTO, TruckPart> mapper)
    {
        _databaseManager = databaseManager;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the <see cref="GetAllTruckPartsWithPaginationQuery"/> request object by querying the database with pagination, search and filter, and returns a paginated list of <see cref="TruckPartDTO"/>.
    /// </summary>
    /// <param name="request">The <see cref="GetAllTruckPartsWithPaginationQuery"/> request object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="PaginatedList{T}"/> of <see cref="TruckPartDTO"/>.</returns>
    public async Task<PaginatedList<TruckPartDTO>> Handle(GetAllTruckPartsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        IQueryable<TruckPart> query = _databaseManager.ApplicationRepository.Table;
        // ADD SEARCH
        query = AddSearch(request, query);
        // ADD FILTER
        query = AddFilter(request, query);
        // RETURN PAGINATED
        return await query.Select(x => _mapper.MapEntityToDto(x)).PaginatedListAsync(request.PageNumber, request.PageSize);
    }

    #region Filter

    /// <summary>
    /// Adds a filter to the given query based on the request's column name and direction.
    /// </summary>
    /// <param name="request">The request object containing the column name and direction to sort by.</param>
    /// <param name="query">The query to apply the filter to.</param>
    /// <returns>The query with the filter applied.</returns>
    private IQueryable<TruckPart> AddFilter(GetAllTruckPartsWithPaginationQuery request, IQueryable<TruckPart> query)
    {
        #region Bunch of case statements
        switch (request.ColumnName)
        {
            case "truckid":
                if (request.Direction == "desc")
                {
                    query = query.OrderByDescending(x => x.TruckId);
                }
                else
                {
                    query = query.OrderBy(x => x.TruckId);
                }
                break;
            case "name":
                if (request.Direction == "desc")
                {
                    query = query.OrderByDescending(x => x.Name);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }
                break;
            case "code":
                if (request.Direction == "desc")
                {
                    query = query.OrderByDescending(x => x.Code);
                }
                else
                {
                    query = query.OrderBy(x => x.Code);
                }
                break;
            case "condition":
                if (request.Direction == "desc")
                {
                    query = query.OrderByDescending(x => x.Condition);
                }
                else
                {
                    query = query.OrderBy(x => x.Condition);
                }
                break;
            default:
                if (request.Direction == "desc")
                {
                    query = query.OrderByDescending(x => x.Id);
                }
                else
                {
                    query = query.OrderBy(x => x.Id);
                }
                break;
        }
        #endregion
        return query;
    }

    #endregion

    #region Search
    /// <summary>
    /// Adds a search filter to the given query based on the request's search string.
    /// </summary>
    /// <param name="request">The request object containing the search string.</param>
    /// <param name="query">The query to apply the search filter to.</param>
    /// <returns>The query with the search filter applied.</returns>
    private IQueryable<TruckPart> AddSearch(GetAllTruckPartsWithPaginationQuery request, IQueryable<TruckPart> query)
    {
        #region Bunch of Search statements
        if (string.IsNullOrEmpty(request.SearchString) == false)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            query = query.Where(x =>
                x.Id.ToString().Contains(request.SearchString) ||
                x.TruckId.ToString().Contains(request.SearchString) ||
                x.Name.ToLower().Contains(request.SearchString) ||
                x.Code.ToLower().Contains(request.SearchString)
                );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
        #endregion
        return query;
    } 
    #endregion
}