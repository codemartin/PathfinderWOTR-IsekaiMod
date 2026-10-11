using System;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("3c131b759b4d4cb28cc25cb4132bbf3a")]
public class ActionEnterColiseum : IsekaiGameAction
{
	public const string DefendersHeartEnterGuid = "81b33df4ecc0f5e44abbb3b66e2e9943";

	public const string WarCampEnterGuid = "e5272ff75e9253d49b358be1af6a0415";

	public const string DrezenCapitalEnterGuid = "cdaba43118a82ff429b803ae846b35b0";

	public const string NexusEnterGuid = "5293f0ed1162da94aa3bff220308ddc1";

	public const string AlushinyrraLowerCityEnterGuid = "592b386cb48f10a4489669ae1d226db9";

	public const string AlushinyrraMediumCityEnterGuid = "b7297dd7e093fc94f91f7da8cbdbbef6";

	public const string AlushinyrraHigherCityEnterGuid = "80e4e8044096b654b9c4a6a3ed93c770";

	public const string ThresholdOutdoorAreaName = "ThresholdOutdoor";

	public const string ThresholdCampEnterGuid = "d43e44a6bb7a62e46907a2b8db461f97";

	internal static readonly string[] HubAreaNames = new string[6] { "DefendersHeart", "WarCamp", "DrezenCapital", "AlushinyrraLowerCity", "AlushinyrraMediumCity", "AlushinyrraHigherCity" };

	internal static readonly string[] AlushinyrraAreaNames = new string[3] { "AlushinyrraLowerCity", "AlushinyrraMediumCity", "AlushinyrraHigherCity" };

	public override string GetCaption()
	{
		return "Enter the Planar Coliseum arena of the current act";
	}

	internal static bool IsHubArea(string areaName)
	{
		if (!string.IsNullOrEmpty(areaName))
		{
			return HubAreaNames.Any((string hub) => string.Equals(hub, areaName, StringComparison.OrdinalIgnoreCase));
		}
		return false;
	}

	internal static bool IsAlushinyrraCity(string areaName)
	{
		if (!string.IsNullOrEmpty(areaName))
		{
			return AlushinyrraAreaNames.Any((string city) => string.Equals(city, areaName, StringComparison.OrdinalIgnoreCase));
		}
		return false;
	}

	internal static string ResolveReturnEnterPoint(string currentAreaName, int chapter)
	{
		currentAreaName = currentAreaName ?? string.Empty;
		if (string.Equals(currentAreaName, "DefendersHeart", StringComparison.OrdinalIgnoreCase))
		{
			return "81b33df4ecc0f5e44abbb3b66e2e9943";
		}
		if (string.Equals(currentAreaName, "WarCamp", StringComparison.OrdinalIgnoreCase))
		{
			return "e5272ff75e9253d49b358be1af6a0415";
		}
		if (string.Equals(currentAreaName, "DrezenCapital", StringComparison.OrdinalIgnoreCase))
		{
			return "cdaba43118a82ff429b803ae846b35b0";
		}
		if (string.Equals(currentAreaName, "AlushinyrraLowerCity", StringComparison.OrdinalIgnoreCase))
		{
			return "592b386cb48f10a4489669ae1d226db9";
		}
		if (string.Equals(currentAreaName, "AlushinyrraMediumCity", StringComparison.OrdinalIgnoreCase))
		{
			return "b7297dd7e093fc94f91f7da8cbdbbef6";
		}
		if (string.Equals(currentAreaName, "AlushinyrraHigherCity", StringComparison.OrdinalIgnoreCase))
		{
			return "80e4e8044096b654b9c4a6a3ed93c770";
		}
		if (string.Equals(currentAreaName, "ThresholdOutdoor", StringComparison.OrdinalIgnoreCase))
		{
			return "d43e44a6bb7a62e46907a2b8db461f97";
		}
		if (chapter <= 1)
		{
			return "81b33df4ecc0f5e44abbb3b66e2e9943";
		}
		if (chapter == 2)
		{
			return "e5272ff75e9253d49b358be1af6a0415";
		}
		if (chapter == 4)
		{
			return "5293f0ed1162da94aa3bff220308ddc1";
		}
		if (chapter >= 6)
		{
			return "d43e44a6bb7a62e46907a2b8db461f97";
		}
		return "cdaba43118a82ff429b803ae846b35b0";
	}

	internal static string CurrentReturnPoint(string currentAreaName, int chapter)
	{
		if (IsAlushinyrraCity(currentAreaName))
		{
			BlueprintAreaEnterPoint blueprintAreaEnterPoint = PlanarColiseumTravel.FindNearestEnterPoint();
			if (blueprintAreaEnterPoint != null)
			{
				return blueprintAreaEnterPoint.AssetGuid.ToString();
			}
		}
		return ResolveReturnEnterPoint(currentAreaName, chapter);
	}

	protected override void RunSafely()
	{
		try
		{
			if (!PlanarColiseumHubManager.IsInColiseum() && !ColiseumGates.RefuseTravel("The rift to the Planar Coliseum"))
			{
				BlueprintAreaEnterPoint blueprintAreaEnterPoint = PlanarColiseumArea.CurrentArenaEnterPoint();
				if (blueprintAreaEnterPoint == null)
				{
					Main.IsekaiContext.Logger.LogError("[PlanarColiseum] No arena was created; nowhere to travel.");
					PlanarColiseumHubManager.PostLog("The rift to the Planar Coliseum will not open: no arena is available right now.");
					return;
				}
				string currentAreaName = Game.Instance?.CurrentlyLoadedArea?.name ?? "";
				int chapter = Game.Instance?.Player?.Chapter ?? 1;
				PlanarColiseumTravel.RecordReturnPoint(CurrentReturnPoint(currentAreaName, chapter));
				PlanarColiseumHubManager.ScheduleTravel(blueprintAreaEnterPoint, PlanarColiseumHubManager.PrepareArrival);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error transitioning into Planar Coliseum: " + ex);
		}
	}
}
