using IsekaiMod.Content.GlobalMap;

namespace IsekaiMod.Content.Arenas;

internal sealed class Delivery
{
	internal DeliveryRoute Route;

	internal BossSiteMapPoints.MapId Map;

	internal string AnchorGuid;

	internal string TownAreaName;

	public override string ToString()
	{
		if (Route != DeliveryRoute.MapTile)
		{
			return "the Otherworldly Emissary in " + TownAreaName;
		}
		return "tile on the " + Map.ToString() + " map beside " + AnchorGuid;
	}
}
