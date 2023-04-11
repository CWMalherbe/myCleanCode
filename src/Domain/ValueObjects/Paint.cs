namespace Domain.ValueObjects;


/// <summary>
/// Represents a color of paint.
/// </summary>
public class Paint : ValueObject
{
    private Paint(string hex)
    {
        Hex = hex;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Paint"/> class with the specified hex color code.
    /// </summary>
    /// <param name="hex">The hex color code of the paint.</param>
    /// <returns>A new instance of the <see cref="Paint"/> class with the specified hex color code.</returns>
    /// <exception cref="NonCatalogedPaintException">Thrown if the hex color code is not supported.</exception>
    public static Paint From(string hex)
    {
        Paint paint = new Paint(hex);

        if (!SupportedPaintColors.Contains(paint))
        {
            throw new NonCatalogedPaintException(hex);
        }

        return paint;
    }

    /// <summary>
    /// Gets the equality components for the paint.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{Object}"/> containing the equality components for the paint.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hex;
    }

    /// <summary>
    /// Converts the <see cref="Paint"/> object to a string.
    /// </summary>
    /// <param name="paint">The <see cref="Paint"/> object to convert to a string.</param>
    /// <returns>The hex color code of the <see cref="Paint"/> object.</returns>
    public static implicit operator string?(Paint paint)
    {
        return paint.ToString();
    }

    /// <summary>
    /// Converts a hex color code to a <see cref="Paint"/> object.
    /// </summary>
    /// <param name="hex">The hex color code to convert to a <see cref="Paint"/> object.</param>
    /// <returns>A new instance of the <see cref="Paint"/> class with the specified hex color code.</returns>
    public static explicit operator Paint(string hex)
    {
        return From(hex);
    }

    /// <summary>
    /// Gets the hex color code of the paint.
    /// </summary>
    public string Hex { get; private set; } = string.Empty;

    /// <summary>
    /// Gets a <see cref="Paint"/> object that represents the color white.
    /// </summary>
    public static Paint White => new("#FFFFFF");

    /// <summary>
    /// Gets a <see cref="Paint"/> object that represents the color black.
    /// </summary>
    public static Paint Black => new("#000000");

    /// <summary>
    /// Gets a <see cref="Paint"/> object that represents an unknown color.
    /// </summary>
    public static Paint Unkown => new("#8B4513");

    /// <summary>
    /// Gets a <see cref="Paint"/> object that represents the color red.
    /// </summary>
    public static Paint Red => new("#FF0000");

    /// <summary>
    /// Gets a <see cref="Paint"/> object that represents of the color yellow.
    /// </summary>
    public static Paint Yellow => new("#FFFF00");

    /// <summary>
    /// Gets a collection of all the supported paint colors.
    /// </summary>
    public static IEnumerable<Paint> SupportedPaintColors
    //protected static IEnumerable<Paint> SupportedPaintColors
    {
        get
        {
            yield return White;
            yield return Black;
            yield return Unkown;
            yield return Red;
            yield return Yellow;
        }
    }
}
