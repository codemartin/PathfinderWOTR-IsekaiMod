using TabletopTweaks.Core.Config;

namespace IsekaiMod.Config
{
	public class AddedContent : IUpdatableSettings
	{
		public bool NewSettingsOffByDefault;

		public bool ExcludeCompanionsFromIsekaiClass = true;

		public bool AllowMainCharacterRetinue;

		public bool RestrictExceptionalFeats;

		public bool RestrictMythicOPAbility;

		public bool RestrictMythicSpecialPower;

		public bool MultipleMythicOPAbility;

		public bool MultipleMythicSpecialPower;

		public bool MergeIsekaiSpellList;

		public bool DisableSpellbookMartialGod;

		public bool DisableSpellbookGodEmperor;

		public bool DisableSpellbookHero;

		public bool DisableSpellbookMastermind;

		public bool DisableSpellbookOverlord;

		public bool DisableSpellbookShadowMonarch;

		public bool EnableLevelUncapping = true;

		public bool EnableCosmicThreatScaling = true;

		public bool EnableRivalReincarnators = true;

		public bool EnableDimensionalReinforcements = true;

		public bool EnableIsekaiEncounterMultiplier;

		public bool EnablePlanarIncursions = true;

		public bool EnableBossPhaseGate = true;

		public int CosmicThreatDifficultyMultiplier = 1;

		public int IsekaiDefaultClothes = 20;

		public int IsekaiSpellsKnownIncrement = 6;

		public SettingGroup Isekai = new SettingGroup();

		public SettingGroup Other = new SettingGroup();

		public void Init()
		{
		}

		public void OverrideSettings(IUpdatableSettings userSettings)
		{
			AddedContent addedContent = userSettings as AddedContent;
			NewSettingsOffByDefault = addedContent.NewSettingsOffByDefault;
			ExcludeCompanionsFromIsekaiClass = addedContent.ExcludeCompanionsFromIsekaiClass;
			AllowMainCharacterRetinue = addedContent.AllowMainCharacterRetinue;
			RestrictExceptionalFeats = addedContent.RestrictExceptionalFeats;
			RestrictMythicOPAbility = addedContent.RestrictMythicOPAbility;
			RestrictMythicSpecialPower = addedContent.RestrictMythicSpecialPower;
			MultipleMythicOPAbility = addedContent.MultipleMythicOPAbility;
			MultipleMythicSpecialPower = addedContent.MultipleMythicSpecialPower;
			IsekaiDefaultClothes = addedContent.IsekaiDefaultClothes;
			IsekaiSpellsKnownIncrement = addedContent.IsekaiSpellsKnownIncrement;
			DisableSpellbookMartialGod = addedContent.DisableSpellbookMartialGod;
			DisableSpellbookGodEmperor = addedContent.DisableSpellbookGodEmperor;
			DisableSpellbookHero = addedContent.DisableSpellbookHero;
			DisableSpellbookMastermind = addedContent.DisableSpellbookMastermind;
			DisableSpellbookOverlord = addedContent.DisableSpellbookOverlord;
			DisableSpellbookShadowMonarch = addedContent.DisableSpellbookShadowMonarch;
			EnableLevelUncapping = addedContent.EnableLevelUncapping;
			EnableCosmicThreatScaling = addedContent.EnableCosmicThreatScaling;
			EnableRivalReincarnators = addedContent.EnableRivalReincarnators;
			EnableDimensionalReinforcements = addedContent.EnableDimensionalReinforcements;
			EnableIsekaiEncounterMultiplier = addedContent.EnableIsekaiEncounterMultiplier;
			EnablePlanarIncursions = addedContent.EnablePlanarIncursions;
			EnableBossPhaseGate = addedContent.EnableBossPhaseGate;
			CosmicThreatDifficultyMultiplier = addedContent.CosmicThreatDifficultyMultiplier;
			MergeIsekaiSpellList = addedContent.MergeIsekaiSpellList;
			Isekai.LoadSettingGroup(addedContent.Isekai, NewSettingsOffByDefault);
			Other.LoadSettingGroup(addedContent.Other, NewSettingsOffByDefault);
		}
	}
}
