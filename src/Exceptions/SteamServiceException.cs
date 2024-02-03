namespace SteamWorkshopStats.Exceptions;

public class SteamServiceException : Exception
{
	public SteamServiceException(string message)
		: base(message) { }
}
