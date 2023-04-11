using Microsoft.AspNetCore.Identity;

namespace Domain.Bases;
/// <summary>
/// Base class for user. 
/// Inherits from Microsoft.AspNetCore.Identity.IdentityUser. 
/// </summary>
public class BaseUser : IdentityUser<int>
{
    /// <summary>
    /// Auth0 identifier
    /// </summary>
    public string? Identifier { get; set; }
}
