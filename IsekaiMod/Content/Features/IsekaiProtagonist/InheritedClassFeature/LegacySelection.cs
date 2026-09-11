using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class LegacySelection
	{
		private static readonly Sprite Icon_AllSkilled = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("f3bc6f9c855b2fb4e9aea364b8163aca")).m_Icon;

		private static BlueprintFeatureSelection ClassSelection;

		private static BlueprintFeatureSelection AbilitySelection;

		private static BlueprintProgression[] registered = new BlueprintProgression[0];

		public static void ConfigureStep1()
		{
			if (ClassSelection != null)
			{
				Main.IsekaiContext.Logger.LogWarning("repeated configuration of =LegacyClassSelection");
				return;
			}
			ClassSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "LegacyClassSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Legacy");
				bp.SetDescription(Main.IsekaiContext, "If not for the gods reincarnating you as an Isekai Hero with great mystic power you would have been...");
				((BlueprintUnitFact)bp).m_Icon = Icon_AllSkilled;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = false;
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
				bp.m_Features = new BlueprintFeatureReference[0];
			});
			AbilitySelection = Helpers.CreateBlueprint(Main.IsekaiContext, "LegacyAbilitySelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Dual Legacy");
				bp.SetDescription(Main.IsekaiContext, "First, I joined the bardic college of Wintermoon. \nThen 3 years later, when I got bored with that, I accidentally stumbled into the theives' guild of Whitesnow...\nWhatever the backstory is, you get access to another legacy class feature.");
				((BlueprintUnitFact)bp).m_Icon = Icon_AllSkilled;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = false;
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
				bp.m_Features = new BlueprintFeatureReference[0];
			});
			AbilitySelection.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = AbilitySelection.ToReference<BlueprintFeatureReference>();
			});
			MartialGodLegacySelection.Configure();
			GodEmperorLegacySelection.Configure();
			HeroLegacySelection.Configure();
			MastermindLegacySelection.Configure();
			OverlordLegacySelection.Configure();
			ShadowMonarchLegacySelection.Configure();
			DevourerLegacySelection.Configure();
			TriLegacySelection.Configure();
		}

		public static BlueprintFeatureSelection GetClassFeature()
		{
			if (ClassSelection != null)
			{
				return ClassSelection;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "LegacyClassSelection");
		}

		public static BlueprintFeatureSelection GetOverwhelmingFeature()
		{
			if (AbilitySelection != null)
			{
				return AbilitySelection;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "LegacyAbilitySelection");
		}

		public static void RegisterForFeat(BlueprintProgression prog)
		{
			if (prog != null)
			{
				AbilitySelection?.AddFeatures(prog);
			}
		}

		public static void Register(BlueprintProgression prog)
		{
			if (prog != null)
			{
				registered = registered.AppendToArray(prog);
			}
		}

		public static void ConfigureStep2()
		{
			ArcanistBasicLegacy.Configure();
			ArcanistBrownFurLegacy.Configure();
			ArcanistEldritchFontLegacy.Configure();
			BarbarianLegacy.Configure();
			BardLegacy.Configure();
			BloodragerChimeraLegacy.Configure();
			CavalierBasicLegacy.Configure();
			CavalierKnightOfWall.Configure();
			CavalierStandardBearerLegacy.Configure();
			DreadKnightLegacy.Configure();
			DruidBaseLegacy.Configure();
			FighterBasicLegacy.Configure();
			Fighter2HandedLegacy.Configure();
			FighterShieldLegacy.Configure();
			InquisitorTacticianLegacy.Configure();
			InquisitorJudgeLegacy.Configure();
			InquisitorDomainLordLegacy.Configure();
			KineticLegacy.Configure();
			KineticKnightLegacy.Configure();
			KineticOverwhelmingSoulLegacy.Configure();
			KineticDarkElementalistLegacy.Configure();
			KineticPsychoLegacy.Configure();
			MagusBasicLegacy.Configure();
			MagusArcherLegacy.Configure();
			MagusDancerLegacy.Configure();
			MagusSpellbladeLegacy.Configure();
			MonkLegacy.Configure();
			MonkScaledFistLegacy.Configure();
			OracleLegacy.Configure();
			PaladinBaseLegacy.Configure();
			PlayerComputerNerdLegacy.Configure();
			RogueLegacy.Configure();
			ShamanLegacy.Configure();
			ShifterLegacy.Configure();
			ShifterBaseLegacy.Configure();
			ShifterStingerLegacy.Configure();
			ShifterDragonLegacy.Configure();
			ShifterGriffonLegacy.Configure();
			ShifterHolyLegacy.Configure();
			SkaldBaseLegacy.Configure();
			SkaldSilverTongueLegacy.Configure();
			SkaldVoiceLegacy.Configure();
			SorcererLegacy.Configure();
			WitchBaseLegacy.Configure();
			AlchemistLegacy.Configure();
			ClericLegacy.Configure();
			HunterLegacy.Configure();
			RangerLegacy.Configure();
			SlayerLegacy.Configure();
			WarpriestLegacy.Configure();
			WizardLegacy.Configure();
			ArcanaSelection.Configure();
			OverpoweredAbilitySelection.AddToSelection(GetOverwhelmingFeature());
		}

		public static void ConfigureStep3()
		{
			ArcanistBasicLegacy.PatchProgression();
			ArcanistBrownFurLegacy.PatchProgression();
			ArcanistEldritchFontLegacy.PatchProgression();
			BarbarianLegacy.PatchProgression();
			BardLegacy.PatchProgression();
			BloodragerChimeraLegacy.PatchProgression();
			CavalierBasicLegacy.PatchProgression();
			CavalierKnightOfWall.PatchProgression();
			CavalierStandardBearerLegacy.PatchProgression();
			DreadKnightLegacy.PatchProgression();
			DruidBaseLegacy.PatchProgression();
			FighterBasicLegacy.PatchProgression();
			Fighter2HandedLegacy.PatchProgression();
			FighterShieldLegacy.PatchProgression();
			InquisitorTacticianLegacy.PatchProgression();
			InquisitorJudgeLegacy.PatchProgression();
			InquisitorDomainLordLegacy.PatchProgression();
			KineticLegacy.PatchProgression();
			KineticKnightLegacy.PatchProgression();
			KineticOverwhelmingSoulLegacy.PatchProgression();
			KineticDarkElementalistLegacy.PatchProgression();
			KineticPsychoLegacy.PatchProgression();
			MagusBasicLegacy.PatchProgression();
			MagusArcherLegacy.PatchProgression();
			MagusDancerLegacy.PatchProgression();
			MagusSpellbladeLegacy.PatchProgression();
			MonkLegacy.PatchProgression();
			MonkScaledFistLegacy.PatchProgression();
			OracleLegacy.PatchProgression();
			PaladinBaseLegacy.PatchProgression();
			PlayerComputerNerdLegacy.PatchProgression();
			RogueLegacy.PatchProgression();
			ShamanLegacy.PatchProgression();
			ShifterLegacy.PatchProgression();
			ShifterBaseLegacy.PatchProgression();
			ShifterStingerLegacy.PatchProgression();
			ShifterDragonLegacy.PatchProgression();
			ShifterGriffonLegacy.PatchProgression();
			ShifterHolyLegacy.PatchProgression();
			SkaldBaseLegacy.PatchProgression();
			SkaldSilverTongueLegacy.PatchProgression();
			SkaldVoiceLegacy.PatchProgression();
			SorcererLegacy.PatchProgression();
			WitchBaseLegacy.PatchProgression();
			AlchemistLegacy.PatchProgression();
			ClericLegacy.PatchProgression();
			HunterLegacy.PatchProgression();
			RangerLegacy.PatchProgression();
			SlayerLegacy.PatchProgression();
			WarpriestLegacy.PatchProgression();
			WizardLegacy.PatchProgression();
			Finish();
			MastermindLegacySelection.Finish();
			OverlordLegacySelection.Finish();
			MartialGodLegacySelection.Finish();
			HeroLegacySelection.Finish();
			GodEmperorLegacySelection.Finish();
			ShadowMonarchLegacySelection.Finish();
			DevourerLegacySelection.Finish();
		}

		public static void Finish()
		{
			AbilitySelection.AddPrerequisite(delegate(PrerequisiteClassLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.Level = 5;
			});
			BlueprintProgression[] array = registered;
			foreach (BlueprintFeature blueprintFeature in array)
			{
				ClassSelection.AddFeatures(blueprintFeature);
			}
			TriLegacySelection.Finish();
		}
	}
}
