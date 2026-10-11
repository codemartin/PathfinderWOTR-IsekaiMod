using System;
using HarmonyLib;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(Polymorph), "OnDeactivate")]
internal static class PolymorphNeverAppliedGuard
{
	internal static bool NeverApplied(int appliedModifiers, int appliedFacts, bool restorePortrait, bool hasViewReplacement)
	{
		if (appliedModifiers == 0 && appliedFacts == 0 && !restorePortrait)
		{
			return !hasViewReplacement;
		}
		return false;
	}

	[HarmonyPrefix]
	private static bool Prefix(Polymorph __instance)
	{
		try
		{
			PolymorphData polymorphData = ((EntityFactComponentDelegate<UnitEntityData, PolymorphData>)__instance)?.Data;
			if (polymorphData == null)
			{
				return true;
			}
			return !NeverApplied(polymorphData.AppliedModifiers.Count, polymorphData.AppliedFacts.Count, polymorphData.RestorePortrait, polymorphData.ViewReplacement != null);
		}
		catch (Exception)
		{
			return true;
		}
	}
}
