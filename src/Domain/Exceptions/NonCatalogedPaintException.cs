namespace Domain.Exceptions;
/// <summary>
/// Exception called when Paint Color has not been Cataloged.
/// </summary>
public class NonCatalogedPaintException : Exception
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="color">Hex of color</param>
    public NonCatalogedPaintException(string color) : base($"Color has not been cataloged: {color}")
    {

    }
}
