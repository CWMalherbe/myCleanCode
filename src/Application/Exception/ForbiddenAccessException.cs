namespace Application.Exception;
/// <summary>
/// Not allowed to action result.
/// Should return error code 418. 
/// </summary>
public class ForbiddenAccessException : System.Exception
{
    /// <summary>
    /// Not allowed to action result.
    /// </summary>
    public ForbiddenAccessException() : base() { }
}
