using SteamWorkshopStats.Services;

namespace SteamWorkshopStats.Middlewares;

public class QueryLoggerMiddleware
{
	private readonly RequestDelegate _next;

	public QueryLoggerMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IDiscordService discordService)
	{
		_ = discordService.LogQueryAsync(
			context.Request.Path,
			context.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
		);

		await _next(context);
	}
}
