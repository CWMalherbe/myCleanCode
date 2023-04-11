namespace Application.Bases;

/// <summary>
/// Represents a base pagination model with default properties for page number, page size, search string, column name and direction.
/// </summary>
public partial record BasePagination
{
    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items to display per page.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the string to search for.
    /// </summary>
    public string SearchString { get; set; } = "";

    /// <summary>
    /// Gets or sets the name of the column to sort by.
    /// </summary>
    public string ColumnName { get; set; } = "";

    /// <summary>
    /// Gets or sets the sort direction ('asc' or 'desc').
    /// </summary>
    public string Direction { get; set; } = "asc";
}