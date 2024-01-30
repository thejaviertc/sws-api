using System.Text.Json.Serialization;

namespace SteamWorkshopStats.Models.Records;

public record class GetUserFiles
{
	[JsonPropertyName("response")]
	public required GetUserFilesResponse Response { get; init; }
}

public record class GetUserFilesResponse
{
	[JsonPropertyName("publishedfiledetails")]
	public IEnumerable<PublishedFile>? PublishedFiles { get; init; }
}

public record class PublishedFile
{
	[JsonPropertyName("publishedfileid")]
	public required long Id { get; init; }

	[JsonPropertyName("title")]
	public required string Title { get; init; }

	[JsonPropertyName("preview_url")]
	public required Uri ImageUrl { get; init; }

	[JsonPropertyName("views")]
	public required int Views { get; init; }

	[JsonPropertyName("subscriptions")]
	public required int Subscribers { get; init; }

	[JsonPropertyName("favorited")]
	public required int Favorites { get; init; }

	[JsonPropertyName("vote_data")]
	public required VoteData Votes { get; init; }
}

public record class VoteData
{
	[JsonPropertyName("score")]
	public required float Score { get; init; }

	[JsonPropertyName("votes_up")]
	public int? Likes { get; init; }

	[JsonPropertyName("votes_down")]
	public int? Dislikes { get; init; }
}
