using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// The base game often makes two class tracks exclusive in one direction only, because its classes
    /// always pick them in a fixed order: a Beneficial Curse variant forbids the matching base curse, a
    /// secondary domain forbids the primary one, and so on. An Isekai Protagonist can take these in any
    /// order across levels, so the reverse rule is added here for every such pair of progressions offered
    /// by the selections the mod exposes. Only progressions are touched, so feats and hexes that merely
    /// grant another feat keep their intended one-way rules.
    /// </summary>
    internal static class PrerequisiteSymmetry {
        private static readonly string[] ExtraSourceSelections = {
            "2dda67424ee8e0b4d83ef01a73ca6bff", // BeneficialCurse
            "b0a5118b4fb793241bc7042464b23fab", // OracleCurseSelection
            "cc6fda79e8c340b88c84689414a9abbe", // SecondCurseSelection
            "4e7265c0ae1345db90d3375f4ced94cc", // MysteryGiftFeatureCurseSelection
            "48525e5da45c9c243a343fc6545dbdb9", // DomainsSelection
            "43281c3d7fe18cc4d91928395837cd1e", // SecondDomainsSelection
            "213a8480d22206b45acbfa0619ca5aaf", // ExtraDomain
            "42b781e4375d499383b2602d90661283", // SecondDomainsSeparatistSelection
            "72909a37a1ed5344f88ec9d1d31f5c5b", // DivineHunterDomainsSelection
        };

        public static void Apply(IEnumerable<BlueprintFeatureSelection> selections) {
            var offered = new Dictionary<BlueprintGuid, BlueprintProgression>();
            foreach (var selection in selections.Concat(ExtraSourceSelections.Select(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>))) {
                if (selection?.m_AllFeatures == null) continue;
                foreach (var reference in selection.m_AllFeatures) {
                    if (reference?.Get() is BlueprintProgression progression) {
                        offered[progression.AssetGuid] = progression;
                    }
                }
            }

            int added = 0;
            foreach (var progression in offered.Values.ToList()) {
                foreach (var requirement in progression.GetComponents<PrerequisiteNoFeature>().ToList()) {
                    if (requirement == null || requirement.Group != Prerequisite.GroupType.All) continue;
                    var blocked = requirement.Feature;
                    if (blocked == null || blocked == progression) continue;
                    if (!offered.TryGetValue(blocked.AssetGuid, out var other)) continue;
                    if (other.GetComponents<PrerequisiteNoFeature>().Any(existing => existing?.Feature == progression)) continue;
                    other.AddComponent(delegate (PrerequisiteNoFeature c) {
                        c.m_Feature = progression.ToReference<BlueprintFeatureReference>();
                        c.Group = Prerequisite.GroupType.All;
                        c.CheckInProgression = requirement.CheckInProgression;
                    });
                    added++;
                }
            }
            if (added > 0) {
                IsekaiContext.Logger.Log($"PrerequisiteSymmetry: added {added} reverse exclusion(s) between progressions offered to the Isekai Protagonist.");
            }
        }
    }
}
