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
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class OracleLegacy
	{
		private static BlueprintProgression prog;

		public static void Configure()
		{
			ExtraOracleSelection.Configure();
			BlueprintFeatureSelection OracleSelection = ExtraOracleSelection.Get();
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "OracleLegacyProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Oracle Legacy - Seeker of Truth");
				bp.SetDescription(Main.IsekaiContext, "Seekers of Truth are driven by a desire to uncover the secrets behind the fundamental forces of nature. \nBecause of their unique perspective as otherworlders, they are able to approach the world with a fresh and unbiased eye, allowing them to see beyond the surface of things and seek out the deeper truth behind the world around them. \nThey are driven by a desire to uncover the secrets of the world that would otherwise remain hidden, and are not satisfied with simply accepting things at face value. \nThis allows them to uncover the secrets of the world that are often only revealed to mortals through revelations.\n\nPlease note that you do not get previous inheritances when seletcing this at higher levels.");
				bp.GiveFeaturesForPreviousLevels = false;
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = IsekaiProtagonistClass.GetReference(),
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[7]
				{
					Helpers.CreateLevelEntry(1, OracleSelection),
					Helpers.CreateLevelEntry(3, OracleSelection),
					Helpers.CreateLevelEntry(6, OracleSelection),
					Helpers.CreateLevelEntry(9, OracleSelection),
					Helpers.CreateLevelEntry(12, OracleSelection),
					Helpers.CreateLevelEntry(15, OracleSelection),
					Helpers.CreateLevelEntry(18, OracleSelection)
				};
				bp.UIGroups = new UIGroup[1] { Helpers.CreateUIGroup(OracleSelection) };
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			GodEmperorLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Register(prog);
			OverlordLegacySelection.Prohibit(prog);
		}

		public static void PatchProgression()
		{
			BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
			BlueprintCharacterClassReference referenceClass = ClassTools.Classes.OracleClass.ToReference<BlueprintCharacterClassReference>();
			PatchTools.PatchClassIntoFeatureOfReferenceClass(FeatTools.Selections.OracleCurseSelection, reference, referenceClass);
			PatchTools.PatchClassIntoFeatureOfReferenceClass(FeatTools.Selections.OracleMysterySelection, reference, referenceClass);
			PatchTools.PatchClassIntoFeatureOfReferenceClass(FeatTools.Selections.OracleRevelationSelection, reference, referenceClass);
			PatchTools.PatchClassIntoFeatureOfReferenceClass(FeatTools.Selections.OracleCureOrInflictSelection, reference, referenceClass);
			// Beneficial Curse's no-penalty curse progressions advance on Oracle levels; let them advance on Isekai levels too.
			PatchTools.PatchClassIntoFeatureOfReferenceClass(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("2dda67424ee8e0b4d83ef01a73ca6bff"), reference, referenceClass);
			prog.AddComponent(delegate(ClassLevelsForPrerequisites c)
			{
				c.m_FakeClass = ClassTools.Classes.OracleClass.ToReference<BlueprintCharacterClassReference>();
				c.m_ActualClass = IsekaiProtagonistClass.GetReference();
				c.Modifier = 1.0;
			});
			PatchTools.PatchResource(BlueprintTools.GetBlueprint<BlueprintAbilityResource>("5f624fa5d4cd4882b9368e4d123306bd"), reference);
			prog.AddPrerequisite(delegate(PrerequisiteNoClassLevel c)
			{
				c.m_CharacterClass = ClassTools.Classes.OracleClass.ToReference<BlueprintCharacterClassReference>();
			});
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "OracleLegacyProgression");
		}
	}
}
