namespace Application.Exception;
/// <summary>
/// Returns a entity in database not found.
/// This is used when finding specific entities.
/// </summary>
public class NotFoundException : System.Exception
{
    /// <summary>
    /// Returns a entity in database not found.
    /// </summary>
    public NotFoundException()
        : base()
    {
    }

    /// <summary>
    /// Returns a entity in database not found.
    /// </summary>
    public NotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Returns a entity in database not found.
    /// </summary>
    public NotFoundException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Returns a entity in database not found.
    /// </summary>
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}
