using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class InquisitorDomainLordLegacy
	{
		private static BlueprintProgression prog;

		private static BlueprintFeatureSelection domains;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "InquisitorDomainLordLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Inquisitor Legacy - Domain Lord");
				bp.SetDescription(Main.IsekaiContext, "Your reincarnation by divine means has strengthened your divine connection above that of normal people. \nThe difference might not be easily visible at the beginning. \nBut as time passes it will slowly grow, granting you access to more and more domains.");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			domains = Helpers.CreateBlueprint(Main.IsekaiContext, "InquisitorAdditionalDomains", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(((BlueprintUnitFact)FeatTools.Selections.DomainsSelection).m_DisplayName);
				bp.SetDescription(((BlueprintUnitFact)FeatTools.Selections.DomainsSelection).m_Description);
				bp.IgnorePrerequisites = true;
				bp.Ranks = 4;
				bp.IsClassFeature = true;
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Prohibit(prog);
			GodEmperorLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Register(prog);
			OverlordLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
			if (prog != null)
			{
				LevelEntry[] array = new LevelEntry[0];
				LevelEntry[] array2 = new LevelEntry[0];
				domains.SetFeatures(FeatTools.Selections.DomainsSelection.m_AllFeatures);
				array2 = array2.AppendToArray(Helpers.CreateLevelEntry(1, FeatTools.Selections.DomainsSelection));
				array = array.AppendToArray(Helpers.CreateLevelEntry(2, InquisitorTacticianLegacy.GetDomains()));
				array = array.AppendToArray(Helpers.CreateLevelEntry(5, domains));
				array = array.AppendToArray(Helpers.CreateLevelEntry(10, domains));
				array = array.AppendToArray(Helpers.CreateLevelEntry(15, domains));
				array = array.AppendToArray(Helpers.CreateLevelEntry(20, domains));
				BlueprintArchetype blueprint = BlueprintTools.GetBlueprint<BlueprintArchetype>("0e5e91c17f114d358910e0da4ae29b50");
				array2 = array2.AppendToArray(blueprint.RemoveFeatures);
				prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.InquisitorClass, array, array2);
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = InquisitorTacticianLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = InquisitorJudgeLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
			}
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.InquisitorClass.ToReference<BlueprintCharacterClassReference>();
			});
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "InquisitorDomainLordLegacy");
		}
	}
}
