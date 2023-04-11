namespace Domain.Exceptions;
/// <summary>
/// <inheritdoc/>
/// </summary>
public class NonCatalogedTruckModelException : Exception
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="truckCode"></param>
    public NonCatalogedTruckModelException(string truckCode) : base($"TruckModel has not been cataloged: {truckCode}")
    {

    }
}
