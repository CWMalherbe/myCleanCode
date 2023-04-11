using System.Security.AccessControl;
using Application.Exception;

namespace Application.Extentions;
/// <summary>
/// A colletion of all the types of validatoins that need to be performed. 
/// Could have universal items, but for now just added the 64.
/// </summary>
public static class ValidationExtentions
{
    /// <summary>
    /// Checks if string field is less than 64. 
    /// Should never cut off the end, rather let user do such.
    /// HINT: typeof(Truck).Name, nameof(Entity.Name)
    /// </summary>
    /// <param name="input">Input string to test</param>
    /// <param name="objectName">Name of type (example TruckPart)</param>
    /// <param name="parameterName">Parameter name of the object</param>
    /// <exception cref="ValidationFailedException"></exception>
    public static void CheckString64Field(string? input, string objectName, string parameterName)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ValidationFailedException("String Field empty", objectName, parameterName);
        }
        else if (input.Length > 64)
        {
            throw new ValidationFailedException("String Field Length greater than 64 character", objectName, parameterName);
        }
    }
}
