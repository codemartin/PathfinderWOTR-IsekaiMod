using HarmonyLib;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using System.Runtime.CompilerServices;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// Throttles the vanilla <see cref="MaxDexBonusIncrease"/> component so it only recalculates armor
    /// when an ability score has really changed.
    ///
    /// WHY THIS PATCH EXISTS
    /// ---------------------
    /// MaxDexBonusIncrease (Armor Training, Armor Saint, the mythic armor feats and several others) subscribes
    /// to <c>ModifiableValue.OnChanged</c> on all six ability scores. Every time any of those events fires it
    /// calls <c>ItemEntityArmor.RecalculateStats()</c> on the worn armor and shield. Vanilla itself fires the
    /// event a handful of times per attack, which is harmless.
    ///
    /// Other mods are not so quiet. CodexLib's <c>IncreaseModifierBonus</c> turns its <c>AddContextStatBonus</c>
    /// components off and on again for every fact the unit gains or loses, and AttributeFeats' Crown of Will
    /// does something similar. A single swing that stacks a few "on hit" buffs (Kraken Wrath, Driving Assault,
    /// Zippy Magic, Arcane Movement and so on) therefore produces hundreds of OnChanged events on the same
    /// unit, and this component dutifully recalculated the armor for each of them: one profiled swing showed
    /// 338 armor recalculations and 0.6 to 0.8 seconds of lag. In almost all of those events the modified value
    /// of the stat ended exactly where it started, because the modifier was removed and immediately re-added.
    ///
    /// WHAT THE PATCH DOES
    /// -------------------
    /// A Harmony prefix on the private, parameterless <c>StatChangeHandler()</c> (the one both the OnChanged
    /// overload and OnTurnOn call) remembers, per component instance, the last set of inputs it recalculated
    /// for: the modified value of the six ability scores plus the identity of the worn armor and shield items.
    /// When the handler fires again with the same inputs it returns false, which skips the original method and
    /// so skips the recalculation. Anything else runs the original untouched.
    ///
    /// WHAT IS DELIBERATELY NOT CHANGED
    /// --------------------------------
    /// - The first call for a component always runs (nothing is cached yet), so OnTurnOn behaves as before.
    /// - The cache is dropped in OnTurnOff, so re-applying the fact (level up, respec, buff refresh) starts
    ///   clean. Changes to the component's own Bonus or ContextBonus arrive through that path.
    /// - Armor and shield identity is part of the key, so swapping equipment still triggers a recalculation
    ///   on the next stat event. Equipping itself recalculates the item through the game's own equip code
    ///   anyway, which this patch does not touch.
    /// - The patch never calls RecalculateStats itself and never alters the values it reads. If the game
    ///   changes the handler's signature the patch simply fails to apply and Harmony logs it; nothing else in
    ///   the mod depends on it.
    /// - Stats are compared by <c>ModifiedValue</c>, which is the number the armor's max Dex bonus and armor
    ///   check penalty are ultimately derived from. A change that only touches <c>PermanentValue</c> while
    ///   leaving the modified value the same has no effect on those two numbers, so skipping it is safe.
    ///
    /// If this ever needs to be disabled for a test, delete or comment out the two <c>[HarmonyPatch]</c>
    /// attributes below; nothing else references this class.
    /// </summary>
    internal static class ArmorRecalculationThrottle {
        private sealed class LastInputs {
            public int Str, Dex, Con, Int, Wis, Cha;
            public ItemEntity Armor;
            public ItemEntity Shield;
        }

        // One record per live component instance. ConditionalWeakTable does not keep the component alive and
        // drops the record with it, so a unit that is unloaded leaves nothing behind.
        private static readonly ConditionalWeakTable<MaxDexBonusIncrease, LastInputs> cache = new ConditionalWeakTable<MaxDexBonusIncrease, LastInputs>();

        private static readonly AccessTools.FieldRef<MaxDexBonusIncrease, UnitDescriptor> ownerRef =
            AccessTools.FieldRefAccess<MaxDexBonusIncrease, UnitDescriptor>("m_OwnerDescriptor");

        [HarmonyPatch(typeof(MaxDexBonusIncrease), "StatChangeHandler", new System.Type[0])]
        internal static class StatChangeHandlerPatch {
            [HarmonyPrefix]
            private static bool Prefix(MaxDexBonusIncrease __instance) {
                UnitDescriptor owner = __instance != null ? ownerRef(__instance) : null;
                // No owner means the component is being torn down or was never turned on; let vanilla decide.
                if (owner?.Stats == null || owner.Body == null) return true;

                CharacterStats stats = owner.Stats;
                ItemEntity armor = owner.Body.Armor?.MaybeArmor;
                ItemEntity shield = owner.Body.SecondaryHand?.MaybeShield;

                if (cache.TryGetValue(__instance, out LastInputs last)
                    && last.Str == stats.Strength.ModifiedValue
                    && last.Dex == stats.Dexterity.ModifiedValue
                    && last.Con == stats.Constitution.ModifiedValue
                    && last.Int == stats.Intelligence.ModifiedValue
                    && last.Wis == stats.Wisdom.ModifiedValue
                    && last.Cha == stats.Charisma.ModifiedValue
                    && ReferenceEquals(last.Armor, armor)
                    && ReferenceEquals(last.Shield, shield)) {
                    // Nothing the armor depends on has moved since the last recalculation. Skip it.
                    return false;
                }

                if (last == null) {
                    last = new LastInputs();
                    cache.Add(__instance, last);
                }
                last.Str = stats.Strength.ModifiedValue;
                last.Dex = stats.Dexterity.ModifiedValue;
                last.Con = stats.Constitution.ModifiedValue;
                last.Int = stats.Intelligence.ModifiedValue;
                last.Wis = stats.Wisdom.ModifiedValue;
                last.Cha = stats.Charisma.ModifiedValue;
                last.Armor = armor;
                last.Shield = shield;
                return true;
            }
        }

        [HarmonyPatch(typeof(MaxDexBonusIncrease), "OnTurnOff")]
        internal static class OnTurnOffPatch {
            // Runs before vanilla unsubscribes, so a later OnTurnOn starts with an empty record and recalculates.
            [HarmonyPrefix]
            private static void Prefix(MaxDexBonusIncrease __instance) {
                if (__instance != null) cache.Remove(__instance);
            }
        }
    }
}
