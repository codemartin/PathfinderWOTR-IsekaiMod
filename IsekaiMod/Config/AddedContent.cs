using TabletopTweaks.Core.Config;

namespace IsekaiMod.Config;

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

	public bool DisableSpellbookPhantomBlade;

	public bool DisableSpellbookGodEmperor;

	public bool DisableSpellbookHero;

	public bool DisableSpellbookMastermind;

	public bool DisableSpellbookOverlord;

	public bool DisableSpellbookShadowMonarch;

	public bool EnableLevelUncapping = true;

	public bool EnableCosmicThreatScaling = true;

	public bool EnableRampantCheating = true;

	public bool EnableRivalReincarnators = true;

	public bool EnableDimensionalReinforcements = true;

	public int CosmicThreatDifficultyMultiplier = 1;

	public bool EnableIsekaiEncounterMultiplier;

	public bool EnableConstellations = true;

	public bool EnableConstellationChatOverlay = true;

	public bool EnableConstellationCombatReactions = true;

	public int ConstellationChatDurationSeconds = 14;

	public int ConstellationChatPosition;

	public int ConstellationChatMaxMessages = 5;

	public int ConstellationChatArrivalTenths = 6;

	public bool EnableDimensionalArena = true;

	public bool IOwnKingmaker;

	public int IsekaiDefaultClothes = 21;

	public int IsekaiSpellsKnownIncrement = 2;

	public SettingGroup Isekai = new SettingGroup();

	public SettingGroup Other = new SettingGroup();

	internal const int CurrentSettingsVersion = 1;

	internal const int DefaultSpellsKnownIncrement = 2;

	internal const int LegacySpellsKnownIncrementDefault = 6;

	internal const int DefaultClothesIndex = 21;

	internal const int LegacyDefaultClothesIndex = 20;

	public int SettingsVersion { get; set; }

	internal static int MigrateSpellsKnownIncrement(int stored, int fileVersion)
	{
		if (fileVersion >= 1 || stored != 6)
		{
			return stored;
		}
		return 2;
	}

	internal static int MigrateDefaultClothes(int stored, int fileVersion)
	{
		if (fileVersion >= 1 || stored != 20)
		{
			return stored;
		}
		return 21;
	}

	public void Init()
	{
		SettingsVersion = 1;
	}

	public void OverrideSettings(IUpdatableSettings userSettings)
	{
		AddedContent addedContent = userSettings as AddedContent;
		int settingsVersion = addedContent.SettingsVersion;
		NewSettingsOffByDefault = addedContent.NewSettingsOffByDefault;
		ExcludeCompanionsFromIsekaiClass = addedContent.ExcludeCompanionsFromIsekaiClass;
		AllowMainCharacterRetinue = addedContent.AllowMainCharacterRetinue;
		RestrictExceptionalFeats = addedContent.RestrictExceptionalFeats;
		RestrictMythicOPAbility = addedContent.RestrictMythicOPAbility;
		RestrictMythicSpecialPower = addedContent.RestrictMythicSpecialPower;
		MultipleMythicOPAbility = addedContent.MultipleMythicOPAbility;
		MultipleMythicSpecialPower = addedContent.MultipleMythicSpecialPower;
		IsekaiDefaultClothes = MigrateDefaultClothes(addedContent.IsekaiDefaultClothes, settingsVersion);
		IsekaiSpellsKnownIncrement = MigrateSpellsKnownIncrement(addedContent.IsekaiSpellsKnownIncrement, settingsVersion);
		DisableSpellbookPhantomBlade = addedContent.DisableSpellbookPhantomBlade;
		DisableSpellbookGodEmperor = addedContent.DisableSpellbookGodEmperor;
		DisableSpellbookHero = addedContent.DisableSpellbookHero;
		DisableSpellbookMastermind = addedContent.DisableSpellbookMastermind;
		DisableSpellbookOverlord = addedContent.DisableSpellbookOverlord;
		DisableSpellbookShadowMonarch = addedContent.DisableSpellbookShadowMonarch;
		EnableLevelUncapping = addedContent.EnableLevelUncapping;
		EnableCosmicThreatScaling = addedContent.EnableCosmicThreatScaling;
		EnableRampantCheating = addedContent.EnableRampantCheating;
		EnableRivalReincarnators = addedContent.EnableRivalReincarnators;
		EnableDimensionalReinforcements = addedContent.EnableDimensionalReinforcements;
		CosmicThreatDifficultyMultiplier = addedContent.CosmicThreatDifficultyMultiplier;
		EnableIsekaiEncounterMultiplier = addedContent.EnableIsekaiEncounterMultiplier;
		EnableConstellations = addedContent.EnableConstellations;
		EnableConstellationChatOverlay = addedContent.EnableConstellationChatOverlay;
		EnableConstellationCombatReactions = addedContent.EnableConstellationCombatReactions;
		ConstellationChatDurationSeconds = addedContent.ConstellationChatDurationSeconds;
		ConstellationChatPosition = addedContent.ConstellationChatPosition;
		ConstellationChatMaxMessages = addedContent.ConstellationChatMaxMessages;
		ConstellationChatArrivalTenths = addedContent.ConstellationChatArrivalTenths;
		EnableDimensionalArena = addedContent.EnableDimensionalArena;
		IOwnKingmaker = addedContent.IOwnKingmaker;
		MergeIsekaiSpellList = addedContent.MergeIsekaiSpellList;
		Isekai.LoadSettingGroup(addedContent.Isekai, NewSettingsOffByDefault);
		Other.LoadSettingGroup(addedContent.Other, NewSettingsOffByDefault);
		SettingsVersion = ((settingsVersion <= 1) ? 1 : settingsVersion);
	}
}
