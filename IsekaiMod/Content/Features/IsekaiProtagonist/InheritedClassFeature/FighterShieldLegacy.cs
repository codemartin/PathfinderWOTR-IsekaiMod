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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class FighterShieldLegacy
	{
		private static string BaseArchetypeId = "a599da9a8a6b9e54083b0a4d2a25db59";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "FighterShieldLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Fighter Legacy - Guardian Shield");
				bp.SetDescription(Main.IsekaiContext, "You are the shield guarding your party.\n With your trusty shield in hand no enemy can break your defense.\nJust let the fool's try...");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
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
			BlueprintCharacterClassReference fighterClass = ClassTools.ClassReferences.FighterClass;
			BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
			prog = PatchTools.PatchClassProgressionBasedonRefArchetype(prog, ClassTools.Classes.FighterClass, BaseArchetype, null);
			PatchTools.PatchProgressionFeaturesBasedOnReferenceArchetype(reference, fighterClass, BaseArchetype);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.FighterClass.ToReference<BlueprintCharacterClassReference>();
			});
			prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = FighterBasicLegacy.Get().ToReference<BlueprintFeatureReference>();
			});
			prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = Fighter2HandedLegacy.Get().ToReference<BlueprintFeatureReference>();
			});
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "FighterShieldLegacy");
		}
	}
}
