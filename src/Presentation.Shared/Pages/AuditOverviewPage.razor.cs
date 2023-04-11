using System.Net.Http.Json;
using Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Presentation.Shared.Pages;

/// <summary>
/// Represents the page for displaying the overview of audit entities.
/// </summary>
public partial class AuditOverviewPage
{
    /// <summary>
    /// The header of the page.
    /// </summary>
    private readonly string PageHeader = "Audit Overview";

    /// <summary>
    /// A list of audit entities to be displayed on the page.
    /// </summary>
    private List<AuditEntity>? Audits { get; set; }

    /// <summary>
    /// A cascading parameter that contains the authentication state of the user.
    /// </summary>
    [CascadingParameter]
    Task<AuthenticationState>? authenticationStateTask { get; set; }

    /// <summary>
    /// Initializes the component after it is added to the UI.
    /// </summary>
    protected async override Task OnInitializedAsync()
    {
        var httpClient = HttpService.HttpClient;
        await BearerAccess.AddBearerToken(authenticationStateTask, httpClient);
        Audits = await httpClient.GetFromJsonAsync<List<AuditEntity>>($"api/audittrail", default);
        await base.OnInitializedAsync();
    }
}