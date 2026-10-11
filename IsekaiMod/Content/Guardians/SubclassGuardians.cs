using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians;

public static class SubclassGuardians
{
	private static bool Added = false;

	private static readonly BlueprintFeature AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

	private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

	private static readonly BlueprintFaction Neutrals = BlueprintTools.GetBlueprint<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad");

	private static readonly BlueprintUnit AstralDevaBase = BlueprintTools.GetBlueprint<BlueprintUnit>("8f3bd0ecea704277a9f2b09296a7b01e");

	private static readonly BlueprintUnit ThanadaemonBase = BlueprintTools.GetBlueprint<BlueprintUnit>("bc6b0477fba49e54e84e2c7ba3cf2144");

	private static readonly BlueprintUnit GhostFighterBase = BlueprintTools.GetBlueprint<BlueprintUnit>("b95b40741207418294a72bdfc409c9c1") ?? ThanadaemonBase ?? AstralDevaBase;

	private static readonly BlueprintUnit ErinyesBase = BlueprintTools.GetBlueprint<BlueprintUnit>("b576f3eb0aa94af44a985f51eda9db7b") ?? BlueprintTools.GetBlueprint<BlueprintUnit>("2db556136eac2544fa9744314c2a5713");

	private static readonly BlueprintUnit Dlc3GraveknightBase = BlueprintTools.GetBlueprint<BlueprintUnit>("7121303d0f344a5abb3b43b0c9cef8e4");

	private static readonly BlueprintUnit GraveknightSummonedBase = BlueprintTools.GetBlueprint<BlueprintUnit>("7237a32613fe55e479d1141682f2bbd4");

	private static readonly BlueprintUnit WolfBase = BlueprintTools.GetBlueprint<BlueprintUnit>("eab864d9ca3415644a792792fd81bf87") ?? AstralDevaBase;

	private static readonly BlueprintUnit FaerieDragonBase = BlueprintTools.GetBlueprint<BlueprintUnit>("4f2ed74aea457fb409ba56872dbc5552") ?? AstralDevaBase;

	private static readonly BlueprintUnit WillOWispYellowBase = BlueprintTools.GetBlueprint<BlueprintUnit>("09fa46bc6d1cb774fb6dfefada332b9f") ?? BlueprintTools.GetBlueprint<BlueprintUnit>("827b90e60645fe641b29098ed1f70219") ?? FaerieDragonBase;

	private static readonly BlueprintUnit DrowMagusFemaleBase = BlueprintTools.GetBlueprint<BlueprintUnit>("030bffa7543f0a04ba7d60d0558a8d36");

	private static readonly BlueprintFeature Airborne = BlueprintTools.GetBlueprint<BlueprintFeature>("70cffb448c132fa409e49156d013b175");

	private static readonly BlueprintItemWeapon GreatswordPlus2 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("8b0ad785727b2154ab4029508738fbdd");

	private static readonly BlueprintItemWeapon SpearPlus4 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("0af80ef3cbfd491428d10232b5eb1af9");

	private static readonly BlueprintItemWeapon WeaponEmptyHand = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("20375b5a0c9243d45966bd72c690ab74");

	internal const string AnimalCompanionSlotFeatureGuid = "75bb2b3c41c99e041b4743fdb16a4289";

	private static readonly BlueprintFeature AnimalCompanionSlotFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("75bb2b3c41c99e041b4743fdb16a4289");

	public static BlueprintFeature TempestStarWolfFeature { get; private set; }

	public static BlueprintFeature ShadowMarshallFeature { get; private set; }

	public static BlueprintFeature OverlordGuardianFeature { get; private set; }

	public static BlueprintFeature DivineHeraldFeature { get; private set; }

	public static BlueprintFeature ChronoSpriteFeature { get; private set; }

	public static BlueprintFeature EnigmaticCoConspiratorFeature { get; private set; }

	public static BlueprintFeature ManifestedMartialSpiritFeature { get; private set; }

