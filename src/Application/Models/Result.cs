namespace Application.Models;


/// <summary>
/// This is a strange one, and I like the Rust language. 
/// Not quite like the rusts, but can collect errors when trying to do something.
/// Should be implemented when returning Void. Rather than return void, return Result.
/// If result contains errors, comment on it and throw exceptions. 
/// Represents the result of an operation that can either succeed or fail with a collection of errors.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Gets an array of error messages if the operation failed.
    /// </summary>
    public string[] Errors { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class with the specified success status and errors.
    /// </summary>
    /// <param name="succeeded">A value indicating whether the operation succeeded.</param>
    /// <param name="errors">An enumerable collection of error messages.</param>
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    /// <summary>
    /// Returns a new instance of the <see cref="Result"/> class representing a successful operation.
    /// </summary>
    /// <returns>A new instance of the <see cref="Result"/> class representing a successful operation.</returns>
    public static Result Ok()
    {
        return new Result(true, Array.Empty<string>());
    }

    /// <summary>
    /// Returns a new instance of the <see cref="Result"/> class representing a failed operation with the specified errors.
    /// </summary>
    /// <param name="errors">An enumerable collection of error messages.</param>
    /// <returns>A new instance of the <see cref="Result"/> class representing a failed operation with the specified errors.</returns>
    public static Result Error(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}