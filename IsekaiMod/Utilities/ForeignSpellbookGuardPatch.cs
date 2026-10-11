using System.Linq;
using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;

namespace IsekaiMod.Utilities;

[HarmonyPatch]
internal static class ForeignSpellbookGuardPatch
{
	[HarmonyPrepare]
	private static bool Prepare()
	{
		return PatchGuard.ResolvesAll(typeof(ForeignSpellbookGuardPatch), (typeof(AddSpecialSpellList), "OnActivate"), (typeof(AddOppositionSchool), "OnActivate"), (typeof(AddOppositionDescriptor), "OnActivate"), (typeof(SpontaneousSpellConversion), "OnTurnOn"), (typeof(SpontaneousSpellConversion), "OnTurnOff"), (typeof(ScrollSpecialization), "OnTurnOn"), (typeof(LearnSpellParametrized), "OnActivate"), (typeof(SpellKenningComponent), "OnTurnOn"));
	}

	internal static bool OwnerHasTheClass(UnitDescriptor owner, BlueprintCharacterClass characterClass)
	{
		if (owner == null || characterClass == null)
		{
			return true;
		}
		try
		{
			return owner.Progression?.GetClassData(characterClass) != null;
		}
		catch
		{
			return true;
		}
	}

	private static UnitDescriptor OwnerOf(object component)
	{
		if (component == null)
		{
			return null;
		}
		try
		{
			object value = Traverse.Create(component).Property("Owner").GetValue();
			if (value is UnitDescriptor result)
			{
				return result;
			}
			if (!(value is UnitEntityData { Descriptor: var descriptor }))
			{
				return null;
			}
			return descriptor;
		}
		catch
		{
			return null;
		}
	}

	private static bool ShouldRun(object component, BlueprintCharacterClass characterClass)
	{
		UnitDescriptor unitDescriptor = OwnerOf(component);
		if (unitDescriptor == null)
		{
			return true;
		}
		return OwnerHasTheClass(unitDescriptor, characterClass);
	}

	[HarmonyPatch(typeof(AddSpecialSpellList), "OnActivate")]
	[HarmonyPrefix]
	public static bool AddSpecialSpellList_OnActivate_Prefix(AddSpecialSpellList __instance)
	{
		return ShouldRun(__instance, __instance?.CharacterClass);
	}

	[HarmonyPatch(typeof(AddOppositionSchool), "OnActivate")]
	[HarmonyPrefix]
	public static bool AddOppositionSchool_OnActivate_Prefix(AddOppositionSchool __instance)
	{
		return ShouldRun(__instance, __instance?.CharacterClass);
	}

	[HarmonyPatch(typeof(AddOppositionDescriptor), "OnActivate")]
	[HarmonyPrefix]
	public static bool AddOppositionDescriptor_OnActivate_Prefix(AddOppositionDescriptor __instance)
	{
		return ShouldRun(__instance, __instance?.CharacterClass);
	}

	[HarmonyPatch(typeof(SpontaneousSpellConversion), "OnTurnOn")]
	[HarmonyPrefix]
	public static bool SpontaneousSpellConversion_OnTurnOn_Prefix(SpontaneousSpellConversion __instance)
	{
		return ShouldRun(__instance, __instance?.CharacterClass);
	}

	[HarmonyPatch(typeof(SpontaneousSpellConversion), "OnTurnOff")]
	[HarmonyPrefix]
	public static bool SpontaneousSpellConversion_OnTurnOff_Prefix(SpontaneousSpellConversion __instance)
	{
		return ShouldRun(__instance, __instance?.CharacterClass);
	}

	[HarmonyPatch(typeof(ScrollSpecialization), "OnTurnOn")]
	[HarmonyPrefix]
	public static bool ScrollSpecialization_OnTurnOn_Prefix(ScrollSpecialization __instance)
	{
		return ShouldRun(__instance, __instance?.SpecializedClass);
	}

	[HarmonyPatch(typeof(LearnSpellParametrized), "OnActivate")]
	[HarmonyPrefix]
	public static bool LearnSpellParametrized_OnActivate_Prefix(LearnSpellParametrized __instance)
	{
		return ShouldRun(__instance, __instance?.SpellcasterClass);
	}

	internal static bool OwnerHasTheSpellbook(UnitDescriptor owner, BlueprintSpellbook book)
	{
		if (owner == null || book == null)
		{
			return true;
		}
		try
		{
			return (owner.Progression?.Classes)?.Any((ClassData data) => data != null && (data.Spellbook == book || data.CharacterClass?.Spellbook == book)) ?? true;
		}
		catch
		{
			return true;
		}
	}

	[HarmonyPatch(typeof(SpellKenningComponent), "OnTurnOn")]
	[HarmonyPrefix]
	public static bool SpellKenningComponent_OnTurnOn_Prefix(SpellKenningComponent __instance)
	{
		BlueprintSpellbook blueprintSpellbook = __instance?.m_Spellbook?.Get();
		if (blueprintSpellbook == null)
		{
			return true;
		}
		UnitDescriptor unitDescriptor = OwnerOf(__instance);
		if (unitDescriptor == null)
		{
			return true;
		}
		return OwnerHasTheSpellbook(unitDescriptor, blueprintSpellbook);
	}
}
