using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

internal static class CompanionDollRace
{
	internal const string SkeletonRaceGuid = "4774ee5a5711433c8c8e008dd3734e56";

	internal const string CambionRaceGuid = "980bed7f6d9e4c33a86b95d199eb4934";

	internal static void EnsureRace(BlueprintUnit unit, string raceGuid)
	{
		if (unit != null && unit.Race == null)
		{
			BlueprintRace blueprint = BlueprintTools.GetBlueprint<BlueprintRace>(raceGuid);
			if (blueprint != null)
			{
				unit.m_Race = blueprint.ToReference<BlueprintRaceReference>();
			}
		}
	}
}
