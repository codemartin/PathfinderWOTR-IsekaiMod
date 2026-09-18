using HarmonyLib;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using System.Collections.Generic;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Guards two game components that throw when a blueprint leaves a field unset.
    /// Both exceptions are swallowed by the game, so the only visible effect was a
    /// bonus that never applied and a little time lost on every attack roll.
    /// The owning blueprint is logged once so the source can be fixed properly.
    /// </summary>
    [HarmonyPatch(typeof(CriticalConfirmationBonus), nameof(CriticalConfirmationBonus.OnEventAboutToTrigger), typeof(RuleAttackRoll))]
    internal static class CriticalConfirmationBonusValueSafety {
        private static readonly HashSet<string> reported = new HashSet<string>();

        [HarmonyPrefix]
        private static void Prefix(CriticalConfirmationBonus __instance) {
            if (__instance == null || __instance.Value != null) return;
            // Value is read before Bonus is added, so a missing one throws and drops the whole bonus.
            __instance.Value = 0;
            string owner = __instance.OwnerBlueprint?.name ?? "(unknown blueprint)";
            if (reported.Add(owner)) {
                IsekaiContext.Logger.Log($"CriticalConfirmationBonus on {owner} had no Value; using 0 so its flat bonus of {__instance.Bonus} applies.");
            }
        }
    }

    [HarmonyPatch(typeof(DerivativeStatBonus), "OnTurnOff")]
    internal static class DerivativeStatBonusTurnOffSafety {
        private static readonly HashSet<string> reported = new HashSet<string>();

        [HarmonyPrefix]
        private static bool Prefix(DerivativeStatBonus __instance) {
            if (__instance?.Owner?.Stats == null) return true;
            if (__instance.Owner.Stats.GetStat(__instance.DerivativeStat) != null) return true;
            // The unit has no such stat, so there is nothing to remove; the original would throw here.
            string owner = __instance.OwnerBlueprint?.name ?? "(unknown blueprint)";
            if (reported.Add(owner)) {
                IsekaiContext.Logger.Log($"DerivativeStatBonus on {owner} points at {__instance.DerivativeStat}, which {__instance.Owner.CharacterName} does not have; skipped removal.");
            }
            return false;
        }
    }

    /// <summary>
    /// The mod's usable items are created in code without a belt prefab or an equipment entity, and the
    /// two getters dereference those links without a null check. The exceptions are swallowed by the game
    /// but fire on every character model refresh while such an item is in a belt or body slot.
    /// </summary>
    [HarmonyPatch(typeof(Kingmaker.Blueprints.Items.Equipment.BlueprintItemEquipmentUsable), "BeltItemPrefab", MethodType.Getter)]
    internal static class UsableItemBeltPrefabSafety {
        private static readonly AccessTools.FieldRef<Kingmaker.Blueprints.Items.Equipment.BlueprintItemEquipmentUsable, Kingmaker.ResourceLinks.PrefabLink> prefabRef =
            AccessTools.FieldRefAccess<Kingmaker.Blueprints.Items.Equipment.BlueprintItemEquipmentUsable, Kingmaker.ResourceLinks.PrefabLink>("m_BeltItemPrefab");

        [HarmonyPrefix]
        private static bool Prefix(Kingmaker.Blueprints.Items.Equipment.BlueprintItemEquipmentUsable __instance, ref UnityEngine.GameObject __result) {
            if (__instance != null && prefabRef(__instance) != null) return true;
            __result = null;
            return false;
        }
    }

    [HarmonyPatch(typeof(Kingmaker.Blueprints.Items.Equipment.BlueprintItemEquipment), "EquipmentEntity", MethodType.Getter)]
    internal static class EquipmentEntitySafety {
        private static readonly AccessTools.FieldRef<Kingmaker.Blueprints.Items.Equipment.BlueprintItemEquipment, Kingmaker.Blueprints.KingmakerEquipmentEntityReference> entityRef =
            AccessTools.FieldRefAccess<Kingmaker.Blueprints.Items.Equipment.BlueprintItemEquipment, Kingmaker.Blueprints.KingmakerEquipmentEntityReference>("m_EquipmentEntity");

        [HarmonyPrefix]
        private static bool Prefix(Kingmaker.Blueprints.Items.Equipment.BlueprintItemEquipment __instance, ref Kingmaker.Visual.CharacterSystem.KingmakerEquipmentEntity __result) {
            if (__instance != null && entityRef(__instance) != null) return true;
            __result = null;
            return false;
        }
    }
}
