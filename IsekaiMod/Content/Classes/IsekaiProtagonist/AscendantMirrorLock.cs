using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

[HarmonyPatch]
internal static class AscendantMirrorLock
{
	internal static LocalizedString MirrorRequiredText;

	internal static void CreateStrings()
	{
		MirrorRequiredText = Helpers.CreateString(Main.IsekaiContext, "IsekaiAscendantArchetypes.MirrorRequired", "An Isekai Protagonist with an archetype continues it on the Isekai Ascendant: select that archetype's Ascendant path under Isekai Ascendant.");
	}

	internal static bool Refuses(bool ascendantSelected, int nextAscendantLevel, bool mirrorRequired, bool mirrorPresent)
	{
		if ((ascendantSelected && nextAscendantLevel == 1) & mirrorRequired)
		{
			return !mirrorPresent;
		}
		return false;
	}

	internal static bool ShouldAddMirror(bool ascendantSelected, int nextAscendantLevel, bool mirrorRequired, bool anyAscendantArchetype)
	{
		if ((ascendantSelected && nextAscendantLevel == 1) & mirrorRequired)
		{
			return !anyAscendantArchetype;
		}
		return false;
	}

	internal static BlueprintArchetype MirrorOf(BlueprintCharacterClass ascendant, IEnumerable<BlueprintArchetype> isekaiArchetypes)
	{
		if (ascendant == null || isekaiArchetypes == null)
		{
			return null;
		}
		HashSet<BlueprintArchetype> hashSet = new HashSet<BlueprintArchetype>(isekaiArchetypes.Where((BlueprintArchetype a) => a != null));
		if (hashSet.Count == 0)
		{
			return null;
		}
		foreach (BlueprintArchetype archetype in ascendant.Archetypes)
		{
			if (archetype == null)
			{
				continue;
			}
			foreach (PrerequisiteArchetypeLevel item in (archetype.ComponentsArray ?? new BlueprintComponent[0]).OfType<PrerequisiteArchetypeLevel>())
			{
				if (item?.Archetype != null && hashSet.Contains(item.Archetype))
				{
					return archetype;
				}
			}
		}
		return null;
	}

	internal static BlueprintArchetype RequiredMirror(UnitDescriptor unit)
	{
		if (unit?.Progression == null)
		{
			return null;
		}
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		BlueprintCharacterClass blueprintCharacterClass2 = IsekaiAscendantClass.Get();
		if (blueprintCharacterClass == null || blueprintCharacterClass2 == null)
		{
			return null;
		}
		ClassData classData = unit.Progression.GetClassData(blueprintCharacterClass);
		if (classData == null)
		{
			return null;
		}
		return MirrorOf(blueprintCharacterClass2, classData.Archetypes);
	}

	internal static bool IsMissingMirror(LevelUpState state, UnitDescriptor unit, out BlueprintArchetype mirror)
	{
		mirror = null;
		if (state == null || unit == null)
		{
			return false;
		}
		BlueprintCharacterClass blueprintCharacterClass = IsekaiAscendantClass.Get();
		if (blueprintCharacterClass == null || state.SelectedClass != blueprintCharacterClass)
		{
			return false;
		}
		mirror = RequiredMirror(unit);
		bool mirrorPresent = mirror != null && (unit.Progression.GetClassData(blueprintCharacterClass)?.Archetypes.Contains(mirror) ?? false);
		return Refuses(ascendantSelected: true, state.NextClassLevel, mirror != null, mirrorPresent);
	}

	[HarmonyPatch(typeof(LevelUpController), "SelectClass")]
	[HarmonyPostfix]
	private static void SelectClass_Postfix(LevelUpController __instance, bool __result)
	{
		try
		{
			if (!__result || __instance?.State == null)
			{
				return;
			}
			BlueprintCharacterClass ascendant = IsekaiAscendantClass.Get();
			if (ascendant == null)
			{
				return;
			}
			UnitDescriptor unitDescriptor = (__instance.Preview ?? __instance.Unit)?.Descriptor;
			if (unitDescriptor == null)
			{
				return;
			}
			if (__instance.State.SelectedClass != ascendant)
			{
				foreach (BlueprintArchetype item in (from a in __instance.LevelUpActions.OfType<AddArchetype>()
					where a.CharacterClass == ascendant
					select a.Archetype).ToList())
				{
					__instance.RemoveArchetype(item);
				}
				return;
			}
			BlueprintArchetype blueprintArchetype = RequiredMirror(unitDescriptor);
			ClassData classData = unitDescriptor.Progression.GetClassData(ascendant);
			bool anyAscendantArchetype = (classData != null && classData.Archetypes.Any()) || __instance.LevelUpActions.OfType<AddArchetype>().Any((AddArchetype a) => a.CharacterClass == ascendant);
			if (ShouldAddMirror(ascendantSelected: true, __instance.State.NextClassLevel, blueprintArchetype != null, anyAscendantArchetype) && !__instance.AddArchetype(ascendant, blueprintArchetype))
			{
				Main.IsekaiContext.Logger.LogWarning("[AscendantMirrorLock] The Isekai Ascendant was taken but its required archetype " + blueprintArchetype.name + " could not be added; the level stays incomplete until it is chosen on the class page.");
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[AscendantMirrorLock] could not add the Isekai Ascendant archetype. {arg}");
		}
	}

	[HarmonyPatch(typeof(LevelUpState), "IsComplete")]
	[HarmonyPostfix]
	private static void IsComplete_Postfix(LevelUpState __instance, ref bool __result)
	{
		try
		{
			if (__result && IsMissingMirror(__instance, __instance.m_Unit?.Descriptor, out var _))
			{
				__result = false;
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[AscendantMirrorLock] could not judge the Isekai Ascendant archetype; the level is left as the engine judged it. {arg}");
		}
	}
}
