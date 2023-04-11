using System.Net.Http.Json;
using Application.Entities.TruckParts;
using Application.Entities.Trucks;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Presentation.Shared.Components;

namespace Presentation.Shared.Pages;

/// <summary>
/// Partial class for the trucks overview page.
/// </summary>
public partial class TrucksOverviewPage
{

    private readonly string PageHeader = "TRUCKS OVERVIEW";
    private readonly string TableHeader = "TRUCKS";
    private readonly string Path = "truck";


    private Dictionary<long, TableObject>? TableObjects { get; set; }
    private TruckDTO? createTruckDTO { get; set; }
    private string searchString { get; set; } = string.Empty;

    #region Parameters

    [CascadingParameter]
    Task<AuthenticationState>? authenticationStateTask { get; set; }

    #endregion

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected async override Task OnInitializedAsync()
    {
        await LoadData();
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Asynchronously loads the data for the page.
    /// </summary>
    private async Task LoadData()
    {
        var httpClient = HttpService.HttpClient;
        await BearerAccess.AddBearerToken(authenticationStateTask, httpClient);
        var trucks = await httpClient.GetFromJsonAsync<List<TruckDTO>>($"api/{Path}");
        if (trucks != null)
        {
            TableObjects = new Dictionary<long, TableObject>();
            foreach (var truck in trucks)
            {
                TableObjects.Add(truck.Id, new TableObject
                {
                    showDetails = false,
                    truck = truck
                });
            }
        }
    }

    #region CRUDOperations

    /// <summary>
    /// Asynchronously opens the dialog to add a new entity.
    /// </summary>
    private async Task OpenCreateDialog()
    {
        if (createTruckDTO == null)
        {
            createTruckDTO = new TruckDTO()
            {
                Items = new List<TruckPartDTO>(),
                Name = string.Empty,
                Paint = Paint.Unkown.Hex
            };
        }
        createTruckDTO.Items = new List<TruckPartDTO>();
        var dialogResult = await Dialog.ShowAsync<EditOrCreateTruckComponent>(string.Empty, new DialogParameters
        {
            ["ItemName"] = "Truck",
            ["Entity"] = createTruckDTO,
            ["ShouldEdit"] = false,
            ["Path"] = "truck"
        },
        new DialogOptions()
        {
            DisableBackdropClick = true
        });
        await dialogResult.Result;
        await LoadData();
    }

    /// <summary>
    /// Asynchronously opens the dialog to edit an existing entity.
    /// </summary>
    /// <param name="entity">The truck to edit.</param>
    private async Task OpenEditDialog(TruckDTO entity)
    {
        var dialogResult = await Dialog.ShowAsync<EditOrCreateTruckComponent>(string.Empty, new DialogParameters
        {
            ["ItemName"] = entity.Name,
            ["Entity"] = entity,
            ["ShouldEdit"] = true,
            ["Path"] = "truck"
        },
        new DialogOptions()
        {
            DisableBackdropClick = true
        });
        await dialogResult.Result;
        await ActionExpandButton(entity.Id);
        await LoadData();
    }

    /// <summary>
    /// Shows a dialog box asking the user to confirm deletion of a truck.
    /// If the user confirms the deletion, the truck will be deleted from the database.
    /// </summary>
    /// <param name="entity">The TruckDTO object to delete.</param>
    private async Task OpenDeleteDialog(TruckDTO entity)
    {
        var dialogResult = await Dialog.ShowAsync<ConfirmDeleteComponent>(string.Empty, new DialogParameters
        {
            ["ItemName"] = entity.Name,
            ["Path"] = "truck",
            ["Entity"] = entity
        },
        new DialogOptions()
        {
            DisableBackdropClick = true
        });
        await dialogResult.Result;
        await ActionExpandButton(entity.Id);
        await LoadData();
    }

    #endregion

    #region CustomCode

    /// <summary>
    /// Expands the action button for the given truck and loads its parts.
    /// </summary>
    /// <param name="id">The ID of the truck to expand the action button for.</param>
    private async Task ActionExpandButton(long id)
    {
        if (TableObjects != null)
        {
            if (TableObjects.ContainsKey(id))
            {
                //Used as reference
                var tmpObject = TableObjects[id];

                // If the details are hidden, load the parts, otherwise set parts to null
                if (tmpObject.showDetails == false)
                {
                    tmpObject.truck.Items = await LoadParts(id);
                }
                else
                {
                    tmpObject.truck.Items = null;
                }

                // Toggle the showDetails flag
                tmpObject.showDetails = !tmpObject.showDetails;
            }
        }
    }

    /// <summary>
    /// A class that represents an object in the table, containing information about the truck entity and its associated parts
    /// </summary>
    private class TableObject
    {
        public bool showDetails { get; set; }
        public TruckDTO truck { get; set; } = new TruckDTO();
    }

    /// <summary>
    /// A function used to filter entities in the table based on a search string
    /// </summary>
    /// <param name="entity">The entity to filter</param>
    /// <returns>True if the entity should be included in the filtered table, false otherwise</returns>
    private bool FilterFunction(KeyValuePair<long, TableObject> entity)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if ($"{entity.Value.truck.Id} {entity.Value.truck.Name} {entity.Value.truck.Paint}".Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    /// <summary>
    /// Asynchronously loads the list of parts for a given truck.
    /// </summary>
    /// <param name="truckId">The ID of the truck to load the parts for.</param>
    /// <returns>A list of TruckPartDTO objects.</returns>
    private async Task<List<TruckPartDTO>> LoadParts(long truckId)
    {
        var httpClient = HttpService.HttpClient;
        await BearerAccess.AddBearerToken(authenticationStateTask, httpClient);
        var truckParts = await httpClient.GetFromJsonAsync<List<TruckPartDTO>>($"api/truckpart/{truckId}");
        if (truckParts != null)
        {
            return truckParts.ToList();
        }
        return new List<TruckPartDTO>();
    }
    #endregion
}