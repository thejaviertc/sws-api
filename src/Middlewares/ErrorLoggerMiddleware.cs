using SteamWorkshopStats.Exceptions;
using SteamWorkshopStats.Services;

namespace SteamWorkshopStats.Middlewares;

public class ErrorLoggerMiddleware
{
	private readonly RequestDelegate _next;

	public ErrorLoggerMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IDiscordService discordService)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			string errorMessage = "HTTP 500 Internal Server Error";

			if (ex is SteamServiceException)
			{
				errorMessage = ex.Message;
			}

			context.Response.StatusCode = StatusCodes.Status500InternalServerError;

			_ = discordService.LogErrorAsync(
				context.Request.Path,
				context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
				errorMessage
			);

			await context.Response.WriteAsJsonAsync(new { Message = errorMessage });
		}
	}
}
