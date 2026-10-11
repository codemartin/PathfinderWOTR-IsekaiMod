using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62Act2
{
	private const string CureModerateWounds = "f991f3051c3b9e64fabc87891077b613";

	internal static readonly Port62SceneGraft[] Grafts = new Port62SceneGraft[18][]
	{
		TourEndQueen(),
		WarcampYakerStart(),
		RegillInBarge(),
		StartAnevia(),
		Nurah(),
		AmongGhouls(),
		VendorQuartermaster(),
		MaskedGhost(),
		BottleneckGateBeforeFight(),
		Council(),
		FinalBattleBefore(),
		GalfreyArrives(),
		SosielIntro(),
		Seelah(),
		JernaughWarcamp(),
		AneviaStart(),
		IrabethRegill(),
		FakeYanielFirst()
	}.SelectMany((Port62SceneGraft[] scene) => scene).ToArray();

	internal static readonly Port62SceneBookGraft[] BookGrafts = new Port62SceneBookGraft[5]
	{
		new Port62SceneBookGraft
		{
			Answer = "IsekaiLepersSmileSpatialStasis",
			PageGuid = "7be206b993f3dea49a89a88ce5e6ce6f",
			NextPageGuid = "7be206b993f3dea49a89a88ce5e6ce6f",
			ExpCr = 5,
			ClaimKey = "Port62:LepersSmileSpatialStasis",
			Coins = 500,
			SixTwoSponsor = "The Lucky Drunk",
			Gold = 4000,
			VanillaItems = new string[2] { "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" }
		},
		new Port62SceneBookGraft
		{
			Answer = "IsekaiLepersSmileMartialSoloSweep",
			PageGuid = "7be206b993f3dea49a89a88ce5e6ce6f",
			NextPageGuid = "7be206b993f3dea49a89a88ce5e6ce6f",
			Gate = "MartialGodProficiencies",
			ExpCr = 5
		},
		new Port62SceneBookGraft
		{
			Answer = "IsekaiLepersSmileMastermindAcousticRupture",
			PageGuid = "7be206b993f3dea49a89a88ce5e6ce6f",
			NextPageGuid = "7be206b993f3dea49a89a88ce5e6ce6f",
			Gate = "MastermindProficiencies",
			ExpCr = 5
		},
		new Port62SceneBookGraft
		{
			Answer = "IsekaiLepersSmileHeroRearguardAegis",
			PageGuid = "7be206b993f3dea49a89a88ce5e6ce6f",
			NextPageGuid = "7be206b993f3dea49a89a88ce5e6ce6f",
			Gate = "HeroProficiencies",
			ExpCr = 5
		},
		new Port62SceneBookGraft
		{
			Answer = "IsekaiLepersSmileOverlordExpendableMarch",
			PageGuid = "7be206b993f3dea49a89a88ce5e6ce6f",
			NextPageGuid = "7be206b993f3dea49a89a88ce5e6ce6f",
			Gate = "OverlordProficiencies",
			ExpCr = 5
		}
	};

	internal static readonly Dictionary<string, string> PagesWithOwnReplyCue = new Dictionary<string, string> { { "7be206b993f3dea49a89a88ce5e6ce6f", "IsekaiLepersSmileReplyCue" } };

	internal const string SwarmReplyCue = "IsekaiLepersSmileReplyCue";

	internal const string SwarmReplyCueText = "{n}Your word travels down the line and the crusaders hold their ground above the canyon. The swarm keeps up its wet, chittering churn below, and for a moment nothing moves but the dust. The Queen's officers look to you to say what happens next.{/n}";

	private static string SpeakerOf(string unitGuid)
	{
		return unitGuid;
	}

	private static Port62SceneGraft[] TourEndQueen()
	{
		return new Port62SceneGraft[6]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiQueenGalfreyLogisticsDoctrine",
				Scene = "c2_vs/Warcamp_IntroTour/Tour_End_Queen",
				ListGuid = "228936fbcc45a42479c63982cc4c1e3f",
				Beat = DialogueBeats.GalfreyBriefsTheDrezenCampaign,
				ReplySpeaker = SpeakerOf("97d1c33c784100140b9fa317aef5a499"),
				ExpCr = 4,
				ClaimKey = "Port62:QueenGalfreyLogisticsDoctrine",
				Coins = 400,
				SixTwoSponsor = "The World Sovereign",
				Gold = 600,
				ModItems = new string[1] { "ItemRoyalMendevianSignet" }
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiQueenGalfreyImperialEdict",
				Scene = "c2_vs/Warcamp_IntroTour/Tour_End_Queen",
				ListGuid = "228936fbcc45a42479c63982cc4c1e3f",
				Beat = DialogueBeats.GalfreyBriefsTheDrezenCampaign,
				Narrated = true,
				Gate = "GodEmperorProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiQueenGalfreyMastermindAudit",
				Scene = "c2_vs/Warcamp_IntroTour/Tour_End_Queen",
				ListGuid = "228936fbcc45a42479c63982cc4c1e3f",
				Beat = DialogueBeats.GalfreyBriefsTheDrezenCampaign,
				ReplySpeaker = SpeakerOf("97d1c33c784100140b9fa317aef5a499"),
				Gate = "MastermindProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 4
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiQueenGalfreyMartialChallenge",
				Scene = "c2_vs/Warcamp_IntroTour/Tour_End_Queen",
				ListGuid = "228936fbcc45a42479c63982cc4c1e3f",
				Beat = DialogueBeats.GalfreyBriefsTheDrezenCampaign,
				ReplySpeaker = SpeakerOf("97d1c33c784100140b9fa317aef5a499"),
				Gate = "MartialGodProficiencies",
				Shift = AlignmentShiftDirection.Chaotic,
				ExpCr = 4
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiQueenGalfreyHeroVow",
				Scene = "c2_vs/Warcamp_IntroTour/Tour_End_Queen",
				ListGuid = "228936fbcc45a42479c63982cc4c1e3f",
				Beat = DialogueBeats.GalfreyBriefsTheDrezenCampaign,
				ReplySpeaker = SpeakerOf("97d1c33c784100140b9fa317aef5a499"),
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 4
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiQueenGalfreyOverlordTithe",
				Scene = "c2_vs/Warcamp_IntroTour/Tour_End_Queen",
				ListGuid = "228936fbcc45a42479c63982cc4c1e3f",
				Beat = DialogueBeats.GalfreyBriefsTheDrezenCampaign,
				ReplySpeaker = SpeakerOf("97d1c33c784100140b9fa317aef5a499"),
				Gate = "OverlordProficiencies",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 4
			}
		};
	}

	private static Port62SceneGraft[] WarcampYakerStart()
	{
		return new Port62SceneGraft[7]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiYakerRapidMobilization",
				Scene = "c2_vs/HellknightsUnderAttack/Warcamp_Yaker_Start",
				ListGuid = "daf5bff8e63f68f49a8f0fc173ae437b",
				Beat = DialogueBeats.YakerAsksForRelief,
				ReplySpeaker = SpeakerOf("c58d89bbe1c7a5542b5e3204f1f4344b"),
				ExpCr = 4,
				ClaimKey = "Port62:YakerRapidMobilization",
				Coins = 400,
				SixTwoSponsor = "The Iron Warden",
				Gold = 400
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiYakerFlashStepVanguard",
				Scene = "c2_vs/HellknightsUnderAttack/Warcamp_Yaker_Start",
				ListGuid = "daf5bff8e63f68f49a8f0fc173ae437b",
				Beat = DialogueBeats.YakerAsksForRelief,
				Narrated = true,
				Gate = "MasterAtArmsProficiencies",
				ExpCr = 4
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiYakerMastermindChokepoint",
				Scene = "c2_vs/HellknightsUnderAttack/Warcamp_Yaker_Start",
				ListGuid = "daf5bff8e63f68f49a8f0fc173ae437b",
				Beat = DialogueBeats.YakerAsksForRelief,
				ReplySpeaker = SpeakerOf("c58d89bbe1c7a5542b5e3204f1f4344b"),
				Gate = "MastermindProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 4
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiYakerHeroCharge",
				Scene = "c2_vs/HellknightsUnderAttack/Warcamp_Yaker_Start",
				ListGuid = "daf5bff8e63f68f49a8f0fc173ae437b",
				Beat = DialogueBeats.YakerAsksForRelief,
				ReplySpeaker = SpeakerOf("c58d89bbe1c7a5542b5e3204f1f4344b"),
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 4
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiYakerOverlordContract",
				Scene = "c2_vs/HellknightsUnderAttack/Warcamp_Yaker_Start",
				ListGuid = "daf5bff8e63f68f49a8f0fc173ae437b",
				Beat = DialogueBeats.YakerAsksForRelief,
				ReplySpeaker = SpeakerOf("c58d89bbe1c7a5542b5e3204f1f4344b"),
				Gate = "OverlordProficiencies",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 4
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2YakerPunctuality",
				Scene = "c2_vs/HellknightsUnderAttack/Warcamp_Yaker_Start",
				ListGuid = "daf5bff8e63f68f49a8f0fc173ae437b",
				Beat = DialogueBeats.YakerAsksForRelief,
				ReplySpeaker = SpeakerOf("c58d89bbe1c7a5542b5e3204f1f4344b"),
				ExpCr = 3,
				ClaimKey = "Port62:Act2YakerPunctuality",
				Coins = 350,
				SixTwoSponsor = "The Inheritor",
				ModItems = new string[1] { "ItemMartialGodFocusBand" }
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2YakerMartialDiscipline",
				Scene = "c2_vs/HellknightsUnderAttack/Warcamp_Yaker_Start",
				ListGuid = "daf5bff8e63f68f49a8f0fc173ae437b",
				Beat = DialogueBeats.YakerAsksForRelief,
				ReplySpeaker = SpeakerOf("c58d89bbe1c7a5542b5e3204f1f4344b"),
				Gate = "MartialGodProficiencies",
				ExpCr = 4
			}
		};
	}

	private static Port62SceneGraft[] RegillInBarge()
	{
		return new Port62SceneGraft[5]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiRegillFieldTriageDoctrine",
				Scene = "c2_vs/HellknightsUnderAttack/RegillInBarge",
				ListGuid = "ab4c009b7ff77da4fad0abe34ad79c08",
				Beat = DialogueBeats.RegillExplainsTheWounded,
				ExpCr = 5,
				ClaimKey = "Port62:RegillFieldTriageDoctrine",
				Coins = 450,
				SixTwoSponsor = "The Iron Warden",
				Gold = 500,
				ModItems = new string[1] { "ItemHellknightOfficerRegalia" }
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiRegillOverlordReclamation",
				Scene = "c2_vs/HellknightsUnderAttack/RegillInBarge",
				ListGuid = "ab4c009b7ff77da4fad0abe34ad79c08",
				Beat = DialogueBeats.RegillExplainsTheWounded,
				Narrated = true,
				Gate = "OverlordProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiRegillMartialStanceCritique",
				Scene = "c2_vs/HellknightsUnderAttack/RegillInBarge",
				ListGuid = "ab4c009b7ff77da4fad0abe34ad79c08",
				Beat = DialogueBeats.RegillExplainsTheWounded,
				Gate = "MartialGodProficiencies",
				Shift = AlignmentShiftDirection.Chaotic,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiRegillHeroMercyRebuke",
				Scene = "c2_vs/HellknightsUnderAttack/RegillInBarge",
				ListGuid = "ab4c009b7ff77da4fad0abe34ad79c08",
				Beat = DialogueBeats.RegillExplainsTheWounded,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiRegillGodEmperorImperialPardon",
				Scene = "c2_vs/HellknightsUnderAttack/RegillInBarge",
				ListGuid = "ab4c009b7ff77da4fad0abe34ad79c08",
				Beat = DialogueBeats.RegillExplainsTheWounded,
				Gate = "GodEmperorProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			}
		};
	}

	private static Port62SceneGraft[] StartAnevia()
	{
		return new Port62SceneGraft[5]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiGargoyleNightPerimeterStarlight",
				Scene = "c2_vs/GargoyleAttack/Start_Anevia",
				ListGuid = "280221f20f506b9409275949d588eb1e",
				Beat = DialogueBeats.GargoylesNamedInTheNight,
				ReplySpeaker = SpeakerOf("01ff140c13b014740968bfcfaf130b2a"),
				ExpCr = 5,
				ClaimKey = "Port62:GargoyleNightPerimeterStarlight",
				Coins = 450,
				SixTwoSponsor = "The Song of the Spheres",
				Gold = 400
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiGargoyleMidnightShadowSeverance",
				Scene = "c2_vs/GargoyleAttack/Start_Anevia",
				ListGuid = "280221f20f506b9409275949d588eb1e",
				Beat = DialogueBeats.GargoylesNamedInTheNight,
				Narrated = true,
				Gate = "MartialGodProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiGargoyleMastermindAcousticTrap",
				Scene = "c2_vs/GargoyleAttack/Start_Anevia",
				ListGuid = "280221f20f506b9409275949d588eb1e",
				Beat = DialogueBeats.GargoylesNamedInTheNight,
				Narrated = true,
				Gate = "MastermindProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiGargoyleHeroHospitalShield",
				Scene = "c2_vs/GargoyleAttack/Start_Anevia",
				ListGuid = "280221f20f506b9409275949d588eb1e",
				Beat = DialogueBeats.CampUnderAttackAtNight,
				Narrated = true,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiGargoyleOverlordNecroticGrasp",
				Scene = "c2_vs/GargoyleAttack/Start_Anevia",
				ListGuid = "280221f20f506b9409275949d588eb1e",
				Beat = DialogueBeats.GargoylesNamedInTheNight,
				Narrated = true,
				Gate = "OverlordProficiencies",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 5
			}
		};
	}

	private static Port62SceneGraft[] Nurah()
	{
		return new Port62SceneGraft[7]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiNurahPheromoneDecomposition",
				Scene = "NPC_Common/Nurah",
				ListGuid = "16ccf8f56019e2c4f92b1caec3864b5d",
				Beat = DialogueBeats.NurahExcusesThePerfume,
				ExpCr = 5,
				ClaimKey = "Port62:NurahPheromoneDecomposition",
				Coins = 450,
				SixTwoSponsor = "The Hermit",
				ModItems = new string[1] { "ItemVescavorPheromoneFlask" }
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiNurahSlimePheromoneDrain",
				Scene = "NPC_Common/Nurah",
				ListGuid = "16ccf8f56019e2c4f92b1caec3864b5d",
				Beat = DialogueBeats.NurahExcusesThePerfume,
				Narrated = true,
				Gate = "DevourerProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiNurahMastermindHandwritingAudit",
				Scene = "NPC_Common/Nurah",
				ListGuid = "16ccf8f56019e2c4f92b1caec3864b5d",
				Beat = DialogueBeats.NurahExcusesTheEvidence,
				Gate = "MastermindProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiNurahMartialPulseLock",
				Scene = "NPC_Common/Nurah",
				ListGuid = "16ccf8f56019e2c4f92b1caec3864b5d",
				Beat = DialogueBeats.NurahExcusesTheEvidence,
				Gate = "MartialGodProficiencies",
				Shift = AlignmentShiftDirection.Chaotic,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiNurahGodEmperorImperialCondemnation",
				Scene = "NPC_Common/Nurah",
				ListGuid = "16ccf8f56019e2c4f92b1caec3864b5d",
				Beat = DialogueBeats.NurahExcusesTheEvidence,
				Gate = "GodEmperorProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2NurahHistoricalTrivia",
				Scene = "NPC_Common/Nurah",
				ListGuid = "16ccf8f56019e2c4f92b1caec3864b5d",
				Beat = DialogueBeats.NurahOffersHerHelp,
				ExpCr = 3,
				ClaimKey = "Port62:Act2NurahHistoricalTrivia",
				Coins = 300,
				SixTwoSponsor = "The Hermit"
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2NurahMastermindDetect",
				Scene = "NPC_Common/Nurah",
				ListGuid = "16ccf8f56019e2c4f92b1caec3864b5d",
				Beat = DialogueBeats.NurahOffersHerHelp,
				Narrated = true,
				Gate = "MastermindProficiencies",
				ExpCr = 4
			}
		};
	}

	private static Port62SceneGraft[] AmongGhouls()
	{
		return new Port62SceneGraft[5]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiLostChapelFlashRescue",
				Scene = "c2_vs/LostChapel/Among_Ghouls",
				ListGuid = "6856a7d6e123a0c46b666e8052008405",
				Beat = DialogueBeats.GhoulAmbushRevealed,
				Narrated = true,
				ExpCr = 5,
				ClaimKey = "Port62:LostChapelFlashRescue",
				Coins = 450,
				SixTwoSponsor = "The Inheritor",
				Buff = "CrusaderLiberatorBuff"
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiLostChapelBeaconOfTheLost",
				Scene = "c2_vs/LostChapel/Among_Ghouls",
				ListGuid = "6856a7d6e123a0c46b666e8052008405",
				Beat = DialogueBeats.GhoulAmbushRevealed,
				Narrated = true,
				Gate = "HeroProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiLostChapelMartialGhoulSeverance",
				Scene = "c2_vs/LostChapel/Among_Ghouls",
				ListGuid = "6856a7d6e123a0c46b666e8052008405",
				Beat = DialogueBeats.GhoulAmbushRevealed,
				Narrated = true,
				Gate = "MartialGodProficiencies",
				Shift = AlignmentShiftDirection.Chaotic,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiLostChapelGodEmperorSolarPurge",
				Scene = "c2_vs/LostChapel/Among_Ghouls",
				ListGuid = "6856a7d6e123a0c46b666e8052008405",
				Beat = DialogueBeats.GhoulAmbushRevealed,
				Narrated = true,
				Gate = "GodEmperorProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiLostChapelMastermindAltarDecryption",
				Scene = "c2_vs/LostChapel/Among_Ghouls",
				ListGuid = "6856a7d6e123a0c46b666e8052008405",
				Beat = DialogueBeats.GhoulAmbushRevealed,
				Narrated = true,
				Gate = "MastermindProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			}
		};
	}

	private static Port62SceneGraft[] VendorQuartermaster()
	{
		return new Port62SceneGraft[1]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiWilcerSupplyChain",
				Scene = "NPC_Common/Vendor_Quartermaster",
				ListGuid = "3c58e83a970a0f643a88e15f2323c805",
				Beat = DialogueBeats.WilcerAtYourService,
				ReplySpeaker = SpeakerOf("1e8d2770a79db2242a90d75df2d5a260"),
				ExpCr = 4,
				ClaimKey = "Port62:WilcerSupplyChain",
				Coins = 350,
				SixTwoSponsor = "The World Sovereign",
				Gold = 500
			}
		};
	}

	private static Port62SceneGraft[] MaskedGhost()
	{
		return new Port62SceneGraft[1]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiConundrumMatrixSolver",
				Scene = "c2_vs/GlobalPuzzle/MaskedGhost",
				ListGuid = "c2691373bfd094544a2d96578aae4f7c",
				Beat = DialogueBeats.MaskedGhostAsks,
				ExpCr = 4,
				ClaimKey = "Port62:ConundrumMatrixSolver",
				Coins = 450,
				SixTwoSponsor = "The Hermit",
				Gold = 600
			}
		};
	}

	private static Port62SceneGraft[] BottleneckGateBeforeFight()
	{
		return new Port62SceneGraft[1]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiChorussinaPortalSeverance",
				Scene = "c2_vs/DrezenSiege/Bottleneck_Gate_Before_Fight",
				ListGuid = "8b3bfe8703cffc44cb09cea58d8acc2c",
				Beat = DialogueBeats.DrezenGateStandoff,
				Narrated = true,
				ExpCr = 5
			}
		};
	}

	private static Port62SceneGraft[] Council()
	{
		return new Port62SceneGraft[1]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiDrezenWarCouncilThreePronged",
				Scene = "c2_vs/DrezenSiege/Council",
				ListGuid = "0c390d22c44f76c4aaa053ee987836d1",
				Beat = DialogueBeats.DrezenWarCouncil,
				ReplySpeaker = SpeakerOf("d1e567736abf23943b9f041ba7a0bc23"),
				RequiredCompanions = new string[3] { "0d37024170b172346b3769df92a971f5", "ae766624c03058440a036de90a7f2009", "cb29621d99b902e4da6f5d232352fbda" },
				ExpCr = 5,
				ClaimKey = "Port62:DrezenWarCouncilThreePronged",
				Coins = 500,
				SixTwoSponsor = "The Grand Arbiter",
				Gold = 800
			}
		};
	}

	private static Port62SceneGraft[] FinalBattleBefore()
	{
		return new Port62SceneGraft[5]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiStauntonMinaghoRedemption",
				Scene = "c2_vs/DrezenSiege/Final_Battle_Before",
				ListGuid = "c0a6d32e56866274bb86b0adcfab1dbe",
				Beat = DialogueBeats.StauntonAndMinaghoAtBay,
				ExpCr = 6,
				ClaimKey = "Port62:StauntonMinaghoRedemption",
				Coins = 600,
				SixTwoSponsor = "The Inheritor",
				Gold = 1000
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiStauntonMinaghoVillainDissection",
				Scene = "c2_vs/DrezenSiege/Final_Battle_Before",
				ListGuid = "c0a6d32e56866274bb86b0adcfab1dbe",
				Beat = DialogueBeats.StauntonAndMinaghoAtBay,
				Narrated = true,
				Gate = "VillainProficiencies",
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiStauntonMartialDuelOfShame",
				Scene = "c2_vs/DrezenSiege/Final_Battle_Before",
				ListGuid = "c0a6d32e56866274bb86b0adcfab1dbe",
				Beat = DialogueBeats.StauntonAndMinaghoAtBay,
				Narrated = true,
				Gate = "MartialGodProficiencies",
				Shift = AlignmentShiftDirection.Chaotic,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiStauntonHeroRedemptionPlea",
				Scene = "c2_vs/DrezenSiege/Final_Battle_Before",
				ListGuid = "c0a6d32e56866274bb86b0adcfab1dbe",
				Beat = DialogueBeats.StauntonAndMinaghoAtBay,
				Narrated = true,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiStauntonGodEmperorTreasonExecution",
				Scene = "c2_vs/DrezenSiege/Final_Battle_Before",
				ListGuid = "c0a6d32e56866274bb86b0adcfab1dbe",
				Beat = DialogueBeats.StauntonAndMinaghoAtBay,
				Narrated = true,
				Gate = "GodEmperorProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 6
			}
		};
	}

	private static Port62SceneGraft[] GalfreyArrives()
	{
		return new Port62SceneGraft[1]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiDrezenVictoryProclamation",
				Scene = "c3/Drezen_C3/GalfreyArrives",
				ListGuid = "44704bddb6223b84989dd26bcf20b601",
				Beat = DialogueBeats.GalfreyPraisesDrezen,
				ExpCr = 6,
				ClaimKey = "Port62:DrezenVictoryProclamation",
				Coins = 1000,
				SixTwoSponsor = "The Grand Arbiter",
				Gold = 1500,
				Feature = "SwordOfValorResonantFeature"
			}
		};
	}

	private static Port62SceneGraft[] SosielIntro()
	{
		return new Port62SceneGraft[3]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2SosielPortraitPose",
				Scene = "c2_vs/WarCamp/Sosiel_Intro",
				ListGuid = "366eb018f3667a6479526a9416c3b412",
				Beat = DialogueBeats.SosielSpeaksOfHisWork,
				ExpCr = 3,
				ClaimKey = "Port62:Act2SosielPortraitPose",
				Coins = 300,
				SixTwoSponsor = "The Song of the Spheres",
				ModItems = new string[1] { "ItemTalismanOfTheCosmicTourist" }
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2SosielMartialStillness",
				Scene = "c2_vs/WarCamp/Sosiel_Intro",
				ListGuid = "366eb018f3667a6479526a9416c3b412",
				Beat = DialogueBeats.SosielSpeaksOfHisWork,
				Gate = "MartialGodProficiencies",
				ExpCr = 4
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2SosielOverlordMajesty",
				Scene = "c2_vs/WarCamp/Sosiel_Intro",
				ListGuid = "366eb018f3667a6479526a9416c3b412",
				Beat = DialogueBeats.SosielSpeaksOfHisWork,
				Gate = "OverlordProficiencies",
				ExpCr = 4
			}
		};
	}

	private static Port62SceneGraft[] Seelah()
	{
		return new Port62SceneGraft[2]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2SeelahRationBanter",
				Scene = "c2_vs/Warcamp_IntroTour/Seelah",
				ListGuid = "3fafdb348338f5e4ebac24a8970494e7",
				Beat = DialogueBeats.SeelahToastsTheQueen,
				ReplySpeaker = SpeakerOf("54be53f0b35bf3c4592a97ae335fe765"),
				Companion = true,
				ExpCr = 3,
				ClaimKey = "Port62:Act2SeelahRationBanter",
				Coins = 250,
				SixTwoSponsor = "The Lucky Drunk"
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2SeelahHeroCheer",
				Scene = "c2_vs/Warcamp_IntroTour/Seelah",
				ListGuid = "3fafdb348338f5e4ebac24a8970494e7",
				Beat = DialogueBeats.SeelahToastsTheQueen,
				ReplySpeaker = SpeakerOf("54be53f0b35bf3c4592a97ae335fe765"),
				Companion = true,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 3
			}
		};
	}

	private static Port62SceneGraft[] JernaughWarcamp()
	{
		return new Port62SceneGraft[2]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2JernaughGermTheory",
				Scene = "c2_vs/Warcamp_IntroTour/Jernaugh_Warcamp",
				ListGuid = "e9355a3b814dbc24ea94193b093ba8e1",
				Beat = DialogueBeats.JernaughHub,
				ExpCr = 3,
				ClaimKey = "Port62:Act2JernaughGermTheory",
				Coins = 300,
				SixTwoSponsor = "The Grand Arbiter",
				VanillaItems = new string[2] { "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" }
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2JernaughSlimeSterilize",
				Scene = "c2_vs/Warcamp_IntroTour/Jernaugh_Warcamp",
				ListGuid = "e9355a3b814dbc24ea94193b093ba8e1",
				Beat = DialogueBeats.JernaughHub,
				Gate = "DevourerProficiencies",
				ExpCr = 4
			}
		};
	}

	private static Port62SceneGraft[] AneviaStart()
	{
		return new Port62SceneGraft[3]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2LostChapelTeaRations",
				Scene = "c2_vs/LostChapel/Anevia_Start",
				ListGuid = "364877496cda83640a9663b45ed0fc10",
				Beat = DialogueBeats.ChapelSurvivorsTended,
				Narrated = true,
				ExpCr = 3,
				ClaimKey = "Port62:Act2LostChapelTeaRations",
				Coins = 300,
				SixTwoSponsor = "The Dawnflower"
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2LostChapelHeroWarmth",
				Scene = "c2_vs/LostChapel/Anevia_Start",
				ListGuid = "364877496cda83640a9663b45ed0fc10",
				Beat = DialogueBeats.ChapelSurvivorsTended,
				Narrated = true,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 3
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2LostChapelAneviaCheck",
				Scene = "c2_vs/LostChapel/Anevia_Start",
				ListGuid = "364877496cda83640a9663b45ed0fc10",
				Beat = DialogueBeats.IrabethIsSafe,
				ExpCr = 3,
				ClaimKey = "Port62:Act2LostChapelAneviaCheck",
				Coins = 300,
				SixTwoSponsor = "The Song of the Spheres"
			}
		};
	}

	private static Port62SceneGraft[] IrabethRegill()
	{
		return new Port62SceneGraft[1]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2LostChapelCommandCalm",
				Scene = "c2_vs/LostChapel/IrabethRegill",
				ListGuid = "2f4e364807b094a4c8fdd1f9f8cfb1dd",
				Beat = DialogueBeats.RegillRebukesIrabeth,
				ReplySpeaker = SpeakerOf("68d4a5b150c7c2c42aeea6f24044113a"),
				ExpCr = 3,
				ClaimKey = "Port62:Act2LostChapelCommandCalm",
				Coins = 350,
				SixTwoSponsor = "The Grand Arbiter"
			}
		};
	}

	private static Port62SceneGraft[] FakeYanielFirst()
	{
		return new Port62SceneGraft[1]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct2DrezenYanielTheatrics",
				Scene = "c2_vs/DrezenSiege/FakeYaniel_First",
				ListGuid = "e47ef9ec640a1b4488ffb0c6a9983348",
				Beat = DialogueBeats.YanielTellsHerStory,
				ExpCr = 3,
				ClaimKey = "Port62:Act2DrezenYanielTheatrics",
				Coins = 350,
				SixTwoSponsor = "The Laughing King"
			}
		};
	}

	public static void Add()
	{
		Port("IsekaiQueenGalfreyLogisticsDoctrine", "(Isekai Protagonist) [Modern Military Logistics & Strategic Echelon Doctrine] \"Your Majesty, study this forward echelon scouting blueprint and modular supply schedule. In my home world, campaigns of attrition are won not through heroic martyrdom, but through redundant supply lines, rotating perimeter watches, and concentrated force projection. Under my command, the Fifth Crusade will breach Drezen's outer walls in twelve days without starving a single soldier.\"", "{n}Queen Galfrey pauses in genuine astonishment, her royal gaze sweeping across your immaculate tactical charts with growing reverence.{/n} \"Redundant supply lines and echelon rotations... by the Inheritor, this is not mere theoretical strategy; it is the doctrine of a true sovereign commander. Mendev has bled for a century under obsolete siege manuals, yet you distill the art of war into pure mathematical triumph. Wear this signet, Commander; let every officer know your word is absolute royal law.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"A true commander does not rely on miracles when logistics can conquer kingdoms!\"</i>"));
		Port("IsekaiQueenGalfreyImperialEdict", "(God Emperor) [Imperial Edict of the Fifth Crusade] \"Queen Galfrey, step down from the war dais in peace. Mendev's century of despair ends today under the sovereign banner of humanity. Drezen shall not merely be recaptured; the Worldwound itself shall be crushed beneath my imperial standard.\"", "{n}An overwhelming aura of celestial majesty radiates from your form, compelling even the warrior queen to fall silent in awe.{/n}");
		Port("IsekaiQueenGalfreyMastermindAudit", "(Mastermind) [Lawful] [Forensic Quartermaster Audit] \"Your Majesty, study these quartermaster margins. 18% of Mendevian grain is lost to mold due to non-ventilated carts. Adopt my compartmentalized storage schematics, and our march stamina doubles.\"", "{n}Queen Galfrey stares at the crisp numerical breakdown, then sets it aside for her heralds.{/n} \"Unbelievable... You exposed a century-old supply defect with a single sheet of parchment. Implement this protocol across all battalions immediately.\"");
		Port("IsekaiQueenGalfreyMartialChallenge", "(Martial God) [Chaotic] [Vanguard Sparring Challenge] \"Ceremonial speeches will not pierce gargoyle hide, Your Majesty. Grant me ten minutes in the sparring ring against your royal champions; I will show your vanguard how to redirect demon momentum before we cross the march line.\"", "{n}Galfrey arches a brow, her lips curling into a rare, intrigued smile.{/n} \"A commander who leads with their own bare knuckles... Show them, Commander. Let our knights witness what true otherworldly martial mastery looks like.\"");
		Port("IsekaiQueenGalfreyHeroVow", "(Hero) [Good] [Vow of the Undying Vanguard] \"Your Majesty, look at these young recruits from Kenabres. They answered the call because they believed in hope. I give you my sacred oath: under my command, no rear guard will be abandoned as expendable. We all return together.\"", "{n}Tears glisten in Galfrey's eyes.{/n} \"A vow of true chivalry... May your light guide their path, Commander. Mendev marches with you.\"");
		Port("IsekaiQueenGalfreyOverlordTithe", "(Overlord) [Evil] [Unforgiving March Mandate] \"Queen Galfrey, spare me moral platitudes. Those who collapse on the march are discarded. Drezen requires ruthless efficiency, not sentimental baggage. Let the weak fall where they stand.\"", "{n}Galfrey's expression hardens into icy royal resolve, yet she does not challenge your terrifying authority.{/n} \"A chilling decree, Commander. Pray your cold iron will does not shatter before we reach the fortress.\"");
		Port("IsekaiYakerRapidMobilization", "(Isekai Protagonist) [Rapid Reactionary Vanguard Deployment] \"Stand at ease, Armiger. Panic will not save your paralictor. We are already mobilizing a light cavalry strike force equipped with cold iron ballistas and incendiary charges. Lead the way to Reliable Redoubt; we will break the gargoyle siege before your lines collapse.\"", "{n}Yaker's panicked breathing hitches in shock. He stiffens into an ironclad Hellknight salute, his eyes wide with profound disbelief and newfound hope.{/n} \"Rapid mobile cavalry and incendiary ballistas?! By the Measure and the Chain, I expected bureaucratic delays from crusaders, not an immediate mechanized counter-offensive! Lead on, Commander! The Godclaw will witness your fury!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#7A1010><b>[The Constellation 'Our Lord in Iron']</b></color>: <i>\"Iron discipline meets lightning speed! The vanguard marches!\"</i>"));
		Port("IsekaiYakerFlashStepVanguard", "(Martial God) [Flash-Step Reconnaissance] \"No need to waste horses on rocky terrain. Point me toward the ridge; my stride outpaces the wind.\"", "{n}A blast of focused ki whips around your boots as Yaker stares slack-jawed at your unearthly velocity.{/n}");
		Port("IsekaiYakerMastermindChokepoint", "(Mastermind) [Lawful] [Topographical Defile Strategy] \"Armiger, catch your breath. The redoubt is situated between two basalt ridges. If we advance through the eastern ravine, the gargoyles lose altitude clearance and become funnel targets for our arbalests.\"", "{n}Yaker quickly sketches your route in the dirt, nodding in disbelief.{/n} \"The Measure dictates terrain exploitation, yet you solved our encirclement in five seconds! Proceeding immediately!\"");
		Port("IsekaiYakerHeroCharge", "(Hero) [Good] [Unwavering Rescue Charge] \"Hold on, Armiger! Tell your brothers in arms to form a defensive shell! We are charging right into the gorge to break the siege!\"", "{n}Yaker's exhausted frame trembles with renewed hope.{/n} \"Understood, Commander! We will hold until your standard arrives!\"");
		Port("IsekaiYakerOverlordContract", "(Overlord) [Evil] [Subjugation of the Besieged] \"Your Order prides itself on merciless order, yet here you crawl. Tell Paralictor Derenge that if I break this siege, his Hellknights owe their lives and swords to my private court.\"", "{n}Yaker gulps, bowing low despite his trembling knees.{/n} \"The Paralictor values survival and order above all else. Your terms will be delivered, Sovereign.\"");
		Port("IsekaiRegillFieldTriageDoctrine", "(Isekai Protagonist) [Tactical Efficiency Counter-Proposal: Otherworlder Field Triage] \"Sheathe your hook-hammer, Paralictor. Your execution arithmetic is dangerously primitive. In my world, combat triage prioritizes rapid stabilized clotting, tourniquets, and concentrated antiseptic poultices. These soldiers are seasoned veterans whose tactical value vastly exceeds the rations they consume. Apply my medical supplies right now; they will be back in formation within forty minutes.\"", "{n}Regill Derenge lowers his gnome hook-hammer by a fraction of an inch, his crimson eyes narrowing as he scrutinizes your modern medical doctrine with razor-sharp analytical precision. A tense, dead silence grips the blood-soaked gorge.{/n} \"Antiseptic clotting agents and stabilized tourniquets... to preserve hardened combat personnel rather than squandering resources on replenishment. Highly unorthodox... yet mathematically impeccable. Very well, Commander. Your logic supersedes standard field protocol. Administer the treatment; if they hold the line, their lives belong to your vanguard.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#7A1010><b>[The Constellation 'Our Lord in Iron']</b></color>: <i>\"Cold logic yields to superior otherworldly doctrine! Zero executions!\"</i>"));
		Port("IsekaiRegillOverlordReclamation", "(Overlord) [Sovereign Battlefield Reclamation] \"A foolish waste of mortal thralls, gnome. If their flesh breaks, my sovereign necromancy re-knits their marrow. Under my reign, none are discarded.\"", "{n}Dark sovereign filaments pulse from your fingertips, sealing the wounded soldiers' flesh with terrifying efficiency. Regill stares with cold fascination.{/n}");
		Port("IsekaiRegillMartialStanceCritique", "(Martial God) [Chaotic] [Critique of Godclaw Rigidity] \"Paralictor, your hooked hammer routines are textbook, but your footwork is far too rigid on uneven scree. If your soldiers had learned evasive flow instead of standing in stagnant lines, half these wounds wouldn't exist.\"", "{n}Regill's jaw tightens by a millimeter, observing your relaxed, perfectly balanced combat stance.{/n} \"Unorthodox footwork... yet undeniably effective against aerial dive angles. Your criticism is noted, Commander. Proceed with your tactics.\"");
		Port("IsekaiRegillHeroMercyRebuke", "(Hero) [Good] [Righteous Rebuke of Field Executions] \"Lower that hammer, Regill! Executing wounded soldiers who bled holding the line for you is cowardice masked as discipline! Step aside; I will heal them myself!\"", "{n}Regill steps back, his face completely expressionless, yet his eyes scrutinize your unwavering conviction.{/n} \"Sentimental defiance. If their wounds heal, they fight. If they fail, the responsibility rests upon your conscience, Commander.\"");
		Port("IsekaiRegillGodEmperorImperialPardon", "(God Emperor) [Lawful] [Imperial Sovereign Requisition] \"Stay your execution hammer, Paralictor. By sovereign decree of the Radiant Throne, these soldiers are pardoned and transferred to the imperial guard. Their lives belong to my empire, not your executioner block.\"", "{n}Regill slowly sheathes his weapon, bowing with rigid formal protocol.{/n} \"A sovereign claim backed by absolute authority supersedes standard military summary execution. They are yours, Commander.\"");
		Port("IsekaiGargoyleNightPerimeterStarlight", "(Isekai Protagonist) [Night Perimeter Starlight Flare & Decoy Ambush] \"Sound the defensive drums and ignite the high-altitude starlight pyres! Gargoyles hunt by thermal contrast against the cold night sky. Blank the camp with illuminated smoke, form phalanx shields around the hospital tents, and target the wings with cold iron nets! Drive these flying stone vultures into the mud!\"", "{n}Anevia's tense grimace transforms into a fierce grin as your tactical directives instantly cut through the midnight panic. Starlight flares burst high above the tents, turning the pitch-black sky blindingly bright and blinding the diving gargoyles as crusader arbalests shoot them from the air.{/n} \"They're blinded by the flares! Form ranks, third company! Box them in! By the gods, Commander, you turned an ambush into a turkey shoot!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"Spectral starlight illuminates the dark! The night ambush is shattered!\"</i>"));
		Port("IsekaiGargoyleMidnightShadowSeverance", "(Martial God) [Heaven-Splitting Sky Strike] \"Hunting from the clouds? You picked the wrong sky, gargoyles. Come down and face true martial mastery!\"", "{n}A shockwave of concentrated kinetic ki detonates upward into the mist, shattering gargoyle wings and sending stones plummeting into the dirt.{/n}");
		Port("IsekaiGargoyleMastermindAcousticTrap", "(Mastermind) [Lawful] [Acoustic Triangulation Crossfire] \"Archers, hold fire until the count of three! Gargoyles rely on wind sheer between pavilion ropes to target tent openings. Loose at grid coordinates Alpha-Four on my whistle!\"", "{n}A concentrated volley of cold iron bolts rips through the mist right as three gargoyles dive, pinning them to the dirt before they can snatch a single soldier.{/n}");
		Port("IsekaiGargoyleHeroHospitalShield", "(Hero) [Good] [Human Fortress of the Infirmary] \"Form a circle around the wounded! As long as my pulse beats, no flying beast will drag a helpless crusader into the night! Fight with me!\"", "{n}A radiant beacon of protective courage blazes from your shield, inspiring the panicking recruits to stand their ground with drawn steel!{/n}");
		Port("IsekaiGargoyleOverlordNecroticGrasp", "(Overlord) [Evil] [Necrotic Gravity Snare] \"Stone-winged vermin dare interrupt an Overlord's sleep? Let the earth drag your hollow bones into the mud! [Black necrotic chains erupt from the soil, snapping gargoyle wings mid-dive].\"", "{n}Terrified screeching echoes through the mist as the gargoyles are violently crushed against the gravel, their stone bodies pulverizing under dark pressure.{/n}");
		Port("IsekaiNurahPheromoneDecomposition", "(Isekai Protagonist) [Appraisal: Alchemical Pheromone Decomposition] \"Drop the charming scholar facade, Nurah. That scent lingering on your cloak isn't lavender oil; it's a hyper-concentrated extract of Vescavor queen pheromones mixed with fire-resistant alchemical accelerant. Hand over the vial, detail your communication channels with Staunton Vhane, and sit down before I freeze your hands to your sleeves.\"", "{n}Nurah's bubbly, scholarly smile stiffens, and for a heartbeat something sharp and frightened crosses her face before she laughs it away.{/n} \"A hyper-concentrated extract? Please! I am a historian, not an alchemist, and I told you: someone used my perfume to set me up.\" {n}Her small hands are shaking as she tucks them into her sleeves.{/n} \"Stop staring at me like that. It is not proof of anything.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"The hidden serpent is unmasked before its venom could poison the camp!\"</i>"));
		Port("IsekaiNurahSlimePheromoneDrain", "(Slime) [Chemical Scent Neutralization] \"Your pheromone vial is already empty, Nurah. My slime pseudopod siphoned and metabolized every volatile hydrocarbon while you were speaking.\"", "{n}Nurah frantically reaches into her satchel only to pull out a dry, glass shell, thoroughly dissolved clean. She stumbles back in terrified silence.{/n}");
		Port("IsekaiNurahMastermindHandwritingAudit", "(Mastermind) [Lawful] [Forensic Handwriting Discrepancy] \"Nurah, the handwriting on the camp's false scouting reports has the exact 12-degree leftward slant as your personal historical monographs. You made three grammatical errors common to Chelish slave ciphers. Your game is over.\"", "{n}Nurah freezes, her ink-stained fingers trembling as her eyes dart between your cold gaze and her confiscated notes.{/n} \"Chelish slave ciphers... You dissect my entire life from a margin note?! You monster!\"");
		Port("IsekaiNurahMartialPulseLock", "(Martial God) [Chaotic] [Pressure Point Meridian Lock] \"Your pulse spiked to 150 the moment I mentioned the vial. Don't reach for that dagger; I've already locked your shoulder meridians.\"", "{n}Nurah gasps as her right arm goes completely numb, the concealed stiletto clattering harmlessly to the carpet.{/n} \"What did you do to my arm?! You witch-doctor!\"");
		Port("IsekaiNurahGodEmperorImperialCondemnation", "(God Emperor) [Lawful] [Imperial Condemnation of Treason] \"Nurah, by sovereign imperial mandate, your petty conspiracies are laid bare before celestial light. Kneel and confess your treason, or face summary imperial judgment.\"", "{n}A crushing wave of regal authority forces Nurah to her knees, breaking through her spiteful sneer until tears of humiliation stream down her face.{/n} \"Curse your light... curse your crown!\"");
		Port("IsekaiLostChapelFlashRescue", "(Isekai Protagonist) [Sovereign Cleansing Aura: The Unbroken Vanguard] \"Abyssal carrion and desecrated dead, hear my decree! You have no jurisdiction over living souls! I unleash the untainted mana of an Otherworlder across this desecrated shrine--let sacred solar fire shatter your necrotic bonds, sear the flesh of the devourers, and restore these fallen crusaders to unbroken valor!\"", "{n}A tidal shockwave of blinding solar radiance erupts from your outstretched hand, incinerating the necrotic binding chains in a shower of golden sparks. The ghouls feigning death shriek as holy light scorches their rotting flesh and lurch upright, their ambush spoiled, while the captive crusaders weep in ecstatic relief and snatch up their weapons.{/n} \"The chains... they melted like morning frost! The Commander has brought Heaven's own wrath down upon this desecrated mountain! For the Commander! No retreat!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"Righteous fury breaks every profane chain! The lost are reclaimed!\"</i>"));
		Port("IsekaiLostChapelBeaconOfTheLost", "(Hero) [Beacon of the Lost] \"Hold fast, soldiers! As long as I draw breath, no evil shall claim you! Break your chains and fight!\"", "{n}Radiant angelic feathers swirl through the hall, restoring the captives' health and courage instantly.{/n}");
		Port("IsekaiLostChapelMartialGhoulSeverance", "(Martial God) [Chaotic] [Severing Ghoul Claws at Mach Speed] \"Rotting nails against transcendent martial technique? You fiends are too slow to graze my shadow! [A storm of kinetic palms meets the first ghoul before it can strike].\"", "{n}The captive crusaders cheer in astonishment as the nearest ghoul's claws shatter into bone fragments, and the rest scramble up from their false death!{/n}");
		Port("IsekaiLostChapelGodEmperorSolarPurge", "(God Emperor) [Lawful] [Solar Consecration of the Desecrated Mount] \"Profane undead, return to the dust! This ancient chapel is reclaimed in the name of the Solar Throne! Blinding dawnfire, consume the dark!\"", "{n}Blinding solar radiance sears the ghoul tormentors and drives them up from their feigned death, while the stone altar glows warm under celestial glory.{/n}");
		Port("IsekaiLostChapelMastermindAltarDecryption", "(Mastermind) [Lawful] [Deconstructing Necrotic Containment Glyphs] \"Notice the glyph symmetry on the sacrificial altars. Disconnect the corner keystone first; the necrotic feedback loop collapses and frees the prisoners without triggering the corpse traps.\"", "{n}Anevia and the scouts marvel as the glowing necrotic runes fizzle out harmlessly like damp wicks.{/n}");
		Port("IsekaiWilcerSupplyChain", "(Isekai Protagonist) [Standardized Quartermaster Protocol: High-Efficiency Supply Lines] \"Master Garms, review this standardized requisition matrix. If we package field rations into vacuum-sealed caloric bricks and standardize ammunition calibers across all auxiliary squads, wagon weight drops by 30% while march endurance increases twofold.\"", "{n}Wilcer Garms gasps, frantically adjusting his glasses as his ink-stained fingers trace your diagrams with boundless excitement.{/n} \"Caloric brick rations... standardized fletching calibers?! Merciful gods, Commander, this solves our entire transport bottleneck across the rocky wastes! With this overhaul, our vanguards will march twice as far with full bellies! You truly are a gift from beyond the stars! Here, take the first of the savings, five hundred in gold, with the quartermaster's thanks!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"Organized logistics feed the fires of victory!\"</i>"));
		Port("IsekaiConundrumMatrixSolver", "(Isekai Protagonist) [Meta-Logic: Runic Slab Topological Inversion] \"Spare me the cryptic pantomime, spirit. This runic grid is a straightforward 4x4 matrix permutation based on dual-axis diagonal parity. The dominoes align along rotational symmetry, unlocking the inner chamber mechanisms instantly.\"", "{n}The Masked Ghost's ethereal posture stiffens in utter, stunned incredulity. The spectral glow behind the golden mask wavers violently before the spirit bows with solemn reverence as ancient stone gears deep within the floor rumble to life.{/n} \"Diagonal... parity?! You solve the primordial puzzle of forgotten ages as though reading nursery rhymes... Stranger from afar, you perceive patterns that eluded Sarkorian archmages for millennia. The vault yields to your boundless intellect.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Ancient riddles unravel before otherworldly logic!\"</i>"));
		Port("IsekaiChorussinaPortalSeverance", "(Isekai Protagonist) [Dimensional Phalanx Breach] \"Crusaders, lock shields and brace for spatial redirection! I fold the kinetic impact of their boiling pitch and ballistas into the astral void! Charge the gatehouse now!\"", "{n}A glittering planar mirror absorbs the enemy catapult fire harmlessly into empty space, leaving the gateway wide open for your shock troops.{/n}");
		Port("IsekaiDrezenWarCouncilThreePronged", "(Isekai Protagonist) [Three-Pronged Synchronized Assault: The Masterstroke] \"We will not choose between half-measures. We execute a synchronized three-pronged assault! Regill leads heavy infantry to smash the temple distractors; Wenduag and Lann infiltrate the prison dungeons from below to liberate the captive smiths; while our main vanguard breaches the southern gates with heavy siege engines! We crush Drezen's garrison on every front at once!\"", "{n}The war tent erupts in stunned, electrifying energy. Regill nods with rare, unvarnished approval, Anevia slams her fist on the table in enthusiastic agreement, and Irabeth's eyes flare with fiery determination.{/n} \"Synchronized multi-front pressure! The demons won't know which breach to reinforce until our steel is at their throats! By the gods, Commander, we aren't just taking Drezen--we are sweeping it clean!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Flawless multi-front coordination! Drezen's walls tremble!\"</i>"));
		Port("IsekaiStauntonMinaghoRedemption", "(Isekai Protagonist) [Redemption of the Fallen Warden & Cosmic Defiance] \"Minagho, your parasitic dominion over Drezen ends today! And Staunton--seventy years of agonizing shame and self-hatred have blinded you to the truth! You were betrayed by a succubus, but your crime was naivety, not malice! The demons used you as a pawn and discarded you into the dirt! Turn your hammer against the monster that ruined your life and reclaim your honor before the Inheritor!\"", "{n}Staunton Vhane's hands tremble violently on the grip of his heavy hammer, his haggard, grief-ravaged face contorting in an agonizing tempest of realization and burning fury. For one heartbeat the dwarf's bloodshot eyes turn toward Minagho, and she shrieks in panicked fury. Then his grip tightens again.{/n} \"Seventy years... she whispered lies in the dark while mocking my disgrace! And still she is all I have left. It is too late for honor, Commander. By Torag's hammer, it ends here!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"The broken oath is reforged in righteous fury! Minagho shall fall!\"</i>"));
		Port("IsekaiStauntonMinaghoVillainDissection", "(Overlord) [Calculated Humiliation] \"Look at the two of you--a pathetic traitor and a disgraced lilitu clinging to stolen stone. Neither of you is worthy to rule this fortress. Die together in the dust.\"", "{n}Minagho's snarl freezes as cold terror grips her black heart, while Staunton roars in frantic desperation.{/n}");
		Port("IsekaiStauntonMartialDuelOfShame", "(Martial God) [Chaotic] [Duel of Seventy Years' Regret] \"Staunton! Your footwork is weighed down by seventy years of self-loathing! Lift your hammer and face me like a warrior, or die as Minagho's pet in the dirt!\"", "{n}Staunton roars, tears burning tracks through the grime on his face as he heaves his hammer with desperate, suicidal fury!{/n}");
		Port("IsekaiStauntonHeroRedemptionPlea", "(Hero) [Good] [Plea for a Fallen Soul] \"Staunton, don't let your story end as an abyssal footnote! Die defending Drezen, not serving the fiends who ruined your kin! You can still choose honor!\"", "{n}Staunton's knees tremble, his grip wavering as he stares at the banner of the Inheritor above the hall.{/n}");
		Port("IsekaiStauntonGodEmperorTreasonExecution", "(God Emperor) [Lawful] [Imperial Sentence of Treason] \"Staunton Vhane, your treason cost Mendev seventy years of blood. By celestial law, the sentence is death. Minagho, you shall be purged alongside your puppet.\"", "{n}Incandescent holy light flares along your blade, casting long shadows across the trembling traitors.{/n}");
		Port("IsekaiDrezenVictoryProclamation", "(Isekai Protagonist) [The Sovereign's Triumphant Proclamation: The Unconquered Citadel] \"Welcome to liberated Drezen, Your Majesty. The Sword of Valor flies above the citadel once more, cleansed of abyssal corruption and burning with otherworldly starlight. What Mendev lost seventy years ago has been reclaimed in blood and glory. Drezen is now our fortress, and from here, we will push the demonic hordes all the way back to the Abyss!\"", "{n}Queen Galfrey looks toward the window, where the radiant Sword of Valor flutters proudly against the northern sky, tears of overwhelming emotion welling in her eyes. Beyond the citadel walls, the garrison is still cheering your name.{/n} \"Drezen is ours... the banner is raised! In seventy years, no mortal army has achieved such an impossible triumph! You have not merely fulfilled your duty, Commander--you have rekindled the dying heart of the entire crusader cause!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"MAGNIFICENT VICTORY! Drezen is reclaimed and the Sword of Valor resonates across the multiverse! +1000 Cosmic Coins!\"</i>"));
		Port("IsekaiAct2SosielPortraitPose", "(Isekai Protagonist) [Iconic Cover Pose & Optical Lenses] \"If you're going to sketch my likeness, Sosiel, wait until I strike the classic light novel cover pose: one hand on my hilt, cloak billowing in an unfelt breeze, looking thoughtfully toward the horizon. In my world, portraits like this sell millions of volumes!\"", "{n}Sosiel blinks in stunned silence before breaking into soft, melodic laughter, his brush hovering over the canvas with genuine delight.{/n} \"A 'light novel cover pose'? You are truly an extraordinary commander! Most warriors demand to look stern and terrifying, but you treat destiny with infectious joy. Hold that stance--the morning light catching your cloak is truly magnificent! Take this lucky silver talisman as a token of my gratitude for being such an inspired subject!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"The heroic pose captured for all eternity! Starlight guides your legend!\"</i>"));
		Port("IsekaiAct2SosielMartialStillness", "(Martial God) [Cultivation Stillness] \"Capture the flow of internal ki. Notice how the breath roots into the earth, circulating without disturbance.\"", "{n}Sosiel nods in quiet awe, sketching the immaculate balance of your stance.{/n} \"The stillness in your center... It's like watching a mountain against the wind.\"");
		Port("IsekaiAct2SosielOverlordMajesty", "(Overlord) [Imperial Portraiture] \"Paint a portrait suited for a sovereign throne room, priest. The canvas should compel all who gaze upon it to kneel.\"", "{n}Sosiel's fingers tremble slightly against his easel.{/n} \"The sheer weight of your presence is unmistakable, Commander. I will do my finest.\"");
		PortCompanion("IsekaiAct2SeelahRationBanter", "(Isekai Protagonist) [Convenience Store Delicacies vs Iron Hardtack] \"Seelah, looking at these rock-hard crusader biscuits makes me miss convenience store instant noodles and fizzy cola so much I could weep. If I had five minutes with an electric kettle, I would revolutionize camp morale overnight!\"", "{n}Seelah laughs until her breastplate rattles, slapping her thigh in shared culinary agony.{/n} \"'Fizzy cola' and 'instant noodles'?! By the Inheritor, whatever miraculous delicacies your realm invented, they sound ten times better than chewing on dried mutton and hardtack! Let's strike a deal: the moment we liberate Drezen, you're the executive chef of our celebration banquet!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"Hardtack is an insult to mortal palates! Raise a cup to better feasts ahead!\"</i>"));
		PortCompanion("IsekaiAct2SeelahHeroCheer", "(Hero) [Good] [High-Spirited Vanguard] \"We march into the teeth of the Worldwound together, Seelah. Keep that radiant laugh loud and proud; it keeps the darkness at bay.\"", "{n}Seelah gives a jaunty salute with a glowing grin.{/n} \"Right beside you, Commander! Let's show these fiends what true crusaders can do!\"");
		Port("IsekaiAct2JernaughGermTheory", "(Isekai Protagonist) [Modern Germ Theory & Sterilization Protocols] \"Brother Jernaugh, before your clerics exhaust their healing spells on minor infections, boil your surgical scalpels, wash your hands in clean alcohol, and use clean boiled bandages. Invisible microscopic pathogens cause wound fever, not bad air.\"", "{n}Brother Jernaugh stares at you in stunned contemplation, rubbing his chin as the sheer revolutionary logic of antiseptic medicine dawns upon him.{/n} \"Invisible contagions on unwashed blades and hands?! Sweet Sarenrae... that explains why fever spreads even when demonic magic is absent! Washing in alcohol and boiling water costs mere pennies compared to divine restorations! You have just saved hundreds of frontline soldiers with simple wisdom, Commander! Take these blessed draughts!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Empirical hygiene reduces field mortality exponentially. Superior medical governance.\"</i>"));
		Port("IsekaiAct2JernaughSlimeSterilize", "(Slime) [Bio-Antiseptic Coating] \"A thin coating of purified digestive fluid neutralizes all bacterial decay on medical instruments instantly without dulling the edge.\"", "{n}Brother Jernaugh watches in astonishment as a corroded needle emerges sparkling clean.{/n} \"Remarkable! A natural antiseptic fluid! This will save countless limbs!\"");
		Port("IsekaiAct2YakerPunctuality", "(Isekai Protagonist) [Corporate Punctuality Commendation] \"Crossing enemy territory without armor or weapons and still arriving three minutes ahead of estimated schedule? Yaker, back in my world, operational efficiency like that earns you a rapid promotion to Senior Logistics Director. Catch your breath and take this focus band; we're marching to relieve the Paralictor right now.\"", "{n}Yaker stammers in disbelief, stiffening into an even more rigid salute as he accepts the headband with trembling hands.{/n} \"Senior... Logistics Director?! Sir, the Order of the Godclaw trains us never to yield to physical exhaustion, but your appreciation of operational tempo honors our discipline. I will guide your vanguard to the redoubt without delay!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"Valorous punctuality rewarded! March forth and crush the gargoyle brood!\"</i>"));
		Port("IsekaiAct2YakerMartialDiscipline", "(Martial God) [Iron Meridian Check] \"Your breathing is labored, but your core meridian remains unbent. Maintain that rhythm; we strike like lightning.\"", "{n}Yaker exhales sharply, steadying his stride.{/n} \"Understood, Commander! Lead on!\"");
		Port("IsekaiAct2NurahHistoricalTrivia", "(Isekai Protagonist) [Comparative Multiversal Historiography] \"Nurah, comparing the First Crusade's annals to the legendary siege wars of my home world is fascinating. Back home, whenever a fortress fell from within, it was almost always a disgruntled court intellectual sabotaging the gates. Good thing Mendevian scholars are purely devoted to the cause, right?\"", "{n}Nurah's hand twitches violently over her parchment, ink splattering across her notes. She forces a high-pitched, strained giggle, quickly brushing away the blot.{/n} \"Hahaha! What an... extraordinarily dark historical observation, Commander! A disgruntled court scholar? Perish the thought! We historians only record history; we certainly don't manufacture catastrophic disasters! Excuse me, I must... organize my index cards!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"The saboteur sweats under the gaze of an Otherworlder. Delicious psychological pressure.\"</i>"));
		Port("IsekaiAct2NurahMastermindDetect", "(Mastermind) [Forensic Micro-Expression Analysis] \"Your pulse elevated by twenty beats when I mentioned sabotaged gates, Nurah. Continue writing; I enjoy observing compromised pawns.\"", "{n}Nurah goes pale as ash, burying her nose in her notes in terrified silence.{/n}");
		Port("IsekaiAct2LostChapelTeaRations", "(Isekai Protagonist) [Warm Provisions & Morale Triage] \"Drink this piping hot spiced broth and wrap yourselves in these wool blankets. The gargoyle swarms are broken and the vanguard holds the hillside. Nobody else is dying on this mountain today.\"", "{n}The shivering, bloodstained crusaders weep with profound relief as the warm broth brings color back to their pale lips. Behind you, holy starlight pushes the mountain chill back into the gloom.{/n} \"The Commander... you came for us in the dark! Praise the Inheritor and praise the Otherworlder! We are saved!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Everbloom']</b></color>: <i>\"Mercy warms the freezing night. Your compassion guides lost souls back to life.\"</i>"));
		Port("IsekaiAct2LostChapelHeroWarmth", "(Hero) [Good] [Beacon of Deliverance] \"Stand tall, soldiers of Mendev! The nightmare is over! As long as my blade draws breath, you will never be forsaken in the dark!\"", "{n}A renewed cheer erupts from the rescued soldiers as hope rekindles in their eyes.{/n}");
		Port("IsekaiAct2LostChapelCommandCalm", "(Isekai Protagonist) [De-escalation of Command Friction] \"Cease the recriminations, both of you. Irabeth, surviving an ambush with your wits intact under torture is a victory, not a failure. Regill, your cold iron logic kept the perimeter from collapsing. We regroup our vanguards and take the summit together.\"", "{n}Irabeth releases a ragged, grateful breath, while Regill inclines his head in clinical acknowledgment.{/n} \"Understood, Commander. Tactical cohesion restored.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Sovereign leadership instantly resolves ideological division on the battlefield.\"</i>"));
		Port("IsekaiAct2LostChapelAneviaCheck", "(Isekai Protagonist) [Orthopedic Check-In] \"How is that leg holding up in this freezing mud, Anevia? If those splints are slipping, I have fresh alchemical wraps. Irabeth is safe, and the path to camp is secure.\"", "{n}Anevia wipes cold sweat from her brow, a weary but deeply affectionate smile breaking through her exhaustion.{/n} \"Leg's holding together thanks to your handiwork, Commander. And hearing that Beth is alive... that's all the medicine I need. Thank you for never giving up on us.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"Love and loyalty endure the darkest mountain night.\"</i>"));
		Port("IsekaiAct2DrezenYanielTheatrics", "(Isekai Protagonist) [Calling Out Theatrical Overacting] \"You lay the tragic martyr act on a little thick, 'Yaniel'. In my world, villains disguised as heroic martyrs usually get exposed in episode two by their over-rehearsed hand-wringing. Minagho's demonic perfume isn't completely masked by that holy aura.\"", "{n}The figure freezes mid-sob, her posture stiffening as her eyes flash with momentary fury before she hastily recomposes herself.{/n} \"Overacting?! What blasphemy is this?! I am Yaniel, chosen of the Inheritor! ...Do not doubt my sorrow, stranger!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Seeing through the demon's B-tier melodrama instantly! Magnifique!\"</i>"));
		AddSwarmBookAnswers();
	}

	private static void Port(string name, string answerText, string replyText, string line = null)
	{
		Port62SceneGraft graft = Port62SceneGraft.Find(Grafts, name, "Port62Act2");
		Condition[] array = Port62SceneGraft.Gates(graft, "Port62Act2");
		if (array != null)
		{
			BlueprintUnit blueprintUnit = (string.IsNullOrEmpty(graft.ReplySpeaker) ? null : BlueprintTools.GetBlueprint<BlueprintUnit>(graft.ReplySpeaker));
			GraftedAnswer graftedAnswer = GraftedAnswer.Named(name).At(graft.Beat).Says(answerText)
				.ShownWhen(array)
				.OnSelect(Port62SceneGraft.OnSelectActions(graft, line))
				.Configure(delegate(BlueprintAnswer bp)
				{
					Port62SceneGraft.Configure(bp, graft);
				})
				.RepliedToBy(name + "Reply", replyText);
			if (blueprintUnit != null)
			{
				graftedAnswer.SpokenBy(blueprintUnit);
			}
			else if (graft.Narrated)
			{
				graftedAnswer.Narrated();
			}
			graftedAnswer.WithSpeakerlessTwin(name + "ReplyNoSpeaker");
			graftedAnswer.ThenBackToTheList();
			graftedAnswer.Graft();
		}
	}

	private static void PortCompanion(string name, string answerText, string replyText, string line = null)
	{
		Port62SceneGraft graft = Port62SceneGraft.Find(Grafts, name, "Port62Act2");
		Condition[] array = Port62SceneGraft.Gates(graft, "Port62Act2");
		if (array == null)
		{
			return;
		}
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>(graft.ReplySpeaker);
		if (blueprint == null)
		{
			Main.IsekaiContext.Logger.LogWarning("Port62Act2: companion " + graft.ReplySpeaker + " not found, skipping " + name + ".");
		}
		else
		{
			GraftedAnswer.Named(name).At(graft.Beat).Says(answerText)
				.ShownWhen(array)
				.OnSelect(Port62SceneGraft.OnSelectActions(graft, line))
				.Configure(delegate(BlueprintAnswer bp)
				{
					Port62SceneGraft.Configure(bp, graft);
				})
				.RepliedToBy(name + "Reply", replyText)
				.SpokenByCompanion(blueprint)
				.WithSpeakerlessTwin(name + "ReplyNoSpeaker")
				.ThenBackToTheList()
				.Graft();
		}
	}

	private static void AddSwarmBookAnswers()
	{
		BlueprintBookPage blueprint = BlueprintTools.GetBlueprint<BlueprintBookPage>("7be206b993f3dea49a89a88ce5e6ce6f");
		if (blueprint == null)
		{
			Main.IsekaiContext.Logger.LogWarning("Port62Act2: SwarmProblem_BE BookPage_0001 was not found; the swarm book answers are left out.");
			return;
		}
		BlueprintAnswer blueprintAnswer = BookAnswer("IsekaiLepersSmileSpatialStasis", "(Isekai Protagonist) [Spatial Stasis Extraction: The Zero-Casualty Protocol] \"Hold the army on the high ridge and form a thermal barricade of cold iron and Greek fire! I will plunge into the ravine alone, enveloped in an Otherworlder spatial stasis barrier. While the swarm fruitlessly breaks against my impenetrable aegis, my dimensional storage will retrieve every submerged supply wagon, relic, and trapped scout without sacrificing a single crusader's life!\"", blueprint, ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"Retrieving the sunken treasure without losing a single drop of soldier blood! Legendary!\"</i>"));
		BlueprintAnswer blueprintAnswer2 = BookAnswer("IsekaiLepersSmileMartialSoloSweep", "(Martial God) [Chaotic] [Ki-Barrier Solo Cultivation] \"A boiling canyon of flesh-eating insects? The ultimate trial for my kinetic ki shield! Hold the army back; I will carve a clean path to the queen with bare hands!\"", blueprint, null);
		BlueprintAnswer blueprintAnswer3 = BookAnswer("IsekaiLepersSmileMastermindAcousticRupture", "(Mastermind) [Lawful] [Resonant Ultrasonic Decoy] \"Tune our war horns to high pitch G-sharp and blast the canyon walls. The ultrasonic resonance will scramble the Vescavors' antennal sensors and force them to turn upon each other!\"", blueprint, null);
		BlueprintAnswer blueprintAnswer4 = BookAnswer("IsekaiLepersSmileHeroRearguardAegis", "(Hero) [Good] [Vanguard Aegis for the Baggage Train] \"Form ranks! I will lead the front row with my shield raised, intercepting every swarm cloud so the baggage handlers can withdraw safely!\"", blueprint, null);
		BlueprintAnswer blueprintAnswer5 = BookAnswer("IsekaiLepersSmileOverlordExpendableMarch", "(Overlord) [Evil] [Calculated Vanguard Sacrifice] \"Order the forward ranks to march straight through the canyon. The insects will exhaust their mandibles on the vanguard while our primary forces advance unhindered. Move!\"", blueprint, null);
		if (blueprintAnswer != null)
		{
			blueprint.Answers?.Add(blueprintAnswer.ToReference<BlueprintAnswerBaseReference>());
		}
		if (blueprintAnswer2 != null)
		{
			blueprint.Answers?.Add(blueprintAnswer2.ToReference<BlueprintAnswerBaseReference>());
		}
		if (blueprintAnswer3 != null)
		{
			blueprint.Answers?.Add(blueprintAnswer3.ToReference<BlueprintAnswerBaseReference>());
		}
		if (blueprintAnswer4 != null)
		{
			blueprint.Answers?.Add(blueprintAnswer4.ToReference<BlueprintAnswerBaseReference>());
		}
		if (blueprintAnswer5 != null)
		{
			blueprint.Answers?.Add(blueprintAnswer5.ToReference<BlueprintAnswerBaseReference>());
		}
		BlueprintAnswer[] answers = new BlueprintAnswer[5] { blueprintAnswer, blueprintAnswer2, blueprintAnswer3, blueprintAnswer4, blueprintAnswer5 }.Where((BlueprintAnswer a) => a != null).ToArray();
		if (answers.Length == 0)
		{
			return;
		}
		BlueprintCue bp = TTCoreExtensions.CreateCue("IsekaiLepersSmileReplyCue", delegate(BlueprintCue blueprintCue)
		{
			blueprintCue.SetText(Main.IsekaiContext, "{n}Your word travels down the line and the crusaders hold their ground above the canyon. The swarm keeps up its wet, chittering churn below, and for a moment nothing moves but the dust. The Queen's officers look to you to say what happens next.{/n}");
			blueprintCue.Speaker = new DialogSpeaker
			{
				NoSpeaker = true
			};
			blueprintCue.TurnSpeaker = false;
			blueprintCue.ShowOnce = false;
			blueprintCue.Conditions = new ConditionsChecker
			{
				Operation = Operation.Or,
				Conditions = ((IEnumerable<BlueprintAnswer>)answers).Select((Func<BlueprintAnswer, Condition>)((BlueprintAnswer a) => new AnswerSelected
				{
					Not = false,
					m_Answer = a.ToReference<BlueprintAnswerReference>()
				})).ToArray()
			};
		});
		blueprint.Cues?.Add(bp.ToReference<BlueprintCueBaseReference>());
	}

	private static BlueprintAnswer BookAnswer(string name, string text, BlueprintBookPage nextPage, string line)
	{
		Port62SceneBookGraft graft = BookGrafts.FirstOrDefault((Port62SceneBookGraft g) => g.Answer == name);
		if (graft == null)
		{
			Main.IsekaiContext.Logger.LogError("Port62Act2: '" + name + "' has no book row, skipping.");
			return null;
		}
		Condition gate = Port62Rewards.Gate(graft.Gate);
		if (gate == null)
		{
			Main.IsekaiContext.Logger.LogWarning("Port62Act2: gate " + graft.Gate + " not found, skipping " + name + ".");
			return null;
		}
		return TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
		{
			bp.SetText(Main.IsekaiContext, text);
			bp.NextCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference> { nextPage.ToReference<BlueprintCueBaseReference>() },
				Strategy = Strategy.First
			};
			bp.ShowOnce = true;
			bp.OnSelect = Helpers.CreateActionList(GuardedGameAction.GuardAll(Port62SceneBookGraft.OnSelectActions(graft, line)));
			bp.ShowConditions.Conditions = bp.ShowConditions.Conditions.AppendToArray(gate);
		});
	}
}
