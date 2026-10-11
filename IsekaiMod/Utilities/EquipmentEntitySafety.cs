using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Visual.CharacterSystem;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(BlueprintItemEquipment), "EquipmentEntity", MethodType.Getter)]
internal static class EquipmentEntitySafety
{
	private static readonly AccessTools.FieldRef<BlueprintItemEquipment, KingmakerEquipmentEntityReference> entityRef = AccessTools.FieldRefAccess<BlueprintItemEquipment, KingmakerEquipmentEntityReference>("m_EquipmentEntity");

	[HarmonyPrefix]
	private static bool Prefix(BlueprintItemEquipment __instance, ref KingmakerEquipmentEntity __result)
	{
		if (__instance != null && entityRef(__instance) != null)
		{
			return true;
		}
		__result = null;
		return false;
	}
}
