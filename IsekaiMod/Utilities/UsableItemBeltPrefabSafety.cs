using HarmonyLib;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.ResourceLinks;
using UnityEngine;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(BlueprintItemEquipmentUsable), "BeltItemPrefab", MethodType.Getter)]
internal static class UsableItemBeltPrefabSafety
{
	private static readonly AccessTools.FieldRef<BlueprintItemEquipmentUsable, PrefabLink> prefabRef = AccessTools.FieldRefAccess<BlueprintItemEquipmentUsable, PrefabLink>("m_BeltItemPrefab");

	[HarmonyPrefix]
	private static bool Prefix(BlueprintItemEquipmentUsable __instance, ref GameObject __result)
	{
		if (__instance != null && prefabRef(__instance) != null)
		{
			return true;
		}
		__result = null;
		return false;
	}
}
