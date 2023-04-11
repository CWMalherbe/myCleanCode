namespace Domain.Entities;

/// <summary>
/// Represents the details of an account.
/// </summary>
public class AccountDetails
{
    /// <summary>
    /// Gets or sets the roles associated with the account.
    /// </summary>
    public string[]? Roles { get; set; }

    /// <summary>
    /// Gets or sets the PSID (Personnel Security Identifier) of the account.
    /// </summary>
    public int Psid { get; set; }

    /// <summary>
    /// Gets or sets the customer site ID associated with the account.
    /// </summary>
    public int CustomerSiteId { get; set; }

    /// <summary>
    /// Gets or sets the list of trucks associated with the account.
    /// </summary>
    public object[]? Trucks { get; set; }

    /// <summary>
    /// Gets or sets the list of sites associated with the account.
    /// </summary>
    public string[]? Sites { get; set; }
}