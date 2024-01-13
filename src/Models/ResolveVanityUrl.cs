using System.Text.Json.Serialization;

namespace SteamWorkshopStats.Models;

public record class ResolveVanityUrl
{
    [JsonPropertyName("response")]
    public ResolveVanityUrlResponse Response { get; init; }
}

public record class ResolveVanityUrlResponse
{
    [JsonPropertyName("steamid")]
    public string SteamId { get; init; }

    [JsonPropertyName("success")]
    public int Success { get; init; }
}
