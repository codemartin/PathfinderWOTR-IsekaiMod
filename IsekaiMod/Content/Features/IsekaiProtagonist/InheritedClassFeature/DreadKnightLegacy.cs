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
	internal class DreadKnightLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint<BlueprintProgression>(Main.IsekaiContext, "DreadKnightLegacy", delegate
			{
			});
		}

		public static void PatchProgression()
		{
			BlueprintCharacterClass DreadKnight = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("d0eb4ca44e11417c9b2f0208491067a0");
			if (ModSupport.IsExpandedContentEnabled && DreadKnight != null)
			{
				prog.SetName(Main.IsekaiContext, "Dread Knight Legacy - Dread Lord");
				prog.SetDescription(Main.IsekaiContext, "*Blinks and slowly backs away*, Uhm are you certain you want to play this? I mean Dread Knights are kind of evil, you know? \nAs in the they are the opposite of anything a paladin represents...");
				prog.GiveFeaturesForPreviousLevels = true;
				prog.AddComponent(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.Evil;
				});
				prog.AddComponent(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = DreadKnight.ToReference<BlueprintCharacterClassReference>();
				});
				prog = PatchTools.PatchClassProgressionBasedOnRefClass(prog, DreadKnight);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, DreadKnight.ToReference<BlueprintCharacterClassReference>());
				LegacySelection.RegisterForFeat(prog);
				LegacySelection.Register(prog);
				MartialGodLegacySelection.Register(prog);
				GodEmperorLegacySelection.Register(prog);
				HeroLegacySelection.Prohibit(prog);
				MastermindLegacySelection.Prohibit(prog);
				OverlordLegacySelection.Register(prog);
			}
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "DreadKnightLegacy");
		}
	}
}
