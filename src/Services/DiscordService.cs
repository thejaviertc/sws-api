﻿using System.Text;
using System.Text.Json;
using SteamWorkshopStats.Models;

namespace SteamWorkshopStats.Services;

public class DiscordService : IDiscordService
{
	private readonly IConfiguration _configuration;

	private readonly IHttpClientFactory _httpClientFactory;

	public DiscordService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
	{
		_configuration = configuration;
		_httpClientFactory = httpClientFactory;
	}

	/// <summary>
	/// Logs the path and the IP of the User into a Discord Channel
	/// </summary>
	/// <param name="path">Path where the User made a query</param>
	/// <param name="ip">IP of the User</param>
	/// <returns></returns>
	public async Task LogQueryAsync(string path, string ip)
	{
		var client = _httpClientFactory.CreateClient("DiscordClient");

		var payload = new
		{
			embeds = new[]
			{
				new
				{
					title = "New Query",
					color = 5814783,
					type = "rich",
					fields = new[]
					{
						new
						{
							name = "Path",
							value = path,
							inline = false
						},
						new
						{
							name = "IP",
							value = ip,
							inline = true
						},
					},
					timestamp = DateTime.UtcNow
				}
			}
		};

		var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

		var response = await client.PostAsync(_configuration["DiscordLogQueryWebhook"], content);

		// TODO:
		if (!response.IsSuccessStatusCode)
			throw new Exception("Unknown Error");
	}

	/// <summary>
	/// Logs the User's Stats into a Discord Channel
	/// </summary>
	/// <param name="user">The User whose data is going to be logged</param>
	/// <returns></returns>
	public async Task LogUserAsync(User user)
	{
		var client = _httpClientFactory.CreateClient("DiscordClient");

		var payload = new
		{
			embeds = new[]
			{
				new
				{
					title = $"{user.Username} ({user.SteamId})",
					color = 5814783,
					type = "rich",
					fields = new[]
					{
						new
						{
							name = "Views",
							value = user.Views,
							inline = true
						},
						new
						{
							name = "Suscribers",
							value = user.Suscribers,
							inline = true
						},
						new
						{
							name = "Favorites",
							value = user.Favorites,
							inline = true
						},
						new
						{
							name = "Likes",
							value = user.Likes,
							inline = true
						},
						new
						{
							name = "Dislikes",
							value = user.Dislikes,
							inline = true
						}
					},
					thumbnail = new { url = user.ProfileImageUrl },
					timestamp = DateTime.UtcNow
				}
			}
		};

		var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

		var response = await client.PostAsync(_configuration["DiscordLogUserWebhook"], content);

		// TODO:
		if (!response.IsSuccessStatusCode)
			throw new Exception("Unknown Error");
	}
}
