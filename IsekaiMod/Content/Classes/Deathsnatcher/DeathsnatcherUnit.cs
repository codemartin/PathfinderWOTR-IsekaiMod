using System.IO;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Content.Guardians;
using IsekaiMod.Utilities;
using Kingmaker.AI.Blueprints;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.Utility;
using Kingmaker.Visual.HitSystem;
using Kingmaker.Visual.Sound;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Classes.Deathsnatcher
{
	internal class DeathsnatcherUnit
	{
		private static readonly BlueprintFeature AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

		private static readonly BlueprintFaction Neutrals = BlueprintTools.GetBlueprint<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad");

		private static readonly BlueprintFeature MonstrousHumanoidType = BlueprintTools.GetBlueprint<BlueprintFeature>("57614b50e8d86b24395931fffc5e409b");

		private static readonly BlueprintFeature RightAndDoubleHandLocatorFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("b7b0360f2e384e55a6c4505242c843b6");

		private static readonly BlueprintBrain DeathsnatcherBrain = BlueprintTools.GetBlueprint<BlueprintBrain>("39efaf3b8a52dd14f972c6d706249ccf");

		private static readonly BlueprintUnitAsksList Deathsnatcher_Barks = BlueprintTools.GetBlueprint<BlueprintUnitAsksList>("ec6a8faba9332024599becceb1da8a54");

		private static readonly BlueprintItemWeapon WeaponEmptyHand = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("20375b5a0c9243d45966bd72c690ab74");

		private static readonly BlueprintItemWeapon Bite2d6 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("2abc1dc6172759c42971bd04b8c115cb");

		private static readonly BlueprintItemWeapon Claw1d6 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("65eb73689b94d894080d33a768cdf645");

		private static readonly BlueprintItemWeapon Sting1d4 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("df44800dbe7b4ba43ac6e0e435041ed8");

		private static readonly BlueprintFeature Airborne = BlueprintTools.GetBlueprint<BlueprintFeature>("70cffb448c132fa409e49156d013b175");

		public static void Add()
		{
			BlueprintFeature DeathsnatcherSizeBaby = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DeathsnatcherSizeBaby");
			BlueprintFeature DeathsnatcherSlotFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherSlotFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(StaticReferences.Strings.Null);
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.MainHand;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.OffHand;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Boots;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Weapon1;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Weapon2;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Weapon3;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Weapon4;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Weapon5;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Weapon6;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Weapon7;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Weapon8;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Ring2;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Glasses;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Shirt;
				});
				bp.AddComponent(delegate(LockEquipmentSlot c)
				{
					c.m_SlotType = LockEquipmentSlot.SlotType.Gloves;
				});
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
					c.m_SlotType = LockEquipmentSlot.SlotType.Cloak;
				});
				bp.HideInUI = true;
				bp.HideInCharacterSheetAndLevelUp = true;
				bp.HideNotAvailibleInUI = true;
			});
			BlueprintPortrait DeathsnatcherPortrait = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherPortrait", delegate(BlueprintPortrait bp)
			{
				bp.Data = AssetLoaderExtension.LoadPortraitData("Deathsnatcher");
			});
			BlueprintUnit DeathsnatcherUnit = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "Deathsnatcher");
				bp.AddComponent(delegate(AddClassLevels c)
				{
					c.m_CharacterClass = DeathsnatcherClass.GetReference();
					c.RaceStat = StatType.Strength;
					c.LevelsStat = StatType.Strength;
					c.Skills = new StatType[0];
					c.DoNotApplyAutomatically = false;
					c.m_MemorizeSpells = new BlueprintAbilityReference[0];
					c.m_SelectSpells = new BlueprintAbilityReference[0];
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { RightAndDoubleHandLocatorFeature.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent<AllowDyingCondition>();
				bp.AddComponent<AddResurrectOnRest>();
				bp.Gender = Gender.Female;
				bp.Size = Size.Medium;
				bp.Color = new Color(0.15f, 0.15f, 0.15f, 1f);
				bp.Alignment = Alignment.ChaoticEvil;
				bp.m_Portrait = DeathsnatcherPortrait.ToReference<BlueprintPortraitReference>();
				bp.Prefab = new UnitViewLink
				{
					AssetId = "261c55913d512ad4aac907f43915183c"
				};
				bp.Visual = new UnitVisualParams
				{
					BloodType = BloodType.Common,
					FootprintType = FootprintType.Humanoid,
					FootprintScale = 1f,
					ArmorFx = new PrefabLink(),
					BloodPuddleFx = new PrefabLink(),
					DismemberFx = new PrefabLink(),
					RipLimbsApartFx = new PrefabLink(),
					IsNotUseDismember = false,
					m_Barks = Deathsnatcher_Barks.ToReference<BlueprintUnitAsksListReference>(),
					ReachFXThresholdBonus = 0f,
					DefaultArmorSoundType = ArmorSoundType.Flesh,
					FootstepSoundSizeType = FootstepSoundSizeType.BootMedium,
					FootSoundType = FootSoundType.HardPaw,
					FootSoundSize = Size.Medium,
					BodySoundType = BodySoundType.Flesh,
					BodySoundSize = Size.Medium,
					FoleySoundPrefix = null,
					NoFinishingBlow = false,
					ImportanceOverride = 0,
					SilentCaster = true
				};
				bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
				bp.FactionOverrides = new FactionOverrides
				{
					m_AttackFactionsToAdd = new BlueprintFactionReference[0],
					m_AttackFactionsToRemove = new BlueprintFactionReference[0]
				};
				bp.m_Brain = DeathsnatcherBrain.ToReference<BlueprintBrainReference>();
				bp.Body = new BlueprintUnit.UnitBody
				{
					DisableHands = false,
					ActiveHandSet = 0,
					m_EmptyHandWeapon = WeaponEmptyHand.ToReference<BlueprintItemWeaponReference>(),
					m_PrimaryHand = Bite2d6.ToReference<BlueprintItemEquipmentHandReference>(),
					m_SecondaryHand = Sting1d4.ToReference<BlueprintItemEquipmentHandReference>(),
					m_PrimaryHandAlternative1 = Bite2d6.ToReference<BlueprintItemEquipmentHandReference>(),
					m_SecondaryHandAlternative1 = Sting1d4.ToReference<BlueprintItemEquipmentHandReference>(),
					m_PrimaryHandAlternative2 = Bite2d6.ToReference<BlueprintItemEquipmentHandReference>(),
					m_SecondaryHandAlternative2 = Sting1d4.ToReference<BlueprintItemEquipmentHandReference>(),
					m_PrimaryHandAlternative3 = Bite2d6.ToReference<BlueprintItemEquipmentHandReference>(),
					m_SecondaryHandAlternative3 = Sting1d4.ToReference<BlueprintItemEquipmentHandReference>(),
					m_AdditionalLimbs = new BlueprintItemWeaponReference[4]
					{
						Claw1d6.ToReference<BlueprintItemWeaponReference>(),
						Claw1d6.ToReference<BlueprintItemWeaponReference>(),
						Claw1d6.ToReference<BlueprintItemWeaponReference>(),
						Claw1d6.ToReference<BlueprintItemWeaponReference>()
					}
				};
				bp.Strength = 19;
				bp.Dexterity = 19;
				bp.Constitution = 18;
				bp.Intelligence = 14;
				bp.Wisdom = 16;
				bp.Charisma = 18;
				bp.Speed = new Feet(30f);
				bp.Skills = new BlueprintUnit.UnitSkills
				{
					Acrobatics = 0,
					Physique = 0,
					Diplomacy = 0,
					Thievery = 0,
					LoreNature = 0,
					Perception = 0,
					Stealth = 0,
					UseMagicDevice = 0,
					LoreReligion = 0,
					KnowledgeWorld = 0,
					KnowledgeArcana = 0
				};
				bp.MaxHP = 0;
				bp.m_AdditionalTemplates = new BlueprintUnitTemplateReference[0];
				bp.AddComponent(delegate(BarkOnClick c)
				{
					c.Bark = Helpers.CreateString(Main.IsekaiContext, "Deathsnatcher.ClickBark", "More vermin to crush, master?");
					c.Cooldown = 3f;
				});
				bp.m_AddFacts = new BlueprintUnitFactReference[5]
				{
					DeathsnatcherSlotFeature.ToReference<BlueprintUnitFactReference>(),
					MonstrousHumanoidType.ToReference<BlueprintUnitFactReference>(),
					DeathsnatcherSizeBaby.ToReference<BlueprintUnitFactReference>(),
					Airborne.ToReference<BlueprintUnitFactReference>(),
					GuardianBarks.DeathsnatcherBarksFeature.ToReference<BlueprintUnitFactReference>()
				};
				bp.IsCheater = false;
				bp.IsFake = false;
			});
			string text = Path.Combine(Main.IsekaiContext.ModEntry.Path, "Assets", "Portraits", "Deathsnatcher", "PetEye.png");
			if (File.Exists(text) && DeathsnatcherUnit.PortraitSafe?.Data != null)
			{
				Sprite sprite = AssetLoaderExtension.LoadPortrait(text, new Vector2Int(176, 24));
				if (sprite != null)
				{
					EyePortraitInjector.Replacements[DeathsnatcherUnit.PortraitSafe.Data] = sprite;
				}
			}
			IsekaiPetSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "★ Deathsnatcher (Special Guardian)");
				bp.SetDescription(Main.IsekaiContext, "[Isekai Protagonist Exclusive Partner]\nDeathsnatchers dwell amid the ruins of fallen civilizations, where they play at being godlings worshiped by undead slaves. Though self-aggrandizing, deathsnatchers are known to give homage to (and claim descent from) the various demon lords of darkness, the desert, and undeath.\n{g|Encyclopedia:Size}Size{/g}: Tiny (Medium at 4th level)\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +4 natural armor (+8 at 4th level, +12 at 12th level)\n{g|Encyclopedia:Attack}Attack{/g}: 1 bite ({g|Encyclopedia:Dice}2d6{/g}), 4 claws (1d6), 1 sting (1d4)\n{g|Encyclopedia:Ability_Scores}Baby ability scores{/g}: {g|Encyclopedia:Strength}Str{/g} 15, {g|Encyclopedia:Dexterity}Dex{/g} 23, {g|Encyclopedia:Constitution}Con{/g} 16, {g|Encyclopedia:Intelligence}Int{/g} 14, {g|Encyclopedia:Wisdom}Wis{/g} 16, {g|Encyclopedia:Charisma}Cha{/g} 18\nAt 4th level matures to Medium size (Str 19, Dex 19, Con 18), natural armor increases to +8, and gains Pounce.");
				bp.AddComponent(delegate(AddPet c)
				{
					c.Type = PetType.AnimalCompanion;
					c.ProgressionType = PetProgressionType.AnimalCompanion;
					c.m_Pet = DeathsnatcherUnit.ToReference<BlueprintUnitReference>();
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
					c.m_Feature = DeathsnatcherProgression.GetCompanionProgression().ToReference<BlueprintFeatureReference>();
				});
				bp.Groups = new FeatureGroup[1] { FeatureGroup.AnimalCompanion };
				bp.ReapplyOnLevelUp = true;
			}));
		}
	}
}
