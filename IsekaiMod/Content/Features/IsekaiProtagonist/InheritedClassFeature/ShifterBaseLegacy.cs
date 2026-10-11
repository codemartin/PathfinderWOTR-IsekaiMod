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
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;

internal class ShifterBaseLegacy
{
	private static BlueprintProgression progAlternate;

	private const string FiendfleshShifterArchetypeId = "f5d22b6b5a7940d49a4d494f67a91efa";

	public static void Configure()
	{
		progAlternate = Helpers.CreateBlueprint(Main.IsekaiContext, "ShifterEvilLegacy", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Shifter Legacy - Skinwalker");
			bp.SetDescription(Main.IsekaiContext, "Skinwalkers are rare and dangerous predators that choose to exist above the natural order. \nRather than communing with nature like the druids, Skinwalkers are known to choose a powerful predator, often times protectors of forests or mountains, before stalking them from the shadows until they find an opportune moment to lay a trap and claim their life. \nThey will then ritually skin their prey before donning their hide and using it to transform into a semblance of its once mighty owner. \nMany a novice necromancer has actually looked at you in horror at the realization of just what this ritual skinning involved, because he originally thought he was the evil member of the group. \nThe stolen hides carry the corruption of the Worldwound: this legacy follows the Fiendflesh Shifter archetype (Fiendish Aspect, Infernal Claws and Fiendish Resilience in place of the usual shifter aspects, Major Form and Defensive Instinct).");
			bp.GiveFeaturesForPreviousLevels = true;
			bp.AddComponent(delegate(PrerequisiteAlignment c)
			{
				c.Alignment = AlignmentMaskType.Evil;
			});
			IsekaiDlcManager.OfferOnlyWithDlc4(bp);
		});
	}

	public static void PatchProgression()
	{
		if (progAlternate == null)
		{
			return;
		}
		if (ClassTools.Classes.ShifterClass == null)
		{
			Main.Log("Shifter Class not present Shifter skipped.");
			return;
		}
		BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
		BlueprintArchetype blueprint = BlueprintTools.GetBlueprint<BlueprintArchetype>("f5d22b6b5a7940d49a4d494f67a91efa");
		if (blueprint != null)
		{
			progAlternate = PatchTools.PatchClassProgressionBasedonRefArchetype(progAlternate, ClassTools.Classes.ShifterClass, blueprint, null);
		}
		else
		{
			Main.Log("Fiendflesh Shifter archetype not found; Skinwalker legacy falls back to the plain Shifter progression.");
			progAlternate = PatchTools.PatchClassProgressionBasedOnRefClass(progAlternate, ClassTools.Classes.ShifterClass);
		}
		PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(progAlternate, reference, ClassTools.ClassReferences.ShifterClass);
		progAlternate.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
		{
			c.m_CharacterClass = ClassTools.Classes.ShifterClass.ToReference<BlueprintCharacterClassReference>();
		});
		LegacySelection.RegisterForFeat(progAlternate);
		LegacySelection.Register(progAlternate);
		MartialGodLegacySelection.Register(progAlternate);
		GodEmperorLegacySelection.Prohibit(progAlternate);
		HeroLegacySelection.Prohibit(progAlternate);
		MastermindLegacySelection.Prohibit(progAlternate);
		OverlordLegacySelection.Register(progAlternate);
		progAlternate.AddPrerequisiteNoFeature(ShifterLegacy.Get());
		progAlternate.AddPrerequisiteNoFeature(ShifterStingerLegacy.Get());
		progAlternate.AddPrerequisiteNoFeature(ShifterDragonLegacy.Get());
		progAlternate.AddPrerequisiteNoFeature(ShifterGriffonLegacy.Get());
		progAlternate.AddPrerequisiteNoFeature(ShifterHolyLegacy.Get());
		progAlternate.AddPrerequisiteNoFeature(DruidBaseLegacy.Get());
	}

	public static BlueprintProgression GetEvilAlternate()
	{
		if (progAlternate != null)
		{
			return progAlternate;
		}
		return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ShifterEvilLegacy");
	}
}
