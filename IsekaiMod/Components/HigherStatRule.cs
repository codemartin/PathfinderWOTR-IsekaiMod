using Kingmaker.EntitySystem.Stats;

namespace IsekaiMod.Components;

internal static class HigherStatRule
{
	internal static StatType Pick(StatType current, int currentBonus, StatType replacement, int replacementBonus)
	{
		if (replacementBonus <= currentBonus)
		{
			return current;
		}
		return replacement;
	}
}
