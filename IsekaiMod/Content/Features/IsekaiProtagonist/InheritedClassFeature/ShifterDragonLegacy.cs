using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class ShifterDragonLegacy
	{
		private static string BaseArchetypeId = "2d5b06e413a9408cbd5bb999b5a4cc4a";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			if (ClassTools.Classes.ShifterClass != null)
			{
				prog = Helpers.CreateBlueprint(Main.IsekaiContext, "ShifterDragonLegacy", delegate(BlueprintProgression bp)
				{
					bp.SetName(Main.IsekaiContext, "Shifter Legacy - Shapeshifted Baby Dragon");
					bp.SetDescription(Main.IsekaiContext, "When reincaranting you wanted to be a dragon. \nBecause dragons are cool! \nBut sadly  that would have been too balancebreaking according to the god of reincarnation. \nWeirdly enough being a shapeshifted baby dragon that learns to reassume his natural form with time is not. \nSo one day you will soar through the sky in your draconic form. \nDragon Claws are useful in the meantime...");
					bp.GiveFeaturesForPreviousLevels = true;
				});
			}
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
			GodEmperorLegacySelection.Prohibit(prog);
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
			prog.AddPrerequisiteNoFeature(ShifterGriffonLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterHolyLegacy.Get());
			prog.AddPrerequisiteNoFeature(ShifterStingerLegacy.Get());
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ShifterDragonLegacy");
		}
	}
}
