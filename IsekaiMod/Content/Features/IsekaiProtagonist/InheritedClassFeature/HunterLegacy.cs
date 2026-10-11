using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;

internal class HunterLegacy
{
	private static BlueprintProgression prog;

	public static void Configure()
	{
		prog = Helpers.CreateBlueprint(Main.IsekaiContext, "HunterLegacy", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Hunter Legacy - Pack Hunter");
			bp.SetDescription(Main.IsekaiContext, "Forging an unbreakable primal pact with beasts and nature, you channel the instinctual focuses of the wild. Through coordinated teamwork tactics, you and your pack strike with predatory precision.");
			bp.GiveFeaturesForPreviousLevels = true;
		});
		LegacySelection.RegisterForFeat(prog);
		LegacySelection.Register(prog);
		MartialGodLegacySelection.Register(prog);
		GodEmperorLegacySelection.Prohibit(prog);
		HeroLegacySelection.Register(prog);
		MastermindLegacySelection.Prohibit(prog);
		OverlordLegacySelection.Prohibit(prog);
		ShadowMonarchLegacySelection.Register(prog);
		DevourerLegacySelection.Register(prog);
	}

	public static BlueprintProgression Get()
	{
		if (prog != null)
		{
			return prog;
		}
		return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "HunterLegacy");
	}

	public static void PatchProgression()
	{
		if (prog != null)
		{
			LevelEntry[] removedReference = PatchTools.BuildRemovalEntries(ClassTools.Classes.HunterClass?.Progression, BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("715ac15eb8bd5e342bc8a0a3c9e3e38f"));
			prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.HunterClass, new LevelEntry[0], removedReference);
			BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
			PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.HunterClass);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.HunterClass.ToReference<BlueprintCharacterClassReference>();
			});
		}
	}
}
