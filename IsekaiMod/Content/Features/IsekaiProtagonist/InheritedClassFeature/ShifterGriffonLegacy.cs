using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
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
	internal class ShifterGriffonLegacy
	{
		private static readonly string BaseArchetypeId = "aed5b306ad734a6da5d5638edcb667c9";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			if (ClassTools.Classes.ShifterClass == null)
			{
				return;
			}
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "ShifterGriffonLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Shifter Legacy - Shapeshifted Griffon");
				bp.SetDescription(Main.IsekaiContext, "Why does everyone always think of dragons first?\nWe griffons are cool too and unlike dragons we are actually reliable servants of the divine and the symbol of nobility. \nSo just sit back and watch me save this world after that dragon messed up, as dragons always do...");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddComponent(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.Good;
				});
			});
		}

		public static void PatchProgression()
		{
			if (ClassTools.Classes.ShifterClass == null)
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
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Prohibit(prog);
			OverlordLegacySelection.Register(prog);
			prog = PatchTools.PatchClassProgressionBasedonRefArchetype(prog, ClassTools.Classes.ShifterClass, BaseArchetype, null);
			BlueprintCharacterClassReference shifterClass = ClassTools.ClassReferences.ShifterClass;
			PatchTools.PatchProgressionFeaturesBasedOnReferenceArchetype(IsekaiProtagonistClass.GetReference(), shifterClass, BaseArchetype);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.ShifterClass.ToReference<BlueprintCharacterClassReference>();
			});
			prog.AddPrerequisiteNoFeature(ShifterLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterBaseLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterBaseLegacy.GetEvilAlternate());
			prog.AddPrerequisiteNoFeature(ShifterStingerLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterHolyLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterDragonLegacy.Get());
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ShifterGriffonLegacy");
		}
	}
}
