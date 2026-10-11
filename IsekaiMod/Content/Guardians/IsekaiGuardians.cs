using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians;

public static class IsekaiGuardians
{
	private static bool Added = false;

	private static readonly BlueprintFeature AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

	private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

	private static readonly BlueprintFaction Neutrals = BlueprintTools.GetBlueprint<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad");

	private static readonly BlueprintUnit AstralDevaBase = BlueprintTools.GetBlueprint<BlueprintUnit>("8f3bd0ecea704277a9f2b09296a7b01e");

	private static readonly BlueprintUnit ThanadaemonBase = BlueprintTools.GetBlueprint<BlueprintUnit>("bc6b0477fba49e54e84e2c7ba3cf2144");

	private static readonly BlueprintUnit SuccubusBase = BlueprintTools.GetBlueprint<BlueprintUnit>("a1fe1265b9baad340b6ba02ab4d95a87") ?? BlueprintTools.GetBlueprint<BlueprintUnit>("2db556136eac2544fa9744314c2a5713");

	private static readonly BlueprintUnit Dlc4DevourerBase = BlueprintTools.GetBlueprint<BlueprintUnit>("eedec6f78f5a4deb9b58666627faf9de");

	private static readonly BlueprintUnit FaerieDragonBase = BlueprintTools.GetBlueprint<BlueprintUnit>("4f2ed74aea457fb409ba56872dbc5552");

	private static readonly BlueprintItemWeapon ScythePlus2 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("a7dfd7a5e59af0b4bbba8d4d24d0bfc9");

	private static readonly BlueprintItemWeapon Claw1d8 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("13a4ac62fe603fc4c99f9ed5e5d0b9d6");

	private static readonly BlueprintFeature Airborne = BlueprintTools.GetBlueprint<BlueprintFeature>("70cffb448c132fa409e49156d013b175");

	internal static readonly BlueprintItemWeapon WeaponEmptyHand = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("20375b5a0c9243d45966bd72c690ab74");

	public static BlueprintFeature GuardianAngelFeature { get; private set; }

	public static BlueprintFeature ShinigamiFeature { get; private set; }

	public static BlueprintFeature LoyalDemonFeature { get; private set; }

	public static BlueprintFeature AstralDevourerFeature { get; private set; }

	public static BlueprintFeature TricksterFriendFeature { get; private set; }

	internal static void EnsureEmptyHand(BlueprintUnit bp)
	{
		if (bp?.Body != null && !bp.Body.DisableHands && WeaponEmptyHand != null && (bp.Body.m_EmptyHandWeapon == null || !(((BlueprintReferenceBase)bp.Body.m_EmptyHandWeapon).deserializedGuid != BlueprintGuid.Empty)))
		{
			bp.Body.m_EmptyHandWeapon = WeaponEmptyHand.ToReference<BlueprintItemWeaponReference>();
		}
	}

