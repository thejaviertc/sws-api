using System.Text.Json.Serialization;

namespace SteamWorkshopStats.Models.Records;

public record class GetPlayerSummaries
{
    [JsonPropertyName("response")]
    public GetPlayerSummariesResponse Response { get; init; }
}

public record class GetPlayerSummariesResponse
{
    [JsonPropertyName("players")]
    public List<GetPlayerSummariesPlayer> Players { get; init; }
}

public record class GetPlayerSummariesPlayer
{
    [JsonPropertyName("personaname")]
    public string Username { get; init; }

    [JsonPropertyName("avatarfull")]
    public Uri ProfileImageUrl { get; init; }
}