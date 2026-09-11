using IsekaiMod.Content.Classes.IsekaiProtagonist;
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
	internal class MonkScaledFistLegacy
	{
		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("5868fc82eb11a4244926363983897279");

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "MonkScaledFistLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Monk Legacy - Fist of a Dragon");
				bp.SetDescription(Main.IsekaiContext, "You have a dragon in your dantian!");
				bp.GiveFeaturesForPreviousLevels = true;
				bp.AddComponent(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.Lawful | AlignmentMaskType.NeutralGood | AlignmentMaskType.TrueNeutral | AlignmentMaskType.NeutralEvil;
				});
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Prohibit(prog);
			OverlordLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
			prog = PatchTools.PatchClassProgressionBasedonRefArchetype(prog, ClassTools.Classes.MonkClass, BaseArchetype, null);
			BlueprintCharacterClassReference monkClass = ClassTools.ClassReferences.MonkClass;
			PatchTools.PatchProgressionFeaturesBasedOnReferenceArchetype(IsekaiProtagonistClass.GetReference(), monkClass, BaseArchetype);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.MonkClass.ToReference<BlueprintCharacterClassReference>();
			});
			prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = MonkLegacy.Get().ToReference<BlueprintFeatureReference>();
			});
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "MonkScaledFistLegacy");
		}
	}
}
