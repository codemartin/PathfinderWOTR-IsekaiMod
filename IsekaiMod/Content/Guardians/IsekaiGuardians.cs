using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians
{
	public static class IsekaiGuardians
	{
		private static bool Added = false;

		private static readonly BlueprintFeature AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

		private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		private static readonly BlueprintFaction Neutrals = BlueprintTools.GetBlueprint<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad");

		private static readonly BlueprintUnit AstralDevaBase = BlueprintTools.GetBlueprint<BlueprintUnit>("8f3bd0ecea704277a9f2b09296a7b01e");

		private static readonly BlueprintUnit ThanadaemonBase = BlueprintTools.GetBlueprint<BlueprintUnit>("bc6b0477fba49e54e84e2c7ba3cf2144");

		private static readonly BlueprintUnit SuccubusBase = BlueprintTools.GetBlueprint<BlueprintUnit>("a1fe1265b9baad340b6ba02ab4d95a87") ?? BlueprintTools.GetBlueprint<BlueprintUnit>("2db556136eac2544fa9744314c2a5713");

		private static readonly BlueprintUnit DevourerBase = BlueprintTools.GetBlueprint<BlueprintUnit>("eedec6f78f5a4deb9b58666627faf9de") ?? BlueprintTools.GetBlueprint<BlueprintUnit>("a1acea7cda5e46d8b8d0549be25e0acb") ?? ThanadaemonBase;

		private static readonly BlueprintUnit FaerieDragonBase = BlueprintTools.GetBlueprint<BlueprintUnit>("4f2ed74aea457fb409ba56872dbc5552");

		public static BlueprintFeature GuardianAngelFeature { get; private set; }

		public static BlueprintFeature ShinigamiFeature { get; private set; }

		public static BlueprintFeature LoyalDemonFeature { get; private set; }

		public static BlueprintFeature AstralDevourerFeature { get; private set; }

		public static BlueprintFeature TricksterFriendFeature { get; private set; }

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
			BlueprintUnit original3 = DevourerBase ?? ThanadaemonBase ?? AstralDevaBase;
			BlueprintUnit original4 = FaerieDragonBase ?? AstralDevaBase;
			if (astralDevaBase == null)
			{
				Main.IsekaiContext.Logger.LogError("Guardian templates could not be loaded: AstralDevaBase is null!");
				return;
			}
			BlueprintFeature GuardianSlotFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianSlotFeature", delegate(BlueprintFeature bp)
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
			BlueprintUnit petUnit = astralDevaBase.CreateCopy(Main.IsekaiContext, "GuardianAngelCompanionUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "Guardian Angel");
				bp.ComponentsArray = new BlueprintComponent[0];
				bp.AddComponent(delegate(AddClassLevels c)
				{
					c.m_CharacterClass = GuardianCompanionClass.GetReference();
					c.RaceStat = StatType.Strength;
					c.LevelsStat = StatType.Strength;
					c.Skills = new StatType[0];
					c.DoNotApplyAutomatically = false;
					c.m_MemorizeSpells = new BlueprintAbilityReference[0];
					c.m_SelectSpells = new BlueprintAbilityReference[0];
				});
				bp.AddComponent<AllowDyingCondition>();
				bp.AddComponent<AddResurrectOnRest>();
				bp.AddComponent(delegate(BarkOnClick c)
				{
					c.Bark = Helpers.CreateString(Main.IsekaiContext, "GuardianAngel.ClickBark", "Heaven's aegis shields you, chosen one.");
					c.Cooldown = 3f;
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
				bp.m_AddFacts = new BlueprintUnitFactReference[3]
				{
					GuardianSlotFeature.ToReference<BlueprintUnitFactReference>(),
					GuardianBarks.AngelBarksFeature.ToReference<BlueprintUnitFactReference>(),
					GuardianProgressions.AngelProgression.ToReference<BlueprintUnitFactReference>()
				};
			});
			GuardianAngelFeature = CreateGuardianFeature("GuardianAngelFeature", "★ Guardian Angel (Special Guardian)", "[Isekai Protagonist Exclusive Partner]\nA celestial emissary of the Upper Planes who descended to act as your divine shield and sword.\nWields radiant holy light, sacred strikes, and divine protective auras.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +3 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 18, Dex 16, Con 16, Int 14, Wis 16, Cha 18\nAdvances 1-20 in Guardian Class with custom celestial milestones (Celestial Resilience, Smite Evil, Angelic Wings, Holy Strike, Aura of Menace, Fast Healing, Archangel Ascension).", petUnit, GuardianProgressions.AngelProgression);
			BlueprintUnit petUnit2 = original.CreateCopy(Main.IsekaiContext, "ShinigamiCompanionUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "Shinigami");
				bp.ComponentsArray = new BlueprintComponent[0];
				bp.AddComponent(delegate(AddClassLevels c)
				{
					c.m_CharacterClass = GuardianCompanionClass.GetReference();
					c.RaceStat = StatType.Strength;
					c.LevelsStat = StatType.Strength;
					c.Skills = new StatType[0];
					c.DoNotApplyAutomatically = false;
					c.m_MemorizeSpells = new BlueprintAbilityReference[0];
					c.m_SelectSpells = new BlueprintAbilityReference[0];
				});
				bp.AddComponent<AllowDyingCondition>();
				bp.AddComponent<AddResurrectOnRest>();
				bp.AddComponent(delegate(BarkOnClick c)
				{
					c.Bark = Helpers.CreateString(Main.IsekaiContext, "Shinigami.ClickBark", "The reaper waits for no one... except you, master.");
					c.Cooldown = 3f;
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
				bp.m_AddFacts = new BlueprintUnitFactReference[3]
				{
					GuardianSlotFeature.ToReference<BlueprintUnitFactReference>(),
					GuardianBarks.ShinigamiBarksFeature.ToReference<BlueprintUnitFactReference>(),
					GuardianProgressions.ShinigamiProgression.ToReference<BlueprintUnitFactReference>()
				};
			});
			ShinigamiFeature = CreateGuardianFeature("ShinigamiFeature", "★ Shinigami (Special Guardian)", "[Isekai Protagonist Exclusive Partner]\nAn otherworldly reaper veiled in darkness, wielding a soul-severing scythe to harvest your enemies.\nDraws strength from the deaths of those who fall before you.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +2 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 18, Dex 16, Con 16, Int 14, Wis 16, Cha 16\nAdvances 1-20 in Guardian Class with custom reaper milestones (Reaper's Shroud, Ghost Touch, Soul-Severing Strike, Ethereal Form, Sovereign of the Dead).", petUnit2, GuardianProgressions.ShinigamiProgression);
			BlueprintUnit petUnit3 = original2.CreateCopy(Main.IsekaiContext, "LoyalDemonCompanionUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "Loyal Demon");
				bp.ComponentsArray = new BlueprintComponent[0];
				bp.AddComponent(delegate(AddClassLevels c)
				{
					c.m_CharacterClass = GuardianCompanionClass.GetReference();
					c.RaceStat = StatType.Dexterity;
					c.LevelsStat = StatType.Dexterity;
					c.Skills = new StatType[0];
					c.DoNotApplyAutomatically = false;
					c.m_MemorizeSpells = new BlueprintAbilityReference[0];
					c.m_SelectSpells = new BlueprintAbilityReference[0];
				});
				bp.AddComponent<AllowDyingCondition>();
				bp.AddComponent<AddResurrectOnRest>();
				bp.AddComponent(delegate(BarkOnClick c)
				{
					c.Bark = Helpers.CreateString(Main.IsekaiContext, "LoyalDemon.ClickBark", "Always at your service, my captivating partner~");
					c.Cooldown = 3f;
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
				bp.m_AddFacts = new BlueprintUnitFactReference[3]
				{
					GuardianSlotFeature.ToReference<BlueprintUnitFactReference>(),
					GuardianBarks.DemonBarksFeature.ToReference<BlueprintUnitFactReference>(),
					GuardianProgressions.DemonProgression.ToReference<BlueprintUnitFactReference>()
				};
			});
			LoyalDemonFeature = CreateGuardianFeature("LoyalDemonFeature", "★ Loyal Demon (Special Guardian)", "[Isekai Protagonist Exclusive Partner]\nA transfixed demonic follower wholly devoted to your service.\nProvides devastating archery, bewitching charms, and abyssal agility.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +2 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 14, Dex 20, Con 16, Int 16, Wis 14, Cha 20\nAdvances 1-20 in Guardian Class with custom abyssal milestones (Abyssal Resilience, Bat Wings, Profane Strike, Bewitching Allure, Queen's Devotion).", petUnit3, GuardianProgressions.DemonProgression);
			BlueprintUnit petUnit4 = original3.CreateCopy(Main.IsekaiContext, "AstralDevourerCompanionUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "Astral Devourer");
				bp.ComponentsArray = new BlueprintComponent[0];
				bp.AddComponent(delegate(AddClassLevels c)
				{
					c.m_CharacterClass = GuardianCompanionClass.GetReference();
					c.RaceStat = StatType.Strength;
					c.LevelsStat = StatType.Strength;
					c.Skills = new StatType[0];
					c.DoNotApplyAutomatically = false;
					c.m_MemorizeSpells = new BlueprintAbilityReference[0];
					c.m_SelectSpells = new BlueprintAbilityReference[0];
				});
				bp.AddComponent<AllowDyingCondition>();
				bp.AddComponent<AddResurrectOnRest>();
				bp.AddComponent(delegate(BarkOnClick c)
				{
					c.Bark = Helpers.CreateString(Main.IsekaiContext, "AstralDevourer.ClickBark", "*Chittering planar harmonics resonate in your mind...*");
					c.Cooldown = 3f;
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
				bp.m_AddFacts = new BlueprintUnitFactReference[3]
				{
					GuardianSlotFeature.ToReference<BlueprintUnitFactReference>(),
					GuardianBarks.DevourerBarksFeature.ToReference<BlueprintUnitFactReference>(),
					GuardianProgressions.DevourerProgression.ToReference<BlueprintUnitFactReference>()
				};
			});
			AstralDevourerFeature = CreateGuardianFeature("AstralDevourerFeature", "★ Astral Devourer (Special Guardian)", "[Isekai Protagonist Exclusive Partner]\nA grotesque entity of pure planar hunger trapped in ethereal ribs.\nConsumes life energy and drains the spell essence of surrounding adversaries.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +4 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 20, Dex 14, Con 18, Int 16, Wis 16, Cha 18\nAdvances 1-20 in Guardian Class with custom void milestones (Void Carapace SR 11+lvl, Ethereal Claws, Void Singularity).", petUnit4, GuardianProgressions.DevourerProgression);
			BlueprintUnit petUnit5 = original4.CreateCopy(Main.IsekaiContext, "HavocDragonCompanionUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "Havoc Dragon");
				bp.ComponentsArray = new BlueprintComponent[0];
				bp.AddComponent(delegate(AddClassLevels c)
				{
					c.m_CharacterClass = GuardianCompanionClass.GetReference();
					c.RaceStat = StatType.Dexterity;
					c.LevelsStat = StatType.Dexterity;
					c.Skills = new StatType[0];
					c.DoNotApplyAutomatically = false;
					c.m_MemorizeSpells = new BlueprintAbilityReference[0];
					c.m_SelectSpells = new BlueprintAbilityReference[0];
				});
				bp.AddComponent<AllowDyingCondition>();
				bp.AddComponent<AddResurrectOnRest>();
				bp.AddComponent(delegate(BarkOnClick c)
				{
					c.Bark = Helpers.CreateString(Main.IsekaiContext, "HavocDragon.ClickBark", "Ooh, what kind of trouble are we getting into now?!");
					c.Cooldown = 3f;
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
				bp.m_AddFacts = new BlueprintUnitFactReference[3]
				{
					GuardianSlotFeature.ToReference<BlueprintUnitFactReference>(),
					GuardianBarks.DragonBarksFeature.ToReference<BlueprintUnitFactReference>(),
					GuardianProgressions.DragonProgression.ToReference<BlueprintUnitFactReference>()
				};
			});
			TricksterFriendFeature = CreateGuardianFeature("TricksterFriendFeature", "★ Trickster Friend (Special Guardian)", "[Isekai Protagonist Exclusive Partner]\nA tiny, mischievous floating dragon of vibrant chaos and boundless trickery.\nNote: Far too spirited, compact, and ethereal to ever serve as a mount, this little companion flits freely across the battlefield casting chaotic spells and confounding enemies.\n\n{g|Encyclopedia:Size}Size{/g}: Tiny\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft. (Flying)\n{g|Encyclopedia:Armor_Class}AC{/g}: +4 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 10, Dex 22, Con 16, Int 16, Wis 16, Cha 18\nAdvances 1-20 in Guardian Class with custom trickster milestones (Havoc Drake Essence, Prankster's Slip, Avatar of Pandemonium).", petUnit5, GuardianProgressions.DragonProgression);
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
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = IsekaiPetProgression.GetCompanionProgression().ToReference<BlueprintFeatureReference>();
				});
				if (guardianProgression != null)
				{
					bp.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = guardianProgression.ToReference<BlueprintFeatureReference>();
					});
				}
			});
		}
	}
}
