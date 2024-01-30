using System.Text.Json.Serialization;

namespace SteamWorkshopStats.Models.Records;

public record class GetPlayerSummaries
{
	[JsonPropertyName("response")]
	public required GetPlayerSummariesResponse Response { get; init; }
}

public record class GetPlayerSummariesResponse
{
	[JsonPropertyName("players")]
	public required List<GetPlayerSummariesPlayer> Players { get; init; }
}

public record class GetPlayerSummariesPlayer
{
	[JsonPropertyName("personaname")]
	public required string Username { get; init; }

	[JsonPropertyName("avatarfull")]
	public required Uri ProfileImageUrl { get; init; }
}