	public static void Add()
	{
		if (Added)
		{
			return;
		}
		Added = true;
		BlueprintUnit astralDevaBase = AstralDevaBase;
		BlueprintUnit original = ThanadaemonBase ?? AstralDevaBase;
		BlueprintUnit original2 = SuccubusBase ?? ThanadaemonBase ?? AstralDevaBase;
		BlueprintUnit devourerBase = IsekaiDlcManager.PreferOwned(IsekaiDlcManager.HasDlc4(), Dlc4DevourerBase, ThanadaemonBase ?? AstralDevaBase);
		BlueprintUnit original3 = FaerieDragonBase ?? AstralDevaBase;
		Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianSlotFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(StaticReferences.Strings.Null);
			bp.AddComponent(delegate(LockEquipmentSlot c)
			{
				c.m_SlotType = LockEquipmentSlot.SlotType.Armor;
			});
			bp.AddComponent(delegate(LockEquipmentSlot c)
			{
				c.m_SlotType = LockEquipmentSlot.SlotType.Headgear;
			});
			bp.AddComponent(delegate(LockEquipmentSlot c)
			{
				c.m_SlotType = LockEquipmentSlot.SlotType.Gloves;
			});
			bp.AddComponent(delegate(LockEquipmentSlot c)
			{
				c.m_SlotType = LockEquipmentSlot.SlotType.Boots;
			});
			bp.AddComponent(delegate(LockEquipmentSlot c)
			{
				c.m_SlotType = LockEquipmentSlot.SlotType.Shirt;
			});
			bp.AddComponent(delegate(LockEquipmentSlot c)
			{
				c.m_SlotType = LockEquipmentSlot.SlotType.Glasses;
			});
			bp.HideInUI = true;
			bp.HideInCharacterSheetAndLevelUp = true;
			bp.HideNotAvailibleInUI = true;
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianNaturalHandSlotFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(StaticReferences.Strings.Null);
			LockEquipmentSlot.SlotType[] naturalHandSlots = GuardianRoles.NaturalHandSlots;
			foreach (LockEquipmentSlot.SlotType slot in naturalHandSlots)
			{
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = slot;
				});
			}
			bp.HideInUI = true;
			bp.HideInCharacterSheetAndLevelUp = true;
			bp.HideNotAvailibleInUI = true;
		});
		BlueprintFeature angelArmor = GuardianCompanionClass.CreateNaturalArmor("GuardianAngelNaturalArmor", "Celestial Plumage", 3);
		BlueprintFeature reaperArmor = GuardianCompanionClass.CreateNaturalArmor("ShinigamiNaturalArmor", "Reaper's Hide", 2);
		BlueprintFeature demonArmor = GuardianCompanionClass.CreateNaturalArmor("LoyalDemonNaturalArmor", "Abyssal Hide", 2);
		BlueprintFeature devourerArmor = GuardianCompanionClass.CreateNaturalArmor("AstralDevourerNaturalArmor", "Void Carapace Plating", 4);
		BlueprintFeature dragonArmor = GuardianCompanionClass.CreateNaturalArmor("HavocDragonNaturalArmor", "Iridescent Scales", 4);
		BlueprintUnit blueprintUnit = astralDevaBase.CreateCopy(Main.IsekaiContext, "GuardianAngelCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Guardian Angel");
			bp.ComponentsArray = new BlueprintComponent[0];
			bp.AddComponent(delegate(AddClassLevels c)
			{
				c.m_CharacterClass = GuardianCompanionClass.GetReference();
				c.RaceStat = StatType.Strength;
				c.LevelsStat = StatType.Strength;
				c.Skills = new StatType[2]
				{
					StatType.SkillPerception,
					StatType.SkillMobility
				};
				c.DoNotApplyAutomatically = false;
				c.m_MemorizeSpells = new BlueprintAbilityReference[0];
				c.m_SelectSpells = new BlueprintAbilityReference[0];
			});
			bp.AddComponent<AllowDyingCondition>();
			bp.AddComponent<AddResurrectOnRest>();
			bp.AddComponent(delegate(BarkOnClick c)
			{
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "GuardianAngel.ClickBark", "Heaven's aegis shields you, chosen one.");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.LawfulGood;
			bp.Strength = 18;
			bp.Dexterity = 16;
			bp.Constitution = 16;
			bp.Intelligence = 14;
			bp.Wisdom = 16;
			bp.Charisma = 18;
			bp.Speed = new Feet(30f);
			bp.MaxHP = 0;
			bp.Size = Size.Medium;
			EnsureEmptyHand(bp);
			bp.m_AddFacts = new BlueprintUnitFactReference[3]
			{
				GuardianBarks.AngelBarksFeature.ToReference<BlueprintUnitFactReference>(),
				GuardianProgressions.AngelProgression.ToReference<BlueprintUnitFactReference>(),
				angelArmor.ToReference<BlueprintUnitFactReference>()
			};
		});
		GuardianAngelFeature = CreateGuardianFeature("GuardianAngelFeature", "★ Guardian Angel (Special Guardian)", "[Isekai Protagonist Exclusive Partner]\nA celestial emissary of the Upper Planes who descended to act as your divine shield and sword.\nWields radiant holy light, sacred strikes, and divine protective auras.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +3 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 18, Dex 16, Con 16, Int 14, Wis 16, Cha 18\nAdvances 1-20 in Guardian Class with custom celestial milestones (Celestial Resilience, Smite Evil, Angelic Wings, Holy Strike, Aura of Menace, Fast Healing, Archangel Ascension).", blueprintUnit, GuardianProgressions.AngelProgression);
		BlueprintUnit blueprintUnit2 = original.CreateCopy(Main.IsekaiContext, "ShinigamiCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Shinigami");
			bp.ComponentsArray = new BlueprintComponent[0];
			bp.AddComponent(delegate(AddClassLevels c)
			{
				c.m_CharacterClass = GuardianCompanionClass.GetReference();
				c.RaceStat = StatType.Strength;
				c.LevelsStat = StatType.Strength;
				c.Skills = new StatType[2]
				{
					StatType.SkillPerception,
					StatType.SkillMobility
				};
				c.DoNotApplyAutomatically = false;
				c.m_MemorizeSpells = new BlueprintAbilityReference[0];
				c.m_SelectSpells = new BlueprintAbilityReference[0];
			});
			bp.AddComponent<AllowDyingCondition>();
			bp.AddComponent<AddResurrectOnRest>();
			bp.AddComponent(delegate(BarkOnClick c)
			{
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "Shinigami.ClickBark", "The reaper waits for no one... except you, master.");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.TrueNeutral;
			bp.Strength = 18;
			bp.Dexterity = 16;
			bp.Constitution = 16;
			bp.Intelligence = 14;
			bp.Wisdom = 16;
			bp.Charisma = 16;
			bp.Speed = new Feet(30f);
			bp.MaxHP = 0;
			bp.Size = Size.Medium;
			if (ScythePlus2 != null)
			{
				bp.Body.m_PrimaryHand = ScythePlus2.ToReference<BlueprintItemEquipmentHandReference>();
			}
			bp.m_AddFacts = new BlueprintUnitFactReference[3]
			{
				GuardianBarks.ShinigamiBarksFeature.ToReference<BlueprintUnitFactReference>(),
				GuardianProgressions.ShinigamiProgression.ToReference<BlueprintUnitFactReference>(),
				reaperArmor.ToReference<BlueprintUnitFactReference>()
			};
		});
		ShinigamiFeature = CreateGuardianFeature("ShinigamiFeature", "★ Shinigami (Special Guardian)", "[Isekai Protagonist Exclusive Partner]\nAn otherworldly reaper veiled in darkness, wielding a soul-severing scythe to harvest your enemies.\nDraws strength from the deaths of those who fall before you.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +2 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 18, Dex 16, Con 16, Int 14, Wis 16, Cha 16\nAdvances 1-20 in Guardian Class with custom reaper milestones (Reaper's Shroud, Ghost Touch, Soul-Severing Strike, Ethereal Form, Sovereign of the Dead).", blueprintUnit2, GuardianProgressions.ShinigamiProgression);
		BlueprintUnit blueprintUnit3 = original2.CreateCopy(Main.IsekaiContext, "LoyalDemonCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Loyal Demon");
			bp.ComponentsArray = new BlueprintComponent[0];
			bp.AddComponent(delegate(AddClassLevels c)
			{
				c.m_CharacterClass = GuardianCompanionClass.GetReference();
				c.RaceStat = StatType.Dexterity;
				c.LevelsStat = StatType.Dexterity;
				c.Skills = new StatType[2]
				{
					StatType.SkillPerception,
					StatType.SkillMobility
				};
				c.DoNotApplyAutomatically = false;
				c.m_MemorizeSpells = new BlueprintAbilityReference[0];
				c.m_SelectSpells = new BlueprintAbilityReference[0];
			});
			bp.AddComponent<AllowDyingCondition>();
			bp.AddComponent<AddResurrectOnRest>();
			bp.AddComponent(delegate(BarkOnClick c)
			{
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "LoyalDemon.ClickBark", "Always at your service, my captivating partner~");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.ChaoticNeutral;
			bp.Strength = 14;
			bp.Dexterity = 20;
			bp.Constitution = 16;
			bp.Intelligence = 16;
			bp.Wisdom = 14;
			bp.Charisma = 20;
			bp.Speed = new Feet(30f);
			bp.MaxHP = 0;
			bp.Size = Size.Medium;
			CompanionDollRace.EnsureRace(bp, "980bed7f6d9e4c33a86b95d199eb4934");
			EnsureEmptyHand(bp);
			bp.m_AddFacts = new BlueprintUnitFactReference[3]
			{
				GuardianBarks.DemonBarksFeature.ToReference<BlueprintUnitFactReference>(),
				GuardianProgressions.DemonProgression.ToReference<BlueprintUnitFactReference>(),
				demonArmor.ToReference<BlueprintUnitFactReference>()
			};
		});
		LoyalDemonFeature = CreateGuardianFeature("LoyalDemonFeature", "★ Loyal Demon (Special Guardian)", "[Isekai Protagonist Exclusive Partner]\nA transfixed demonic follower wholly devoted to your service.\nProvides devastating archery, bewitching charms, and abyssal agility.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +2 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 14, Dex 20, Con 16, Int 16, Wis 14, Cha 20\nAdvances 1-20 in Guardian Class with custom abyssal milestones (Abyssal Resilience, Bat Wings, Profane Strike, Bewitching Allure, Queen's Devotion).", blueprintUnit3, GuardianProgressions.DemonProgression);
		BlueprintUnit blueprintUnit4 = devourerBase.CreateCopy(Main.IsekaiContext, "AstralDevourerCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Astral Devourer");
			bp.ComponentsArray = new BlueprintComponent[0];
			bp.AddComponent(delegate(AddClassLevels c)
			{
				c.m_CharacterClass = GuardianCompanionClass.GetReference();
				c.RaceStat = StatType.Strength;
				c.LevelsStat = StatType.Strength;
				c.Skills = new StatType[2]
				{
					StatType.SkillPerception,
					StatType.SkillMobility
				};
				c.DoNotApplyAutomatically = false;
				c.m_MemorizeSpells = new BlueprintAbilityReference[0];
				c.m_SelectSpells = new BlueprintAbilityReference[0];
			});
			bp.AddComponent<AllowDyingCondition>();
			bp.AddComponent<AddResurrectOnRest>();
			bp.AddComponent(delegate(BarkOnClick c)
			{
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "AstralDevourer.ClickBark", "*Chittering planar harmonics resonate in your mind...*");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.NeutralEvil;
			bp.Strength = 20;
			bp.Dexterity = 14;
			bp.Constitution = 18;
			bp.Intelligence = 16;
			bp.Wisdom = 16;
			bp.Charisma = 18;
			bp.Speed = new Feet(30f);
			bp.MaxHP = 0;
			bp.Size = Size.Medium;
			if (devourerBase != Dlc4DevourerBase && Claw1d8 != null)
			{
				bp.Body.DisableHands = true;
				bp.Body.m_PrimaryHand = null;
				bp.Body.m_SecondaryHand = null;
				bp.Body.m_AdditionalLimbs = new BlueprintItemWeaponReference[2]
				{
					Claw1d8.ToReference<BlueprintItemWeaponReference>(),
					Claw1d8.ToReference<BlueprintItemWeaponReference>()
				};
			}
			bp.m_AddFacts = new BlueprintUnitFactReference[3]
			{
				GuardianBarks.DevourerBarksFeature.ToReference<BlueprintUnitFactReference>(),
				GuardianProgressions.DevourerProgression.ToReference<BlueprintUnitFactReference>(),
				devourerArmor.ToReference<BlueprintUnitFactReference>()
			};
		});
		AstralDevourerFeature = CreateGuardianFeature("AstralDevourerFeature", "★ Astral Devourer (Special Guardian)", "[Isekai Protagonist Exclusive Partner]\nA grotesque entity of pure planar hunger trapped in ethereal ribs.\nConsumes life energy and drains the spell essence of surrounding adversaries.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +4 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 20, Dex 14, Con 18, Int 16, Wis 16, Cha 18\nAdvances 1-20 in Guardian Class with custom void milestones (Void Carapace SR 11+lvl, Ethereal Claws, Void Singularity).", blueprintUnit4, GuardianProgressions.DevourerProgression);
		BlueprintUnit blueprintUnit5 = original3.CreateCopy(Main.IsekaiContext, "HavocDragonCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Havoc Dragon");
			bp.ComponentsArray = new BlueprintComponent[0];
			bp.AddComponent(delegate(AddClassLevels c)
			{
				c.m_CharacterClass = GuardianCompanionClass.GetReference();
				c.RaceStat = StatType.Dexterity;
				c.LevelsStat = StatType.Dexterity;
				c.Skills = new StatType[2]
				{
					StatType.SkillPerception,
					StatType.SkillMobility
				};
				c.DoNotApplyAutomatically = false;
				c.m_MemorizeSpells = new BlueprintAbilityReference[0];
				c.m_SelectSpells = new BlueprintAbilityReference[0];
			});
			bp.AddComponent<AllowDyingCondition>();
			bp.AddComponent<AddResurrectOnRest>();
			bp.AddComponent(delegate(BarkOnClick c)
			{
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "HavocDragon.ClickBark", "Ooh, what kind of trouble are we getting into now?!");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.ChaoticNeutral;
			bp.Size = Size.Tiny;
			bp.Strength = 10;
			bp.Dexterity = 22;
			bp.Constitution = 16;
			bp.Intelligence = 16;
			bp.Wisdom = 16;
			bp.Charisma = 18;
			bp.Speed = new Feet(30f);
			bp.MaxHP = 0;
			bp.m_AddFacts = new BlueprintUnitFactReference[4]
			{
				GuardianBarks.DragonBarksFeature.ToReference<BlueprintUnitFactReference>(),
				GuardianProgressions.DragonProgression.ToReference<BlueprintUnitFactReference>(),
				dragonArmor.ToReference<BlueprintUnitFactReference>(),
				Airborne.ToReference<BlueprintUnitFactReference>()
			};
		});
		TricksterFriendFeature = CreateGuardianFeature("TricksterFriendFeature", "★ Trickster Friend (Special Guardian)", "[Isekai Protagonist Exclusive Partner]\nA tiny, mischievous floating dragon of vibrant chaos and boundless trickery.\nNote: Far too spirited, compact, and ethereal to ever serve as a mount, this little companion flits freely across the battlefield casting chaotic spells and confounding enemies.\n\n{g|Encyclopedia:Size}Size{/g}: Tiny\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft. (Flying)\n{g|Encyclopedia:Armor_Class}AC{/g}: +4 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 10, Dex 22, Con 16, Int 16, Wis 16, Cha 18\nAdvances 1-20 in Guardian Class with custom trickster milestones (Havoc Drake Essence, Prismatic Havoc Breath, Prankster's Slip, Confounding Prank, Avatar of Pandemonium).", blueprintUnit5, GuardianProgressions.DragonProgression);
		GuardianVisualScale.Register(blueprintUnit, blueprintUnit2, blueprintUnit3, blueprintUnit4, blueprintUnit5);
		BorrowedModelFixes.RegisterUnits(blueprintUnit, blueprintUnit2, blueprintUnit3, blueprintUnit4, blueprintUnit5);
	}

	private static BlueprintFeature CreateGuardianFeature(string name, string displayName, string description, BlueprintUnit petUnit, BlueprintProgression guardianProgression)
	{
		if (petUnit == null)
		{
			return null;
		}
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.ReapplyOnLevelUp = true;
			bp.Groups = new FeatureGroup[1] { FeatureGroup.AnimalCompanion };
			GuardianPetLevel.Register(bp);
			bp.AddComponent(delegate(AddPet c)
			{
				c.Type = PetType.AnimalCompanion;
				c.ProgressionType = PetProgressionType.AnimalCompanion;
				c.m_Pet = petUnit.ToReference<BlueprintUnitReference>();
				c.m_LevelRank = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
			});
			bp.AddComponent(delegate(PrerequisitePet c)
			{
				c.NoCompanion = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[2]
				{
					AnimalCompanionRank.ToReference<BlueprintUnitFactReference>(),
					IsekaiPetProgression.GetCompanionProgression().ToReference<BlueprintUnitFactReference>()
				};
			});
			bp.AddComponent(delegate(UpdateProgressionOnApply c)
			{
				c.m_Feature = IsekaiPetProgression.GetCompanionProgression().ToReference<BlueprintFeatureReference>();
			});
		});
	}
}
