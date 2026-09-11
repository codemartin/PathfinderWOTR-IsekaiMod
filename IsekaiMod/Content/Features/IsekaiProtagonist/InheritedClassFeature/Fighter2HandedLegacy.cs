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
	internal class Fighter2HandedLegacy
	{
		private static string BaseArchetypeId = "84643e02a764bff4a9c1aba333a53c89";

		private static BlueprintArchetype BaseArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>(BaseArchetypeId);

		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "Fighter2HandedLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Fighter Legacy - Two Handed Fighter");
				bp.SetDescription(Main.IsekaiContext, "What is cooler than swinging a sword at a speed the eye can barely follow?\nSwinging a massive two handed sword at that speed and watching your enemy as he runs in fear at the sheer size of your weapon and the speed at which you wield it.\nEven better if it is an elven blade or estoc so you can stack sneak attack on top for some truly critical hits...");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
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
				c.m_Feature = FighterShieldLegacy.Get().ToReference<BlueprintFeatureReference>();
			});
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "Fighter2HandedLegacy");
		}
	}
}
