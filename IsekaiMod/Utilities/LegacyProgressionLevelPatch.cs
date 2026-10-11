using System;
using System.Collections.Generic;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(BlueprintProgression), "CalcLevel")]
internal static class LegacyProgressionLevelPatch
{
	internal static int Level(int calculated, int isekaiContribution, bool hasRealClassLevels)
	{
		if (!hasRealClassLevels || isekaiContribution <= 0)
		{
			return calculated;
		}
		return Math.Max(0, calculated - isekaiContribution);
	}

	[HarmonyPostfix]
	private static void Postfix(BlueprintProgression __instance, UnitDescriptor unit, ref int __result)
	{
		try
		{
			if (__result <= 0 || unit?.Progression == null || !LegacyProgressionBackfill.IsRegistered(__instance))
			{
				return;
			}
			BlueprintProgression.ClassWithLevel[] classes = __instance.m_Classes;
			if (classes == null || classes.Length == 0)
			{
				return;
			}
			HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>();
			BlueprintCharacterClassReference[] array = IsekaiAscendantClass.ClassLevelScalingClasses();
			foreach (BlueprintCharacterClassReference blueprintCharacterClassReference in array)
			{
				if (blueprintCharacterClassReference != null)
				{
					hashSet.Add(blueprintCharacterClassReference.Guid);
				}
			}
			int num = 0;
			bool hasRealClassLevels = false;
			BlueprintProgression.ClassWithLevel[] array2 = classes;
			foreach (BlueprintProgression.ClassWithLevel classWithLevel in array2)
			{
				BlueprintCharacterClass blueprintCharacterClass = classWithLevel?.Class;
				if (blueprintCharacterClass != null)
				{
					int classLevel = unit.Progression.GetClassLevel(blueprintCharacterClass);
					if (hashSet.Contains(blueprintCharacterClass.AssetGuid))
					{
						num += Math.Max(0, classLevel + classWithLevel.AdditionalLevel);
					}
					else if (classLevel > 0)
					{
						hasRealClassLevels = true;
					}
				}
			}
			__result = Level(__result, num, hasRealClassLevels);
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[LegacyProgressionLevelPatch] could not separate the Isekai levels on {__instance?.name}; the game's own count stands. {arg}");
		}
	}
}
