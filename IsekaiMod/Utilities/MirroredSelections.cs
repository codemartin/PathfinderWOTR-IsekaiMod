using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using System.Collections.Generic;
using System.Linq;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {

    /// <summary>
    /// Tracks Isekai selections whose feature lists are derived from base-game selections.
    ///
    /// Isekai builds its blueprints in a BlueprintsCache.Init postfix with Priority.First, so any
    /// copy of a base-game selection's feature list is taken before other mods add their own
    /// features to that selection. Registering the mirror here lets FinalPatcher call Sync() after
    /// every other mod's init postfix has run, appending whatever the sources gained in between.
    /// </summary>
    internal static class MirroredSelections {
        private class Mirror {
            public BlueprintFeatureSelection Target;
            public BlueprintFeatureSelection[] Sources;
            public HashSet<BlueprintGuid> Excluded;
        }

        private static readonly List<Mirror> mirrors = new List<Mirror>();

        /// <summary>
        /// Registers <paramref name="target"/> as a mirror of <paramref name="sources"/>. The target's
        /// current feature array is replaced with a private copy so it no longer aliases a base-game array.
        /// </summary>
        public static void Register(BlueprintFeatureSelection target, params BlueprintFeatureSelection[] sources) {
            Register(target, null, sources);
        }

        /// <summary>
        /// Same as <see cref="Register(BlueprintFeatureSelection, BlueprintFeatureSelection[])"/>, but features in
        /// <paramref name="exclude"/> are never copied from the sources.
        /// </summary>
        public static void Register(BlueprintFeatureSelection target, IEnumerable<BlueprintFeatureReference> exclude, params BlueprintFeatureSelection[] sources) {
            if (target == null) return;
            target.m_AllFeatures = (target.m_AllFeatures ?? new BlueprintFeatureReference[0]).ToArray();
            mirrors.Add(new Mirror {
                Target = target,
                Sources = (sources ?? new BlueprintFeatureSelection[0]).Where(s => s != null).ToArray(),
                Excluded = new HashSet<BlueprintGuid>((exclude ?? Enumerable.Empty<BlueprintFeatureReference>()).Where(r => r != null).Select(r => r.Guid))
            });
        }

        /// <summary>
        /// Appends to every registered mirror the source features it does not already contain.
        /// Existing entries, their order, and Isekai-specific additions or removals are preserved.
        /// </summary>
        public static void Sync() {
            int touched = 0;
            foreach (Mirror mirror in mirrors) {
                int added = Apply(mirror);
                if (added > 0) {
                    touched++;
                    IsekaiContext.Logger.Log($"MirroredSelections: {mirror.Target.name} gained {added} feature(s) added by other mods");
                }
            }
            IsekaiContext.Logger.Log($"MirroredSelections: synced {mirrors.Count} mirror(s), {touched} changed");
        }

        private static int Apply(Mirror mirror) {
            BlueprintFeatureReference[] existing = mirror.Target.m_AllFeatures ?? new BlueprintFeatureReference[0];
            var result = existing.ToList();
            var seen = new HashSet<BlueprintGuid>(existing.Where(r => r != null).Select(r => r.Guid));
            foreach (BlueprintFeatureSelection source in mirror.Sources) {
                foreach (BlueprintFeatureReference reference in source.m_AllFeatures ?? new BlueprintFeatureReference[0]) {
                    if (reference == null) continue;
                    if (mirror.Excluded.Contains(reference.Guid)) continue;
                    if (seen.Add(reference.Guid)) result.Add(reference);
                }
            }
            int added = result.Count - existing.Length;
            if (added > 0) {
                mirror.Target.m_AllFeatures = result.ToArray();
                if (mirror.Target.m_Features != null && mirror.Target.m_Features.Length > 0) {
                    mirror.Target.m_Features = mirror.Target.m_AllFeatures;
                }
            }
            return added;
        }
    }
}
