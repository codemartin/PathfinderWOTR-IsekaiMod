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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class ShifterBaseLegacy
	{
		private static BlueprintProgression prog;

		private static BlueprintProgression progAlternate;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "ShifterBaseLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Shifter Legacy - Beast Shifter");
				bp.SetDescription(Main.IsekaiContext, "You have a connection to nature, maybe one of your ancestors as a were creature or a druidic circle initiated you into the secrets of shapeshifting, regardless of the source you have a connection to the animal kingdrom enabling you to shapeshift. \nAs a result of the nature of your shapeshifter ability you have less choice (you will never be a tree or an elemental) but greater control, allowing for partial shifts.");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddComponent(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.NeutralGood | AlignmentMaskType.LawfulNeutral | AlignmentMaskType.TrueNeutral | AlignmentMaskType.ChaoticNeutral | AlignmentMaskType.NeutralEvil;
				});
			});
			progAlternate = Helpers.CreateBlueprint(Main.IsekaiContext, "ShifterEvilLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Shifter Legacy - Skinwalker");
				bp.SetDescription(Main.IsekaiContext, "Skinwalkers are rare and dangerous predators that choose to exist above the natural order. \nRather than communing with nature like the druids, Skinwalkers are known to choose a powerful predator, often times protectors of forests or mountains, before stalking them from the shadows until they find an opportune moment to lay a trap and claim their life. \nThey will then ritually skin their prey before donning their hide and using it to transform into a semblance of its once mighty owner. \nMany a novice necromancer has actually looked at you in horror at the realization of just what this ritual skinning involved, because he originally thought he was the evil member of the group.");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddComponent(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.Evil;
				});
			});
		}

		public static void PatchProgression()
		{
			if (prog == null)
			{
				return;
			}
			if (ClassTools.Classes.ShifterClass == null)
			{
				Main.Log("Shifter Class not present Shifter skipped.");
				return;
			}
			BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
			prog = PatchTools.PatchClassProgressionBasedOnRefClass(prog, ClassTools.Classes.ShifterClass);
			PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.ShifterClass);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.ShifterClass.ToReference<BlueprintCharacterClassReference>();
			});
			progAlternate = PatchTools.PatchClassProgressionBasedOnRefClass(progAlternate, ClassTools.Classes.ShifterClass);
			PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(progAlternate, reference, ClassTools.ClassReferences.ShifterClass);
			progAlternate.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.ShifterClass.ToReference<BlueprintCharacterClassReference>();
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.RegisterForFeat(progAlternate);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			GodEmperorLegacySelection.Prohibit(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Prohibit(prog);
			OverlordLegacySelection.Register(prog);
			LegacySelection.Register(progAlternate);
			MartialGodLegacySelection.Register(progAlternate);
			GodEmperorLegacySelection.Prohibit(progAlternate);
			HeroLegacySelection.Prohibit(progAlternate);
			MastermindLegacySelection.Prohibit(progAlternate);
			OverlordLegacySelection.Register(progAlternate);
			prog.AddPrerequisiteNoFeature(ShifterLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterStingerLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterDragonLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterGriffonLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterHolyLegacy.Get());
			prog.AddPrerequisiteNoFeature(progAlternate);
			progAlternate.AddPrerequisiteNoFeature(ShifterLegacy.Get());
			progAlternate.AddPrerequisiteNoFeature(ShifterStingerLegacy.Get());
			progAlternate.AddPrerequisiteNoFeature(ShifterDragonLegacy.Get());
			progAlternate.AddPrerequisiteNoFeature(ShifterGriffonLegacy.Get());
			progAlternate.AddPrerequisiteNoFeature(ShifterHolyLegacy.Get());
			progAlternate.AddPrerequisiteNoFeature(prog);
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ShifterBaseLegacy");
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
}
