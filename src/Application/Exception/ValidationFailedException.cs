namespace Application.Exception;
/// <summary>
/// Used when entity implementation of validation failed.
/// </summary>
public class ValidationFailedException : System.Exception
{
    /// <summary>
    /// Used when entity implementation of validation failed.
    /// </summary>
    /// <param name="reason"></param>
    /// <param name="objectName"></param>
    /// <param name="parameterName"></param>
    public ValidationFailedException(string reason, string objectName, string parameterName)
    : base($"Validation for {typeof(object)} failed: {objectName}.{parameterName}")
    {
    }
}
