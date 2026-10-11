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

internal class ArcanistEldritchFontLegacy
{
	private static string BaseArchetypeId = "2f35608fc073c04448a9c162a20852ce";

	private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

	private static BlueprintProgression prog;

	public static void Configure()
	{
		prog = Helpers.CreateBlueprint(Main.IsekaiContext, "ArcanistEldritchFontLegacy", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Arcanist Legacy - Eldritch Font");
			bp.SetDescription(Main.IsekaiContext, "In the life you did not live magic came to you as a flood rather than as a discipline. \nYou never learned restraint, only how to survive the surge: a deeper arcane reservoir, a wilder swell of power on every casting, and the scars to prove you have ridden it out before.");
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
			c.m_Feature = ArcanistBrownFurLegacy.Get().ToReference<BlueprintFeatureReference>();
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
		return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ArcanistEldritchFontLegacy");
	}
}
