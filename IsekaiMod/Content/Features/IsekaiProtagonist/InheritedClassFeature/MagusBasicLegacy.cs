using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class MagusBasicLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "MagusBasicLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Magus Legacy - Magus");
				bp.SetDescription(Main.IsekaiContext, "You blend the powers of martial traning and spellcasting into a single style. \nAfter all hundrets of anime/games/books taught you just how dangerous overspecialisation in either art can be.");
				bp.GiveFeaturesForPreviousLevels = true;
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
			if (prog != null)
			{
				prog = PatchTools.PatchClassProgressionBasedOnRefClass(prog, ClassTools.Classes.MagusClass);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.MagusClass);
				prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = ClassTools.Classes.MagusClass.ToReference<BlueprintCharacterClassReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = MagusArcherLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = MagusDancerLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = MagusSpellbladeLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
			}
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "MagusBasicLegacy");
		}

		public static void PatchForBroadStudy()
		{
			if (prog != null)
			{
				prog.AddComponent(delegate(BroadStudyComponent c)
				{
					c.CharacterClass = IsekaiProtagonistClass.GetReference();
				});
			}
			if (MagusArcherLegacy.Get() != null)
			{
				MagusArcherLegacy.Get().AddComponent(delegate(BroadStudyComponent c)
				{
					c.CharacterClass = IsekaiProtagonistClass.GetReference();
				});
			}
			if (MagusDancerLegacy.Get() != null)
			{
				MagusDancerLegacy.Get().AddComponent(delegate(BroadStudyComponent c)
				{
					c.CharacterClass = IsekaiProtagonistClass.GetReference();
				});
			}
			if (MagusSpellbladeLegacy.Get() != null)
			{
				MagusSpellbladeLegacy.Get().AddComponent(delegate(BroadStudyComponent c)
				{
					c.CharacterClass = IsekaiProtagonistClass.GetReference();
				});
			}
		}
	}
}
