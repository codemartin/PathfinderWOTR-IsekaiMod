using System;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.Kineticist;

namespace IsekaiMod.Utilities;

[HarmonyPatch]
internal static class KineticistClassLevelPatch
{
	[HarmonyPrepare]
	private static bool Prepare()
	{
		return PatchGuard.Resolves(typeof(KineticistClassLevelPatch), typeof(UnitPartKineticist), "get_ClassLevel");
	}

	internal static int ClassLevel(int kineticistLevels, int isekaiLevels)
	{
		if (kineticistLevels <= 0)
		{
			return Math.Max(0, isekaiLevels);
		}
		return kineticistLevels;
	}

	[HarmonyPatch(typeof(UnitPartKineticist), "ClassLevel", MethodType.Getter)]
	[HarmonyPostfix]
	private static void ClassLevel_Postfix(UnitPartKineticist __instance, ref int __result)
	{
		try
		{
			if (__result > 0)
			{
				return;
			}
			UnitProgressionData unitProgressionData = __instance?.Owner?.Progression;
			if (unitProgressionData == null)
			{
				return;
			}
			int num = 0;
			BlueprintCharacterClassReference[] array = IsekaiAscendantClass.ClassLevelScalingClasses();
			for (int i = 0; i < array.Length; i++)
			{
				BlueprintCharacterClass blueprintCharacterClass = array[i]?.Get();
				if (blueprintCharacterClass != null)
				{
					num += unitProgressionData.GetClassLevel(blueprintCharacterClass);
				}
			}
			__result = ClassLevel(__result, num);
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[KineticistClassLevelPatch] could not read the Isekai levels; the Kineticist level stands. {arg}");
		}
	}
}
