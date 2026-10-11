using System;

namespace IsekaiMod.Components;

internal static class TwinAppearance
{
	internal static bool ShouldCopyRace(bool masterHasDoll, string petRaceId, string masterRaceId)
	{
		if (!masterHasDoll || string.IsNullOrEmpty(masterRaceId))
		{
			return false;
		}
		return !string.Equals(petRaceId, masterRaceId, StringComparison.OrdinalIgnoreCase);
	}

	internal static bool SamePortrait(string petBlueprintId, string petCustomId, string masterBlueprintId, string masterCustomId)
	{
		if (!string.IsNullOrEmpty(masterCustomId))
		{
			if (string.Equals(petCustomId, masterCustomId, StringComparison.Ordinal))
			{
				return string.IsNullOrEmpty(petBlueprintId);
			}
			return false;
		}
		if (string.IsNullOrEmpty(masterBlueprintId))
		{
			return true;
		}
		if (string.Equals(petBlueprintId, masterBlueprintId, StringComparison.OrdinalIgnoreCase))
		{
			return string.IsNullOrEmpty(petCustomId);
		}
		return false;
	}
}