	public static void Add()
	{
		if (Added)
		{
			return;
		}
		Added = true;
		BlueprintFeature wolfArmor = GuardianCompanionClass.CreateNaturalArmor("TempestStarWolfNaturalArmor", "Storm-Charged Pelt", 4);
		BlueprintFeature igrisArmor = GuardianCompanionClass.CreateNaturalArmor("ShadowMarshallNaturalArmor", "Dread Knight Plating", 6);
		BlueprintFeature valkyrieArmor = GuardianCompanionClass.CreateNaturalArmor("OverlordGuardianNaturalArmor", "Jet-Black Aegis", 5);
		BlueprintFeature heraldArmor = GuardianCompanionClass.CreateNaturalArmor("DivineHeraldNaturalArmor", "Apostolic Halo Plating", 6);
		BlueprintFeature spriteArmor = GuardianCompanionClass.CreateNaturalArmor("ChronoSpriteNaturalArmor", "Temporal Shell", 6);
		BlueprintFeature conspiratorArmor = GuardianCompanionClass.CreateNaturalArmor("EnigmaticCoConspiratorNaturalArmor", "Contract-Bound Wards", 4);
		BlueprintFeature spiritArmor = GuardianCompanionClass.CreateNaturalArmor("ManifestedMartialSpiritNaturalArmor", "Spirit Sheath", 5);
		BlueprintUnit blueprintUnit = (WolfBase ?? AstralDevaBase).CreateCopy(Main.IsekaiContext, "TempestStarWolfCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Tempest Star Wolf");
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
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "TempestStarWolf.ClickBark", "Grr... Master, the pack strikes as one!");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.TrueNeutral;
			bp.Strength = 16;
			bp.Dexterity = 20;
			bp.Constitution = 16;
			bp.Intelligence = 12;
			bp.Wisdom = 16;
			bp.Charisma = 14;
			bp.MaxHP = 0;
			bp.Size = Size.Medium;
			bp.Prefab = new UnitViewLink
			{
				AssetId = "0dc0f602a83a2034ba5842f73c0012c1"
			};
			bp.Speed = new Feet(50f);
			List<BlueprintUnitFactReference> list = new List<BlueprintUnitFactReference>
			{
				GuardianBarks.TempestStarWolfBarksFeature.ToReference<BlueprintUnitFactReference>(),
				SubclassGuardianProgressions.TempestStarWolfProgression.ToReference<BlueprintUnitFactReference>(),
				wolfArmor.ToReference<BlueprintUnitFactReference>()
			};
			if (AnimalCompanionSlotFeature != null)
			{
				list.Add(AnimalCompanionSlotFeature.ToReference<BlueprintUnitFactReference>());
			}
			bp.m_AddFacts = list.ToArray();
		});
		TempestStarWolfFeature = CreateSubclassGuardianFeature("TempestStarWolfFeature", "★ Tempest Star Wolf (Slime Exclusive)", "[Slime Archetype Exclusive Partner]\nA majestic silver wolf whose fur crackles with azure lightning and shadow mist.\nBorn from the tempestuous residual energy of an apex predator, it coordinates lethal pack tactics with its slime sovereign.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 60 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +4 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 16, Dex 20, Con 16, Int 12, Wis 16, Cha 14\nAdvances 1-20 in Guardian Class with custom storm-pack milestones.", blueprintUnit, SubclassGuardianProgressions.TempestStarWolfProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "DevourerArchetype"));
		BlueprintUnit blueprintUnit2 = (IsekaiDlcManager.PreferOwned(IsekaiDlcManager.HasDlc3(), Dlc3GraveknightBase, GraveknightSummonedBase ?? AstralDevaBase) ?? AstralDevaBase).CreateCopy(Main.IsekaiContext, "ShadowMarshallCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Shadow Marshal (Igris)");
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
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "ShadowMarshall.ClickBark", "*Kneeling with greatsword lowered* My liege, I await your command.");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.LawfulNeutral;
			bp.Strength = 22;
			bp.Dexterity = 16;
			bp.Constitution = 18;
			bp.Intelligence = 14;
			bp.Wisdom = 14;
			bp.Charisma = 16;
			bp.MaxHP = 0;
			bp.Size = Size.Medium;
			bp.Speed = new Feet(30f);
			CompanionDollRace.EnsureRace(bp, "4774ee5a5711433c8c8e008dd3734e56");
			if (GreatswordPlus2 != null)
			{
				bp.Body.m_PrimaryHand = GreatswordPlus2.ToReference<BlueprintItemEquipmentHandReference>();
			}
			BlueprintUnitFactReference[] addFacts = new BlueprintUnitFactReference[3]
			{
				GuardianBarks.ShadowMarshallBarksFeature.ToReference<BlueprintUnitFactReference>(),
				SubclassGuardianProgressions.ShadowMarshallProgression.ToReference<BlueprintUnitFactReference>(),
				igrisArmor.ToReference<BlueprintUnitFactReference>()
			};
			bp.m_AddFacts = addFacts;
		});
		ShadowMarshallFeature = CreateSubclassGuardianFeature("ShadowMarshallFeature", "★ Shadow Marshal (Shadow Monarch Exclusive)", "[Shadow Monarch Archetype Exclusive Partner]\nThe legendary red-plumed dread knight, vanguard marshal of the shadow army.\nWielding an imposing two-handed greatsword with silent perfection, Igris guards his monarch with eternal devotion.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +6 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 22, Dex 16, Con 18, Int 14, Wis 14, Cha 16\nAdvances 1-20 in Guardian Class with shadow vanguard commander milestones.", blueprintUnit2, SubclassGuardianProgressions.ShadowMarshallProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "ShadowMonarchArchetype"));
		BlueprintUnit blueprintUnit3 = (ErinyesBase ?? AstralDevaBase).CreateCopy(Main.IsekaiContext, "OverlordGuardianCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Overlord Guardian (Dark Valkyrie)");
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
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "OverlordGuardian.ClickBark", "Supreme One, the Floor Guardians exist only for your supreme glory!");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.LawfulEvil;
			bp.Strength = 20;
			bp.Dexterity = 18;
			bp.Constitution = 16;
			bp.Intelligence = 16;
			bp.Wisdom = 14;
			bp.Charisma = 20;
			bp.MaxHP = 0;
			bp.Size = Size.Medium;
			bp.Speed = new Feet(35f);
			CompanionDollRace.EnsureRace(bp, "980bed7f6d9e4c33a86b95d199eb4934");
			if (SpearPlus4 != null)
			{
				bp.Body.m_PrimaryHand = SpearPlus4.ToReference<BlueprintItemEquipmentHandReference>();
			}
			BlueprintUnitFactReference[] addFacts = new BlueprintUnitFactReference[4]
			{
				GuardianBarks.OverlordGuardianBarksFeature.ToReference<BlueprintUnitFactReference>(),
				SubclassGuardianProgressions.OverlordGuardianProgression.ToReference<BlueprintUnitFactReference>(),
				valkyrieArmor.ToReference<BlueprintUnitFactReference>(),
				Airborne.ToReference<BlueprintUnitFactReference>()
			};
			bp.m_AddFacts = addFacts;
		});
		OverlordGuardianFeature = CreateSubclassGuardianFeature("OverlordGuardianFeature", "★ Overlord Guardian (Overlord Exclusive)", "[Overlord Archetype Exclusive Partner]\nThe supreme floor overseer of the Great Tomb, a dark winged valkyrie clad in jet-black armor.\nWielding an unholy spear and radiating disdain for all lower life forms, she lives only to serve the Supreme Being.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 35 ft. (Flying)\n{g|Encyclopedia:Armor_Class}AC{/g}: +5 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 20, Dex 18, Con 16, Int 16, Wis 14, Cha 20\nAdvances 1-20 in Guardian Class with supreme overseer aegis milestones.", blueprintUnit3, SubclassGuardianProgressions.OverlordGuardianProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "OverlordArchetype"));
		BlueprintUnit blueprintUnit4 = AstralDevaBase.CreateCopy(Main.IsekaiContext, "DivineHeraldCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Divine Herald (The First Apostle)");
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
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "DivineHerald.ClickBark", "Your divine mandate shall be inscribed upon the cosmos, Your Majesty.");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.LawfulGood;
			bp.Strength = 20;
			bp.Dexterity = 18;
			bp.Constitution = 18;
			bp.Intelligence = 16;
			bp.Wisdom = 18;
			bp.Charisma = 20;
			bp.MaxHP = 0;
			bp.Size = Size.Medium;
			bp.Speed = new Feet(40f);
			IsekaiGuardians.EnsureEmptyHand(bp);
			BlueprintUnitFactReference[] addFacts = new BlueprintUnitFactReference[4]
			{
				GuardianBarks.DivineHeraldBarksFeature.ToReference<BlueprintUnitFactReference>(),
				SubclassGuardianProgressions.DivineHeraldProgression.ToReference<BlueprintUnitFactReference>(),
				heraldArmor.ToReference<BlueprintUnitFactReference>(),
				Airborne.ToReference<BlueprintUnitFactReference>()
			};
			bp.m_AddFacts = addFacts;
		});
		DivineHeraldFeature = CreateSubclassGuardianFeature("DivineHeraldFeature", "★ Divine Herald (God Emperor Exclusive)", "[God Emperor Archetype Exclusive Partner]\nThe First Apostle of the Imperial Cult, a radiant celestial seraph crowned with halos of apotheosis.\nSpeaks with absolute imperial authority and wields solar brilliance against all cosmic heresy.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 40 ft. (Flying)\n{g|Encyclopedia:Armor_Class}AC{/g}: +6 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 20, Dex 18, Con 18, Int 16, Wis 18, Cha 20\nAdvances 1-20 in Guardian Class with celestial sanctified radiance milestones.", blueprintUnit4, SubclassGuardianProgressions.DivineHeraldProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "GodEmperorArchetype"));
		BlueprintFeature glowFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSpriteFairyGlowFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Chrono Sprite Fairy Glow");
			bp.SetDescription(Main.IsekaiContext, "Hides the internal skull skeleton, allowing the Chrono Sprite to manifest as a pure, floating orb of luminescent fairy light.");
			bp.IsClassFeature = true;
			bp.AddComponent<ChronoSpriteFairyGlowComponent>();
		});
		BlueprintUnit blueprintUnit5 = (WillOWispYellowBase ?? FaerieDragonBase).CreateCopy(Main.IsekaiContext, "ChronoSpriteCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Chrono Sprite (Fairy of Time)");
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
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "ChronoSprite.ClickBark", "Hey! Listen! Watch out ahead, Hero!");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.NeutralGood;
			bp.Strength = 8;
			bp.Dexterity = 24;
			bp.Constitution = 14;
			bp.Intelligence = 16;
			bp.Wisdom = 18;
			bp.Charisma = 18;
			bp.MaxHP = 0;
			bp.Size = Size.Diminutive;
			bp.Speed = new Feet(40f);
			bp.Gender = Gender.Female;
			BlueprintUnitFactReference[] addFacts = new BlueprintUnitFactReference[5]
			{
				GuardianBarks.ChronoSpriteBarksFeature.ToReference<BlueprintUnitFactReference>(),
				SubclassGuardianProgressions.ChronoSpriteProgression.ToReference<BlueprintUnitFactReference>(),
				spriteArmor.ToReference<BlueprintUnitFactReference>(),
				glowFeature.ToReference<BlueprintUnitFactReference>(),
				Airborne.ToReference<BlueprintUnitFactReference>()
			};
			bp.m_AddFacts = addFacts;
		});
		ChronoSpriteFeature = CreateSubclassGuardianFeature("ChronoSpriteFeature", "★ Chrono Sprite (Hero Exclusive)", "[Hero Archetype Exclusive Partner]\nA luminous, floating orb of golden fairy light and temporal magic who never stops buzzing with advice.\n'Hey! Listen!' she cries, accelerating the timelines of allies and granting constant Haste fields.\n\n{g|Encyclopedia:Size}Size{/g}: Diminutive\n{g|Encyclopedia:Speed}Speed{/g}: 40 ft. (Flying)\n{g|Encyclopedia:Armor_Class}AC{/g}: +6 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 8, Dex 24, Con 14, Int 16, Wis 18, Cha 18\nAdvances 1-20 in Guardian Class with temporal pulse and timeline evasion milestones.", blueprintUnit5, SubclassGuardianProgressions.ChronoSpriteProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "HeroArchetype"));
		BlueprintUnit blueprintUnit6 = (DrowMagusFemaleBase ?? ErinyesBase ?? AstralDevaBase).CreateCopy(Main.IsekaiContext, "EnigmaticCoConspiratorCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Enigmatic Co-Conspirator");
			bp.ComponentsArray = new BlueprintComponent[0];
			bp.AddComponent(delegate(AddClassLevels c)
			{
				c.m_CharacterClass = GuardianCompanionClass.GetReference();
				c.RaceStat = StatType.Intelligence;
				c.LevelsStat = StatType.Intelligence;
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
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "EnigmaticCoConspirator.ClickBark", "Did you account for all variables, Mastermind?");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.TrueNeutral;
			bp.Strength = 14;
			bp.Dexterity = 18;
			bp.Constitution = 16;
			bp.Intelligence = 22;
			bp.Wisdom = 18;
			bp.Charisma = 18;
			bp.MaxHP = 0;
			bp.Size = Size.Medium;
			bp.Speed = new Feet(30f);
			bp.Gender = Gender.Female;
			CompanionDollRace.EnsureRace(bp, "980bed7f6d9e4c33a86b95d199eb4934");
			BlueprintUnitFactReference[] addFacts = new BlueprintUnitFactReference[3]
			{
				GuardianBarks.EnigmaticCoConspiratorBarksFeature.ToReference<BlueprintUnitFactReference>(),
				SubclassGuardianProgressions.EnigmaticCoConspiratorProgression.ToReference<BlueprintUnitFactReference>(),
				conspiratorArmor.ToReference<BlueprintUnitFactReference>()
			};
			bp.m_AddFacts = addFacts;
		});
		EnigmaticCoConspiratorFeature = CreateSubclassGuardianFeature("EnigmaticCoConspiratorFeature", "★ Enigmatic Co-Conspirator (Mastermind Exclusive)", "[Mastermind Archetype Exclusive Partner]\nA mysterious, immortal advisor bound by an otherworldly contract.\nShe watches over the grand chessboard of fate with unreadable calm, shielding the Mastermind's mind and demanding eccentric snacks.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +4 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 14, Dex 18, Con 16, Int 22, Wis 18, Cha 18\nAdvances 1-20 in Guardian Class with cognitive link and command protocol zero milestones.", blueprintUnit6, SubclassGuardianProgressions.EnigmaticCoConspiratorProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "MastermindArchetype"));
		BlueprintUnit blueprintUnit7 = (GhostFighterBase ?? ThanadaemonBase ?? AstralDevaBase).CreateCopy(Main.IsekaiContext, "ManifestedMartialSpiritCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Manifested Martial Spirit");
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
				c.Bark = Helpers.CreateString(Main.IsekaiContext, "ManifestedMartialSpirit.ClickBark", "Our fists and blades strike with one soul.");
			});
			bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			bp.Alignment = Alignment.LawfulNeutral;
			bp.Strength = 20;
			bp.Dexterity = 20;
			bp.Constitution = 16;
			bp.Intelligence = 14;
			bp.Wisdom = 18;
			bp.Charisma = 12;
			bp.MaxHP = 0;
			bp.Size = Size.Medium;
			bp.Speed = new Feet(40f);
			CompanionDollRace.EnsureRace(bp, "4774ee5a5711433c8c8e008dd3734e56");
			bp.Body.DisableHands = false;
			bp.Body.m_AdditionalLimbs = new BlueprintItemWeaponReference[0];
			if (WeaponEmptyHand != null)
			{
				bp.Body.m_EmptyHandWeapon = WeaponEmptyHand.ToReference<BlueprintItemWeaponReference>();
			}
			BlueprintUnitFactReference[] addFacts = new BlueprintUnitFactReference[3]
			{
				GuardianBarks.ManifestedMartialSpiritBarksFeature.ToReference<BlueprintUnitFactReference>(),
				SubclassGuardianProgressions.ManifestedMartialSpiritProgression.ToReference<BlueprintUnitFactReference>(),
				spiritArmor.ToReference<BlueprintUnitFactReference>()
			};
			bp.m_AddFacts = addFacts;
		});
		BlueprintArchetype modBlueprint = BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "MartialGodArchetype");
		ManifestedMartialSpiritFeature = CreateSubclassGuardianFeature("ManifestedMartialSpiritFeature", "★ Manifested Martial Spirit (Martial God Exclusive)", "[Martial God Archetype Exclusive Partner]\nA translucent spectral battle spirit that mirrors the Martial God's fighting forms.\nWhether fighting with bare fists or legendary blades, the spirit flows between offense and defensive parries with supernatural speed.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 40 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +5 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 20, Dex 20, Con 16, Int 14, Wis 18, Cha 12\nAdvances 1-20 in Guardian Class with twin flow and reactive parry milestones.", blueprintUnit7, SubclassGuardianProgressions.ManifestedMartialSpiritProgression, modBlueprint);
		GuardianVisualScale.Register(blueprintUnit, blueprintUnit2, blueprintUnit3, blueprintUnit4, blueprintUnit5, blueprintUnit6, blueprintUnit7);
		BorrowedModelFixes.RegisterUnits(blueprintUnit, blueprintUnit2, blueprintUnit3, blueprintUnit4, blueprintUnit5, blueprintUnit6, blueprintUnit7);
		if (TempestStarWolfFeature != null)
		{
			IsekaiPetSelection.AddToSelection(TempestStarWolfFeature);
		}
		if (ShadowMarshallFeature != null)
		{
			IsekaiPetSelection.AddToSelection(ShadowMarshallFeature);
		}
		if (OverlordGuardianFeature != null)
		{
			IsekaiPetSelection.AddToSelection(OverlordGuardianFeature);
		}
		if (DivineHeraldFeature != null)
		{
			IsekaiPetSelection.AddToSelection(DivineHeraldFeature);
		}
		if (ChronoSpriteFeature != null)
		{
			IsekaiPetSelection.AddToSelection(ChronoSpriteFeature);
		}
		if (EnigmaticCoConspiratorFeature != null)
		{
			IsekaiPetSelection.AddToSelection(EnigmaticCoConspiratorFeature);
		}
		if (ManifestedMartialSpiritFeature != null)
		{
			IsekaiPetSelection.AddToSelection(ManifestedMartialSpiritFeature);
		}
	}

	private static BlueprintFeature CreateSubclassGuardianFeature(string name, string displayName, string description, BlueprintUnit petUnit, BlueprintProgression guardianProgression, BlueprintArchetype prerequisiteArchetype)
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
			if (prerequisiteArchetype != null)
			{
				bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.m_Archetype = prerequisiteArchetype.ToReference<BlueprintArchetypeReference>();
					c.Level = 1;
				});
			}
		});
	}
}
