using Domain.Bases;

namespace Infrastructure.Security;

/// <summary>
/// Represents a legacy user in the Tris system. This class is used instead of Guids.
/// Inherits from BaseUser.
/// </summary>
public class LegacyUser : BaseUser
{
    /// <summary>
    /// The Guid identifier for this user.
    /// </summary>
    public Guid GuidId { get; set; }

    /// <summary>
    /// The unique identifier for this user.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// The short name of the application associated with this user.
    /// </summary>
    public string? ApplicationShortName { get; set; }

    /// <summary>
    /// The username of this user.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// The full name of this user.
    /// </summary>
    public string? Fullname { get; set; }

    /// <summary>
    /// The roles assigned to this user.
    /// </summary>
    public List<string> Roles { get; private set; }

    /// <summary>
    /// Initializes a new instance of the LegacyUser class.
    /// </summary>
    public LegacyUser()
    {
        Roles = new List<string>();
    }
}