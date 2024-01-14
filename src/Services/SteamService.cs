using SteamWorkshopStats.Models;
using SteamWorkshopStats.Models.Records;

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

            return null;
        }

        // TODO: 
        throw new Exception("Unknown Error");
    }

    //public User GetUser(string steamId)
    //{
    //    return new User
    //    {
    //        SteamId = steamId,
    //        Username = "Paco",
    //        ProfileImageUrl = "adadas",
    //        Views = 10,
    //        Suscribers = 100,
    //        Favorites = 12,
    //        Likes = 10,
    //        Dislikes = 2,
    //        Addons = new List<Addon>()
    //    };
    //}

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

            return null;
        }

        // TODO: 
        throw new Exception("Unknown Error");
    }

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
                    addons.Add(new Addon
                    {
                        Id = addon.Id,
                        Title = addon.Title,
                        ImageUrl = addon.ImageUrl,
                        Views = addon.Views,
                        Suscribers = addon.Subscribers,
                        Favorites = addon.Favorites,
                        Likes = addon.Votes.Likes,
                        Dislikes = addon.Votes.Dislikes
                    });
                }
            }

            return addons;
        }

        // TODO: 
        throw new Exception("Unknown Error");
    }
}
