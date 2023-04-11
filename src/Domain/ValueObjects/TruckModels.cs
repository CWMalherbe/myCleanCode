namespace Domain.ValueObjects;

/// <summary>
/// A value object representing different truck models.
/// </summary>
public class TruckModels : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TruckModels"/> class.
    /// </summary>
    /// <param name="truckCode">The code representing the truck model.</param>
    private TruckModels(string truckCode)
    {
        TruckCode = truckCode;
    }

    /// <summary>
    /// Returns a new instance of the <see cref="TruckModels"/> class with the specified truck code.
    /// </summary>
    /// <param name="truckCode">The code representing the truck model.</param>
    /// <returns>A new instance of the <see cref="TruckModels"/> class with the specified truck code.</returns>
    /// <exception cref="NonCatalogedPaintException">Thrown when the specified truck code is not in the list of supported truck models.</exception>
    public static TruckModels From(string truckCode)
    {
        TruckModels truckModels = new TruckModels(truckCode);

        if (!SupportedTruckModels.Contains(truckModels))
        {
            throw new NonCatalogedPaintException(truckCode);
        }

        return truckModels;
    }

    /// <summary>
    /// Gets the list of supported truck models.
    /// </summary>
    protected static IEnumerable<TruckModels> SupportedTruckModels
    {
        get
        {
            yield return Garbage;
            yield return Hauling;
            yield return Refrigerant;
        }
    }

    /// <summary>
    /// Implicitly converts a <see cref="TruckModels"/> object to a string.
    /// </summary>
    /// <param name="truckModel">The <see cref="TruckModels"/> object to convert.</param>
    /// <returns>The string representation of the specified <see cref="TruckModels"/> object.</returns>
    public static implicit operator string?(TruckModels truckModel)
    {
        return truckModel.ToString();
    }

    /// <summary>
    /// Explicitly converts a string to a <see cref="TruckModels"/> object.
    /// </summary>
    /// <param name="truckCode">The string representation of the truck model.</param>
    /// <returns>A new instance of the <see cref="TruckModels"/> class with the specified truck code.</returns>
    public static explicit operator TruckModels(string truckCode)
    {
        return From(truckCode);
    }

    /// <summary>
    /// Gets the code representing the truck model.
    /// </summary>
    public string TruckCode { get; private set; } = string.Empty;

    /// <summary>
    /// Gets a <see cref="TruckModels"/> object representing the garbage truck model.
    /// </summary>
    public static TruckModels Garbage => new("QBEJ");

    /// <summary>
    /// Gets a <see cref="TruckModels"/> object representing the hauling truck model.
    /// </summary>
    public static TruckModels Hauling => new("LFHI");

    /// <summary>
    /// Gets a <see cref="TruckModels"/> object representing the refrigerant truck model.
    /// </summary>
    public static TruckModels Refrigerant => new("AAAA");

    /// <summary>
    /// Returns an enumerator that iterates through the equality components of the <see cref="TruckModels"/> object.
    /// </summary>
    /// <returns>An enumerator that iterates through the equality components of the <see cref="TruckModels"/> object.</returns>
    protected override IEnumerable<object> GetEqualityComponents()

    {
        yield return TruckCode;
    }
}
