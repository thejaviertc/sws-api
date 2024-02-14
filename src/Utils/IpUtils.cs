namespace SteamWorkshopStats.Utils;

public static class IpUtils
{
	/// <summary>
	/// Obtains the IP of the client that made the request
	/// </summary>
	/// <param name="context">Context of the Request</param>
	/// <returns>The IP of the Request</returns>
	public static string GetIp(HttpContext context)
	{
		string? ip = context.Request.Headers["X-Forwarded-For"];

		if (string.IsNullOrEmpty(ip))
		{
			ip = context.Connection.RemoteIpAddress?.ToString();
		}

		return ip ?? "Unknown";
	}
}
