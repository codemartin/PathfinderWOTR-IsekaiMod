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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class ShifterStingerLegacy
	{
		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("1a51b5856d3b48d78b3e967dc5f20bd6");

		private static BlueprintProgression prog;

		public static void Configure()
		{
			if (ClassTools.Classes.ShifterClass != null)
			{
				prog = Helpers.CreateBlueprint(Main.IsekaiContext, "ShifterStingerLegacy", delegate(BlueprintProgression bp)
				{
					bp.SetName(Main.IsekaiContext, "Shifter Legacy - Stinger");
					bp.SetDescription(Main.IsekaiContext, "Some things cannot be easily forgotten, and in your dreaded former life you were called a hedgehog... \nBut this new world shall be respectful to your newfound stingers, as they are even more dangerous than your sharp tongue.");
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
				BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("1a51b5856d3b48d78b3e967dc5f20bd6");
				if (BaseArchetype == null)
				{
					return;
				}
			}
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
			GodEmperorLegacySelection.Prohibit(prog);
			HeroLegacySelection.Prohibit(prog);
			MastermindLegacySelection.Register(prog);
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
			prog.AddPrerequisiteNoFeature(ShifterDragonLegacy.Get());
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "ShifterStingerLegacy");
		}
	}
}
