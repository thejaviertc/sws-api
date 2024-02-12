using SteamWorkshopStats.Services;
using SteamWorkshopStats.Utils;

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
		_ = discordService.LogQueryAsync(context.Request.Path, IpUtils.GetIp(context));

		await _next(context);
	}
}
