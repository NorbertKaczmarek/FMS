namespace FMS.API.Models;

public enum SortDirection
{
    ASC,
    DESC
}

public class PageQuery
{
    public string SearchPhrase { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SortBy { get; set; }
    public SortDirection SortDirection { get; set; }
}
