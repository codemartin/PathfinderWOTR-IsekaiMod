using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;

internal class ArcanistBasicLegacy
{
	private static BlueprintProgression prog;

	public static void Configure()
	{
		prog = Helpers.CreateBlueprint(Main.IsekaiContext, "ArcanistBasicLegacy", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Arcanist Legacy - Arcanist");
			bp.SetDescription(Main.IsekaiContext, "Your otherworldy background allows you to  easily graps some of the exploits available within the laws of magic. \nThis allows you to shape the magic around you in specific ways that might  be similar to spells but are not quite spells.");
			bp.GiveFeaturesForPreviousLevels = true;
		});
		LegacySelection.RegisterForFeat(prog);
		LegacySelection.Register(prog);
		MartialGodLegacySelection.Prohibit(prog);
		GodEmperorLegacySelection.Prohibit(prog);
		HeroLegacySelection.Register(prog);
		MastermindLegacySelection.Prohibit(prog);
		OverlordLegacySelection.Register(prog);
	}

	public static void PatchProgression()
	{
		if (prog != null)
		{
			prog = PatchTools.PatchClassProgressionBasedOnRefClass(prog, ClassTools.Classes.ArcanistClass);
			BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
			PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.ArcanistClass);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.ArcanistClass.ToReference<BlueprintCharacterClassReference>();
			});
			prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = ArcanistBrownFurLegacy.Get().ToReference<BlueprintFeatureReference>();
			});
			prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = ArcanistEldritchFontLegacy.Get().ToReference<BlueprintFeatureReference>();
			});
		}
	}

	public static BlueprintProgression Get()
	{
		if (prog != null)
		{
			return prog;
		}
		return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ArcanistBasicLegacy");
	}
}
