using System.Net.Http.Json;
using Application.Entities.TruckParts;
using Application.Entities.Trucks;
using Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Presentation.Shared.Components;

/// <summary>
/// Represents a dialog component for editing a truck part.
/// </summary>
public partial class AddTruckPartDialogComponent
{
    /// <summary>
    /// Gets or sets the <see cref="MudDialogInstance"/> instance.
    /// </summary>
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = new MudDialogInstance();

    /// <summary>
    /// Gets or sets the <see cref="TruckPartDTO"/> instance.
    /// </summary>
    [Parameter]
    public TruckPartDTO entityDTO { get; set; } = new TruckPartDTO();

    /// <summary>
    /// Gets or sets the list of <see cref="TruckDTO"/> instances.
    /// </summary>
    private List<TruckDTO>? Trucks { get; set; } = new List<TruckDTO>();

    /// <summary>
    /// Gets or sets a value indicating whether the trucks have been loaded.
    /// </summary>
    private bool loadedTrucks { get; set; } = false;

    /// <summary>
    /// Gets or sets the <see cref="AuthenticationState"/> task.
    /// </summary>
    [CascadingParameter]
    Task<AuthenticationState>? authenticationStateTask { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="EditContext"/> instance.
    /// </summary>
    private EditContext editContext { get; set; } = new EditContext(new object());

    /// <summary>
    /// Executes initialization logic after all parameters have been assigned.
    /// </summary>
    protected async override Task OnParametersSetAsync()
    {
        editContext = new EditContext(entityDTO);
        if (entityDTO.TruckId < 0)
        {
            loadedTrucks = true;
            var httpClient = HttpService.HttpClient;
            await BearerAccess.AddBearerToken(authenticationStateTask, httpClient);
            Trucks = await httpClient.GetFromJsonAsync<List<TruckDTO>>($"api/truck");
        }
        await base.OnParametersSetAsync();
    }

    /// <summary>
    /// Submits the edited truck part to the server.
    /// </summary>
    private void Submit()
    {
        bool validationStatus = editContext.Validate();
        // I know I can cut this short, but it is in here for testing reasons
        if (validationStatus == true)
        {
            entityDTO.Condition = Enum.GetName(typeof(ConditionEnum), entityDTO.ConditionValue);
            Snackbar.Add($"Truck Part {entityDTO.Name} was created successfully", Severity.Info);
            MudDialog.Close(DialogResult.Ok(true));
        }
    }

    /// <summary>
    /// Cancels the dialog.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
