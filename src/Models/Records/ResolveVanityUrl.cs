using System.Text.Json.Serialization;

namespace SteamWorkshopStats.Models.Records;

public record class ResolveVanityUrl
{
	[JsonPropertyName("response")]
	public required ResolveVanityUrlResponse Response { get; init; }
}

public record class ResolveVanityUrlResponse
{
	[JsonPropertyName("success")]
	public required int Success { get; init; }

	[JsonPropertyName("steamid")]
	public string? SteamId { get; init; }
}
