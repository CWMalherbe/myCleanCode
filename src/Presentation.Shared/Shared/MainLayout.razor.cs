namespace Presentation.Shared.Shared;

/// <summary>
/// Partial class for the main layout of the application.
/// </summary>
public partial class MainLayout
{
    /// <summary>
    /// The name of the application.
    /// </summary>
    private readonly string applicationName = "The Template";

    /// <summary>
    /// A boolean indicating whether the drawer is open or closed.
    /// </summary>
    private bool _drawerOpen = true;

    /// <summary>
    /// Toggles the drawer open or closed.
    /// </summary>
    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}