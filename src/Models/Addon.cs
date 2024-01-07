namespace SteamWorkshopStats.Models;

public struct Addon
{
    public string Id { get; init; }

    public string Title { get; init; }

    public string ImageUrl { get; init; }

    public int Views { get; init; }

    public int Suscribers { get; init; }

    public int Favorites { get; init; }

    public int Likes { get; init; }

    public int Dislikes { get; init; }

    public int Stars { get; init; }
}
