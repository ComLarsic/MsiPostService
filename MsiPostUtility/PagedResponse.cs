namespace MsiPostUtility;

/// <summary>
/// A paged response
/// </summary>
public struct PagedResponse<T>
{
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public List<T> Values { get; set; }
}
