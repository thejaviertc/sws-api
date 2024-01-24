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
	public async Task<string> GetSteamId(string profileId)
	{
		var client = _httpClientFactory.CreateClient("SteamClient");

		var response = await client.GetAsync(
			$"ISteamUser/ResolveVanityURL/v1/?key={_configuration["SteamApiKey"]}&vanityurl={profileId}"
		);

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadFromJsonAsync<ResolveVanityUrl>();

			if (responseData.Response.Success == 1)
				return responseData.Response.SteamId;

			// TODO:
			return null;
		}

		// TODO:
		throw new Exception("Unknown Error");
	}

	/// <summary>
	/// Retrieves the User's profile information using the SteamID.
	/// </summary>
	/// <param name="steamId">The SteamID of the User</param>
	/// <returns>The User's profile information, including the username and the profile image URL.</returns>
	/// <exception cref="Exception"></exception>
	public async Task<GetPlayerSummariesPlayer> GetProfileInfo(string steamId)
	{
		var client = _httpClientFactory.CreateClient("SteamClient");

		var response = await client.GetAsync(
			$"ISteamUser/GetPlayerSummaries/v2/?key={_configuration["SteamApiKey"]}&steamids={steamId}"
		);

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadFromJsonAsync<GetPlayerSummaries>();

			if (responseData.Response.Players.Count > 0)
				return responseData.Response.Players[0];

			// TODO:
			return null;
		}

		// TODO:
		throw new Exception("Unknown Error");
	}

	/// <summary>
	/// Retrieves a list of Addons made by the User based on their SteamID.
	/// </summary>
	/// <param name="steamId">The SteamID of the User.</param>
	/// <returns>A list of Addons sorted from newest to oldest.</returns>
	public async Task<List<Addon>> GetAddons(string steamId)
	{
		var client = _httpClientFactory.CreateClient("SteamClient");

		var response = await client.GetAsync(
			$"IPublishedFileService/GetUserFiles/v1/?key={_configuration["SteamApiKey"]}&steamid={steamId}&numperpage=500&return_vote_data=true"
		);

		if (response.IsSuccessStatusCode)
		{
			var responseData = await response.Content.ReadFromJsonAsync<GetUserFiles>();

			List<Addon> addons = new List<Addon>();

			if (responseData.Response.PublishedFiles.Count > 0)
			{
				foreach (var addon in responseData.Response.PublishedFiles)
				{
					addons.Add(
						new Addon
						{
							Id = addon.Id,
							Title = addon.Title,
							ImageUrl = addon.ImageUrl,
							Views = addon.Views,
							Suscribers = addon.Subscribers,
							Favorites = addon.Favorites,
							Likes = addon.Votes.Likes,
							Dislikes = addon.Votes.Dislikes,
							Stars = Addon.GetStars(
								addon.Votes.Likes + addon.Votes.Dislikes,
								addon.Votes.Score
							)
						}
					);
				}

				addons.Sort();
			}

			return addons;
		}

		// TODO:
		throw new Exception("Unknown Error");
	}
}
