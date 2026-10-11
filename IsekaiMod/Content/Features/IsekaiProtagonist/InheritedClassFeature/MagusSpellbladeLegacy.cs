using IsekaiMod.Content.Classes.IsekaiProtagonist;
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

internal class MagusSpellbladeLegacy
{
	private static string BaseArchetypeId = "7d6678f2160018049814838af2ab4236";

	private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

	private static BlueprintProgression prog;

	public static void Configure()
	{
		prog = Helpers.CreateBlueprint(Main.IsekaiContext, "MagusSpellbladeLegacy", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Magus Legacy - Sword Saint");
			bp.SetDescription(Main.IsekaiContext, "In the life you did not live you spent it drawing one sword ten thousand times. \nSpell and stroke are the same motion to you: you strike from a perfect stance, your blade carries the spell, and the first cut is usually the only one you need.");
			bp.GiveFeaturesForPreviousLevels = true;
		});
		LegacySelection.RegisterForFeat(prog);
		LegacySelection.Register(prog);
		MartialGodLegacySelection.Register(prog);
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
		prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.MagusClass, array, array2);
		BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
		PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.MagusClass);
		prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
		{
			c.m_Feature = MagusBasicLegacy.Get().ToReference<BlueprintFeatureReference>();
		});
		prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
		{
			c.m_Feature = MagusArcherLegacy.Get().ToReference<BlueprintFeatureReference>();
		});
		prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
		{
			c.m_Feature = MagusDancerLegacy.Get().ToReference<BlueprintFeatureReference>();
		});
		prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
		{
			c.m_CharacterClass = ClassTools.Classes.MagusClass.ToReference<BlueprintCharacterClassReference>();
		});
	}

	public static BlueprintProgression Get()
	{
		if (prog != null)
		{
			return prog;
		}
		return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "MagusSpellbladeLegacy");
	}
}
