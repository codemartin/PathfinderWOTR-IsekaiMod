using Kingmaker.UnitLogic;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;

namespace IsekaiMod.Components;

internal static class TwinRacialCompatibility
{
    // Race metadata alone does not activate third-party racial progression components.
    // Keep this opt-in compatibility narrow: do not copy the master's acquired feats,
    // milestone facts, class levels or arbitrary race selections onto a Guardian.
    internal static void Sync(BlueprintRace race, UnitEntityData pet)
    {
        const string ascendantRace = "566c822788d4483d8c9d984d0e03aeb0";
        if (race == null || pet?.Descriptor?.Progression?.Race != race ||
            race.AssetGuid.ToString() != ascendantRace || race.m_Features == null)
            return;

        foreach (var reference in race.m_Features)
        {
            if (!(reference?.Get() is BlueprintFeature feature) ||
                feature is Kingmaker.Blueprints.Classes.Selection.BlueprintFeatureSelection ||
                pet.Descriptor.HasFact(feature))
                continue;
            pet.Descriptor.Progression.Features.AddFeature(feature);
            Main.IsekaiContext.Logger.Log($"Twin Avatar racial compatibility: granted {feature.name} to {pet.CharacterName} at character level {pet.Progression.CharacterLevel}");
        }
    }

}
