using System.Net.Http.Json;
using Application.Bases;
using Application.Entities.TruckParts;
using Application.Entities.TruckParts.Commands;
using Application.Models;
using Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Presentation.Shared.Components;
using Presentation.Shared.Extentions;

namespace Presentation.Shared.Pages;


/// <summary>
/// Represents a paginated page for displaying information about truck parts.
/// </summary>
public partial class TruckPartsOverviewPage
{

    private readonly string PageHeader = "TRUCK PARTS OVERVIEW";
    private readonly string TableHeader = "PARTS";
    private readonly string Path = "truckpart";



    /// <summary>
    /// The table of truck parts.
    /// </summary>
    private MudTable<TruckPartDTO> table { get; set; } = new MudTable<TruckPartDTO>();

    /// <summary>
    /// The truck part object being created.
    /// </summary>
    private TruckPartDTO? createTruckPartDTO { get; set; }

    /// <summary>
    /// The list of truck parts to be displayed in the table.
    /// </summary>
    private PaginatedList<TruckPartDTO>? TableObjects { get; set; }

    /// <summary>
    /// The pagination properties used for retrieving data from the server.
    /// </summary>
    private BasePagination paginationProperties { get; set; } = new BasePagination();

    /// <summary>
    /// The task representing the authentication state.
    /// </summary>
    [CascadingParameter]
    Task<AuthenticationState>? authenticationStateTask { get; set; }

    /// <summary>
    /// Initializes the page asynchronously.
    /// </summary>
    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Retrieves data from the server and returns a TableData object.
    /// </summary>
    /// <param name="state">The state of the table.</param>
    /// <returns>A TableData object containing information about the retrieved truck parts.</returns>
    private async Task<TableData<TruckPartDTO>> ServerReload(TableState state)
    {
        TableData<TruckPartDTO> returnValue = new TableData<TruckPartDTO>();
        // SET PAGINATION VALUES
        paginationProperties.PageNumber = state.Page + 1;
        paginationProperties.PageSize = state.PageSize;
        paginationProperties.ColumnName = state.SortLabel;
        paginationProperties.Direction = state.SortDirection == SortDirection.Descending ? "desc" : "asc";
        //REQUEST DATA
        await LoadData();
        //SET VALUES
        if (TableObjects != null)
        {
            state.Page = TableObjects.PageNumber - 1;
            returnValue.TotalItems = TableObjects.TotalCount;
            returnValue.Items = TableObjects.Items;
        }
        return returnValue;
    }


    private async Task LoadData()
    {
        var httpClient = HttpService.HttpClient;
        await BearerAccess.AddBearerToken(authenticationStateTask, httpClient);
        TableObjects = await httpClient.GetFromJsonAsync<PaginatedList<TruckPartDTO>>($"api/{Path}/{QueryStringExtentions.GetPaginationQueryString(paginationProperties)}");
    }

    /// <summary>
    /// Function that handles the changes in the search bar. 
    /// No checks are done here
    /// </summary>
    /// <param name="text">The value from the search bar</param>
    /// <returns>Completed Task</returns>
    private async Task OnSearch(string text)
    {
        paginationProperties.SearchString = text.ToLower();
        await table.ReloadServerData();
    }

    #region CRUD OPERATIONS

    private async Task OpenCreateDialog()
    {
        if (createTruckPartDTO == null)
        {
            createTruckPartDTO = new TruckPartDTO();
        }
        var dialogResult = await Dialog.ShowAsync<EditOrCreateTruckPartComponent>(string.Empty, new DialogParameters
        {
            ["ItemName"] = "Truck Part",
            ["Entity"] = createTruckPartDTO,
            ["ShouldEdit"] = false,
            ["Path"] = Path
        },
        new DialogOptions()
        {
            DisableBackdropClick = true
        });
        await dialogResult.Result;
        await table.ReloadServerData();
    }

    /// <summary>
    /// Asynchronously opens the edit dialog for creating a new TruckPartDTO object.
    /// </summary>
    private async Task OpenEditDialog(TruckPartDTO entity)
    {
        var dialogResult = await Dialog.ShowAsync<EditOrCreateTruckPartComponent>(string.Empty, new DialogParameters
        {
            ["ItemName"] = entity.Name,
            ["Entity"] = entity,
            ["ShouldEdit"] = true,
            ["Path"] = Path
        },
        new DialogOptions()
        {
            DisableBackdropClick = true
        });
        await dialogResult.Result;
        await table.ReloadServerData();
    }

    /// <summary>
    /// Shows a dialog box asking the user to confirm deletion of a truck.
    /// If the user confirms the deletion, the truck will be deleted from the database.
    /// </summary>
    /// <param name="entity">The TruckPartDTO object to delete.</param>
    private async Task DeleteEntityDialog(TruckPartDTO entity)
    {
        // Show confirmation dialog box
        var dialogResult = await Dialog.ShowAsync<ConfirmDeleteComponent>(string.Empty, new DialogParameters
        {
            ["ItemName"] = entity.Name,
            ["Path"] = Path,
            ["Entity"] = entity
        },
        new DialogOptions()
        {
            DisableBackdropClick = true
        });
        await dialogResult.Result;
        await table.ReloadServerData();


        #endregion

    }
}