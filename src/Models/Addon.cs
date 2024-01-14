﻿namespace SteamWorkshopStats.Models;

public struct Addon : IComparable<Addon>
{
    public long Id { get; init; }

    public string Title { get; init; }

    public Uri ImageUrl { get; init; }

    public int Views { get; init; }

    public int Suscribers { get; init; }

    public int Favorites { get; init; }

    public int Likes { get; init; }

    public int Dislikes { get; init; }

    public int Stars { get; init; }

    public int CompareTo(Addon other)
    {
        return other.Id.CompareTo(Id);
    }
}
