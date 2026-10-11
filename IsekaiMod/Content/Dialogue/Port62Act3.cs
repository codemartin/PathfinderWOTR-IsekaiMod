using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62Act3
{
	private const string CureModerateWounds = "f991f3051c3b9e64fabc87891077b613";

	internal static readonly Port62SceneGraft[] Grafts = new Port62SceneGraft[18][]
	{
		IrabethC3Intro(),
		Herald(),
		HeartOfLab(),
		JerribethReveal(),
		GrimborAmbush(),
		Kyadomaindialogue(),
		FortressArushaEnd(),
		Vorimeraak(),
		Xanthirbefore(),
		GalfreyArrives(),
		FaneEntrance(),
		BartenderFye(),
		StoryTellerMainDialogue(),
		CiarMainSquare(),
		VendorArsinoe(),
		JoranInPresentMain(),
		Deserter(),
		GateGuard()
	}.SelectMany((Port62SceneGraft[] scene) => scene).ToArray();

	private static string SpeakerOf(string unitGuid)
	{
		return unitGuid;
	}

	private static Port62SceneGraft[] IrabethC3Intro()
	{
		return new Port62SceneGraft[6]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiDrezenIntroImperialMandate",
				Scene = "c3/Drezen_C3/Irabeth_C3_Intro",
				ListGuid = "143d77815c045dd4abf5e81e948393b6",
				Beat = DialogueBeats.IrabethReportsOnDrezen,
				Narrated = true,
				Gate = "GodEmperorProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiDrezenIntroMartialCrucible",
				Scene = "c3/Drezen_C3/Irabeth_C3_Intro",
				ListGuid = "143d77815c045dd4abf5e81e948393b6",
				Beat = DialogueBeats.IrabethReportsOnDrezen,
				Narrated = true,
				Gate = "MartialGodProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiDrezenIntroOverlordDominion",
				Scene = "c3/Drezen_C3/Irabeth_C3_Intro",
				ListGuid = "143d77815c045dd4abf5e81e948393b6",
				Beat = DialogueBeats.IrabethReportsOnDrezen,
				Narrated = true,
				Gate = "OverlordProficiencies",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiDrezenIntroMastermindLogistics",
				Scene = "c3/Drezen_C3/Irabeth_C3_Intro",
				ListGuid = "143d77815c045dd4abf5e81e948393b6",
				Beat = DialogueBeats.IrabethReportsOnDrezen,
				Gate = "MastermindProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiDrezenIntroHeroLiberationCry",
				Scene = "c3/Drezen_C3/Irabeth_C3_Intro",
				ListGuid = "143d77815c045dd4abf5e81e948393b6",
				Beat = DialogueBeats.IrabethReportsOnDrezen,
				Narrated = true,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3IrabethWellnessCheck",
				Scene = "c3/Drezen_C3/Irabeth_C3_Intro",
				ListGuid = "143d77815c045dd4abf5e81e948393b6",
				Beat = DialogueBeats.IrabethReportsOnDrezen,
				ExpCr = 5,
				ClaimKey = "Port62:Act3IrabethWellnessCheck",
				Coins = 300,
				SixTwoSponsor = "The Dawnflower"
			}
		};
	}

	private static Port62SceneGraft[] Herald()
	{
		return new Port62SceneGraft[6]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiHeraldPlanarCosmology",
				Scene = "c3/Drezen_C3/Herald",
				ListGuid = "b9c56617dec2b7d42aea7b1c33fe5cbd",
				Beat = DialogueBeats.HeraldKneels,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiHeraldAngelSovereignty",
				Scene = "c3/Drezen_C3/Herald",
				ListGuid = "b9c56617dec2b7d42aea7b1c33fe5cbd",
				Beat = DialogueBeats.HeraldKneels,
				Gate = "GodEmperorProficiencies",
				Mythic = "a5a9fe8f663d701488bd1db8ea40484e",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiHeraldMartialSparChallenge",
				Scene = "c3/Drezen_C3/Herald",
				ListGuid = "b9c56617dec2b7d42aea7b1c33fe5cbd",
				Beat = DialogueBeats.HeraldKneels,
				Gate = "MartialGodProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiHeraldAeonJurisdiction",
				Scene = "c3/Drezen_C3/Herald",
				ListGuid = "b9c56617dec2b7d42aea7b1c33fe5cbd",
				Beat = DialogueBeats.HeraldKneels,
				Gate = "MastermindProficiencies",
				Mythic = "15a85e67b7d69554cab9ed5830d0268e",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiHeraldOverlordPragmatism",
				Scene = "c3/Drezen_C3/Herald",
				ListGuid = "b9c56617dec2b7d42aea7b1c33fe5cbd",
				Beat = DialogueBeats.HeraldKneels,
				Gate = "OverlordProficiencies",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiHeraldAzataJoyousAffirmation",
				Scene = "c3/Drezen_C3/Herald",
				ListGuid = "b9c56617dec2b7d42aea7b1c33fe5cbd",
				Beat = DialogueBeats.HeraldKneels,
				Gate = "HeroProficiencies",
				Mythic = "9a3b2c63afa79744cbca46bea0da9a16",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			}
		};
	}

	private static Port62SceneGraft[] HeartOfLab()
	{
		return new Port62SceneGraft[6]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAreeluLabCheatCodeMockery",
				Scene = "c3/AreeluLaboratory/HeartOfLab",
				ListGuid = "cd9c9facc3a8ded4e9683cde8958295e",
				Beat = DialogueBeats.SutureOffersTheLexicon,
				Narrated = true,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAreeluLabAeonEquationSunder",
				Scene = "c3/AreeluLaboratory/HeartOfLab",
				ListGuid = "cd9c9facc3a8ded4e9683cde8958295e",
				Beat = DialogueBeats.SutureOffersTheLexicon,
				Narrated = true,
				Gate = "MastermindProficiencies",
				Mythic = "15a85e67b7d69554cab9ed5830d0268e",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAreeluLabLegendMartialStrike",
				Scene = "c3/AreeluLaboratory/HeartOfLab",
				ListGuid = "cd9c9facc3a8ded4e9683cde8958295e",
				Beat = DialogueBeats.SutureOffersTheLexicon,
				Narrated = true,
				Gate = "MartialGodProficiencies",
				Mythic = "3d420403f3e7340499931324640efe96",
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAreeluLabLichNahyndrianDrain",
				Scene = "c3/AreeluLaboratory/HeartOfLab",
				ListGuid = "cd9c9facc3a8ded4e9683cde8958295e",
				Beat = DialogueBeats.SutureOffersTheLexicon,
				Narrated = true,
				Gate = "OverlordProficiencies",
				Mythic = "5d501618a28bdc24c80007a5c937dcb7",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAreeluLabAngelSolarPurge",
				Scene = "c3/AreeluLaboratory/HeartOfLab",
				ListGuid = "cd9c9facc3a8ded4e9683cde8958295e",
				Beat = DialogueBeats.SutureOffersTheLexicon,
				Narrated = true,
				Gate = "GodEmperorProficiencies",
				Mythic = "a5a9fe8f663d701488bd1db8ea40484e",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAreeluLabHeroPurityVow",
				Scene = "c3/AreeluLaboratory/HeartOfLab",
				ListGuid = "cd9c9facc3a8ded4e9683cde8958295e",
				Beat = DialogueBeats.SutureOffersTheLexicon,
				Narrated = true,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			}
		};
	}

	private static Port62SceneGraft[] JerribethReveal()
	{
		return new Port62SceneGraft[6]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiWintersunIllusionMockery",
				Scene = "c3/Wintersun/JerribethReveal",
				ListGuid = "f240afdd8595d2340aa18ecf5ed233a3",
				Beat = DialogueBeats.JerribethNamesHerself,
				Narrated = true,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiWintersunMastermindAeonDispersion",
				Scene = "c3/Wintersun/JerribethReveal",
				ListGuid = "f240afdd8595d2340aa18ecf5ed233a3",
				Beat = DialogueBeats.JerribethNamesHerself,
				Narrated = true,
				Gate = "MastermindProficiencies",
				Mythic = "15a85e67b7d69554cab9ed5830d0268e",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiWintersunGodEmperorRadiantTruth",
				Scene = "c3/Wintersun/JerribethReveal",
				ListGuid = "f240afdd8595d2340aa18ecf5ed233a3",
				Beat = DialogueBeats.JerribethNamesHerself,
				Narrated = true,
				Gate = "GodEmperorProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiWintersunMartialGodKillingIntent",
				Scene = "c3/Wintersun/JerribethReveal",
				ListGuid = "f240afdd8595d2340aa18ecf5ed233a3",
				Beat = DialogueBeats.JerribethNamesHerself,
				Narrated = true,
				Gate = "MartialGodProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiWintersunOverlordDominationDemand",
				Scene = "c3/Wintersun/JerribethReveal",
				ListGuid = "f240afdd8595d2340aa18ecf5ed233a3",
				Beat = DialogueBeats.JerribethNamesHerself,
				Narrated = true,
				Gate = "OverlordProficiencies",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiWintersunHeroAzataAwakening",
				Scene = "c3/Wintersun/JerribethReveal",
				ListGuid = "f240afdd8595d2340aa18ecf5ed233a3",
				Beat = DialogueBeats.JerribethNamesHerself,
				Narrated = true,
				Gate = "HeroProficiencies",
				Mythic = "9a3b2c63afa79744cbca46bea0da9a16",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			}
		};
	}

	private static Port62SceneGraft[] GrimborAmbush()
	{
		return new Port62SceneGraft[4]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiDragonhuntFlightTrajectory",
				Scene = "Companions/CompanionQuests/Grimbor/DragonHunt/GrimborAmbush",
				ListGuid = "feac1861da9b5ae4b8e2629b84cac14e",
				Beat = DialogueBeats.GreyborPlansTheAmbush,
				ReplySpeaker = SpeakerOf("f72bb7c48bb3e45458f866045448fb58"),
				Companion = true,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiDragonhuntMartialGodSingleStroke",
				Scene = "Companions/CompanionQuests/Grimbor/DragonHunt/GrimborAmbush",
				ListGuid = "feac1861da9b5ae4b8e2629b84cac14e",
				Beat = DialogueBeats.GreyborPlansTheAmbush,
				ReplySpeaker = SpeakerOf("f72bb7c48bb3e45458f866045448fb58"),
				Companion = true,
				Gate = "MartialGodProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiDragonhuntOverlordTrophyClaim",
				Scene = "Companions/CompanionQuests/Grimbor/DragonHunt/GrimborAmbush",
				ListGuid = "feac1861da9b5ae4b8e2629b84cac14e",
				Beat = DialogueBeats.GreyborPlansTheAmbush,
				ReplySpeaker = SpeakerOf("f72bb7c48bb3e45458f866045448fb58"),
				Companion = true,
				Gate = "OverlordProficiencies",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3DragonhuntRaidMechanics",
				Scene = "Companions/CompanionQuests/Grimbor/DragonHunt/GrimborAmbush",
				ListGuid = "feac1861da9b5ae4b8e2629b84cac14e",
				Beat = DialogueBeats.GreyborPlansTheAmbush,
				ReplySpeaker = SpeakerOf("f72bb7c48bb3e45458f866045448fb58"),
				Companion = true,
				ExpCr = 4,
				ClaimKey = "Port62:Act3DragonhuntRaidMechanics",
				Coins = 350,
				SixTwoSponsor = "The Lucky Drunk"
			}
		};
	}

	private static Port62SceneGraft[] Kyadomaindialogue()
	{
		return new Port62SceneGraft[3]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiKyadoTruthExtraction",
				Scene = "c3/TempleOfDelamere/Kyado_main_dialogue",
				ListGuid = "d7aadaa261a8023489bdba49216cc334",
				Beat = DialogueBeats.KyadoHidesTheCrypt,
				ExpCr = 4
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiKyadoMastermindContractDissection",
				Scene = "c3/TempleOfDelamere/Kyado_main_dialogue",
				ListGuid = "d7aadaa261a8023489bdba49216cc334",
				Beat = DialogueBeats.KyadoHidesTheCrypt,
				Narrated = true,
				Gate = "MastermindProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiKyadoHeroRedemptionOffer",
				Scene = "c3/TempleOfDelamere/Kyado_main_dialogue",
				ListGuid = "d7aadaa261a8023489bdba49216cc334",
				Beat = DialogueBeats.KyadoHidesTheCrypt,
				Narrated = true,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 5
			}
		};
	}

	private static Port62SceneGraft[] FortressArushaEnd()
	{
		return new Port62SceneGraft[2]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiGreengatesArueshalaeAffirmation",
				Scene = "c3/ArueshalaeRedoubt/Fortress_Arusha_End",
				ListGuid = "6e5d9e9370306194ca15420193bae14d",
				Beat = DialogueBeats.ArueshalaeAtTheGrave,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiGreengatesHeroAzataSong",
				Scene = "c3/ArueshalaeRedoubt/Fortress_Arusha_End",
				ListGuid = "6e5d9e9370306194ca15420193bae14d",
				Beat = DialogueBeats.ArueshalaeAtTheGrave,
				Narrated = true,
				Gate = "HeroProficiencies",
				Mythic = "9a3b2c63afa79744cbca46bea0da9a16",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			}
		};
	}

	private static Port62SceneGraft[] Vorimeraak()
	{
		return new Port62SceneGraft[5]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiMoltenScarVrockMockery",
				Scene = "c3/MoltenScar/Vorimeraak",
				ListGuid = "a01c59afcc9038c46a8bee2de45427cf",
				Beat = DialogueBeats.VorimeraakBoasts,
				Narrated = true,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiMoltenScarMartialGodDecapitation",
				Scene = "c3/MoltenScar/Vorimeraak",
				ListGuid = "a01c59afcc9038c46a8bee2de45427cf",
				Beat = DialogueBeats.VorimeraakBoasts,
				Narrated = true,
				Gate = "MartialGodProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiMoltenScarGodEmperorExecution",
				Scene = "c3/MoltenScar/Vorimeraak",
				ListGuid = "a01c59afcc9038c46a8bee2de45427cf",
				Beat = DialogueBeats.VorimeraakBoasts,
				Narrated = true,
				Gate = "GodEmperorProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiMoltenScarHeroRescueVow",
				Scene = "c3/MoltenScar/Vorimeraak",
				ListGuid = "a01c59afcc9038c46a8bee2de45427cf",
				Beat = DialogueBeats.VorimeraakBoasts,
				Narrated = true,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiMoltenScarOverlordTorment",
				Scene = "c3/MoltenScar/Vorimeraak",
				ListGuid = "a01c59afcc9038c46a8bee2de45427cf",
				Beat = DialogueBeats.VorimeraakBoasts,
				Narrated = true,
				Gate = "OverlordProficiencies",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 5
			}
		};
	}

	private static Port62SceneGraft[] Xanthirbefore()
	{
		return new Port62SceneGraft[7]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiXanthirSwarmVacuum",
				Scene = "c3/IvorySanctum/Xanthir_before",
				ListGuid = "cbe3b0c3ea8b47045b66c085c1a1b354",
				Beat = DialogueBeats.XanthirRailsAtHisServants,
				ReplySpeaker = SpeakerOf("082edd80ca45af84ab602fbe06e53f55"),
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiXanthirSlimeDigestion",
				Scene = "c3/IvorySanctum/Xanthir_before",
				ListGuid = "cbe3b0c3ea8b47045b66c085c1a1b354",
				Beat = DialogueBeats.XanthirRailsAtHisServants,
				Narrated = true,
				Gate = "DevourerProficiencies",
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiXanthirAngelSolarFire",
				Scene = "c3/IvorySanctum/Xanthir_before",
				ListGuid = "cbe3b0c3ea8b47045b66c085c1a1b354",
				Beat = DialogueBeats.XanthirRailsAtHisServants,
				Narrated = true,
				Gate = "GodEmperorProficiencies",
				Mythic = "a5a9fe8f663d701488bd1db8ea40484e",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiXanthirLegendMartialCleave",
				Scene = "c3/IvorySanctum/Xanthir_before",
				ListGuid = "cbe3b0c3ea8b47045b66c085c1a1b354",
				Beat = DialogueBeats.XanthirRailsAtHisServants,
				Narrated = true,
				Gate = "MartialGodProficiencies",
				Mythic = "3d420403f3e7340499931324640efe96",
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiXanthirMastermindPheromoneDisruption",
				Scene = "c3/IvorySanctum/Xanthir_before",
				ListGuid = "cbe3b0c3ea8b47045b66c085c1a1b354",
				Beat = DialogueBeats.XanthirRailsAtHisServants,
				Narrated = true,
				Gate = "MastermindProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiXanthirLichResearchHarvest",
				Scene = "c3/IvorySanctum/Xanthir_before",
				ListGuid = "cbe3b0c3ea8b47045b66c085c1a1b354",
				Beat = DialogueBeats.XanthirRailsAtHisServants,
				Narrated = true,
				Gate = "OverlordProficiencies",
				Mythic = "5d501618a28bdc24c80007a5c937dcb7",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiXanthirHeroAvengerVow",
				Scene = "c3/IvorySanctum/Xanthir_before",
				ListGuid = "cbe3b0c3ea8b47045b66c085c1a1b354",
				Beat = DialogueBeats.XanthirRailsAtHisServants,
				Narrated = true,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			}
		};
	}

	private static Port62SceneGraft[] GalfreyArrives()
	{
		return new Port62SceneGraft[6]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiGalfreyArrivesTricksterHumor",
				Scene = "c3/Drezen_C3/GalfreyArrives",
				ListGuid = "44704bddb6223b84989dd26bcf20b601",
				Beat = DialogueBeats.GalfreyVisitsDrezen,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiGalfreyArrivesGodEmperorPeers",
				Scene = "c3/Drezen_C3/GalfreyArrives",
				ListGuid = "44704bddb6223b84989dd26bcf20b601",
				Beat = DialogueBeats.GalfreyVisitsDrezen,
				Narrated = true,
				Gate = "GodEmperorProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiGalfreyArrivesMartialGodAssurance",
				Scene = "c3/Drezen_C3/GalfreyArrives",
				ListGuid = "44704bddb6223b84989dd26bcf20b601",
				Beat = DialogueBeats.GalfreyPlansTheFane,
				Gate = "MartialGodProficiencies",
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiGalfreyArrivesOverlordWarning",
				Scene = "c3/Drezen_C3/GalfreyArrives",
				ListGuid = "44704bddb6223b84989dd26bcf20b601",
				Beat = DialogueBeats.GalfreyPlansTheFane,
				Gate = "OverlordProficiencies",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiGalfreyArrivesMastermindReport",
				Scene = "c3/Drezen_C3/GalfreyArrives",
				ListGuid = "44704bddb6223b84989dd26bcf20b601",
				Beat = DialogueBeats.GalfreyPlansTheFane,
				Gate = "MastermindProficiencies",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 5
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiGalfreyArrivesHeroGratitude",
				Scene = "c3/Drezen_C3/GalfreyArrives",
				ListGuid = "44704bddb6223b84989dd26bcf20b601",
				Beat = DialogueBeats.GalfreyVisitsDrezen,
				Gate = "HeroProficiencies",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 5
			}
		};
	}

	private static Port62SceneGraft[] FaneEntrance()
	{
		return new Port62SceneGraft[6]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiFaneBreachTricksterDive",
				Scene = "c3/MidnightFane/FaneEntrance",
				ListGuid = "d8d8512aa3c48a342a68e9873f92e88e",
				Beat = DialogueBeats.FaneThreshold,
				Narrated = true,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiFaneBreachAngelImperialConquest",
				Scene = "c3/MidnightFane/FaneEntrance",
				ListGuid = "d8d8512aa3c48a342a68e9873f92e88e",
				Beat = DialogueBeats.FaneThreshold,
				Narrated = true,
				Gate = "GodEmperorProficiencies",
				Mythic = "a5a9fe8f663d701488bd1db8ea40484e",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiFaneBreachLegendMartialPinnacle",
				Scene = "c3/MidnightFane/FaneEntrance",
				ListGuid = "d8d8512aa3c48a342a68e9873f92e88e",
				Beat = DialogueBeats.FaneThreshold,
				Narrated = true,
				Gate = "MartialGodProficiencies",
				Mythic = "3d420403f3e7340499931324640efe96",
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiFaneBreachLichDreadVanguard",
				Scene = "c3/MidnightFane/FaneEntrance",
				ListGuid = "d8d8512aa3c48a342a68e9873f92e88e",
				Beat = DialogueBeats.FaneThreshold,
				Narrated = true,
				Gate = "OverlordProficiencies",
				Mythic = "5d501618a28bdc24c80007a5c937dcb7",
				Shift = AlignmentShiftDirection.Evil,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiFaneBreachMastermindAeonCalculus",
				Scene = "c3/MidnightFane/FaneEntrance",
				ListGuid = "d8d8512aa3c48a342a68e9873f92e88e",
				Beat = DialogueBeats.FaneThreshold,
				Narrated = true,
				Gate = "MastermindProficiencies",
				Mythic = "15a85e67b7d69554cab9ed5830d0268e",
				Shift = AlignmentShiftDirection.Lawful,
				ExpCr = 6
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiFaneBreachHeroAzataDawnMarch",
				Scene = "c3/MidnightFane/FaneEntrance",
				ListGuid = "d8d8512aa3c48a342a68e9873f92e88e",
				Beat = DialogueBeats.FaneThreshold,
				Narrated = true,
				Gate = "HeroProficiencies",
				Mythic = "9a3b2c63afa79744cbca46bea0da9a16",
				Shift = AlignmentShiftDirection.Good,
				ExpCr = 6
			}
		};
	}

	private static Port62SceneGraft[] BartenderFye()
	{
		return new Port62SceneGraft[2]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3FyeBrewingMasterclass",
				Scene = "NPC_Common/BartenderFye",
				ListGuid = "9b15b09c244076047b02f317e55ef5e3",
				Beat = DialogueBeats.FyeWelcomes,
				ExpCr = 4,
				ClaimKey = "Port62:Act3FyeBrewingMasterclass",
				Coins = 400,
				SixTwoSponsor = "The Lucky Drunk",
				Gold = 500,
				ModItems = new string[5] { "ItemOtherworldEspresso", "ItemOtherworldEspresso", "ItemOtherworldEspresso", "ItemOtherworldEspresso", "ItemOtherworldEspresso" },
				Buff = "OtherworldEspressoBuff",
				BuffMinutes = 10
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3FyeOverlordVintage",
				Scene = "NPC_Common/BartenderFye",
				ListGuid = "9b15b09c244076047b02f317e55ef5e3",
				Beat = DialogueBeats.FyeWelcomes,
				Gate = "OverlordProficiencies",
				ExpCr = 4
			}
		};
	}

	private static Port62SceneGraft[] StoryTellerMainDialogue()
	{
		return new Port62SceneGraft[2]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3StorytellerEpicNovels",
				Scene = "NPC_Common/StoryTeller_MainDialogue",
				ListGuid = "2f5b7e0b76d3c5a42a431e1e33a8db09",
				Beat = DialogueBeats.StorytellerAsksForAStory,
				ExpCr = 4,
				ClaimKey = "Port62:Act3StorytellerEpicNovels",
				Coins = 400,
				SixTwoSponsor = "The Hermit",
				ModItems = new string[1] { "ItemRingOfTheGrandAuditor" }
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3StorytellerMastermindTropes",
				Scene = "NPC_Common/StoryTeller_MainDialogue",
				ListGuid = "2f5b7e0b76d3c5a42a431e1e33a8db09",
				Beat = DialogueBeats.StorytellerAsksForAStory,
				Gate = "MastermindProficiencies",
				ExpCr = 5
			}
		};
	}

	private static Port62SceneGraft[] CiarMainSquare()
	{
		return new Port62SceneGraft[2]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3CiarTrafficLaws",
				Scene = "c3/Drezen_C3/Ciar_MainSquare",
				ListGuid = "76d61bf7eb5ba0c4087cf98ab106385c",
				Beat = DialogueBeats.CiarBeforeMorvegLeaves,
				ExpCr = 4,
				ClaimKey = "Port62:Act3CiarTrafficLaws",
				Coins = 300,
				SixTwoSponsor = "The Laughing King"
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3CiarMartialStretches",
				Scene = "c3/Drezen_C3/Ciar_MainSquare",
				ListGuid = "76d61bf7eb5ba0c4087cf98ab106385c",
				Beat = DialogueBeats.CiarBeforeMorvegLeaves,
				Gate = "MartialGodProficiencies",
				ExpCr = 4
			}
		};
	}

	private static Port62SceneGraft[] VendorArsinoe()
	{
		return new Port62SceneGraft[2]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3ArsinoeTaxTithes",
				Scene = "NPC_Common/VendorArsinoe",
				ListGuid = "ecaf5cfe8087a4f45a2269974f4885c9",
				Beat = DialogueBeats.ArsinoeGreets,
				ExpCr = 4,
				ClaimKey = "Port62:Act3ArsinoeTaxTithes",
				Coins = 350,
				SixTwoSponsor = "The Dark Prince",
				Gold = 400,
				ModItems = new string[1] { "ItemAmuletOfOtherworldEquilibrium" }
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3ArsinoeGodEmperorPatron",
				Scene = "NPC_Common/VendorArsinoe",
				ListGuid = "ecaf5cfe8087a4f45a2269974f4885c9",
				Beat = DialogueBeats.ArsinoeGreets,
				Gate = "GodEmperorProficiencies",
				ExpCr = 5
			}
		};
	}

	private static Port62SceneGraft[] JoranInPresentMain()
	{
		return new Port62SceneGraft[2]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3JoranBessemerMetallurgy",
				Scene = "c3/Mythic_Aeon/Joran_In_Present_Main",
				ListGuid = "23ab8e982ccf3d348a486db7488ddd3d",
				Beat = DialogueBeats.JoranAtTheForge,
				ExpCr = 4,
				ClaimKey = "Port62:Act3JoranBessemerMetallurgy",
				Coins = 400,
				SixTwoSponsor = "The Grand Arbiter"
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3JoranMartialBalance",
				Scene = "c3/Mythic_Aeon/Joran_In_Present_Main",
				ListGuid = "23ab8e982ccf3d348a486db7488ddd3d",
				Beat = DialogueBeats.JoranAtTheForge,
				Gate = "MartialGodProficiencies",
				ExpCr = 4
			}
		};
	}

	private static Port62SceneGraft[] Deserter()
	{
		return new Port62SceneGraft[2]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3MoltenScarDeescalation",
				Scene = "c3/MoltenScar/Deserter",
				ListGuid = "b2b77a591c8808f409aa2083bd1b318a",
				Beat = DialogueBeats.JannahExplainsHerFlight,
				ReplySpeaker = SpeakerOf("4880d0b16ca74fa46a167914e2b44bcc"),
				ExpCr = 4,
				ClaimKey = "Port62:Act3MoltenScarDeescalation",
				Coins = 350,
				SixTwoSponsor = "The Inheritor"
			},
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3MoltenScarOverlordReprieve",
				Scene = "c3/MoltenScar/Deserter",
				ListGuid = "b2b77a591c8808f409aa2083bd1b318a",
				Beat = DialogueBeats.JannahExplainsHerFlight,
				ReplySpeaker = SpeakerOf("4880d0b16ca74fa46a167914e2b44bcc"),
				Gate = "OverlordProficiencies",
				ExpCr = 4
			}
		};
	}

	private static Port62SceneGraft[] GateGuard()
	{
		return new Port62SceneGraft[1]
		{
			new Port62SceneGraft
			{
				Answer = "IsekaiAct3WintersunHorrorFlags",
				Scene = "c3/Wintersun/GateGuard",
				ListGuid = "ee598777c32735a4eb1d19f7763a6382",
				Beat = DialogueBeats.WintersunGateWarning,
				Narrated = true,
				ExpCr = 4,
				ClaimKey = "Port62:Act3WintersunHorrorFlags",
				Coins = 300,
				SixTwoSponsor = "The Laughing King"
			}
		};
	}

	public static void Add()
	{
		Port("IsekaiDrezenIntroImperialMandate", "(God Emperor) [Imperial Mandate] \"Drezen is no longer merely a border outpost. By imperial decree, this citadel is consecrated as the eternal bastion of mortal sovereignty, where demonkind will learn divine judgment!\"", "{n}A stunned hush falls over the council chamber. The crusader captains straighten their postures, struck by an overwhelming wave of regal authority.{/n}");
		Port("IsekaiDrezenIntroMartialCrucible", "(Martial God) [The Martial Crucible] \"Walls do not defend a city; warriors do. From this sunrise forth, every crusader in Drezen undergoes otherworlder combat drills until their reflexes outpace abyssal fiends.\"", "{n}The veteran sergeants exchange wide-eyed glances as your aura of pure martial mastery washes over the hall.{/n} \"Sir! We will double the training regimens immediately!\"");
		Port("IsekaiDrezenIntroOverlordDominion", "(Overlord) [Iron Dominion] \"Drezen answers to my absolute authority. Any soldier who falters will find my displeasure far more terrifying than whatever lurks in the Worldwound.\"", "{n}The temperature in the throne room plummets as shadows dance obediently at your command. Irabeth swallows hard, silently nodding in grim compliance.{/n}");
		Port("IsekaiDrezenIntroMastermindLogistics", "(Mastermind) [Grand Logistics Calculation] \"Factoring regional mana leylines and abyssal weather cycles, standard supply attrition drops by 74% if we route through the southern ridges. Here is the operational blueprint.\"", "{n}Irabeth examines your crystalline analytical charts in sheer disbelief.{/n} \"This is... mathematically flawless. How did you compute weeks of logistics in moments?\"");
		Port("IsekaiDrezenIntroHeroLiberationCry", "(Hero) [Beacon of Dawn] \"We took Drezen for everyone who suffered under the Worldwound's shadow. As long as I draw breath, no demon will ever breach these gates again!\"", "{n}Cheers erupt from the doorway as junior crusaders and volunteers raise their swords in fervent salute!{/n}");
		Port("IsekaiHeraldPlanarCosmology", "(Isekai Protagonist) [Dimensional Comparative Analysis] \"I have seen cosmic pantheons across different realities, Herald. Heaven's order is admirable, but mortal determination is the true pivot of this war.\"", "{n}The Hand of the Inheritor tilts his golden helm thoughtfully.{/n} \"A perspective as profound as it is foreign to Golarion. I shall watch your deeds with keen interest, Commander.\"");
		Port("IsekaiHeraldAngelSovereignty", "(God Emperor) [Angel] [Radiant Sovereignty] \"Heaven and the mortal realm stand as sovereign allies, Herald. Together, our golden light shall burn away the Worldwound's corruption down to the bedrock.\"", "{n}Golden halos interweave above you and the Herald as divine resonance echoes through the sanctum.{/n} \"Well spoken, divine sibling. May justice guide your sword!\"");
		Port("IsekaiHeraldMartialSparChallenge", "(Martial God) [Blade Evaluation] \"You have fought fiends across ten thousand celestial wars, Herald. Sometime soon, cross blades with me; I wish to see how heaven's finest measures against otherworlder sword arts.\"", "{n}A spark of genuine warrior's fire ignites in the celestial's golden gaze.{/n} \"An extraordinary proposition, mortal warrior. I accept; when our duties permit, let us test our metal.\"");
		Port("IsekaiHeraldAeonJurisdiction", "(Mastermind) [Aeon] [Cosmic Jurisdiction] \"The Worldwound violates the prime cosmic axis. My duty is to restore universal balance, with or without celestial dispensation.\"", "{n}The Herald observes the geometric ribbons of cosmic law swirling around your hands.{/n} \"The scales of eternity move through you, Commander. Walk the path of balance without wavering.\"");
		Port("IsekaiHeraldOverlordPragmatism", "(Overlord) [Ruthless Necessity] \"Prayers and righteousness failed Kenabres for a century, Herald. Only overwhelming force and cold dominion will cleanse this continent.\"", "{n}The Herald's expression hardens with sorrow and warning.{/n} \"Take care, Commander. In hunting monsters with ruthless zeal, do not forge your own abyssal chains.\"");
		Port("IsekaiHeraldAzataJoyousAffirmation", "(Hero) [Azata] [Song of the Free Hearts] \"We don't need rigid dogmas to save this world, Herald. As long as freedom, kindness, and fellowship guide us, the demons don't stand a chance!\"", "{n}The Herald chuckles softly, his stern celestial demeanor softening.{/n} \"Your innocence and boundless courage bring warmth to this scarred earth, young hero.\"");
		Port("IsekaiAreeluLabCheatCodeMockery", "(Isekai Protagonist) [Reality Architecture Analysis] \"Vorlesh thought she was running an unprecedented experiment. She didn't realize she wrote developer cheat codes directly into the spatial matrix for an Otherworlder to claim!\"", "{n}The laboratory's ambient arcane machinery hums erratically as your fingers effortlessly trace the dimensional seams, disarming latent warding traps instantly.{/n}");
		Port("IsekaiAreeluLabAeonEquationSunder", "(Mastermind) [Aeon] [Dimensional Axiomatic Sunder] \"This planar suture is fundamentally illegal under cosmic geometry. I will excise Vorlesh's parasitic equations and restore the baseline reality coordinate.\"", "{n}Glowing mathematical runes of crystalline law circle your fingertips, suppressing the toxic abyssal seepage into inert vapor.{/n}");
		Port("IsekaiAreeluLabLegendMartialStrike", "(Martial God) [Legend] [Spatial Void Severance] \"I need neither demon flesh nor celestial favor to dismantle this rift. My blade cuts through the concept of the portal itself!\"", "{n}With a thunderous strike that rattles your teeth, your weapon cleaves the metaphysical seam, leaving the abyssal vortex shuddering and silenced!{/n}");
		Port("IsekaiAreeluLabLichNahyndrianDrain", "(Overlord) [Lich] [Nahyndrian Necrotic Harvest] \"Vorlesh refined abyssal power for her petty schemes. I shall harvest these raw Nahyndrian currents into my own immortal soul crucible.\"", "{n}Pale necrotic vortexes swirl from your outstretched palm, siphoning the agonizingly potent rift energies directly into your dark core!{/n}");
		Port("IsekaiAreeluLabAngelSolarPurge", "(God Emperor) [Angel] [Solar Consecration] \"Every inch of this foul laboratory reeks of violated mortal souls. In the name of heaven and earth, let holy fire cleanse this blasphemy!\"", "{n}Blinding solar flames cascade across the lab tables, incinerating horrific flesh-vats into clean, fragrant ash.{/n}");
		Port("IsekaiAreeluLabHeroPurityVow", "(Hero) [Inviolate Mortal Heart] \"No matter what experiments Vorlesh performed, my heart belongs to those who trust me. I will wield this power to protect, never to dominate.\"", "{n}A comforting, warm radiance emanates from your chest, reassuring your companions and repelling the oppressive dread of the laboratory.{/n}");
		Port("IsekaiWintersunIllusionMockery", "(Isekai Protagonist) [Phantasm Compression Disruption] \"A psychic projection this cheap wouldn't even pass for a budget virtual simulation back home. Drop the glamour, demon; your puppet strings are visible to anyone with eyes.\"", "{n}Jerribeth's porcelain smile twitches in momentary astonishment as your mana pulse ripples across the village square, momentarily revealing the rot beneath the idyllic village.{/n}");
		Port("IsekaiWintersunMastermindAeonDispersion", "(Mastermind) [Aeon] [Axiomatic Deconstruction] \"Your illusion violates cognitive symmetry. By introducing an inverse harmonic frequency, your enchantment grid collapses into inert noise.\"", "{n}With a crisp snap of your fingers, a lattice of blue starlight flashes across the settlement, tearing through Jerribeth's phantasms instantly!{/n}");
		Port("IsekaiWintersunGodEmperorRadiantTruth", "(God Emperor) [The Emperor's Sun] \"You dare pose as a deity before a true sovereign? Begone, vermin, before the blinding dawn of righteous order!\"", "{n}A solar pillar erupts behind you, casting blinding rays that sear Jerribeth's demonic flesh and send her recoiling in agony!{/n}");
		Port("IsekaiWintersunMartialGodKillingIntent", "(Martial God) [Sunder Glamour: Apex Bloodlust] \"Your illusions reek of cheap perfume and rotten meat. Stop talking before my blade cleaves your voice from your throat.\"", "{n}A localized hurricane of sheer, suffocating killing intent pins Jerribeth in place, wiping the smirk cleanly from her face.{/n}");
		Port("IsekaiWintersunOverlordDominationDemand", "(Overlord) [Subjugation or Annihilation] \"You played with mortals like cattle, Jerribeth. Now kneel before a true master of shadows, or be ground into dust under my boot.\"", "{n}Your shadow engulfs the stone terrace, demonic whispers turning into shrill whimpers beneath your domineering pressure.{/n}");
		Port("IsekaiWintersunHeroAzataAwakening", "(Hero) [Azata] [Song of Awakening] \"People of Wintersun, hear my voice! The nightmare is over! Break free from her whispers and reclaim your freedom!\"", "{n}Your passionate cry echoes across the valley like spring rain, causing the blinded villagers to blink and weep as the trance dissolves!{/n}");
		PortCompanion("IsekaiDragonhuntFlightTrajectory", "(Isekai Protagonist) [Monster Hunter Meta-Appraisal] \"Devarra's injured wing forces a 30-degree banking turn near the cliffside. Station our ballistas there; the dragon will fly directly into our trap.\"", "{n}Greybor pauses, squints at your tactical map, and nods with genuine professional respect.{/n} \"Flawless deduction, Commander. The beast won't know what hit it.\"");
		PortCompanion("IsekaiDragonhuntMartialGodSingleStroke", "(Martial God) [Dragon-Slaying Stance] \"Let the beast fly wherever it wishes. One clean vertical draw from my blade will sever its fiery breath at the larynx.\"", "{n}Greybor raises an appreciative eyebrow.{/n} \"Ambitious. Reckless, even. But looking at your stance, I have little doubt you can deliver on that boast.\"");
		PortCompanion("IsekaiDragonhuntOverlordTrophyClaim", "(Overlord) [Predatory Harvest] \"The red dragon's heart and scales will be harvested for my personal armory. Greybor, see to it that the corpse is not ruined by clumsy hands.\"", "{n}Greybor gives a curt, pragmatic nod.{/n} \"As long as my contract fee clears, Commander, the dragon's carcass is entirely yours to harvest.\"");
		Port("IsekaiKyadoTruthExtraction", "(Isekai Protagonist) [Spiritual Scrutiny] \"Kyado, your prayers ring hollow and your hands tremble with sulfur residue. Speak the truth of your pact of silence before your soul is forfeit.\"", "{n}Kyado collapses to his knees, sobbing hysterically as he confesses the pact of silence.{/n} \"Forgive me! They threatened to tear my soul apart! I will tell you everything!\"");
		Port("IsekaiKyadoMastermindContractDissection", "(Mastermind) [Abyssal Contract Invalidation] \"You swore your silence out of duress and existential panic, Kyado. Under fundamental planar jurisprudence, an oath sworn without true informed consent has four fatal loopholes. Tell me its exact words.\"", "{n}Kyado stares at you in slack-jawed bewilderment as you systematically tear the magical clauses of his demonic bondage to shreds!{/n}");
		Port("IsekaiKyadoHeroRedemptionOffer", "(Hero) [Compassionate Redemption] \"Fear makes cowards of us all, Kyado. But redemption begins with confession. Stand behind us; we will protect you from the demons you feared.\"", "{n}Tears stream down Kyado's cheeks weeping in profound relief as the words he has held back for so long pour out.{/n}");
		Port("IsekaiGreengatesArueshalaeAffirmation", "(Isekai Protagonist) [Soul Affirmation] \"Your past in the Abyss does not define your future, Arueshalae. Every dream of freedom is proof that your soul belongs to you, not the demons.\"", "{n}Arueshalae's eyes fill with tears of profound gratitude as a faint breeze stirs the old bell above the grave.{/n} \"Thank you... with you by my side, I will never look back!\"");
		Port("IsekaiGreengatesHeroAzataSong", "(Hero) [Azata] [Chime of Elysium] \"Let the bells ring with the joy of tomorrow, Arueshalae! Even in the deepest shadows, the seeds of Elysium will always blossom!\"", "{n}Vibrant rainbow petals swirl around the belltower as celestial melodies dissolve every trace of demonic taint in the grove.{/n}");
		Port("IsekaiMoltenScarVrockMockery", "(Isekai Protagonist) [Vocal Frequency Jamming] \"Save your shrieking dance, Vorimeraak. In my home world, we have audio equalizers that cancel out dissonant bird screeches entirely.\"", "{n}Vorimeraak gags as an acoustic dampening field chokes her throat mid-screech, leaving her flapping her wings in comical indignity.{/n}");
		Port("IsekaiMoltenScarMartialGodDecapitation", "(Martial God) [Feather-Severing Draw] \"Flap your wings all you like; my blade moves faster than the sound of your screams.\"", "{n}A flash of steel leaves Vorimeraak's plumes fluttering to the ash before she can even raise her talons.{/n}");
		Port("IsekaiMoltenScarGodEmperorExecution", "(God Emperor) [Imperial Condemnation] \"You have preyed on crusader scouts for the last time. As sovereign of Drezen, I sentence you to immediate purification!\"", "{n}Solar brilliance erupts from your hand, burning the vrock's plumage into glowing cinders!{/n}");
		Port("IsekaiMoltenScarHeroRescueVow", "(Hero) [No Soldier Left Behind] \"Release the captured crusaders immediately! Not another soul will die in your sulfur pits today!\"", "{n}The captive soldiers behind Vorimeraak shout in renewed hope at your fearless arrival!{/n}");
		Port("IsekaiMoltenScarOverlordTorment", "(Overlord) [Subservience or Ashes] \"Vrock, you may either bow and deliver the prisoners unharmed, or your wings will decorate my fortress gates.\"", "{n}The monstrous bird shivers in genuine terror beneath your suffocating aura of malevolent authority.{/n}");
		Port("IsekaiXanthirSwarmVacuum", "(Isekai Protagonist) [Atmospheric Vacuum] \"A swarm body relies on atmospheric air pressure to maintain cohesion. An inverted mana vortex scatters your locusts to the winds.\"", "{n}Xanthir Vang shrieks as your mana vortex tears his insectoid matrix apart, leaving him helpless.{/n} \"Curse you! My swarm! My glorious ascension!\"");
		Port("IsekaiXanthirSlimeDigestion", "(Slime) [Apex Swarm Digestion] \"A swarm of insects? You are merely a buffet of protein. My digestive enzymes consume your swarm faster than you can regenerate.\"", "{n}Your slime pseudopods engulf whole swarms in seconds, digesting them into inert biomass.{/n}");
		Port("IsekaiXanthirAngelSolarFire", "(God Emperor) [Angel] [Solar Incineration] \"Your defiled insects will not darken this world another day. Heaven's righteous light reduces your hive to ashes!\"", "{n}A torrent of pure solar fire engulfs the room, burning tens of thousands of locusts into white ash in a heartbeat!{/n}");
		Port("IsekaiXanthirLegendMartialCleave", "(Martial God) [Legend] [Thousand-Blade Whirlwind] \"You think safety lies in numbers? My blade executes a thousand slashes in a single breath!\"", "{n}Blades of pressurized air crisscross the chamber, slicing Xanthir's swarm into millions of lifeless fragments!{/n}");
		Port("IsekaiXanthirMastermindPheromoneDisruption", "(Mastermind) [Pheromone Matrix Jamming] \"Your hive consciousness relies on high-frequency chemical signaling. By dispersing an inverted sulfur-enzyme, I turn your own swarm into cannibalistic chaos.\"", "{n}Xanthir howls in utter despair as the locusts forming his flesh begin viciously devouring each other!{/n}");
		Port("IsekaiXanthirLichResearchHarvest", "(Overlord) [Lich] [Entomological Soul Enslavement] \"Your bio-alchemy is crude, Xanthir, but your locust matrices will make excellent necrotic drones for my undead armies.\"", "{n}Black necrotic frost encases the swarm bodies, subjugating their lingering souls into your dark reservoir!{/n}");
		Port("IsekaiXanthirHeroAvengerVow", "(Hero) [Vow for the Lost] \"For every scout you tortured in these vile labs, your monstrous experiments end right here, right now!\"", "{n}Your battle-cry echoes through the stone corridors, invigorating your companions with relentless courage!{/n}");
		Port("IsekaiGalfreyArrivesTricksterHumor", "(Isekai Protagonist) [Sovereignty Protocol Bypass] \"Welcome to Drezen, Your Majesty! I took the liberty of skipping royal formalities and reallocating the throne room budget to defensive enchantments.\"", "{n}Galfrey pauses, blinking in surprise before offering a tired, genuine smile.{/n} \"I would expect nothing less from you, Commander. Results speak louder than courtiers' etiquette.\"");
		Port("IsekaiGalfreyArrivesGodEmperorPeers", "(God Emperor) [Equal Sovereigns] \"Welcome, Queen of Mendev. You stand in Drezen not as my monarch, but as an honored fellow sovereign in our shared crusade against the Abyss.\"", "{n}Galfrey's royal guard stiffens, but the Queen holds up a hand, looking upon your majestic imperial aura with profound solemnity.{/n}");
		Port("IsekaiGalfreyArrivesMartialGodAssurance", "(Martial God) [The Unconquered Blade] \"Rest easy, Galfrey. The Midnight Fane and whatever demon lords lurk beneath will break upon my blade just like the rest.\"", "{n}Galfrey looks at your confident, relaxed martial posture and lets out a long breath of relief.{/n} \"May Iomedae grant your blade swift passage, warrior.\"");
		Port("IsekaiGalfreyArrivesOverlordWarning", "(Overlord) [Sovereign Ultimatum] \"Drezen and the Crusade answer to me now, Galfrey. Do not mistake my alliance for subservience to Nerosyan's distant crown.\"", "{n}Galfrey meets your icy gaze without flinching, though tension fills the air like drawn bowstrings.{/n} \"We shall see where your ambition leads you, Commander.\"");
		Port("IsekaiGalfreyArrivesMastermindReport", "(Mastermind) [Comprehensive Strategic Audit] \"Here are the logistical metrics, casualty estimates, and tactical projections for breaching the Midnight Fane. We have minimized allied casualties to under 2.3%.\"", "{n}Galfrey reviews the precise tactical scrolls in amazement.{/n} \"A masterpiece of operational planning. You have transformed the crusade into an unyielding machine.\"");
		Port("IsekaiGalfreyArrivesHeroGratitude", "(Hero) [Heart of the Crusade] \"Thank you for trusting me with Drezen, Your Majesty. We will finish what the crusaders started and bring peace back to Mendev!\"", "{n}Galfrey smiles warmly and places a gentle hand on your shoulder.{/n} \"The people believe in you, Commander. And so do I.\"");
		Port("IsekaiFaneBreachTricksterDive", "(Isekai Protagonist) [The Otherworlder Dive] \"The demons have been invading our world for a century. It's about time someone invaded the Abyss right back and showed them how an Otherworlder plays the game!\"", "{n}Your bold cheer spreads through the crusader ranks like wildfire, turning grim anxiety into exhilarating battle fervor!{/n}");
		Port("IsekaiFaneBreachAngelImperialConquest", "(God Emperor) [Angel] [Holy Conqueror's Decree] \"We do not enter the Abyss as prey or martyrs! We march as divine conquerors to deliver holy vengeance to the demon lords' doorsteps!\"", "{n}Blinding golden wings flare behind your silhouette as the entire army roars with unstoppable holy zeal!{/n}");
		Port("IsekaiFaneBreachLegendMartialPinnacle", "(Martial God) [Legend] [Apex Challenger] \"An entire dimension crawling with the strongest fiends in existence? This is the ultimate crucible. Every demon in the Abyss will learn to fear my blade!\"", "{n}You draw your weapon with a single sharp ring that echoes down into the endless dark of the rift, cutting through the abyssal chill!{/n}");
		Port("IsekaiFaneBreachLichDreadVanguard", "(Overlord) [Lich] [The Abyss Shall Bow] \"The Abyss prides itself on dread and agony. When my shadow falls upon the Midnight Isles, even demon lords will know true fear.\"", "{n}Dark frost creeps along the edge of the portal as your dread aura pulses, chilling the demonic flames into ash.{/n}");
		Port("IsekaiFaneBreachMastermindAeonCalculus", "(Mastermind) [Aeon] [Planar Ingress Synchronization] \"Portal coordinates locked. Ingress vector synchronized with prime reality anchor. Let us execute the operation.\"", "{n}Geometric arrays of pure cosmic law stabilize the portal threshold, turning the chaotic vortex into a smooth transit gate.{/n}");
		Port("IsekaiFaneBreachHeroAzataDawnMarch", "(Hero) [Azata] [Marching toward the Dawn] \"For everyone waiting for us back home, we march into hell and bring back the dawn! Follow me, everyone!\"", "{n}A chorus of hopeful song swells among your companions and soldiers, banishing all shadow from their hearts!{/n}");
		Port("IsekaiAct3FyeBrewingMasterclass", "(Isekai Protagonist) [Otherworld Coffee & Carbonated Cocktail Masterclass] \"Fye, sour ale and murky water won't keep the crusade sharp during long midnight watches. Let me teach you how to roast imported beans into steaming espresso and brew fizzy iced cocktails. One cup of this, and your patrons will have the energy of a raging barbarian!\"", "{n}Fye takes a tentative sip of the foaming, jet-black brew, his eyes bulging wide as a jolt of pure caffeinated lightning electrifies his senses. He lets out a breathless whoop, wiping down his bar counter with blinding speed.{/n} \"Sweet Cayden's tankard! My heart is pounding like a war drum and I feel like I could sprint straight to Kenabres and back! What is this sorcery?! You're telling me soldiers will pay double coin for this 'espresso' every morning?! Commander, you just made this tavern the most popular establishment in the entire Mendevian north! Take these freshly brewed thermoses!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"The sacred nectar of vigilance! Fye's tavern is now an official holy site of Cayden Cailean!\"</i>"));
		Port("IsekaiAct3FyeOverlordVintage", "(Overlord) [Imperial Tavern Decree] \"This tavern now operates under imperial sovereignty, barkeep. Stock the finest vintages in the realm for my officers, or find your taps frozen.\"", "{n}Fye bows nervously, hurriedly setting aside his private reserve.{/n} \"At once, my lord! Only the finest cellar casks for your court!\"");
		Port("IsekaiAct3StorytellerEpicNovels", "(Isekai Protagonist) [Multiversal Narrative Parallels & Epic Literature] \"Storyteller, hearing your accounts of ancient Earthfall and the Age of Anguish reminds me of the serialized epic fantasy chronicles from my home dimension. We had storytellers who spent decades worldbuilding grand sagas with magic academies, chosen ones, and apocalyptic worldwounds.\"", "{n}The blind ancient elf smiles with profound, gentle nostalgia, his weathered fingers lightly tracing the cover of an ancient grimoire.{/n} \"Ah... across the ocean of stars, mortal hearts still yearn for the same light amidst the dark. We weave chronicles of suffering and triumph so that those who follow will remember that even after Earthfall, the sun eventually rises. Take this ring, child of another sky; let the annals of forgotten times guide your wisdom.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"The echoes of distant universes resonate within the Storyteller's tower. Wisdom rewarded.\"</i>"));
		Port("IsekaiAct3StorytellerMastermindTropes", "(Mastermind) [Deconstructing Historical Tragedies] \"Ancient civilizations consistently collapsed due to pride, lack of strategic redundancy, and poor communication protocols. Observe how the Starstone crisis could have been mitigated.\"", "{n}The Storyteller chuckles softly, thoroughly intrigued by your clinical breakdown of ancient history.{/n} \"A remarkably pragmatic mind. If only the emperors of ancient Azlant possessed your analytical foresight.\"");
		Port("IsekaiAct3CiarTrafficLaws", "(Isekai Protagonist) [Cavalry Traffic Regulation & Courtyard Speed Limits] \"Sir Ciar, your Everbright knights are charging through the lower courtyard like they're trying to win an Otherworld street race. We need pedestrian crosswalks, designated mount parking stalls, and a fifteen-mile-per-hour speed limit before someone tramples an alchemist's cart!\"", "{n}Sir Ciar's stern, weathered features twitch with dry, exasperated amusement, though he crosses his gauntleted arms with reluctant approval.{/n} \"'Designated mount parking stalls' and 'speed limits'?! Hmph. Most commanders demand our knights ride faster, not obey courtyard traffic regulations! But... you aren't wrong. Sir Morveg nearly clipped a crate of alchemical fire yesterday. I will instruct our squires to establish designated cavalry lanes immediately.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Imposing municipal traffic laws on proud knights! Hilarious civic order!\"</i>"));
		Port("IsekaiAct3CiarMartialStretches", "(Martial God) [Saddle Fatigue Breathing Kata] \"Your cavaliers suffer from pelvic meridian constriction after twelve hours in heavy stirrups. Teach them this core-alignment stretch before evening muster.\"", "{n}Sir Ciar tests the stretch, his spine popping with immediate relief.{/n} \"By the sun... the lower back stiffness is gone. You have my gratitude, Commander; this will keep our riders combat-effective.\"");
		Port("IsekaiAct3ArsinoeTaxTithes", "(Isekai Protagonist) [Temple Tithe Optimization & Scroll Subscriptions] \"Priestess Arsinoe, selling single scrolls over a dusty counter is terribly inefficient. In my world, temples offer seasonal subscription packages for restorative potions and tax-deductible crusade donations. Standardize your pricing, and cathedral revenue will triple!\"", "{n}Arsinoe's eyes widen as she hurriedly pulls out an abacus and inkpot, muttering prayers of astonishment as the numbers line up.{/n} \"Seasonal scroll subscriptions and tax-deductible crusade donations?! Merciful Abadar... that is sheer clerical brilliance! We can fund three new field hospices with those margins! Take this blessed donation receipt, Commander, and a talisman of equilibrium with it if you have not been given one already!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>: <i>\"Modern financial wizardry applied to clerical dogma. Flawless execution.\"</i>"));
		Port("IsekaiAct3ArsinoeGodEmperorPatron", "(God Emperor) [Imperial Temple Charter] \"The cathedral now serves under the sovereign charter of humanity. Tend to the people's spirits; imperial treasury funds will cover all expenses.\"", "{n}Arsinoe bows with deep reverent grace.{/n} \"The Inheritor smiles upon such magnificent generosity, Your Majesty.\"");
		Port("IsekaiAct3JoranBessemerMetallurgy", "(Isekai Protagonist) [Modern Metallurgy & Carbon Folding] \"Master Vhane, your dwarven tempering is legendary, but study this: by injecting pressurized air through molten pig iron to burn off excess carbon, and adding trace amounts of vanadium, we can produce high-tensile cold iron steel that never chips against demon bone.\"", "{n}Joran drops his tongs with a resounding clatter, peering into the forge coals with wild, fanatical craftsmanship.{/n} \"Pressurized oxygen blast to purge carbon impurities?! And vanadium alloys?! By Torag's beard, that's not just smithing--that's true elemental transmutation! I can forge breastplates half the weight with twice the tensile strength! Hand me that slate, Commander; the citadel armory is about to make history!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Industrial metallurgy eradicates obsolete forging techniques. Superb technological injection.\"</i>"));
		Port("IsekaiAct3JoranMartialBalance", "(Martial God) [Center-of-Mass Hilt Tuning] \"Shave two ounces from the pommel counterweight. The blade should align seamlessly with the radial nerve, becoming an extension of personal ki.\"", "{n}Joran files the hilt, handing the weapon back with a stunned nod.{/n} \"It balances like a feather on the fingertip... Immaculate eye, warrior.\"");
		Port("IsekaiAct3IrabethWellnessCheck", "(Isekai Protagonist) [Mental Health Check-In & Warm Pastry] \"Irabeth, put down the requisition quill for five minutes and eat this warm almond pastry. You survived the Lost Chapel, you reclaimed Drezen, and you're doing an extraordinary job holding our supply lines together. Stop tormenting yourself with self-doubt.\"", "{n}Irabeth freezes, staring at the warm pastry in your hand as a fragile, trembling sigh escapes her lips. She rubs her weary eyes, her voice thick with unvoiced gratitude.{/n} \"Commander... I... thank you. It's so easy to drown in casualty reports and convince myself I'm unworthy of this uniform. Having someone acknowledge the struggle... it means more than you know. I'll take a breath. And... the pastry is delicious.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Everbloom']</b></color>: <i>\"A compassionate word lifts the crushing weight of command from a weary paladin's heart.\"</i>"));
		Port("IsekaiAct3MoltenScarDeescalation", "(Isekai Protagonist) [Combat Stress Psychology & Honorable Reprieve] \"Lower your dagger. In my world, we understand that human minds break under relentless psychological trauma--we call it combat fatigue. You aren't a cowardly traitor; you are an exhausted soldier driven mad by demonic whispers. Stand down, report to the Drezen rear guard for medical rehabilitation, and your life is spared.\"", "{n}The trembling deserter collapses to her knees in the volcanic ash, tears streaming down her soot-streaked face as she casts her weapon aside.{/n} \"Combat... fatigue? You don't think I'm a filthy traitor deserving the gallows?! Merciful gods... the voices in the sulfur drove me half-insane! Thank you, Commander! I will serve in the supply trains until my dying breath!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"Compassion redeems broken souls where cold iron would only foster despair.\"</i>"));
		Port("IsekaiAct3MoltenScarOverlordReprieve", "(Overlord) [Conditional Sovereign Pardon] \"Fools who flee before my standard commit treason, yet squandering living labor is inefficient. Return to the Drezen vanguard as frontline sappers, or be erased.\"", "{n}The deserter bows frantically in terror.{/n} \"I obey! I will clear the trenches with my bare hands! Thank you, Lord Commander!\"");
		Port("IsekaiAct3WintersunHorrorFlags", "(Isekai Protagonist) [Horror Film Rural Trope Analysis] \"An isolated forest hamlet where everyone smiles too cheerfully, pretends the Worldwound doesn't exist, and serves mystery stew? In my world, this place is ticking literally every single 'haunted pagan cult movie' checklist. Keep your shields raised; this idyllic village is an illusion waiting to bite.\"", "{n}Your companions glance warily around the quiet settlement, hands instinctively tightening on their weapon pommels as the uncanny, Stepford-like stillness of the villagers becomes unnervingly apparent.{/n} \"A 'horror film checklist'?! Well, whatever strange stories they tell in your world, Commander, the hairs on the back of my neck agree completely. Something is deeply rotten in Wintersun.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Spotting the creepy village tropes before the jump-scare! A seasoned veteran!\"</i>"));
		PortCompanion("IsekaiAct3DragonhuntRaidMechanics", "(Isekai Protagonist) [MMO Dragon Raid Phase Tactics] \"Listen up! Flying dragons always cycle through three distinct combat phases: ground breath cone, aerial dive bombs, and tail cleave! Greybor, hold the flank; casters spread out in a sixty-degree arc to avoid stacking breath damage! When she lands, burn all cooldowns!\"", "{n}Greybor spits into the mountain gravel, a grim, admiring smirk crossing his dwarven features as he tightens his axe grips.{/n} \"'Aerial dive bombs' and 'avoid stacking breath cone damage'? Heh. Never heard dragon hunting described like an engineering drill, Commander, but the tactical logic is airtight. Let's ground this overgrown lizard and collect our bounty!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"Raiding the red dragon with textbook tactical positioning! Let's get that loot!\"</i>"));
	}

	private static void Port(string name, string answerText, string replyText, string line = null)
	{
		Port62SceneGraft graft = Port62SceneGraft.Find(Grafts, name, "Port62Act3");
		Condition[] array = Port62SceneGraft.Gates(graft, "Port62Act3");
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
		Port62SceneGraft graft = Port62SceneGraft.Find(Grafts, name, "Port62Act3");
		Condition[] array = Port62SceneGraft.Gates(graft, "Port62Act3");
		if (array == null)
		{
			return;
		}
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>(graft.ReplySpeaker);
		if (blueprint == null)
		{
			Main.IsekaiContext.Logger.LogWarning("Port62Act3: companion " + graft.ReplySpeaker + " not found, skipping " + name + ".");
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
}
