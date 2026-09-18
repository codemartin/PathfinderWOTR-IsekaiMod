using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Root;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist
{
	internal static class LevelUncapperPatch
	{
		[HarmonyPatch(typeof(UnitProgressionData), "get_MaxCharacterLevel")]
		private static class UnitProgressionData_MaxCharacterLevel_Patch
		{
			public static void Postfix(ref int __result)
			{
				if (Main.IsekaiContext.AddedContent.EnableLevelUncapping && __result < 40)
				{
					__result = 40;
				}
			}
		}

		[HarmonyPatch(typeof(UnitProgressionData), "get_ExperienceTable")]
		private static class UnitProgressionData_ExperienceTable_Patch
		{
			public static void Postfix(UnitProgressionData __instance, ref BlueprintStatProgression __result)
			{
				if (Main.IsekaiContext.AddedContent.EnableLevelUncapping)
				{
					BlueprintStatProgression blueprintStatProgression = BlueprintRoot.Instance?.Progression?.LegendXPTable;
					if (blueprintStatProgression != null)
					{
						__result = blueprintStatProgression;
					}
				}
			}
		}

		[HarmonyPatch(typeof(BlueprintCharacterClass), "MeetsPrerequisites")]
		private static class BlueprintCharacterClass_MeetsPrerequisites_Patch
		{
			public static void Postfix(BlueprintCharacterClass __instance, UnitDescriptor unit, LevelUpState state, bool ignoreAlignment, ref bool __result)
			{
				if (__result || !Main.IsekaiContext.AddedContent.EnableLevelUncapping || __instance == null || unit == null || unit.Progression == null || __instance.PrestigeClass || __instance != IsekaiProtagonistClass.Get())
				{
					return;
				}
				int classLevel = unit.Progression.GetClassLevel(__instance);
				int num = 40;
				if (classLevel < 20 || classLevel >= num)
				{
					return;
				}
				bool flag = true;
				bool flag2 = false;
				bool flag3 = false;
				BlueprintComponent[] componentsArray = __instance.ComponentsArray;
				if (componentsArray != null)
				{
					for (int i = 0; i < componentsArray.Length; i++)
					{
						if (!(componentsArray[i] is Prerequisite prerequisite) || ((prerequisite is PrerequisiteAlignment) & ignoreAlignment))
						{
							continue;
						}
						bool flag4 = false;
						try
						{
							flag4 = prerequisite.Check(null, unit, state);
						}
						catch
						{
							flag4 = false;
						}
						if (prerequisite.Group == Prerequisite.GroupType.All)
						{
							if (!flag4)
							{
								flag = false;
							}
						}
						else if (prerequisite.Group == Prerequisite.GroupType.Any)
						{
							flag2 = true;
							if (flag4)
							{
								flag3 = true;
							}
						}
					}
				}
				if (flag && (!flag2 | flag3))
				{
					__result = true;
				}
			}
		}
	}
}
