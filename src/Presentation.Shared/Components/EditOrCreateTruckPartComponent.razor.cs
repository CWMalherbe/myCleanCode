using System.Net.Http.Json;
using Application.Entities.TruckParts;
using Application.Entities.TruckParts.Commands;
using Application.Entities.Trucks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Presentation.Shared.Components;
public partial class EditOrCreateTruckPartComponent
{
    #region Parameters

    /// <summary>
    /// Gets or sets the <see cref="AuthenticationState"/> task.
    /// </summary>
    [CascadingParameter]
    Task<AuthenticationState>? authenticationStateTask { get; set; }
    /// <summary>
    /// Gets or sets the <see cref="MudDialogInstance"/> instance.
    /// </summary>
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = new MudDialogInstance();
    /// <summary>
    /// Item name is a placeholder to display the name on the dialog
    /// </summary>
    [Parameter]
    public string ItemName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the <see cref="TruckPartDTO"/> instance.
    /// </summary>
    [Parameter]
    public TruckPartDTO Entity { get; set; } = new TruckPartDTO();
    /// <summary>
    /// True if edit mode
    /// </summary>
    [Parameter]
    public bool ShouldEdit { get; set; } = true;
    /// <summary>
    /// Holder for the path to only keep track of one source
    /// </summary>
    [Parameter]
    public string Path { get; set; } = string.Empty;

    #endregion

    /// <summary>
    /// Gets or sets the list of <see cref="TruckDTO"/> instances.
    /// </summary>
    private List<TruckDTO>? trucks = new List<TruckDTO>();
    /// <summary>
    /// Gets or sets the <see cref="EditContext"/> instance.
    /// </summary>
    private EditContext editContext = new EditContext(new object());
    private readonly DialogOptions dialogOptions = new DialogOptions();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected async override Task OnInitializedAsync()
    {
        var httpClient = HttpService.HttpClient;
        await BearerAccess.AddBearerToken(authenticationStateTask, httpClient);
        trucks = await httpClient.GetFromJsonAsync<List<TruckDTO>>($"api/truck");
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Executes initialization logic after all parameters have been assigned.
    /// </summary>
    protected async override Task OnParametersSetAsync()
    {
        editContext = new EditContext(Entity);
        dialogOptions.DisableTrucksDropdown = !(ShouldEdit == false && Entity.TruckId == 0);
        await base.OnParametersSetAsync();
    }

    #region ActionControls


    /// <summary>
    /// Submits the edited truck part to the server.
    /// </summary>
    private async Task Submit()
    {
        bool validationStatus = editContext.Validate();
        if (validationStatus == true)
        {
            var httpClient = HttpService.HttpClient;
            await BearerAccess.AddBearerToken(authenticationStateTask, httpClient);
            bool result = false;
            if (ShouldEdit == true)
            {
                HttpResponseMessage? response = await httpClient.PutAsJsonAsync($"api/{Path}", new UpdateTruckPartCommand()
                {
                    TruckId = Entity.TruckId,
                    TruckPartId = Entity.Id,
                    Code = Entity.Code,
                    Condition = Entity.ConditionValue,
                    Name = Entity.Name
                });
                result = response.IsSuccessStatusCode;
            }
            else
            {
                HttpResponseMessage? response = await httpClient.PostAsJsonAsync($"api/{Path}", new CreateTruckPartCommand()
                {
                    Code = Entity.Code,
                    Condition = Entity.ConditionValue,
                    Name = Entity.Name,
                    TruckId = Entity.TruckId
                });
                result = response.IsSuccessStatusCode;
            }
            if (result == true)
            {
                Snackbar.Add($"{ItemName} {Entity.Name} was {(ShouldEdit ? "updated" : "created")} successfully", Severity.Success);
                Entity = new TruckPartDTO();
            }
            else
            {
                Snackbar.Add($"{ItemName} {Entity.Name} {(ShouldEdit ? "update" : "creation")} failed", Severity.Error);
            }
            MudDialog.Close(DialogResult.Ok(result));
        }
    }

    /// <summary>
    /// Cancels the dialog.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }


    #endregion

    private class DialogOptions
    {
        public bool DisableTrucksDropdown = true;
    }
}

