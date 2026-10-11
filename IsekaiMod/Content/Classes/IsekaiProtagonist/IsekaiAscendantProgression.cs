using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

internal class IsekaiAscendantProgression
{
	internal const string BlueprintName = "IsekaiAscendantProgression";

	public static void Add()
	{
		BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
		BlueprintFeatureSelection modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SpecialPowerSelection");
		BlueprintFeatureSelection modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelection");
		BlueprintFeatureSelection modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection");
		BlueprintFeatureSelection modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HaxSelection");
		BlueprintFeatureSelection isekaiPrestigeSpellbookSelection = PrestigeClassReplaceSpellbook.GetIsekaiPrestigeSpellbookSelection();
		BlueprintProgression blueprintProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiAscendantProgression", delegate(BlueprintProgression bp)
		{
			bp.SetName(StaticReferences.Strings.Null);
			bp.SetDescription(Main.IsekaiContext, "Past the twentieth level the protagonist's story stops borrowing the world's rules and starts writing them. The powers keep arriving on the same cadence they always did, and the spellbook you already carry keeps growing with them.");
			((BlueprintUnitFact)bp).m_AllowNonContextActions = false;
			bp.IsClassFeature = true;
			bp.m_FeaturesRankIncrease = null;
			bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
			{
				new BlueprintProgression.ClassWithLevel
				{
					m_Class = IsekaiAscendantClass.GetReference(),
					AdditionalLevel = 0
				}
			};
		});
		blueprintProgression.LevelEntries = new LevelEntry[20]
		{
			CreateLevelEntry(1, isekaiPrestigeSpellbookSelection, modBlueprint2),
			CreateLevelEntry(2, modBlueprint),
			CreateLevelEntry(3, modBlueprint2),
			CreateLevelEntry(4, modBlueprint),
			CreateLevelEntry(5, modBlueprint3, modBlueprint2),
			CreateLevelEntry(6, modBlueprint),
			CreateLevelEntry(7, modBlueprint2),
			CreateLevelEntry(8, modBlueprint),
			CreateLevelEntry(9, modBlueprint2),
			CreateLevelEntry(10, modBlueprint, modBlueprint3, modBlueprint4),
			CreateLevelEntry(11, modBlueprint2),
			CreateLevelEntry(12, modBlueprint),
			CreateLevelEntry(13, modBlueprint2),
			CreateLevelEntry(14, modBlueprint),
			CreateLevelEntry(15, modBlueprint3, modBlueprint2),
			CreateLevelEntry(16, modBlueprint),
			CreateLevelEntry(17, modBlueprint2),
			CreateLevelEntry(18, modBlueprint),
			CreateLevelEntry(19, modBlueprint2),
			CreateLevelEntry(20, modBlueprint, modBlueprint3, modBlueprint5)
		};
		blueprintProgression.UIGroups = new UIGroup[2]
		{
			CreateSafeUIGroup(modBlueprint3, modBlueprint2),
			CreateSafeUIGroup(modBlueprint4, modBlueprint5)
		}.Where((UIGroup g) => g != null).ToArray();
		blueprintProgression.m_UIDeterminatorsGroup = (from f in new BlueprintFeatureBase[1] { isekaiPrestigeSpellbookSelection }
			where f != null
			select f.ToReference<BlueprintFeatureBaseReference>()).ToArray();
		IsekaiAscendantClass.SetProgression(blueprintProgression);
	}

	private static LevelEntry CreateLevelEntry(int level, params BlueprintFeatureBase[] features)
	{
		RegressionSanityCheck.ReportMissingLevelFeatures("IsekaiAscendantProgression", level, features);
		List<BlueprintFeatureBaseReference> features2 = (from f in features
			where f != null
			select f.ToReference<BlueprintFeatureBaseReference>()).ToList();
		return new LevelEntry
		{
			Level = level,
			m_Features = features2
		};
	}

	private static UIGroup CreateSafeUIGroup(params BlueprintFeatureBase[] features)
	{
		List<BlueprintFeatureBaseReference> list = (from f in features
			where f != null
			select f.ToReference<BlueprintFeatureBaseReference>()).ToList();
		if (list.Count == 0)
		{
			return null;
		}
		return new UIGroup
		{
			m_Features = list
		};
	}
}
