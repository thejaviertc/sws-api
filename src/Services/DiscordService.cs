using System.Text;
using System.Text.Json;
using SteamWorkshopStats.Models;

namespace SteamWorkshopStats.Services;

public class DiscordService
{
	private readonly IConfiguration _configuration;
	private readonly IHttpClientFactory _httpClientFactory;

	public DiscordService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
	{
		_configuration = configuration;
		_httpClientFactory = httpClientFactory;
	}

	public async void LogUser(User user)
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

		var content = new StringContent(
			JsonSerializer.Serialize(payload),
			Encoding.UTF8,
			"application/json"
		);

		var response = await client.PostAsync(_configuration["DiscordLogUserWebhook"], content);

		if (!response.IsSuccessStatusCode)
			throw new Exception("Unknown Error");
	}
}
