using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62Act6Dlc
{
	internal static readonly Port62LateActs.Graft[] Grafts = new Port62LateActs.Graft[53]
	{
		new Port62LateActs.Graft
		{
			Answer = "IsekaiThresholdArmySeriesFinale",
			Scene = "c6/ThresholdExterior/AreeluThresholdWelcome",
			ListGuid = "bb3528754c7e741438acef95ec3b6430",
			ExpCr = 8
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiThresholdArmyGodEmperorAscent",
			Scene = "c6/ThresholdExterior/AreeluThresholdWelcome",
			ListGuid = "bb3528754c7e741438acef95ec3b6430",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 8
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiThresholdArmyMartialGodPinnacle",
			Scene = "c6/ThresholdExterior/AreeluThresholdWelcome",
			ListGuid = "bb3528754c7e741438acef95ec3b6430",
			Gate = "MartialGodProficiencies",
			ExpCr = 8
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiThresholdArmyOverlordGrandDominion",
			Scene = "c6/ThresholdExterior/AreeluThresholdWelcome",
			ListGuid = "bb3528754c7e741438acef95ec3b6430",
			Gate = "OverlordProficiencies",
			MythicPath = "Lich",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 8
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiThresholdArmyMastermindPlanarCollapse",
			Scene = "c6/ThresholdExterior/AreeluThresholdWelcome",
			ListGuid = "bb3528754c7e741438acef95ec3b6430",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 8
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiThresholdArmyHeroDawnOfTomorrow",
			Scene = "c6/ThresholdExterior/AreeluThresholdWelcome",
			ListGuid = "bb3528754c7e741438acef95ec3b6430",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiSeelahFarewellHeroBond",
			Scene = "c6/ThresholdExterior/CompanionFarewells/SeelahFarewell",
			ListGuid = "2b1cc60873a86314c9b81b93635fa430",
			Companion = "54be53f0b35bf3c4592a97ae335fe765",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiSeelahFarewellMartialGodRespect",
			Scene = "c6/ThresholdExterior/CompanionFarewells/SeelahFarewell",
			ListGuid = "2b1cc60873a86314c9b81b93635fa430",
			Companion = "54be53f0b35bf3c4592a97ae335fe765",
			Gate = "MartialGodProficiencies",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiRegillFarewellMastermindDiscipline",
			Scene = "c6/ThresholdExterior/CompanionFarewells/RegillFarewell",
			ListGuid = "bc23c9f44b93ab9428f62db47ac84b3f",
			Companion = "0d37024170b172346b3769df92a971f5",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiRegillFarewellOverlordIronPact",
			Scene = "c6/ThresholdExterior/CompanionFarewells/RegillFarewell",
			ListGuid = "bc23c9f44b93ab9428f62db47ac84b3f",
			Companion = "0d37024170b172346b3769df92a971f5",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiWoljifFarewellProtagonistRogue",
			Scene = "c6/ThresholdExterior/CompanionFarewells/WoljifFarewell",
			ListGuid = "234f289beeac48e458189a331295669f",
			Companion = "766435873b1361c4287c351de194e5f9",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiWoljifFarewellHeroFamily",
			Scene = "c6/ThresholdExterior/CompanionFarewells/WoljifFarewell",
			ListGuid = "234f289beeac48e458189a331295669f",
			Companion = "766435873b1361c4287c351de194e5f9",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiLannFarewellHeroBrotherhood",
			Scene = "c6/ThresholdExterior/CompanionFarewells/LannFarewell",
			ListGuid = "c170936600403124291df84fa2e9e9fa",
			Companion = "cb29621d99b902e4da6f5d232352fbda",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiLannFarewellMartialGodArchery",
			Scene = "c6/ThresholdExterior/CompanionFarewells/LannFarewell",
			ListGuid = "c170936600403124291df84fa2e9e9fa",
			Companion = "cb29621d99b902e4da6f5d232352fbda",
			Gate = "MartialGodProficiencies",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiWenduagFarewellOverlordEmpress",
			Scene = "c6/ThresholdExterior/CompanionFarewells/WenduagFarewell",
			ListGuid = "945033166e2524a46b3282f15a925139",
			Companion = "ae766624c03058440a036de90a7f2009",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 7,
			Changed = "Wenduag has no arachnid limbs."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiWenduagFarewellMartialGodApexHunt",
			Scene = "c6/ThresholdExterior/CompanionFarewells/WenduagFarewell",
			ListGuid = "945033166e2524a46b3282f15a925139",
			Companion = "ae766624c03058440a036de90a7f2009",
			Gate = "MartialGodProficiencies",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiArueFarewellHeroDreamBloom",
			Scene = "c6/ThresholdExterior/CompanionFarewells/ArueshalaeFarewell",
			ListGuid = "8bc697f29600c9446a5d531a2128f4b2",
			Companion = "a352873d37ec6c54c9fa8f6da3a6b3e1",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiArueFarewellCharacterArcPraise",
			Scene = "c6/ThresholdExterior/CompanionFarewells/ArueshalaeFarewell",
			ListGuid = "8bc697f29600c9446a5d531a2128f4b2",
			Companion = "a352873d37ec6c54c9fa8f6da3a6b3e1",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiEmberFarewellHeroGentleSun",
			Scene = "c6/ThresholdExterior/CompanionFarewells/EmberFarewell",
			ListGuid = "979f96a0b504daf4297e9a91016ab446",
			Companion = "2779754eecffd044fbd4842dba55312c",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiEmberFarewellGodEmperorAegis",
			Scene = "c6/ThresholdExterior/CompanionFarewells/EmberFarewell",
			ListGuid = "979f96a0b504daf4297e9a91016ab446",
			Companion = "2779754eecffd044fbd4842dba55312c",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDaeranFarewellMultiversalBanquet",
			Scene = "c6/ThresholdExterior/CompanionFarewells/DaeranFarewell",
			ListGuid = "727ea1dcd38772c4089ee5c696de60eb",
			Companion = "096fc4a96d675bb45a0396bcaa7aa993",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDaeranFarewellOverlordAristocracy",
			Scene = "c6/ThresholdExterior/CompanionFarewells/DaeranFarewell",
			ListGuid = "727ea1dcd38772c4089ee5c696de60eb",
			Companion = "096fc4a96d675bb45a0396bcaa7aa993",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiGreyborFarewellMartialMastery",
			Scene = "c6/ThresholdExterior/CompanionFarewells/GreyborFarewell",
			ListGuid = "98a6dde2e5e7b2a4abd97a0bc3719a96",
			Companion = "f72bb7c48bb3e45458f866045448fb58",
			Gate = "MartialGodProficiencies",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiGreyborFarewellMastermindBonus",
			Scene = "c6/ThresholdExterior/CompanionFarewells/GreyborFarewell",
			ListGuid = "98a6dde2e5e7b2a4abd97a0bc3719a96",
			Companion = "f72bb7c48bb3e45458f866045448fb58",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNenioFarewellEncyclopediaPeak",
			Scene = "c6/ThresholdExterior/CompanionFarewells/NenioFarewell",
			ListGuid = "6b068a702077f1c448d66f64afeb65f7",
			Companion = "1b893f7cf2b150e4f8bc2b3c389ba71d",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNenioFarewellMastermindEmpiricalPeer",
			Scene = "c6/ThresholdExterior/CompanionFarewells/NenioFarewell",
			ListGuid = "6b068a702077f1c448d66f64afeb65f7",
			Companion = "1b893f7cf2b150e4f8bc2b3c389ba71d",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAivuFarewellHeroAzataCookies",
			Scene = "c6/ThresholdExterior/CompanionFarewells/AivuFarewell",
			ListGuid = "2b6acff363cf61346b319befd6229bb9",
			Gate = "HeroProficiencies",
			MythicPath = "Azata",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAreeluFinalProtagonistParadox",
			Scene = "c6/SecondFloor/AreeluLetsFinalFight",
			ListGuid = "90861396a1375684eb5bf894bedecff3",
			ExpCr = 8
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAreeluFinalGodEmperorSolarJudgment",
			Scene = "c6/SecondFloor/AreeluLetsFinalFight",
			ListGuid = "90861396a1375684eb5bf894bedecff3",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8,
			Changed = "6.2 purged the chamber in the middle of the conversation before the fight."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAreeluFinalMartialGodWorldCleave",
			Scene = "c6/SecondFloor/AreeluLetsFinalFight",
			ListGuid = "90861396a1375684eb5bf894bedecff3",
			Gate = "MartialGodProficiencies",
			MythicPath = "Legend",
			ExpCr = 8,
			Changed = "6.2 sliced Areelu's wards apart before the fight begins."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAreeluFinalOverlordDominionClaim",
			Scene = "c6/SecondFloor/AreeluLetsFinalFight",
			ListGuid = "90861396a1375684eb5bf894bedecff3",
			Gate = "OverlordProficiencies",
			MythicPath = "Lich",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 8,
			Changed = "6.2 subjugated Areelu's life's work before the fight begins."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAreeluFinalMastermindAeonLoopSever",
			Scene = "c6/SecondFloor/AreeluLetsFinalFight",
			ListGuid = "90861396a1375684eb5bf894bedecff3",
			Gate = "MastermindProficiencies",
			MythicPath = "Aeon",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 8,
			Changed = "6.2 severed the loop and released the spirits before the fight begins."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAreeluFinalHeroCompassionateResolution",
			Scene = "c6/SecondFloor/AreeluLetsFinalFight",
			ListGuid = "90861396a1375684eb5bf894bedecff3",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPharasmaReincarnationAuditHumor",
			Scene = "Epilogues_afterlogues",
			ListGuid = "f0d89a1e468b4c6f9affc010e8d47596",
			ExpCr = 8,
			Changed = "6.2 had Pharasma weigh the Commander's own soul and said the Worldwound was closed; Cue_4 is Pharasma weighing Areelu, and it also plays after endings that leave the Wound open."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPharasmaGodEmperorMultiversalParity",
			Scene = "Epilogues_afterlogues",
			ListGuid = "f0d89a1e468b4c6f9affc010e8d47596",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 8,
			Changed = "6.2 had Pharasma weigh the Commander's own soul and said the Worldwound was closed; Cue_4 is Pharasma weighing Areelu, and it also plays after endings that leave the Wound open."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPharasmaMartialGodBladeInviolate",
			Scene = "Epilogues_afterlogues",
			ListGuid = "f0d89a1e468b4c6f9affc010e8d47596",
			Gate = "MartialGodProficiencies",
			MythicPath = "Legend",
			ExpCr = 8,
			Changed = "6.2 had Pharasma weigh the Commander's own soul and said the Worldwound was closed; Cue_4 is Pharasma weighing Areelu, and it also plays after endings that leave the Wound open."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPharasmaOverlordTombAscension",
			Scene = "Epilogues_afterlogues",
			ListGuid = "f0d89a1e468b4c6f9affc010e8d47596",
			Gate = "OverlordProficiencies",
			MythicPath = "Lich",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 8,
			Changed = "6.2 had Pharasma weigh the Commander's own soul and said the Worldwound was closed; Cue_4 is Pharasma weighing Areelu, and it also plays after endings that leave the Wound open."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPharasmaMastermindCosmicReconciliation",
			Scene = "Epilogues_afterlogues",
			ListGuid = "f0d89a1e468b4c6f9affc010e8d47596",
			Gate = "MastermindProficiencies",
			MythicPath = "Aeon",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 8,
			Changed = "6.2 had Pharasma weigh the Commander's own soul and said the Worldwound was closed; Cue_4 is Pharasma weighing Areelu, and it also plays after endings that leave the Wound open."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPharasmaHeroHeartOfKindness",
			Scene = "Epilogues_afterlogues",
			ListGuid = "f0d89a1e468b4c6f9affc010e8d47596",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8,
			Changed = "6.2 had Pharasma weigh the Commander's own soul and said the Worldwound was closed; Cue_4 is Pharasma weighing Areelu, and it also plays after endings that leave the Wound open."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct6AneviaIrabethVacation",
			Scene = "c6/ThresholdExterior/IrabethAnevia_Threshold",
			ListGuid = "46e1e280571bd4249af979c38846fe04",
			ExpCr = 7,
			ClaimKey = "Port62:Act6AneviaIrabethVacation",
			Coins = 400,
			SixTwoSponsor = "The Song of the Spheres",
			Changed = "Anevia swore by the Godclaw, a Hellknight god neither of them follows."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct6AneviaIrabethHeroLove",
			Scene = "c6/ThresholdExterior/IrabethAnevia_Threshold",
			ListGuid = "46e1e280571bd4249af979c38846fe04",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct6GalfreyBossRoom",
			Scene = "c6/ThresholdExterior/GalfreyThreshold",
			ListGuid = "bc636f62a8c94ab4798b32bba398bebb",
			ExpCr = 7,
			ClaimKey = "Port62:Act6GalfreyBossRoom",
			Coins = 450,
			SixTwoSponsor = "The Inheritor"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct6GalfreyGodEmperorPeer",
			Scene = "c6/ThresholdExterior/GalfreyThreshold",
			ListGuid = "bc636f62a8c94ab4798b32bba398bebb",
			Gate = "GodEmperorProficiencies",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct6NorthernLightsAnimeEnding",
			Scene = "c6/ThresholdExterior/NorthernLights",
			ListGuid = "422a385b41f72094783b676f54aad062",
			ExpCr = 7,
			ClaimKey = "Port62:Act6NorthernLightsAnimeEnding",
			Coins = 400,
			SixTwoSponsor = "The Song of the Spheres"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct6NorthernLightsMartialPrana",
			Scene = "c6/ThresholdExterior/NorthernLights",
			ListGuid = "422a385b41f72094783b676f54aad062",
			Gate = "MartialGodProficiencies",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct6ComeInsideSwagger",
			Scene = "c6/ThresholdExterior/ComeInside",
			ListGuid = "cf957e3715b7e3840aaab58a6eb372d0",
			ExpCr = 6,
			ClaimKey = "Port62:Act6ComeInsideSwagger",
			Coins = 400,
			SixTwoSponsor = "The Laughing King",
			Changed = "6.2 walked you into Threshold, but the reply returns to the gate's choices.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct6AreeluStaging",
			Scene = "c6/ThresholdExterior/AreeluThresholdWelcome",
			ListGuid = "bb3528754c7e741438acef95ec3b6430",
			ExpCr = 7,
			ClaimKey = "Port62:Act6AreeluStaging",
			Coins = 500,
			SixTwoSponsor = "The Grand Arbiter"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDlc1AxiomaticDebug",
			Scene = "DLC1_Megaepic/TresholdOutdoor/ValmallosWelcome",
			ListGuid = "b686f80a62f24ca08899e558b96c7a1b",
			DlcReward = "313fb87e4c0143bd84b3fc997c3c2266",
			ExpCr = 7,
			Changed = "6.2's 'null pointer' put the bare word 'null' on screen, which the game's text checks read as placeholder text; it is a 'void pointer' now."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDlc3PirateKingTrope",
			Scene = "DLC3_Roguelike/Navigator",
			ListGuid = "aac49e0c7220417c89bde055dd698ebb",
			DlcReward = "04b3bf450b1e44438854f25ba1290a98",
			ExpCr = 6,
			Changed = "6.2 spoke to a hooded 'Mad Steersman'; the navigator is an elf."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDlc4SpiritCleansing",
			Scene = "DLC4_Shifter/DLC4_Gundrun/DLC4_Gundrun_SecondState_EpicSpeech",
			ListGuid = "57929455cd9546d28fc159656bf41eda",
			DlcReward = "e38ada3f1eab4110ae331b613162a05b",
			ExpCr = 6,
			Changed = "6.2 healed a grove with Ulbrig watching; the scene is the army's speeches before the battle."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDlc5AbsoluteZeroRegulation",
			Scene = "DLC5_StoryEnds/DLC5_Postgame/SithhudHimself",
			ListGuid = "0cbb2e81134942d2aac2cf6312072c80",
			DlcReward = "1c893fd3059d4b09bd37d089aff7866e",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDlc6MainCharacterRecognition",
			Scene = "DLC6_ADanceOfMasks/KenabresFestival/RazmEvent1_FakeCommander",
			ListGuid = "470d79f6ad25457582490fd9cddb712e",
			DlcReward = "b94f823171a84e30ad7a1b892433ab5d",
			ExpCr = 5,
			Changed = "6.2 had the crowd pelt the impostors, but the scene goes on with the crowd cheering them.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDlc6ArenaFinalFlex",
			Scene = "DLC6_ADanceOfMasks/DLC6_Arena/MasterOfArena_AfterWin",
			ListGuid = "41e6f63769614e82a63f26c00e482112",
			DlcReward = "b94f823171a84e30ad7a1b892433ab5d",
			ExpCr = 8,
			Changed = "6.2 grafted it onto a list no cue presents; it now answers the announcement of the last opponent, where no fight has happened yet."
		}
	};

	internal static Port62LateActs.Graft Find(string answer)
	{
		return Grafts.FirstOrDefault((Port62LateActs.Graft g) => g.Answer == answer);
	}

	public static void Add()
	{
		Port(DialogueBeats.AreeluWelcomesYouToThreshold, "IsekaiThresholdArmySeriesFinale", "(Isekai Protagonist) [Series Finale Trope - The Final Dungeon] \"This is it, team: the grand finale! Every arc, every battle, and every choice brought us to this threshold. Let's make this last dungeon run unforgettable!\"", "{n}A roaring cheer shakes the volcanic crags as your companions and the entire allied army draw steel with exhilarating fervor!{/n}");
		Port(DialogueBeats.AreeluWelcomesYouToThreshold, "IsekaiThresholdArmyGodEmperorAscent", "(God Emperor) [Angel] [The Dawn of the Holy Empire] \"Soldiers of the Crusade! We stand at the precipice of destiny! By our holy blades and unbreakable resolve, the Worldwound closes today under the righteous sun!\"", "{n}A thermonuclear corona of blinding solar light illuminates the corrupted sky, giving hope to the thousands gathered below!{/n}");
		Port(DialogueBeats.AreeluWelcomesYouToThreshold, "IsekaiThresholdArmyMartialGodPinnacle", "(Martial God) [The Apex Crucible] \"Beyond this gate lies the ultimate confrontation between dimensions. My blade has trained across realities for this single hour. Follow me into legend!\"", "{n}Your weapon emits a peerless resonant hum that vibrates through the armor of every crusader in the army.{/n}");
		Port(DialogueBeats.AreeluWelcomesYouToThreshold, "IsekaiThresholdArmyOverlordGrandDominion", "(Overlord) [Lich] [The Living and the Dead Shall March] \"Let the Abyss tremble. We do not march to defend Golarion; we march to claim sovereign dominion over the rift and bend eternity to our will!\"", "{n}Legions of undead knights and shadow vanguard raise their halberds in chilling, rhythmic salute!{/n}");
		Port(DialogueBeats.AreeluWelcomesYouToThreshold, "IsekaiThresholdArmyMastermindPlanarCollapse", "(Mastermind) [Ontological Inversion Calculus] \"The rift coordinates are primed for irreversible collapse. Upon defeating Vorlesh, the Worldwound's geometry will fold into zero dimensional volume.\"", "{n}Your tactical lieutenants nod in grim, admiring confidence at your flawless predictive model.{/n}");
		Port(DialogueBeats.AreeluWelcomesYouToThreshold, "IsekaiThresholdArmyHeroDawnOfTomorrow", "(Hero) [For Everyone Waiting at Home] \"For everyone we loved and lost, and for the bright dawn that will rise tomorrow: together, we finish this right now!\"", "{n}A wave of tearful, unbreakable courage washes across the allied ranks as tears of hope glisten on every face!{/n}");
		PortFarewell(DialogueBeats.SeelahFarewellHub, "IsekaiSeelahFarewellHeroBond", BlueprintTools.GetBlueprint<BlueprintUnit>("54be53f0b35bf3c4592a97ae335fe765"), "(Hero) [Comrades of the Shield] \"Seelah, your laughter and faith carried us through the darkest trenches of this war. Stand with me one last time, my friend!\"", "{n}Seelah grins through tears, strapping her shield tighter with fierce pride.{/n} \"To the very end, Commander! Drinks on me in Drezen when we walk out of here!\"");
		PortFarewell(DialogueBeats.SeelahFarewellHub, "IsekaiSeelahFarewellMartialGodRespect", BlueprintTools.GetBlueprint<BlueprintUnit>("54be53f0b35bf3c4592a97ae335fe765"), "(Martial God) [Vanguard Respect] \"Keep your shield high, Seelah. When the dust settles, no blade in any world will match the valor of your guard.\"", "{n}Seelah nods firmly, her paladin aura burning steady and bright.{/n} \"Right behind you, Commander. Let's make every strike count!\"");
		PortFarewell(DialogueBeats.RegillFarewellHub, "IsekaiRegillFarewellMastermindDiscipline", BlueprintTools.GetBlueprint<BlueprintUnit>("0d37024170b172346b3769df92a971f5"), "(Mastermind) [Exemplary Operational Precision] \"Paralictor Regill, your discipline was the cornerstone of our tactical victory. It has been an honor orchestrating this war alongside you.\"", "{n}Regill salutes with immaculate Hellknight precision, a rare flicker of profound professional respect in his eyes.{/n} \"The Order of the Godclaw could not have engineered a more flawless campaign, Commander. Dismissed to the battlefield.\"");
		PortFarewell(DialogueBeats.RegillFarewellHub, "IsekaiRegillFarewellOverlordIronPact", BlueprintTools.GetBlueprint<BlueprintUnit>("0d37024170b172346b3769df92a971f5"), "(Overlord) [Iron Recognizes Iron] \"You understand what weak hearts deny, Regill: victory demands unyielding resolve. Your service will be honored in my new world order.\"", "{n}Regill meets your dark gaze without flinching.{/n} \"Order is order, Commander. Deliver victory first; history will judge the rest.\"");
		PortFarewell(DialogueBeats.WoljifFarewellHub, "IsekaiWoljifFarewellProtagonistRogue", BlueprintTools.GetBlueprint<BlueprintUnit>("766435873b1361c4287c351de194e5f9"), "(Isekai Protagonist) [From Cell to Sovereign Vanguard] \"From a cellar cage in Kenabres to the final dungeon of the Worldwound! You're the finest shadow rogue an Otherworlder could ever recruit, Woljif.\"", "{n}Woljif wipes his nose on his sleeve, his daggers gleaming with magical shadows.{/n} \"Aw, chief... don't make me get all sentimental! I've got your back, always!\"");
		PortFarewell(DialogueBeats.WoljifFarewellHub, "IsekaiWoljifFarewellHeroFamily", BlueprintTools.GetBlueprint<BlueprintUnit>("766435873b1361c4287c351de194e5f9"), "(Hero) [You Are Family Now] \"You're not that frightened kid alone in the cellar anymore, Woljif. You are a hero of the crusade, and we are family.\"", "{n}Woljif blinks back genuine tears, grinning with newfound, unshakable confidence.{/n} \"Family, huh? Yeah... yeah, I like the sound of that!\"");
		PortFarewell(DialogueBeats.LannFarewellHub, "IsekaiLannFarewellHeroBrotherhood", BlueprintTools.GetBlueprint<BlueprintUnit>("cb29621d99b902e4da6f5d232352fbda"), "(Hero) [Brothers of the Sun] \"Lann, you gave the mongrels a future and gave me an unbreakable brother. We walk out of Threshold under the open sky together.\"", "{n}Lann chuckles, testing his bowstring with steady hands.{/n} \"Under the open sky, chief. Not a bad place for a couple of cavern rats to make history!\"");
		PortFarewell(DialogueBeats.LannFarewellHub, "IsekaiLannFarewellMartialGodArchery", BlueprintTools.GetBlueprint<BlueprintUnit>("cb29621d99b902e4da6f5d232352fbda"), "(Martial God) [The Apex Archer] \"Your arrows have never missed a mark when my back was turned. Keep one shaft nocked for Vorlesh's heart.\"", "{n}Lann smiles with grim warrior satisfaction.{/n} \"Already done, Commander. She won't know which eye to close first.\"");
		PortFarewell(DialogueBeats.WenduagFarewellHub, "IsekaiWenduagFarewellOverlordEmpress", BlueprintTools.GetBlueprint<BlueprintUnit>("ae766624c03058440a036de90a7f2009"), "(Overlord) [Consort of the Shadow Sovereign] \"You recognized true power when all others wavered, Wenduag. Rule at my side when this continent is remade in our image.\"", "{n}Wenduag bows low, her clawed hands trembling with fierce devotion.{/n} \"My master... my god. Every throat in Threshold will be severed for your glory.\"");
		PortFarewell(DialogueBeats.WenduagFarewellHub, "IsekaiWenduagFarewellMartialGodApexHunt", BlueprintTools.GetBlueprint<BlueprintUnit>("ae766624c03058440a036de90a7f2009"), "(Martial God) [The Apex Hunt] \"Your predatory instincts are sharpened to perfection, Wenduag. Strike from my shadow and feast upon the hearts of our foes.\"", "{n}Wenduag bares her sharp fangs in a bloodthirsty grin.{/n} \"The hunt ends with their extinction, master!\"");
		PortFarewell(DialogueBeats.ArueshalaeFarewellHub, "IsekaiArueFarewellHeroDreamBloom", BlueprintTools.GetBlueprint<BlueprintUnit>("a352873d37ec6c54c9fa8f6da3a6b3e1"), "(Hero) [The Blossom of Desna] \"Arueshalae, you showed the multiverse that a soul is defined by its choices, not its origins. Your wings will carry the dawn into Threshold!\"", "{n}Tears of pure happiness sparkle in Arueshalae's eyes as starlight flutters around her wings.{/n} \"Thank you... with you, I am whole. I will never be afraid again!\"");
		PortFarewell(DialogueBeats.ArueshalaeFarewellHub, "IsekaiArueFarewellCharacterArcPraise", BlueprintTools.GetBlueprint<BlueprintUnit>("a352873d37ec6c54c9fa8f6da3a6b3e1"), "(Isekai Protagonist) [Masterpiece Character Arc] \"Look at how far you've come, Arueshalae! From a succubus in the Abyss to the savior of Golarion! That is a 10 out of 10 character arc!\"", "{n}Arueshalae laughs melodically, nocking an arrow of pure starlight.{/n} \"Then let us write the greatest ending this world has ever seen, Commander!\"");
		PortFarewell(DialogueBeats.EmberFarewellHub, "IsekaiEmberFarewellHeroGentleSun", BlueprintTools.GetBlueprint<BlueprintUnit>("2779754eecffd044fbd4842dba55312c"), "(Hero) [The Gentle Saint] \"Ember, your kindness made even demons weep. We will end this war so that every child can grow up in a world filled with warmth and love.\"", "{n}Ember smiles her gentle, radiant smile, holding your hand.{/n} \"They are just sad and hurt, but you showed them the way. I'm not afraid at all with you!\"");
		PortFarewell(DialogueBeats.EmberFarewellHub, "IsekaiEmberFarewellGodEmperorAegis", BlueprintTools.GetBlueprint<BlueprintUnit>("2779754eecffd044fbd4842dba55312c"), "(God Emperor) [Angel] [The Saint's Eternal Aegis] \"Little saint, your compassion shines brighter than all the heavens. Walk in safety behind my imperial shield; no evil will touch you.\"", "{n}A golden crown of celestial warmth rests upon Ember's head, shielding her in divine tranquility.{/n}");
		PortFarewell(DialogueBeats.DaeranFarewellHub, "IsekaiDaeranFarewellMultiversalBanquet", BlueprintTools.GetBlueprint<BlueprintUnit>("096fc4a96d675bb45a0396bcaa7aa993"), "(Isekai Protagonist) [The Multiversal Banquet Promise] \"Daeran, when we walk out of here alive, you owe me the most decadent, ridiculous banquet in the history of the multiverse.\"", "{n}Daeran pours a goblet of fine wine, raising it in a mocking yet genuine toast.{/n} \"Consider it already funded, my dear Commander. Only the very best vintage for the savior of the world.\"");
		PortFarewell(DialogueBeats.DaeranFarewellHub, "IsekaiDaeranFarewellOverlordAristocracy", BlueprintTools.GetBlueprint<BlueprintUnit>("096fc4a96d675bb45a0396bcaa7aa993"), "(Overlord) [Privileged Courtier] \"Your sharp tongue amused me through this dull war, Daeran. In my new empire, your debauchery will remain completely unmolested.\"", "{n}Daeran offers an extravagant, theatrical bow.{/n} \"How dreadfully generous, Your Malevolence. I shall prepare the royal suites at once.\"");
		PortFarewell(DialogueBeats.GreyborFarewellHub, "IsekaiGreyborFarewellMartialMastery", BlueprintTools.GetBlueprint<BlueprintUnit>("f72bb7c48bb3e45458f866045448fb58"), "(Martial God) [Professional Brotherhood] \"Greybor, a true master of the contract. After this, the tales of our partnership will make your guild famous across multiple dimensions.\"", "{n}Greybor puffs his pipe, nodding with quiet, solemn pride.{/n} \"A contract worthy of a lifetime, Commander. Let's finish the job.\"");
		PortFarewell(DialogueBeats.GreyborFarewellHub, "IsekaiGreyborFarewellMastermindBonus", BlueprintTools.GetBlueprint<BlueprintUnit>("f72bb7c48bb3e45458f866045448fb58"), "(Mastermind) [The Ultimate Completion Bonus] \"All contract performance milestones have been exceeded by 400%, Greybor. The completion payout will dwarf the treasury of Isger.\"", "{n}Greybor grins broadly, securing his twin axes.{/n} \"Music to my ears. You point, I execute.\"");
		PortFarewell(DialogueBeats.NenioFarewellHub, "IsekaiNenioFarewellEncyclopediaPeak", BlueprintTools.GetBlueprint<BlueprintUnit>("1b893f7cf2b150e4f8bc2b3c389ba71d"), "(Isekai Protagonist) [The Greatest Chapter in Science] \"Nenio, keep that pen moving! This final battle will be the most famous chapter in the entire Encyclopedia Golariana!\"", "{n}Nenio scribbles frantically in her notebook, tail swishing with hyperactive scientific joy!{/n} \"Fascinating! The emotional resonance of the protagonist before final planar annihilation! A landmark monograph!\"");
		PortFarewell(DialogueBeats.NenioFarewellHub, "IsekaiNenioFarewellMastermindEmpiricalPeer", BlueprintTools.GetBlueprint<BlueprintUnit>("1b893f7cf2b150e4f8bc2b3c389ba71d"), "(Mastermind) [Empirical Peer Review] \"Your analytical documentation provided essential ontological data, Nenio. Together, we shall publish the ultimate proof of planar mechanics.\"", "{n}Nenio adjusts her glasses with scholarly triumph.{/n} \"An empirical peer of true intellect! Let us conclude our peer review in the blood of Vorlesh!\"");
		Port(DialogueBeats.AivuFarewellHub, "IsekaiAivuFarewellHeroAzataCookies", "(Hero) [Azata] [Cookies and Sunshine Forever] \"Aivu, my favorite little dragon in the whole multiverse! Let's close this nasty Worldwound so we can eat sweet cookies and fly in the sunshine forever!\"", "{n}Aivu bounces into the air, flapping her butterfly wings in ecstatic happiness!{/n} \"YAY! Cookies! Sunshine! Let's go zap the bad lady and save everyone!\"");
		Port(DialogueBeats.AreeluFinalTalkHub, "IsekaiAreeluFinalProtagonistParadox", "(Isekai Protagonist) [The Reincarnation Paradox: Unintended Protagonist] \"Areelu Vorlesh, Architect of the Worldwound! You thought your dimensional suture would resurrect your lost child. Instead, you summoned an Otherworlder who rewrote your entire experiment!\"", "{n}Areelu Vorlesh's crystalline mask trembles, her eyes widening in profound, cosmic realization as your otherworldly soul signature outshines her spells!{/n} \"An anomaly... an impossible soul from beyond the Great Beyond! How could my calculations have summoned... YOU?!\"");
		Port(DialogueBeats.AreeluFinalTalkHub, "IsekaiAreeluFinalGodEmperorSolarJudgment", "(God Emperor) [Angel] [The Final Solar Judication] \"A century of suffering, tears, and desecration ends now, Vorlesh! By the mandate of heaven and mortal sovereignty, your Worldwound is purged forever!\"", "{n}Solar fire flares around you, and the fleshcraft on the walls seems to shrink from it. Areelu watches with detached, clinical interest.{/n}");
		Port(DialogueBeats.AreeluFinalTalkHub, "IsekaiAreeluFinalMartialGodWorldCleave", "(Martial God) [Legend] [The Blade That Cuts Reality] \"You sought to engineer the ultimate living weapon, Areelu. Look upon me: the Martial God who cuts through destiny itself!\"", "{n}Your blade draws a line of absolute nothingness in the air between you, and Areelu's abyssal wards shiver in its wake.{/n}");
		Port(DialogueBeats.AreeluFinalTalkHub, "IsekaiAreeluFinalOverlordDominionClaim", "(Overlord) [Lich] [The Great Tomb Claims the Worldwound] \"Your experiments were magnificent, Areelu, but you lacked the grand vision of an Overlord. Your knowledge, your power, and this rift belong to my eternal empire!\"", "{n}Pale soul-vortices stir around the altar as the dead whisper your name. Areelu only raises an eyebrow.{/n} \"Ambition. At least I recognize it.\"");
		Port(DialogueBeats.AreeluFinalTalkHub, "IsekaiAreeluFinalMastermindAeonLoopSever", "(Mastermind) [Aeon] [Axiomatic Loop Severance] \"Your grief created a temporal and planar singularity. I have almost solved the equation of your child's soul. By cosmic law, this loop can be severed and balance restored.\"", "{n}Geometric arrays of sapphire light trace the tangled soul-threads of the chamber, and Areelu falls very still as she watches them.{/n}");
		Port(DialogueBeats.AreeluFinalTalkHub, "IsekaiAreeluFinalHeroCompassionateResolution", "(Hero) [No More Tears, No More Blood] \"I understand the unbearable grief that broke your heart, Areelu. But no love justifies destroying the lives of millions. It ends today: rest your weary heart.\"", "{n}A gentle, warm radiance touches Areelu's face, bringing a single tear of peace to the Architect's sorrowful eyes.{/n}");
		Port(DialogueBeats.PharasmaWeighsYourSoul, "IsekaiPharasmaReincarnationAuditHumor", "(Isekai Protagonist) [The Reincarnation Audit] \"Lady Pharasma, before you rule on Areelu: I know a thing or two about souls that ended up somewhere they were never meant to be. Judge what she did, but remember what grief made of her.\"", "{n}The Lady of Graves turns her gaze from Areelu to you, her stern, eternal visage softening with a ghost of an amused, cosmic smile.{/n} \"An Otherworlder pleading for the Architect of the Wound. The tapestry of fate grows stranger with every thread you pull. Your words are weighed as well.\"");
		Port(DialogueBeats.PharasmaWeighsYourSoul, "IsekaiPharasmaGodEmperorMultiversalParity", "(God Emperor) [Angel] [Sovereignty Across Eternity] \"You preside over mortal souls, Lady of Graves, and Areelu's is yours to weigh. But the Supreme Sovereign bows to no single pantheon, and stands witness to this verdict.\"", "{n}Golden imperial light surrounds you, an equal radiant power before the throne of death, as the scales hang still between Pharasma and the Architect.{/n}");
		Port(DialogueBeats.PharasmaWeighsYourSoul, "IsekaiPharasmaMartialGodBladeInviolate", "(Martial God) [Legend] [The Inviolate Blade of Fate] \"Weigh her soul however your scales desire, goddess. I met her creation blade to blade; whatever you decide, my sword has already said all it had to say.\"", "{n}The scales of fate tremble as Pharasma silently acknowledges the unconquered warrior who dared to break her silence.{/n}");
		Port(DialogueBeats.PharasmaWeighsYourSoul, "IsekaiPharasmaOverlordTombAscension", "(Overlord) [Lich] [The New Sovereign of Death] \"Your River of Souls is slow, Pharasma. If you have no use for the Architect's mind, my Great Tomb would gladly keep it, where death obeys my supreme intellect!\"", "{n}The starry court shudders at your dark majesty, and the Lady of Graves regards you with the cold patience of one who has outlasted every would-be master of death.{/n}");
		Port(DialogueBeats.PharasmaWeighsYourSoul, "IsekaiPharasmaMastermindCosmicReconciliation", "(Mastermind) [Aeon] [Cosmic Audit Complete] \"Submitting a finalized audit of the defendant: a century of planar causality violations, every one traceable to a single unhealed grief. The books are open, Lady Pharasma. The verdict is yours.\"", "{n}A crystal scroll of cosmic law unfurls before the throne, listing Areelu's deeds line by line, and Pharasma reads it without a word.{/n}");
		Port(DialogueBeats.PharasmaWeighsYourSoul, "IsekaiPharasmaHeroHeartOfKindness", "(Hero) [A Heart That Loved Golarion] \"She hurt so many people, and I will never forget that. But she was a mother who lost her child. If there is any mercy left for her, Lady Pharasma, let her have it.\"", "{n}A warm, gentle light bathes the Boneyard as Pharasma inclines her head toward you, and for a moment the silence feels a little less cold.{/n}");
		Port(DialogueBeats.IrabethAneviaThresholdHub, "IsekaiAct6AneviaIrabethVacation", "(Isekai Protagonist) [The Post-War Hot Spring Vacation Pact] \"Anevia, Irabeth... look how far we've come. From popping broken shinbones in the subterranean rubble of Kenabres to standing together at the gates of Threshold. Once we close this Worldwound, I'm booking you two an all-expenses-paid vacation to a peaceful seaside hot spring. No armor, no demons, no war. Just peace.\"", "{n}Anevia and Irabeth stand side by side, their fingers tightly intertwined. Tears shimmer in Irabeth's eyes as she presses her gauntleted fist against her chest, while Anevia lets out a soft, shaky laugh of overflowing happiness.{/n} \"An 'all-expenses-paid seaside vacation'... Desna and the Inheritor, that sounds like heaven itself, Commander. We started this journey with you in the dark underground, and we will see it through to the sunrise. Thank you for giving us a future to dream of.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"A sacred promise of peace and love beneath the final sky. Desna's starlight smiles upon you.\"</i>"));
		Port(DialogueBeats.IrabethAneviaThresholdHub, "IsekaiAct6AneviaIrabethHeroLove", "(Hero) [Good] [Love as the Ultimate Aegis] \"Your love for each other was the true light that carried us through the dark. When history remembers this crusade, it will remember that hope never failed.\"", "{n}Irabeth nods proudly, her blade glowing with pure celestial fire.{/n} \"For Kenabres, for Mendev, and for our Commander!\"");
		Port(DialogueBeats.GalfreyAtTheThreshold, "IsekaiAct6GalfreyBossRoom", "(Isekai Protagonist) [The Classic Boss Room Preparation] \"Your Majesty, in all the greatest chronicles of my world, the final dungeon always ends the same way: the hero and the queen march shoulder-to-shoulder, the epic finale soundtrack swells to a crescendo, and the ancient evil gets clobbered into dust. Check your potions and buffs; we're wrapping up this campaign in style.\"", "{n}Queen Galfrey lets out a soft, beautiful laugh that echoes across the frozen rocks of Threshold, her royal standard snapping fiercely in the rift wind. Her eyes shine with total, radiant confidence in your vanguard.{/n} \"A 'final dungeon' and an 'epic finale soundtrack'?! Truly, you carry the boundless spirit of another world into our darkest hour, Commander. For a hundred years, Mendev marched toward this gate in dread. But today, standing beside you... I feel only the joy of impending victory. Lead on; let us finish this chronicle together!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"The Queen and the Otherworlder march to destiny! The final hour is at hand!\"</i>"));
		Port(DialogueBeats.GalfreyAtTheThreshold, "IsekaiAct6GalfreyGodEmperorPeer", "(God Emperor) [Sovereigns of Humanity] \"Galfrey, you bore the crown of Mendev through a century of torment. Walk with me now not as a desperate monarch, but as a sovereign peer sealing the Abyss forever.\"", "{n}Galfrey bows her head in profound respect, drawing her holy blade beside yours.{/n} \"Under your standard, humanity shall triumph.\"");
		Port(DialogueBeats.NorthernLightsArtifact, "IsekaiAct6NorthernLightsAnimeEnding", "(Isekai Protagonist) [The Grand Finale Skyline] \"Look at those cosmic auroras dancing above the wound... In my home world, this is the exact moment in the anime finale where the orchestral theme reaches its peak. We're about to write the greatest ending this world has ever seen.\"", "{n}Your companions gather around you beneath the swirling emerald and violet lights, looking up into the heavens with wide, awe-struck eyes. A peaceful, solemn warmth settles over the entire crusade vanguard.{/n} \"The finale soundtrack... You really believe we're going to win, don't you? Yes. Standing under these lights with you, Commander, failure is completely unthinkable.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"The stars sing across the heavens as the final curtain rises!\"</i>"));
		Port(DialogueBeats.NorthernLightsArtifact, "IsekaiAct6NorthernLightsMartialPrana", "(Martial God) [Atmospheric Prana Gathering] \"The cosmic collision between planar thresholds supercharges the prana of the air. Draw it into your dantian; let every strike carry the weight of a dying star.\"", "{n}Your companions exhale in unison, their weapons humming with intense, shimmering internal energy.{/n} \"The power... It feels limitless!\"");
		PortNarrated(DialogueBeats.ThresholdGates, "IsekaiAct6ComeInsideSwagger", "(Isekai Protagonist) [Confident Final Dungeon Entry Swagger] \"Alright, Threshold. You've been the dreaded final dungeon on the map for a hundred years. Let's see what you've got. Time to wrap up season one.\"", "{n}With a confident grin, you size up the dark gates of Threshold, and your vanguard falls in behind you, ready for your word.{/n}", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Entering the final dungeon like a true protagonist! Show them how it's done!\"</i>"));
		Port(DialogueBeats.AreeluWelcomesYouToThreshold, "IsekaiAct6AreeluStaging", "(Isekai Protagonist) [Critique of Dramatic Monologue Staging] \"Areelu, the dramatic shadow lighting and standing at the top of the grand marble stairs with your cape spreading out? Be honest: how many times did you rehearse this entrance in front of a scrying mirror? It's a solid nine out of ten for theatrical presence!\"", "{n}Areelu Vorlesh pauses mid-incantation, her piercing, otherworldly eyes narrowing as a faint, almost imperceptible twitch of a wry smirk touches the corner of her lips.{/n} \"'Rehearsed in a mirror'?! You never cease to amaze me, child. Even standing before the Architect of the Worldwound at the edge of oblivion, your thoughts wander to the staging of the theater. Perhaps that irrepressible spirit is why my experiment exceeded every calculation. Ascend, then; let us conclude our drama.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Mocking the Architect's final monologue staging to her face! Absolute legendary audacity!\"</i>"));
		Port(DialogueBeats.ValmallosHub, "IsekaiDlc1AxiomaticDebug", "(Isekai Protagonist) [Debug Mode - Axiomatic Code Injection] \"Your clockwork simulation has an unhandled void pointer exception in thread 0x7F. Let me patch your axioms before your gears melt.\"", "{n}Valmallos's mechanical processors freeze in astonishment as the simulation stabilizes with zero glitches.{/n} \"Anomalous code accepted. Inevitable protocol updated.\"");
		Port(DialogueBeats.NavigatorHub, "IsekaiDlc3PirateKingTrope", "(Isekai Protagonist) [Pirate King Trope - Charting the Outer Dark] \"Navigator, your charts point to Nahyndri's ancient curse. My coordinates point to infinite plunder across the stars. Set sail under my command!\"", "{n}The weathered elf squints at you, then breaks into a wide grin and taps the chart.{/n} \"Plunder across the stars? Aye, Captain... I'll plot you a course.\"");
		Port(DialogueBeats.GundrunEpicSpeech, "IsekaiDlc4SpiritCleansing", "(Isekai Protagonist) [Shamanic Otherworlder Attunement] \"Ancient spirits of Sarkoris, hear the voice of one who walked between the stars! Let the abyssal corruption slough away from your roots!\"", "{n}The Sarkorian warriors fall silent as your words ring out over the camp, and for a moment the air around the gathered army feels clean and old as the land itself.{/n}");
		Port(DialogueBeats.SithhudAsksWhoYouAre, "IsekaiDlc5AbsoluteZeroRegulation", "(Isekai Protagonist) [Thermal Mana Regulation - Absolute Zero Resistance] \"You think this frozen void chills me? In my previous world, thermodynamic entropy is merely atomic vibration at 0 Kelvin.\"", "{n}A thermal aura envelops your companions, completely shielding them from the biting frost.{/n}");
		PortNarrated(DialogueBeats.FakeCommanderAtTheFestival, "IsekaiDlc6MainCharacterRecognition", "(Isekai Protagonist) [Main Character Recognition Protocol] \"You hired cheap circus performers to impersonate ME? Their posture is atrocious, their wigs are falling off, and their battle aura is zero out of ten.\"", "{n}A few people in the festival crowd laugh uncertainly and look from you to the impostor, but most keep cheering. The performers do not miss a beat.{/n}");
		Port(DialogueBeats.ArenaMasterAnnouncesTheLast, "IsekaiDlc6ArenaFinalFlex", "(Isekai Protagonist) [The Final Flex: Arena Sweep] \"You call your last creation an invincible legend? In my world, it is the mid-season boss who exists solely to showcase my new transformation.\"", "{n}The Master of the Arena laughs in many voices at once.{/n} \"Then show us your transformation, great one. The last of our creations is waiting.\"");
	}

	private static void Port(GraftBeat beat, string name, string answerText, string replyText, string line = null)
	{
		Port62LateActs.Graft graft = Port62LateActs.Find(Grafts, name, beat, "Port62Act6Dlc", Port62LateActs.ReplyShape.DialogSpeaker);
		if (graft != null)
		{
			GraftedAnswer.Named(name).At(beat).Says(answerText)
				.ShownWhen(Port62LateActs.ShowConditions(graft))
				.OnSelect(Port62LateActs.OnSelectActions(graft, line))
				.Configure(delegate(BlueprintAnswer bp)
				{
					Port62LateActs.Configure(bp, graft);
				})
				.RepliedToBy(name + "Reply", replyText)
				.WithSpeakerlessTwin(name + "ReplyNoSpeaker")
				.ThenBackToTheList()
				.Graft();
		}
	}

	private static void PortNarrated(GraftBeat beat, string name, string answerText, string replyText, string line = null)
	{
		Port62LateActs.Graft graft = Port62LateActs.Find(Grafts, name, beat, "Port62Act6Dlc", Port62LateActs.ReplyShape.Narrated);
		if (graft != null)
		{
			GraftedAnswer.Named(name).At(beat).Says(answerText)
				.ShownWhen(Port62LateActs.ShowConditions(graft))
				.OnSelect(Port62LateActs.OnSelectActions(graft, line))
				.Configure(delegate(BlueprintAnswer bp)
				{
					Port62LateActs.Configure(bp, graft);
				})
				.RepliedToBy(name + "Reply", replyText)
				.Narrated()
				.WithSpeakerlessTwin(name + "ReplyNoSpeaker")
				.ThenBackToTheList()
				.Graft();
		}
	}

	private static void PortFarewell(GraftBeat beat, string name, BlueprintUnit companion, string answerText, string replyText, string line = null)
	{
		Port62LateActs.Graft graft = Port62LateActs.Find(Grafts, name, beat, "Port62Act6Dlc", Port62LateActs.ReplyShape.Companion);
		if (graft != null)
		{
			GraftedAnswer.Named(name).At(beat).Says(answerText)
				.ShownWhen(Port62LateActs.ShowConditions(graft))
				.OnSelect(Port62LateActs.OnSelectActions(graft, line))
				.Configure(delegate(BlueprintAnswer bp)
				{
					Port62LateActs.Configure(bp, graft);
				})
				.RepliedToBy(name + "Reply", replyText)
				.SpokenByCompanion(companion)
				.WithSpeakerlessTwin(name + "ReplyNoSpeaker")
				.ThenBackToTheList()
				.Graft();
		}
	}
}
