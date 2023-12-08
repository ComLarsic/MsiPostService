using System.Text.Json.Serialization;

namespace MsiPostServer.DTO;

/// <summary>
/// The Data Transfer Object for a profile post request
/// </summary>
[Serializable]
public struct ProfilePostRequestDTO
{
    [JsonPropertyName("profile")]
    public Guid Profile { get; set; }
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("PageSize")]
    public int PageSize { get; set; }
}
