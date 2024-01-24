using System.Text.Json.Serialization;

namespace SteamWorkshopStats.Models.Records;

public record class GetUserFiles
{
	[JsonPropertyName("response")]
	public GetUserFilesResponse Response { get; init; }
}

public record class GetUserFilesResponse
{
	[JsonPropertyName("publishedfiledetails")]
	public List<PublishedFile> PublishedFiles { get; init; }
}

public record class PublishedFile
{
	[JsonPropertyName("publishedfileid")]
	public long Id { get; init; }

	[JsonPropertyName("title")]
	public string Title { get; init; }

	[JsonPropertyName("preview_url")]
	public Uri ImageUrl { get; init; }

	[JsonPropertyName("views")]
	public int Views { get; init; }

	[JsonPropertyName("subscriptions")]
	public int Subscribers { get; init; }

	[JsonPropertyName("favorited")]
	public int Favorites { get; init; }

	[JsonPropertyName("vote_data")]
	public VoteData Votes { get; init; }
}

public record class VoteData
{
	[JsonPropertyName("score")]
	public float Score { get; init; }

	[JsonPropertyName("votes_up")]
	public int Likes { get; init; }

	[JsonPropertyName("votes_down")]
	public int Dislikes { get; init; }
}
