using Application.Bases;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Presentation.Shared.Components;

/// <summary>
/// This class represents a component that confirms deletion of an item.
/// </summary>
public partial class ConfirmDeleteComponent
{
    #region Parameters
    [CascadingParameter]
    Task<AuthenticationState>? authenticationStateTask { get; set; }
    /// <summary>
    /// The instance of the MudDialog component that is used to display the confirmation dialog.
    /// </summary>
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; } = new MudDialogInstance();

    /// <summary>
    /// The name of the item that is being deleted.
    /// </summary>
    [Parameter]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// Path to API
    /// </summary>
    [Parameter]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Base for any entity to delete
    /// </summary>
    [Parameter]
    public BaseEntityDTO Entity { get; set; } = new BaseEntityDTO();

    #endregion

    /// <summary>
    /// Closes the MudDialog component with a DialogResult of Ok, indicating that the deletion should proceed.
    /// </summary>
    private async Task Submit()
    {
        var httpClient = HttpService.HttpClient;
        await BearerAccess.AddBearerToken(authenticationStateTask, httpClient);
        bool result = false;

        HttpResponseMessage? response = await httpClient.DeleteAsync($"api/{Path}/{Entity.Id}");
        result = response.IsSuccessStatusCode;

        if (result == true)
        {
            Snackbar.Add($"{ItemName} was deleted successfully", Severity.Success);
        }
        else
        {
            Snackbar.Add($"{ItemName} deletion failed", Severity.Error);
        }
        MudDialog.Close(DialogResult.Ok(true));
    }

    /// <summary>
    /// Cancels the deletion operation by closing the MudDialog component without setting a DialogResult.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}