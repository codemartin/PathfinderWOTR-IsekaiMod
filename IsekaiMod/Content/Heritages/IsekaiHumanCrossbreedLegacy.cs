using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using TabletopTweaks.Core.Utilities;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Heritages {
    internal class IsekaiHumanCrossbreedLegacy {
        private static BlueprintFeature ourHeritage;

        public static void Add() {
            ourHeritage = Helpers.CreateBlueprint<BlueprintFeature>(IsekaiContext, "IsekaiHumanCrossbreedLegacy", bp => {
                bp.SetName(IsekaiContext, "Isekai Crossbreed Human");
                bp.SetDescription(IsekaiContext,
                    "Let us be honest, there is a reason why for any mixed race classification it is always enough to name the non human options. \n" +
                    "Because generally it is simply save to assume that the other part of it will be human. \n" +
                    "There is no other race, except for dragons, that is as flexible when it comes to its choice of bed partners and none as flexible when it comes to being fertile when crossbreeding and realistically by now there are no true pure humans left because everyone is at least (1/2)^100 something else. \n" +
                    "And no matter how close to 0 that number is it is not 0 and for some reason in you all that blood became active in a way that allows you access to some of your ancestors powers.");


                bp.Groups = new FeatureGroup[0];
                // This marker has no fact components to rebuild. Reapplying it
                // during every level-up refresh only churns an empty feature.
                bp.ReapplyOnLevelUp = false;
            });

            HumanHeritageSelection.Register(ourHeritage);
        }

        /// <summary>
        /// True when the unit has the Crossbreed heritage and so counts as every race for prerequisite purposes.
        /// Null heritage means Isekai Heritages are disabled, in which case nothing changes.
        /// </summary>
        private static bool CountsAsAnyRace(UnitDescriptor unit) {
            return ourHeritage != null && unit != null && unit.HasFact(ourHeritage);
        }

        private static bool IsRace(BlueprintFeature feature) {
            return feature is BlueprintRace;
        }

        /// <summary>
        /// Race gates are expressed as a PrerequisiteFeature on the race blueprint. Treat them as met for Crossbreed.
        /// Only the prerequisite result changes; the feat's other prerequisites still apply.
        /// </summary>
        [HarmonyPatch(typeof(PrerequisiteFeature), nameof(PrerequisiteFeature.CheckInternal))]
        private static class PrerequisiteFeaturePatcher {
            [HarmonyPostfix]
            private static void Postfix(PrerequisiteFeature __instance, UnitDescriptor unit, ref bool __result) {
                if (__result) return;
                if (IsRace(__instance.Feature) && CountsAsAnyRace(unit)) {
                    __result = true;
                }
            }
        }

        /// <summary>
        /// "One of these races" gates use PrerequisiteFeaturesFromList. Count every race entry as owned for Crossbreed
        /// and re-evaluate against the required amount, so mixed lists keep their non-race requirements.
        /// </summary>
        [HarmonyPatch(typeof(PrerequisiteFeaturesFromList), nameof(PrerequisiteFeaturesFromList.CheckInternal))]
        private static class PrerequisiteFeaturesFromListPatcher {
            [HarmonyPostfix]
            private static void Postfix(PrerequisiteFeaturesFromList __instance, UnitDescriptor unit, ref bool __result) {
                if (__result) return;
                if (__instance.m_Features == null) return;

                bool hasRace = false;
                foreach (var reference in __instance.m_Features) {
                    if (IsRace(reference?.Get())) {
                        hasRace = true;
                        break;
                    }
                }

                // Most failed feature-list checks are unrelated to race. Do not
                // query the Crossbreed marker or allocate a resolved array for them.
                if (!hasRace || !CountsAsAnyRace(unit)) return;

                int owned = 0;
                foreach (var reference in __instance.m_Features) {
                    var feature = reference?.Get();
                    if (feature != null && (IsRace(feature) || unit.HasFact(feature))) {
                        owned++;
                    }
                }
                if (owned >= __instance.Amount) {
                    __result = true;
                }
            }
        }
    }
}
