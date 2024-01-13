using SteamWorkshopStats.Models;

namespace SteamWorkshopStats.Services;

public class SteamService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public SteamService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> LogTest(string id)
    {
        var client = _httpClientFactory.CreateClient("SteamClient");

        var response = await client.GetAsync(
            $"ISteamUser/ResolveVanityURL/v1/?key={_configuration["SteamApiKey"]}&vanityurl={id}"
        );

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadFromJsonAsync<ResolveVanityUrl>();

            if (responseData.Response.Success == 1)
                return responseData.Response.SteamId;

            throw new Exception("User doesn't exists");
        }

        throw new Exception("Unknown Error");
    }
}
