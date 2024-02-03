using SteamWorkshopStats.Models;
using SteamWorkshopStats.Models.Records;

namespace SteamWorkshopStats.Services;

public class SteamService : ISteamService
{
	private readonly IConfiguration _configuration;

	private readonly IHttpClientFactory _httpClientFactory;

	public SteamService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
	{
		_configuration = configuration;
		_httpClientFactory = httpClientFactory;
	}

	/// <summary>
	/// Retrieves the SteamID of the User, using the Vanity URL.
	/// </summary>
	/// <param name="profileId">The ProfileID from the URL of the User's profile</param>
	/// <returns>The SteamID of the User</returns>
	/// <exception cref="Exception"></exception>
	public async Task<string?> GetSteamIdAsync(string profileId)
	{
		var client = _httpClientFactory.CreateClient("SteamClient");

		var response = await client.GetAsync(
			$"ISteamUser/ResolveVanityURL/v1/?key={_configuration["SteamApiKey"]}&vanityurl={profileId}"
		);

		if (!response.IsSuccessStatusCode)
			return null;

		var responseData = await response.Content.ReadFromJsonAsync<ResolveVanityUrl>();

		if (responseData is null || responseData.Response.Success != 1)
			return null;

		return responseData.Response.SteamId;
	}

	/// <summary>
	/// Retrieves the User's profile information using the SteamID.
	/// </summary>
	/// <param name="steamId">The SteamID of the User</param>
	/// <returns>The User's profile information, including the username and the profile image URL.</returns>
	/// <exception cref="Exception"></exception>
	public async Task<GetPlayerSummariesPlayer?> GetProfileInfoAsync(string steamId)
	{
		var client = _httpClientFactory.CreateClient("SteamClient");

		var response = await client.GetAsync(
			$"ISteamUser/GetPlayerSummaries/v2/?key={_configuration["SteamApiKey"]}&steamids={steamId}"
		);

		if (!response.IsSuccessStatusCode)
			return null;

		var responseData = await response.Content.ReadFromJsonAsync<GetPlayerSummaries>();

		if (responseData is null || responseData.Response.Players.Count == 0)
			return null;

		return responseData.Response.Players.ElementAt(0);
	}

	/// <summary>
	/// Retrieves a list of Addons made by the User based on their SteamID.
	/// </summary>
	/// <param name="steamId">The SteamID of the User.</param>
	/// <returns>A list of Addons sorted from newest to oldest.</returns>
	public async Task<List<Addon>> GetAddonsAsync(string steamId)
	{
		var client = _httpClientFactory.CreateClient("SteamClient");

		var response = await client.GetAsync(
			$"IPublishedFileService/GetUserFiles/v1/?key={_configuration["SteamApiKey"]}&steamid={steamId}&numperpage=500&return_vote_data=true"
		);

		if (!response.IsSuccessStatusCode)
			return new List<Addon>();

		var responseData = await response.Content.ReadFromJsonAsync<GetUserFiles>();

		if (responseData is null || responseData.Response.PublishedFiles is null)
			return new List<Addon>();

		List<Addon> addons = new List<Addon>();

		foreach (var addon in responseData.Response.PublishedFiles)
		{
			int likes = addon.Votes.Likes ?? 0;
			int dislikes = addon.Votes.Dislikes ?? 0;

			addons.Add(
				new Addon
				{
					Id = addon.Id,
					Title = addon.Title,
					ImageUrl = addon.ImageUrl,
					Views = addon.Views,
					Suscribers = addon.Subscribers,
					Favorites = addon.Favorites,
					Likes = likes,
					Dislikes = dislikes,
					Stars = Addon.GetNumberOfStars(likes + dislikes, addon.Votes.Score)
				}
			);
		}

		addons.Sort();

		return addons;
	}
}
