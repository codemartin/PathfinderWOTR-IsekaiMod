using System;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

internal static class PrerequisiteAlternatives
{
    internal static bool Add(BlueprintFeature target, BlueprintFeature source, BlueprintFeature equivalent)
    {
        if (target == null || source == null || equivalent == null) return false;
        bool changed = false;
        // Replace a single required fact with a one-of list in the SAME group.
        // Moving it into Group.Any would weaken unrelated existing requirements.
        foreach (var requirement in target.GetComponents<PrerequisiteFeature>().ToArray())
        {
            if (requirement.Feature != source) continue;
            target.ComponentsArray = target.ComponentsArray.Where(c => c != requirement).ToArray();
            target.AddComponent(delegate(PrerequisiteFeaturesFromList c)
            {
                c.Group = requirement.Group;
                c.CheckInProgression = requirement.CheckInProgression;
                c.HideInUI = requirement.HideInUI;
                c.Amount = 1;
                c.m_Features = new[] { source.ToReference<BlueprintFeatureReference>(), equivalent.ToReference<BlueprintFeatureReference>() };
            });
            changed = true;
        }
        foreach (var requirement in target.GetComponents<PrerequisiteFeaturesFromList>())
        {
            var choices = requirement.m_Features ?? Array.Empty<BlueprintFeatureReference>();
            if (!choices.Any(f => f?.Guid == source.AssetGuid) || choices.Any(f => f?.Guid == equivalent.AssetGuid)) continue;
            // Multi-fact requirements cannot safely count source and equivalent twice.
            if (requirement.Amount != 1)
            {
                Main.IsekaiContext.Logger.LogWarning($"PrerequisiteAlternatives: skipped multi-fact requirement on {target.name}");
                continue;
            }
            requirement.m_Features = choices.Append(equivalent.ToReference<BlueprintFeatureReference>()).ToArray();
            changed = true;
        }
        if (changed) Main.IsekaiContext.Logger.Log($"PrerequisiteAlternatives: {equivalent.name} now satisfies {target.name}");
        return changed;
    }

    internal static void RequireUnownedChoices(BlueprintFeatureSelection selection)
    {
        foreach (var reference in selection?.m_AllFeatures ?? Array.Empty<BlueprintFeatureReference>())
        {
            var choice = reference?.Get();
            if (choice == null || choice.GetComponents<PrerequisiteNoFeature>().Any(c => c.m_Feature?.Guid == choice.AssetGuid)) continue;
            choice.AddComponent(delegate(PrerequisiteNoFeature c) { c.m_Feature = choice.ToReference<BlueprintFeatureReference>(); });
        }
    }
}
