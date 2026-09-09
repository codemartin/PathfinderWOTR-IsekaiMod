using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.EdgeLord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature {
    internal class BloodragerChimeraLegacy {
        private static BlueprintProgression prog;
        private static BlueprintFeatureSelection bloodlines;


        public static void Configure() {
            prog = Helpers.CreateBlueprint<BlueprintProgression>(IsekaiContext, "BloodragerChimeraLegacy", bp => {
                bp.SetName(IsekaiContext, "Bloodrager Legacy - Chimeric Rager");
                bp.SetDescription(IsekaiContext,
                    "Much like the Chimera you draw upon the power of inhuman bloodlines and learned how to slowly either awaken or fuse more of them into yourself. \n" +
                    "However, you would rather use that to empower your melee attacks rather than your spells.\n" +
                    "After all, what is the point of draconic claws or a phoenixes burning wings if you only use them as a fallback?");
                bp.GiveFeaturesForPreviousLevels = true;
            });
            bloodlines = Helpers.CreateBlueprint<BlueprintFeatureSelection>(IsekaiContext, "IsekaiBloodragerSelection", bp => {
                bp.SetName(FeatTools.Selections.BloodragerBloodlineSelection.m_DisplayName);
                bp.SetDescription(FeatTools.Selections.BloodragerBloodlineSelection.m_Description);
                bp.Ranks = 1;
                bp.IgnorePrerequisites = true;
                bp.IsClassFeature = true;
            });

            LegacySelection.RegisterForFeat(prog);
            LegacySelection.Register(prog);
            EdgeLordLegacySelection.Register(prog);
            GodEmperorLegacySelection.Prohibit(prog);
            HeroLegacySelection.Prohibit(prog);
            MastermindLegacySelection.Prohibit(prog);
            OverlordLegacySelection.Register(prog);
        }

        public static void PatchProgression() {
            if (prog != null) {
                LevelEntry[] addentries = new LevelEntry[] { };
                LevelEntry[] removeentries = new LevelEntry[] { };
                removeentries = removeentries.AppendToArray(Helpers.CreateLevelEntry(1, FeatTools.Selections.BloodragerBloodlineSelection));
                bloodlines.SetFeatures(FeatTools.Selections.BloodragerBloodlineSelection.m_AllFeatures);

                addentries = addentries.AppendToArray<LevelEntry>(Helpers.CreateLevelEntry(1, bloodlines));
                addentries = addentries.AppendToArray<LevelEntry>(Helpers.CreateLevelEntry(5, bloodlines));
                addentries = addentries.AppendToArray<LevelEntry>(Helpers.CreateLevelEntry(10, bloodlines));
                addentries = addentries.AppendToArray<LevelEntry>(Helpers.CreateLevelEntry(15, bloodlines));

                prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.BloodragerClass, addentries, removeentries);

                BlueprintCharacterClassReference myClass = IsekaiProtagonistClass.GetReference();
                PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, myClass, ClassTools.ClassReferences.BloodragerClass);
                PatchBloodragerResistanceBuffs(myClass);

                prog.AddPrerequisite<PrerequisiteNoFeature>(c => { c.m_Feature = SorcererLegacy.Get().ToReference<BlueprintFeatureReference>(); });
                prog.AddPrerequisite<PrerequisiteNoFeature>(c => { c.m_Feature = BarbarianLegacy.Get().ToReference<BlueprintFeatureReference>(); });
                prog.AddPrerequisite<PrerequisiteNoFeature>(c => { c.m_Feature = SkaldBaseLegacy.Get().ToReference<BlueprintFeatureReference>(); });
                prog.AddPrerequisite<PrerequisiteNoFeature>(c => { c.m_Feature = SkaldVoiceLegacy.Get().ToReference<BlueprintFeatureReference>(); });
                prog.AddPrerequisite<PrerequisiteNoFeature>(c => { c.m_Feature = SkaldSilverTongueLegacy.Get().ToReference<BlueprintFeatureReference>(); });
            }
        }

        public static void PatchPrerequisiteCompatibility() {
            PrerequisiteAlternatives.Add(
                BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19"),
                FeatTools.Selections.BloodragerBloodlineSelection,
                bloodlines);
            PrerequisiteAlternatives.Add(
                BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b7f62628915bdb14d8888c25da3fac56"),
                FeatTools.Selections.BloodragerBloodlineSelection,
                bloodlines);
        }

        private static void PatchBloodragerResistanceBuffs(BlueprintCharacterClassReference myClass) {
            // Bloodline resistance features apply separate buffs during Bloodrage. Those buffs are
            // not children of the bloodline progressions, so the normal inherited-feature traversal
            // cannot find their Bloodrager-only ContextRankConfig components.
            string[] resistanceBuffGuids = {
                "982a14da9e8a4714a739e45111a1ae5a", // BloodragerAberrantResistanceBuff
                "f2ad59f72a9544738a20dfb4e3d34ddd", // BloodragerAbyssalResistanceBuff
                "f1ceaeea9311e0f4884f069ebfa00b3b", // BloodragerCelestialResistancesBuff
                "89ac2964ef12ddb468d5c3ade461eb68", // BloodragerDraconicResistanceACBuff
                "2ac2345547bf3674e931d2b87ebc555d", // BloodragerDraconicResistanceBuffAcid
                "87282f7914eea69498fa8513108cd573", // BloodragerDraconicResistanceBuffCold
                "d70a60fa4ae9b3245b86da2ef2b618d1", // BloodragerDraconicResistanceBuffElectricity
                "54d7b792a931f20459bb983a063aa534", // BloodragerDraconicResistanceBuffFire
                "641d5bdb776d58548ae889102a2d330c", // BloodragerElementalAcidResistanceBuff
                "f8a35d21218171f43ab36deaf4c5c441", // BloodragerElementalColdResistanceBuff
                "2892fe41538768141b5838e4a9fecfa0", // BloodragerElementalElectricityResistanceBuff
                "bd823e94454ab7f4599f846ca4a3bec1", // BloodragerElementalFireResistanceBuff
                "66c973ccad1293345ae6c5bfeced4a7b", // BloodragerInfernalResistanceBuff
                "ef2d9784fcd404d4f9bd97840541b2e5" // BloodragerSerpentineResistanesBuff
            };

            foreach (string buffGuid in resistanceBuffGuids) {
                BlueprintBuff resistanceBuff = BlueprintTools.GetBlueprint<BlueprintBuff>(buffGuid);
                PatchTools.PatchClassIntoFeatureOfReferenceClass(
                    resistanceBuff,
                    myClass,
                    ClassTools.ClassReferences.BloodragerClass);
            }
        }

        public static BlueprintProgression Get() {
            if (prog != null) return prog;
            return BlueprintTools.GetModBlueprint<BlueprintProgression>(IsekaiContext, "BloodragerChimeraLegacy");
        }
    }
}
