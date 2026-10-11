using System;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Constellations;

internal static class CosmicWishCharges
{
	[HarmonyPatch(typeof(UnitAbilityResourceCollection), "Restore", new Type[]
	{
		typeof(BlueprintScriptableObject),
		typeof(int),
		typeof(bool)
	})]
	internal static class UnitAbilityResourceCollection_Restore_Patch
	{
		[HarmonyPrefix]
		internal static bool Prefix(BlueprintScriptableObject blueprint, bool restoreFull)
		{
			return !BlocksRestore(blueprint, restoreFull);
		}
	}

	[HarmonyPatch(typeof(UnitAbilityResourceCollection), "Add", new Type[]
	{
		typeof(BlueprintScriptableObject),
		typeof(bool)
	})]
	internal static class UnitAbilityResourceCollection_Add_Patch
	{
		[HarmonyPrefix]
		internal static void Prefix(BlueprintScriptableObject blueprint, ref bool restoreAmount)
		{
			restoreAmount = AllowsRestoreOnAdd(blueprint, restoreAmount);
		}
	}

	internal static BlueprintScriptableObject WishResource;

	internal static bool IsWishResource(BlueprintScriptableObject blueprint)
	{
		if (blueprint != null && WishResource != null)
		{
			return blueprint == WishResource;
		}
		return false;
	}

	internal static bool BlocksRestore(BlueprintScriptableObject blueprint, bool restoreFull)
	{
		if (restoreFull)
		{
			return IsWishResource(blueprint);
		}
		return false;
	}

	internal static bool AllowsRestoreOnAdd(BlueprintScriptableObject blueprint, bool restoreAmount)
	{
		if (restoreAmount)
		{
			return !IsWishResource(blueprint);
		}
		return false;
	}
}
