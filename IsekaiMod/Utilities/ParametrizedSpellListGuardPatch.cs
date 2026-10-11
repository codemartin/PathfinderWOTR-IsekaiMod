using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(BlueprintParametrizedFeature), "ExtractSelectionItems")]
internal static class ParametrizedSpellListGuardPatch
{
	internal static bool DropUnit(bool learnSpell, bool hasSpellcasterClass, bool unitHasClass)
	{
		if (learnSpell & hasSpellcasterClass)
		{
			return !unitHasClass;
		}
		return false;
	}

	internal static IEnumerable<IFeatureSelectionItem> EmptyList()
	{
		return Enumerable.Empty<IFeatureSelectionItem>();
	}

	[HarmonyPrefix]
	private static bool Prefix(BlueprintParametrizedFeature __instance, UnitDescriptor beforeLevelUpUnit, ref IEnumerable<IFeatureSelectionItem> __result)
	{
		try
		{
			if (__instance == null || beforeLevelUpUnit?.Progression == null)
			{
				return true;
			}
			BlueprintCharacterClass spellcasterClass = __instance.SpellcasterClass;
			bool unitHasClass = spellcasterClass != null && beforeLevelUpUnit.Progression.GetClassData(spellcasterClass) != null;
			if (DropUnit(__instance.ParameterType == FeatureParameterType.LearnSpell, spellcasterClass != null, unitHasClass))
			{
				__result = EmptyList();
				return false;
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[ParametrizedSpellListGuardPatch] could not guard {__instance?.name}; the list is built as the game builds it. {arg}");
		}
		return true;
	}
}
