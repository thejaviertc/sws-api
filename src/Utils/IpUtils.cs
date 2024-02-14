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

		ip = string.IsNullOrEmpty(ip) ? context.Connection.RemoteIpAddress?.ToString() : ip.Split(",")[0].Trim();

		return ip ?? "Unknown";
	}
}
