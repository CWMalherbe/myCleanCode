using System.Web;
using Application.Bases;

namespace Presentation.Shared.Extentions;

/// <summary>
/// Extension methods for <see cref="System.Object"/> to create a query string from its properties with non-null values.
/// </summary>
public static class QueryStringExtentions
{
    /// <summary>
    /// Returns a query string that represents the object, using property names as keys and property values as values.
    /// </summary>
    /// <param name="obj">The object to serialize as a query string.</param>
    /// <returns>A query string representation of the object.</returns>
    public static string GetQueryGeneralString(this object obj)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.

        // Get all properties of the object that have a non-null value.
        var properties = from p in obj.GetType().GetProperties()
                         where p.GetValue(obj, null) != null
                         select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

        // Join the properties into a query string.
        return string.Join("&", properties.ToArray());

#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    /// <summary>
    /// Returns a query string that represents the pagination properties of the input object.
    /// </summary>
    /// <param name="input">The input object of type <see cref="BasePagination"/>.</param>
    /// <returns>A string that contains a URL-encoded query string for pagination.</returns>
    public static string GetPaginationQueryString(BasePagination input)
    {
        // Initialize the return value with the base URL and the page number and page size properties
        string returnValue = $"Paginated?" +
            $"{nameof(input.PageNumber)}={input.PageNumber}&" +
            $"{nameof(input.PageSize)}={input.PageSize}";

        // If the search string property is not null or empty, append it to the return value
        if (string.IsNullOrEmpty(input.SearchString) == false)
        {
            returnValue += $"&{nameof(input.SearchString).ToLower()}={HttpUtility.UrlEncode(input.SearchString)}";
        }

        // If the column name property is not null or empty, append it to the return value
        if (string.IsNullOrEmpty(input.ColumnName) == false)
        {
            returnValue += $"&{nameof(input.ColumnName).ToLower()}={HttpUtility.UrlEncode(input.ColumnName)}";
        }

        // If the direction property is not null or empty, append it to the return value
        if (string.IsNullOrEmpty(input.Direction) == false)
        {
            returnValue += $"&{nameof(input.Direction).ToLower()}={HttpUtility.UrlEncode(input.Direction)}";
        }

        // Return the resulting string
        return returnValue;
    }
}
