using SteamWorkshopStats.Models;

namespace SteamWorkshopStats.Tests.Models;

public class AddonTest
{
	[Theory]
	[InlineData(0, 0.0, 0)]
	[InlineData(25, 0.1, 1)]
	[InlineData(25, 0.3, 2)]
	[InlineData(25, 0.5, 3)]
	[InlineData(25, 0.7, 4)]
	[InlineData(25, 0.9, 5)]
	public void GetStars_ReturnsValid(int votes, float score, int expected)
	{
		Assert.Equal(expected, Addon.GetStars(votes, score));
	}
}