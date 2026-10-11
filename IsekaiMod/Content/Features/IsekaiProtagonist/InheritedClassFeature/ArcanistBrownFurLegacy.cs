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

internal class ArcanistBrownFurLegacy
{
	private static string BaseArchetypeId = "ce63b2df8c6c12649b02c278e73d8bfa";

	private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

	private static BlueprintProgression prog;

	public static void Configure()
	{
		prog = Helpers.CreateBlueprint(Main.IsekaiContext, "ArcanistBrownFurLegacy", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Arcanist Legacy - Brown Fur Transmuter");
			bp.SetDescription(Main.IsekaiContext, "In the life you did not live you were a hedge transmuter, and you were better at it than anyone wanted to admit. \nEvery shaping spell you cast on yourself lands twice as hard, and the arcane reservoir you draw on never quite runs dry when the fight matters.");
			bp.GiveFeaturesForPreviousLevels = true;
		});
		LegacySelection.RegisterForFeat(prog);
		LegacySelection.Register(prog);
		MartialGodLegacySelection.Register(prog);
		GodEmperorLegacySelection.Prohibit(prog);
		HeroLegacySelection.Prohibit(prog);
		MastermindLegacySelection.Prohibit(prog);
		OverlordLegacySelection.Register(prog);
	}

	public static void PatchProgression()
	{
		if (prog == null)
		{
			return;
		}
		if (BaseArchetype == null)
		{
			BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);
			if (BaseArchetype == null)
			{
				return;
			}
		}
		LevelEntry[] array = new LevelEntry[0];
		LevelEntry[] array2 = new LevelEntry[0];
		array2 = array2.AppendToArray(BaseArchetype.RemoveFeatures);
		array = array.AppendToArray(BaseArchetype.AddFeatures);
		prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.ArcanistClass, array, array2);
		BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
		PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.ArcanistClass);
		prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
		{
			c.m_Feature = ArcanistBasicLegacy.Get().ToReference<BlueprintFeatureReference>();
		});
		prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
		{
			c.m_Feature = ArcanistEldritchFontLegacy.Get().ToReference<BlueprintFeatureReference>();
		});
		prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
		{
			c.m_CharacterClass = ClassTools.Classes.ArcanistClass.ToReference<BlueprintCharacterClassReference>();
		});
	}

	public static BlueprintProgression Get()
	{
		if (prog != null)
		{
			return prog;
		}
		return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ArcanistBrownFurLegacy");
	}
}
