namespace Application.Interfaces;
/// <summary>
/// Interface for JWT token creation and Validation
/// </summary>
public interface IJWTAuthentication
{
    /// <summary>
    /// Generates a token from a valid userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public string GenerateToken(int userId);
    /// <summary>
    /// Validates a token from context header
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public int ValidateToken(string token);
    /// <summary>
    /// Validates a suername from token header
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public string ValidateUsernameFromToken(string token);
}
