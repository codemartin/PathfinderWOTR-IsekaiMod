using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using static IsekaiMod.Main;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature {
    internal class MagusDancerLegacy {
        private static string BaseArchetypeId = "1125145639129cf45b6b9b674cbd62b1";
        private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

        private static BlueprintProgression prog;

        public static void Configure() {
            prog = Helpers.CreateBlueprint<BlueprintProgression>(IsekaiContext, "MagusDancerLegacy", bp => {
                bp.SetName(IsekaiContext, "Magus Legacy - Spell Dancer");
                bp.SetDescription(IsekaiContext,
                    "So you were not really into martial arts or physical fighting of any kind. \n" +
                    "More like hundrets of hours of ballet lessons now serve as he basis of some really good dodging skills. \n" +
                    "Now with magical powers running through your veins you are even more flexible and thus have an even easier time dodging. \n" +
                    "According to some elf you met this is actually a special elven form of magic called spell dancing."
                    );
                bp.GiveFeaturesForPreviousLevels = true;
            });

            LegacySelection.RegisterForFeat(prog);
            LegacySelection.Register(prog);
            //EdgeLordLegacySelection.Register(prog);
            //GodEmperorLegacySelection.Prohibit(prog);
            HeroLegacySelection.Register(prog);
            MastermindLegacySelection.Prohibit(prog);
            OverlordLegacySelection.Register(prog);
        }
        public static void PatchProgression() {
            if (prog != null) {
                if (BaseArchetype == null) {
                    BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);
                    if (BaseArchetype == null) { return; }
                }
                LevelEntry[] addentries = new LevelEntry[] { };
                LevelEntry[] removeentries = new LevelEntry[] { };

                removeentries = removeentries.AppendToArray(BaseArchetype.RemoveFeatures);
                addentries = addentries.AppendToArray(BaseArchetype.AddFeatures);

                prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.MagusClass, addentries, removeentries);

                // Spell Dancer retains the normal Magus arcane pool. Owlcat keeps that feature
                // outside the archetype additions copied above in some game versions, which left
                // inherited Spell Dancers without the pool and unable to qualify for related feats.
                var arcanePool = BlueprintTools.GetBlueprint<BlueprintFeature>("a2fe27d00ece46a5a109d2d1f0bfafa7");
                if (arcanePool == null) return;
                bool hasArcanePool = false;
                foreach (LevelEntry levelEntry in prog.LevelEntries) {
                    foreach (BlueprintFeatureBaseReference feature in levelEntry.m_Features) {
                        if (feature.Guid == arcanePool.AssetGuid) {
                            hasArcanePool = true;
                            break;
                        }
                    }
                    if (hasArcanePool) break;
                }
                if (!hasArcanePool) {
                    LevelEntry levelOne = null;
                    foreach (LevelEntry levelEntry in prog.LevelEntries) {
                        if (levelEntry.Level == 1) {
                            levelOne = levelEntry;
                            break;
                        }
                    }
                    if (levelOne == null) {
                        prog.LevelEntries = prog.LevelEntries.AppendToArray(Helpers.CreateLevelEntry(1, arcanePool));
                    } else {
                        levelOne.m_Features.Add(arcanePool.ToReference<BlueprintFeatureBaseReference>());
                    }
                }

                PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, IsekaiProtagonistClass.GetReference(), ClassTools.ClassReferences.MagusClass);

                prog.AddPrerequisite<PrerequisiteNoFeature>(c => { c.m_Feature = MagusBasicLegacy.Get().ToReference<BlueprintFeatureReference>(); });
                prog.AddPrerequisite<PrerequisiteNoFeature>(c => { c.m_Feature = MagusArcherLegacy.Get().ToReference<BlueprintFeatureReference>(); });
                prog.AddPrerequisite<PrerequisiteNoFeature>(c => { c.m_Feature = MagusSpellbladeLegacy.Get().ToReference<BlueprintFeatureReference>(); });
            }
        }
        public static BlueprintProgression Get() {
            if (prog != null) return prog;
            return BlueprintTools.GetModBlueprint<BlueprintProgression>(IsekaiContext, "MagusDancerLegacy");
        }
    }
}
