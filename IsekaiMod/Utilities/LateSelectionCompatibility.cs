using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

// Refresh copied choices after initial registration and again after late-loading mods.
internal static class LateSelectionCompatibility
{
    private static BlueprintFeatureSelection Own(string name) =>
        BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, name);
    private static BlueprintFeatureSelection Vanilla(string guid) =>
        BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(guid);

    internal static BlueprintFeatureReference[] Merge(IEnumerable<BlueprintFeatureReference> own,
        IEnumerable<BlueprintFeatureReference> added)
    {
        var seen = new HashSet<BlueprintGuid>();
        return (own ?? Enumerable.Empty<BlueprintFeatureReference>())
            .Concat(added ?? Enumerable.Empty<BlueprintFeatureReference>())
            .Where(f => f != null && seen.Add(f.Guid)).ToArray();
    }

    private static void Sync(string name, params BlueprintFeatureSelection[] sources)
    {
        var target = Own(name);
        if (target == null) return;
        var before = target.m_AllFeatures?.Length ?? 0;
        var previous = new HashSet<BlueprintGuid>((target.m_AllFeatures ?? Array.Empty<BlueprintFeatureReference>()).Where(f => f != null).Select(f => f.Guid));
        target.m_AllFeatures = Merge(target.m_AllFeatures,
            sources.Where(s => s != null).SelectMany(s => s.m_AllFeatures ?? Array.Empty<BlueprintFeatureReference>()));
        target.m_Features = (BlueprintFeatureReference[])target.m_AllFeatures.Clone();
        BlueprintCharacterClassReference sourceClass = name switch
        {
            "IsekaiBloodlineSelection" => ClassTools.ClassReferences.SorcererClass,
            "IsekaiBloodragerSelection" => ClassTools.ClassReferences.BloodragerClass,
            "IsekaiOracleCurseSelection" or "IsekaiOracleMysterySelection" => ClassTools.ClassReferences.OracleClass,
            "IsekaiHexSelection" or "IsekaiSpiritSelection" => ClassTools.ClassReferences.ShamanClass,
            "IsekaiWitchSelection" => ClassTools.ClassReferences.WitchClass,
            "InquisitorDomains" or "InquisitorAdditionalDomains" => ClassTools.ClassReferences.InquisitorClass,
            _ => null
        };
        if (sourceClass != null)
            foreach (var added in target.m_AllFeatures.Where(f => !previous.Contains(f.Guid)))
                PatchTools.PatchClassIntoFeatureOfReferenceClass(added.Get(), IsekaiProtagonistClass.GetReference(), sourceClass);
        if (target.m_AllFeatures.Length != before)
            Main.IsekaiContext.Logger.Log($"LateSelectionCompatibility: {name}: {before} -> {target.m_AllFeatures.Length} choices");
    }

    internal static void Refresh()
    {
        BonusFeatListRefresh.RefreshAll();
        Sync("IsekaiBloodlineSelection", StaticReferences.SorcererBloodlineSelection);
        Sync("IsekaiBloodragerSelection", FeatTools.Selections.BloodragerBloodlineSelection);
        Sync("IsekaiOracleCurseSelection", FeatTools.Selections.OracleCurseSelection);
        Sync("IsekaiOracleMysterySelection", FeatTools.Selections.OracleMysterySelection);
        Sync("IsekaiHexSelection", ShamanLegacy.shamanHex);
        Sync("IsekaiSpiritSelection", ShamanLegacy.shamanSpirit);
        Sync("IsekaiWitchSelection", FeatTools.Selections.WitchPatronSelection);
        Sync("IsekaiFamiliarSelection", FeatTools.Selections.WitchFamiliarSelection);
        Sync("InquisitorDomains", FeatTools.Selections.DomainsSelection);
        Sync("InquisitorAdditionalDomains", FeatTools.Selections.DomainsSelection);
        Sync("ExtraSpecialPowerSelection", Own("SpecialPowerSelection"));
        Sync("BlessingOfTheTrickster", Vanilla("4fbc563529717de4d92052048143e0f1"), Vanilla("5cd96c3460844fc458dc3e1656dafa42"), Vanilla("446f4a8b32019f5478a8dfeddac74710"));
        Sync("BlessingOfTheAzata", Vanilla("8a30e92cd04ff5b459ba7cb03584fda0"));
        Sync("BlessingOfTheLich", Vanilla("1f646b820a37d3d4a8ab116a24ee0022"));
        Sync("BlessingOfTheAngel", Vanilla("bdbc41e2bad92a640bd58acf74e2af8b"), Vanilla("e0ce40968bf0007408b11089a10f36cf"));
        PatchPrerequisites();
        PrerequisiteSymmetry.Apply(new[] {
            Own("LegacyClassSelection"), Own("LegacyAbilitySelection"), Own("IsekaiBloodlineSelection"), Own("IsekaiBloodragerSelection"),
            Own("IsekaiOracleCurseSelection"), Own("IsekaiOracleMysterySelection"), Own("IsekaiHexSelection"),
            Own("IsekaiSpiritSelection"), Own("IsekaiWitchSelection"), Own("InquisitorDomains"), Own("InquisitorAdditionalDomains")
        }.Where(s => s != null));
    }

    private static void Accept(string guid, BlueprintFeature source, string equivalent) =>
        PrerequisiteAlternatives.Add(BlueprintTools.GetBlueprint<BlueprintFeature>(guid), source, Own(equivalent));

    private static void PatchPrerequisites()
    {
        Accept("ce85aee1726900641ab53ede61ac5c19", StaticReferences.SorcererBloodlineSelection, "IsekaiBloodlineSelection");
        Accept("3cf2ab2c320b73347a7c21cf0d0995bd", StaticReferences.SorcererBloodlineSelection, "IsekaiBloodlineSelection");
        Accept("ce85aee1726900641ab53ede61ac5c19", FeatTools.Selections.BloodragerBloodlineSelection, "IsekaiBloodragerSelection");
        Accept("b7f62628915bdb14d8888c25da3fac56", FeatTools.Selections.BloodragerBloodlineSelection, "IsekaiBloodragerSelection");
        Accept("2dda67424ee8e0b4d83ef01a73ca6bff", FeatTools.Selections.OracleCurseSelection, "IsekaiOracleCurseSelection");
        Accept("277b0164740b97945a3f8022bd572f48", FeatTools.Selections.OracleMysterySelection, "IsekaiOracleMysterySelection");
        Accept("08d9f686b2944ba6b3f7763882c0ded4", ShamanLegacy.shamanHex, "IsekaiHexSelection");
        Accept("2faa80662a56ab644aec2f875a68597f", ShamanLegacy.shamanSpirit, "IsekaiSpiritSelection");
        Accept("24afc8be7a964e5a939b2a199ba60682", FeatTools.Selections.WitchPatronSelection, "IsekaiWitchSelection");
        foreach (var guid in new[] { "2de64f6a1f2baee4f9b7e52e3f046ec5", "213a8480d22206b45acbfa0619ca5aaf" })
        {
            Accept(guid, FeatTools.Selections.DomainsSelection, "InquisitorDomains");
            Accept(guid, FeatTools.Selections.DomainsSelection, "InquisitorAdditionalDomains");
        }
        PrerequisiteAlternatives.RequireUnownedChoices(Vanilla("b7f62628915bdb14d8888c25da3fac56"));
    }
}
