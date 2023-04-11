using Application.Entities.TruckParts;
using Microsoft.AspNetCore.Components;

namespace Presentation.Shared.Components;

/// <summary>
/// Represents a component for displaying a table of truck parts.
/// </summary>
public partial class TruckPartTableComponent
{
    /// <summary>
    /// Gets or sets a list of truck parts to display in the table.
    /// </summary>
    [Parameter]
    public List<TruckPartDTO>? TruckParts { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display controls for adding new truck parts.
    /// </summary>
    [Parameter]
    public bool addControls { get; set; } = false;

    /// <summary>
    /// Gets or sets the search string used for filtering the table.
    /// </summary>
    private string searchString { get; set; } = string.Empty;

    /// <summary>
    /// Invoked when the component has been rendered in the UI.
    /// </summary>
    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Determines whether the specified truck part should be included in the filtered table based on the current search string.
    /// </summary>
    /// <param name="entity">The truck part to evaluate.</param>
    /// <returns>true if the truck part should be included in the filtered table; otherwise, false.</returns>
    private bool FilterFunction(TruckPartDTO entity)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if ($"{entity.Id} {entity.Name} {entity.Code} {entity.Condition} {entity.TruckId}".Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
