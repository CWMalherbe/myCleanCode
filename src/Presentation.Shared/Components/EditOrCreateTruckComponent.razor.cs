using System.Net.Http.Json;
using Application.Entities.TruckParts;
using Application.Entities.TruckParts.Commands;
using Application.Entities.Trucks;
using Application.Entities.Trucks.Commands;
using Domain.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Presentation.Shared.Components;
public partial class EditOrCreateTruckComponent
{
    #region Paramters
    [CascadingParameter]
    Task<AuthenticationState>? authenticationStateTask { get; set; }
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = new MudDialogInstance();
    /// <summary>
    /// Item name is a placeholder to display the name on the dialog
    /// </summary>
    [Parameter]
    public string ItemName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the <see cref="TruckDTO"/> instance.
    /// </summary>
    [Parameter]
    public TruckDTO Entity { get; set; } = new TruckDTO();
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

    private EditContext editContext = new EditContext(new object());
    private TruckPartDTO? createTruckPartDTO { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected async override Task OnParametersSetAsync()
    {
        editContext = new EditContext(Entity);
        await base.OnParametersSetAsync();
    }

    #region ActionControls

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
                HttpResponseMessage? response = await httpClient.PutAsJsonAsync($"api/{Path}", new UpdateTruckCommand()
                {
                    Name = Entity.Name,
                    Paint = Entity.Paint,
                    TruckId = Entity.Id,
                });
                //Not super sure how else to do this, but as long as the truck edit is successful
                if (Entity.Items != null)
                {
                    foreach (var part in Entity.Items)
                    {
                        if (part.Id == 0)
                        {
                            await httpClient.PostAsJsonAsync($"api/truckpart", new CreateTruckPartCommand
                            {
                                Name = part.Name,
                                Code = part.Code,
                                Condition = part.ConditionValue,
                                TruckId = part.TruckId,
                            });
                        }
                    }
                }
                result = response.IsSuccessStatusCode;
            }
            else
            {
                HttpResponseMessage? response = await httpClient.PostAsJsonAsync($"api/{Path}", new CreateTruckCommand()
                {
                    Name = Entity.Name,
                    Paint = Entity.Paint,
                    Items = Entity.Items == null ? new List<CreateTruckPartCommand>() : Entity.Items.Select(x => new CreateTruckPartCommand
                    {
                        Code = x.Code,
                        Name = x.Name,
                        TruckId = x.TruckId,
                        Condition = x.ConditionValue
                    }).ToList(),
                });
                result = response.IsSuccessStatusCode;
            }
            if (result == true)
            {
                Snackbar.Add($"{ItemName} {Entity.Name} was {(ShouldEdit ? "updated" : "created")} successfully", Severity.Success);
                Entity = new TruckDTO();
            }
            else
            {
                Snackbar.Add($"{ItemName} {Entity.Name} {(ShouldEdit ? "update" : "creation")} failed", Severity.Error);
            }
            MudDialog.Close(DialogResult.Ok(result));
        }
    }
    private void Cancel()
    {
        MudDialog.Cancel();
    }


    #endregion

    private async Task AddPart()
    {
        if (createTruckPartDTO == null)
        {
            createTruckPartDTO = new TruckPartDTO()
            {
                Code = string.Empty,
                Name = string.Empty,
                Condition = string.Empty,
                ConditionValue = ConditionEnum.Unknown,
            };
        }
        var dialog = await Dialog.ShowAsync<AddTruckPartDialogComponent>(string.Empty, new DialogParameters { ["entityDTO"] = createTruckPartDTO }, new DialogOptions() { DisableBackdropClick = true });
        var result = await dialog.Result;
        if (result.Canceled == false && (bool)result.Data == true)
        {
            if (Entity != null && Entity.Items != null)
            {
                Entity.Items.Add(new TruckPartDTO
                {
                    Code = createTruckPartDTO.Code,
                    Condition = createTruckPartDTO.Condition,
                    ConditionValue = createTruckPartDTO.ConditionValue,
                    Name = createTruckPartDTO.Name,
                    TruckId = Entity.Id,
                });
            }
        }
    }
}
