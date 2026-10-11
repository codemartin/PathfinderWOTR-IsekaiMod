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
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;

internal class KineticPsychoLegacy
{
	private static string BaseArchetypeId = "f2847dd4b12fffd41beaa3d7120d27ad";

	private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

	private static BlueprintProgression prog;

	public static void Configure()
	{
		prog = Helpers.CreateBlueprint(Main.IsekaiContext, "KineticPsychoLegacy", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Kinetic Legacy - Elemental Ascendent");
			bp.SetDescription(Main.IsekaiContext, "The unfathomable willpower needed to fuel your own ascenscion allows you to bend the elements as you see fit. \nAs your divine essence matures you push the limits of your mind ever so far to claim dominion over the fundamental forces of the multiverse, sometimes at the risk of being overwhelmed by them.");
			bp.GiveFeaturesForPreviousLevels = true;
			bp.AddComponent(delegate(AddProficiencies c)
			{
				c.WeaponProficiencies = new WeaponCategory[1] { WeaponCategory.KineticBlast };
			});
		});
		LegacySelection.RegisterForFeat(prog);
		LegacySelection.Register(prog);
		MartialGodLegacySelection.Prohibit(prog);
		GodEmperorLegacySelection.Register(prog);
		HeroLegacySelection.Register(prog);
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
		prog = PatchTools.PatchClassProgressionBasedonRefArchetype(prog, ClassTools.Classes.KineticistClass, BaseArchetype, null);
		BlueprintCharacterClassReference kineticistClass = ClassTools.ClassReferences.KineticistClass;
		BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
		PatchTools.PatchProgressionFeaturesBasedOnReferenceArchetype(reference, kineticistClass, BaseArchetype);
		PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, kineticistClass);
		prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
		{
			c.m_CharacterClass = ClassTools.Classes.KineticistClass.ToReference<BlueprintCharacterClassReference>();
		});
		prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
		{
			c.m_Feature = KineticDarkElementalistLegacy.Get().ToReference<BlueprintFeatureReference>();
		});
		prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
		{
			c.m_Feature = KineticKnightLegacy.Get().ToReference<BlueprintFeatureReference>();
		});
		prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
		{
			c.m_Feature = KineticLegacy.Get().ToReference<BlueprintFeatureReference>();
		});
		prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
		{
			c.m_Feature = KineticOverwhelmingSoulLegacy.Get().ToReference<BlueprintFeatureReference>();
		});
	}

	public static BlueprintProgression Get()
	{
		if (prog != null)
		{
			return prog;
		}
		return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "KineticPsychoLegacy");
	}
}
