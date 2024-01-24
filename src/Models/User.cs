﻿namespace SteamWorkshopStats.Models;

public struct User
{
	public string SteamId { get; init; }

	public string Username { get; init; }

	public Uri ProfileImageUrl { get; init; }

	public int Views { get; init; }

	public int Suscribers { get; init; }

	public int Favorites { get; init; }

	public int Likes { get; init; }

	public int Dislikes { get; init; }

	public List<Addon> Addons { get; init; }
}
