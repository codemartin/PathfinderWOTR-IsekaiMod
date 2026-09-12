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
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class InquisitorTacticianLegacy
	{
		private static BlueprintProgression prog;

		private static BlueprintFeatureSelection domains;

		public static void Configure()
		{
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "InquisitorTacticianLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Inquisitor Legacy - Tactician");
				bp.SetDescription(Main.IsekaiContext, "You are used to telling people what to do. \nYou were the tactician who led your guild to first kill Onyxia. \nThis, as far as you are concerned, is just another raid to conquer with your brilliant tactics and perfect preparation... \nUsing your good Judgement you can quickly spot the weaknesses of the opponent and exploit them even as you tactically help your allies to avoid their own.");
				bp.GiveFeaturesForPreviousLevels = true;
			});
			domains = Helpers.CreateBlueprint(Main.IsekaiContext, "InquisitorDomains", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(((BlueprintUnitFact)FeatTools.Selections.DomainsSelection).m_DisplayName);
				bp.SetDescription(((BlueprintUnitFact)FeatTools.Selections.DomainsSelection).m_Description);
				bp.Ranks = 1;
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
				BlueprintFeatureSelection blueprint = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("208662443a1e46d5a97a5e1bff663da1");
				BlueprintFeature blueprint2 = BlueprintTools.GetBlueprint<BlueprintFeature>("dd442ac7be344355887d937fd74e9ff7");
				if (ModSupport.IsTableTopTweakBaseEnabled && blueprint != null && blueprint2 != null)
				{
					array2 = array2.AppendToArray(Helpers.CreateLevelEntry(20, blueprint));
					array = array.AppendToArray(Helpers.CreateLevelEntry(20, blueprint2));
				}
				array2 = array2.AppendToArray(Helpers.CreateLevelEntry(1, FeatTools.Selections.DomainsSelection));
				BlueprintFeature blueprint3 = BlueprintTools.GetBlueprint<BlueprintFeature>("93e78cad499b1b54c859a970cbe4f585");
				BlueprintFeature blueprint4 = BlueprintTools.GetBlueprint<BlueprintFeature>("4ca47c023f1c158428bd55deb44c735f");
				array = array.AppendToArray(Helpers.CreateLevelEntry(2, domains));
				array = array.AppendToArray(Helpers.CreateLevelEntry(3, BlueprintTools.GetBlueprint<BlueprintFeature>("1b9916f7675d6ef4fb427081250d49de"), blueprint3));
				array = array.AppendToArray(Helpers.CreateLevelEntry(5, BlueprintTools.GetBlueprint<BlueprintFeature>("c3abcce19f9f80640a867c9e75f880b2")));
				array = array.AppendToArray(Helpers.CreateLevelEntry(12, blueprint4));
				prog = PatchTools.PatchClassProgressionBasedOnSeparateLists(prog, ClassTools.Classes.InquisitorClass, array, array2);
				BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
				PatchTools.PatchProgressionFeaturesBasedOnReferenceClass(prog, reference, ClassTools.ClassReferences.InquisitorClass);
				prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
				{
					c.m_CharacterClass = ClassTools.Classes.InquisitorClass.ToReference<BlueprintCharacterClassReference>();
				});
				PatchTools.PatchClassIntoFeatureOfReferenceClass(blueprint3, reference, ClassTools.ClassReferences.InquisitorClass);
				PatchTools.PatchClassIntoFeatureOfReferenceClass(blueprint4, reference, ClassTools.ClassReferences.InquisitorClass);
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = InquisitorJudgeLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = InquisitorDomainLordLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = CavalierBasicLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = CavalierStandardBearerLegacy.Get().ToReference<BlueprintFeatureReference>();
				});
			}
		}

		public static void PatchPrerequisiteCompatibility()
		{
			string[] mythicFeatureIds = new string[2]
			{
				"2de64f6a1f2baee4f9b7e52e3f046ec5", // Domain Mastery
				"213a8480d22206b45acbfa0619ca5aaf" // Extra Domain
			};
			foreach (string mythicFeatureId in mythicFeatureIds)
			{
				PrerequisiteAlternatives.Add(BlueprintTools.GetBlueprint<BlueprintFeature>(mythicFeatureId), FeatTools.Selections.DomainsSelection, domains);
			}
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "InquisitorTacticianLegacy");
		}

		public static BlueprintFeatureSelection GetDomains()
		{
			if (domains != null)
			{
				return domains;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "InquisitorDomains");
		}
	}
}
