using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62Act5
{
	internal static readonly Port62LateActs.Graft[] Grafts = new Port62LateActs.Graft[57]
	{
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDrezenReturnGodEmperorMajesty",
			Scene = "c5/DrezenMain_C5/Ciar",
			ListGuid = "234a43b05125963409b93aded03a5125",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7,
			Changed = "6.2 spread a holy aura over the entire citadel; the scene is Ciar's greeting."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDrezenReturnMartialGodPoise",
			Scene = "c5/DrezenMain_C5/Ciar",
			ListGuid = "234a43b05125963409b93aded03a5125",
			Gate = "MartialGodProficiencies",
			ExpCr = 6
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDrezenReturnOverlordDominion",
			Scene = "c5/DrezenMain_C5/Ciar",
			ListGuid = "234a43b05125963409b93aded03a5125",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 6,
			Changed = "6.2 silenced the whole city; the scene is Ciar's greeting."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDrezenReturnMastermindReorganization",
			Scene = "c5/DrezenMain_C5/Ciar",
			ListGuid = "234a43b05125963409b93aded03a5125",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 6,
			Changed = "6.2 had quartermasters who are not in the scene."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDrezenReturnHeroTriumphantEmbrace",
			Scene = "c5/DrezenMain_C5/Ciar",
			ListGuid = "234a43b05125963409b93aded03a5125",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2 had soldiers rushing forward in a scene that is Ciar's greeting."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiCoronationProtagonistSovereignty",
			Scene = "c5/DrezenMain_C5/Coronation",
			ListGuid = "a53f40ba02af23444b63de6b9337702d",
			ExpCr = 7,
			Changed = "6.2 defied the Queen deposing you; this list is Anevia after the Queen's death, and nobody deposes you.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiCoronationGodEmperorSelfCoronation",
			Scene = "c5/DrezenMain_C5/Coronation",
			ListGuid = "a53f40ba02af23444b63de6b9337702d",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 8,
			Changed = "6.2 crowned you before a throne room answering a deposition that never happens; the Queen is dead here.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiCoronationMartialGodEdictSunder",
			Scene = "c5/DrezenMain_C5/Coronation",
			ListGuid = "a53f40ba02af23444b63de6b9337702d",
			Gate = "MartialGodProficiencies",
			ExpCr = 7,
			Changed = "6.2 cut up a royal edict dismissing you; no edict exists at this beat.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiCoronationOverlordAbsoluteSubmission",
			Scene = "c5/DrezenMain_C5/Coronation",
			ListGuid = "a53f40ba02af23444b63de6b9337702d",
			Gate = "OverlordProficiencies",
			MythicPath = "Lich",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 8,
			Changed = "6.2 threatened a living Galfrey; the Queen is dead at this beat.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiCoronationMastermindConstitutionalAudit",
			Scene = "c5/DrezenMain_C5/Coronation",
			ListGuid = "a53f40ba02af23444b63de6b9337702d",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7,
			Changed = "6.2 voided a dismissal edict from Galfrey; the Queen is dead and no edict exists.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiCoronationHeroShieldOfThePeople",
			Scene = "c5/DrezenMain_C5/Coronation",
			ListGuid = "a53f40ba02af23444b63de6b9337702d",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7,
			Changed = "6.2 answered the Queen offering to take your title; the Queen is dead at this beat.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzPlagueBombsTriadSalvation",
			Scene = "c5/Iz/GalfreyOnTheEdge",
			ListGuid = "ec740ac6a039d8c4b83a076cee6c1ff0",
			ExpCr = 7,
			Changed = "6.2 described the rescue as done at the briefing before Iz; it is now your plan for Iz.",
			IzTriad = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzMartialGodKiFlash",
			Scene = "c5/Iz/GalfreyOnTheEdge",
			ListGuid = "ec740ac6a039d8c4b83a076cee6c1ff0",
			Gate = "MartialGodProficiencies",
			ExpCr = 7,
			Changed = "6.2 had Mutasafen standing defeated in the Queen's briefing, where he is not.",
			IzTriad = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzAngelSolarAegisSave",
			Scene = "c5/Iz/GalfreyOnTheEdge",
			ListGuid = "ec740ac6a039d8c4b83a076cee6c1ff0",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8,
			Changed = "6.2 described the shield absorbing detonations at the briefing before Iz; it is now your vow.",
			IzTriad = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzMastermindTemporalDisplacement",
			Scene = "c5/Iz/GalfreyOnTheEdge",
			ListGuid = "ec740ac6a039d8c4b83a076cee6c1ff0",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7,
			Changed = "6.2 described an evacuation at the briefing before Iz; it is now your plan.",
			IzTriad = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzHeroUltimateShield",
			Scene = "c5/Iz/GalfreyOnTheEdge",
			ListGuid = "ec740ac6a039d8c4b83a076cee6c1ff0",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7,
			Changed = "6.2 absorbed an explosion at the briefing before Iz; it is now your vow.",
			IzTriad = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiTerendelevMemoryResonance",
			Scene = "c5/Iz/AreeluIntro",
			ListGuid = "09b8d5eb5dae3634d9dc3d8c7bdd6e9d",
			ExpCr = 7,
			Changed = "6.2 spoke to the dracolich; this list is Areelu's introduction at the gates of Iz, where Terendelev is not."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiTerendelevAngelSolarRedemption",
			Scene = "c5/Iz/AreeluIntro",
			ListGuid = "09b8d5eb5dae3634d9dc3d8c7bdd6e9d",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8,
			Changed = "6.2 laid the dracolich to rest in Areelu's introduction, where Terendelev is not."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiTerendelevMartialGodMercifulRelease",
			Scene = "c5/Iz/AreeluIntro",
			ListGuid = "09b8d5eb5dae3634d9dc3d8c7bdd6e9d",
			Gate = "MartialGodProficiencies",
			ExpCr = 7,
			Changed = "6.2 cut the dracolich free in Areelu's introduction, where Terendelev is not."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiTerendelevLichDominionClaim",
			Scene = "c5/Iz/AreeluIntro",
			ListGuid = "09b8d5eb5dae3634d9dc3d8c7bdd6e9d",
			Gate = "OverlordProficiencies",
			MythicPath = "Lich",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 8,
			Changed = "6.2 enslaved the dracolich in Areelu's introduction, where Terendelev is not."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiTerendelevMastermindRuneSever",
			Scene = "c5/Iz/AreeluIntro",
			ListGuid = "09b8d5eb5dae3634d9dc3d8c7bdd6e9d",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7,
			Changed = "6.2 shattered the dracolich's control gem in Areelu's introduction, where Terendelev is not."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiTerendelevHeroSacredVow",
			Scene = "c5/Iz/AreeluIntro",
			ListGuid = "09b8d5eb5dae3634d9dc3d8c7bdd6e9d",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7,
			Changed = "6.2 laid the dragon's remains to rest in Areelu's introduction, where Terendelev is not."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiStoneManuscriptsTemporalStasis",
			Scene = "c5/Iz/Anemora",
			ListGuid = "c6d5321c350bf7c4fa364e34c0ee30a4",
			ExpCr = 6,
			Changed = "6.2 put the key and the manuscripts in your pack, which this answer does not do."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiStoneManuscriptsAeonArchivalPreservation",
			Scene = "c5/Iz/Anemora",
			ListGuid = "c6d5321c350bf7c4fa364e34c0ee30a4",
			Gate = "MastermindProficiencies",
			MythicPath = "Aeon",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7,
			Changed = "6.2 reassembled tablets into your hands, which this answer does not do."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiStoneManuscriptsMartialGodSpeedSnatch",
			Scene = "c5/Iz/Anemora",
			ListGuid = "c6d5321c350bf7c4fa364e34c0ee30a4",
			Gate = "MartialGodProficiencies",
			ExpCr = 6,
			Changed = "6.2 secured the documents, which this answer does not do."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiStoneManuscriptsGodEmperorSanctum",
			Scene = "c5/Iz/Anemora",
			ListGuid = "c6d5321c350bf7c4fa364e34c0ee30a4",
			Gate = "GodEmperorProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2 preserved the archive for eternity; nothing changes in the temple."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiStoneManuscriptsOverlordTomeHarvest",
			Scene = "c5/Iz/Anemora",
			ListGuid = "c6d5321c350bf7c4fa364e34c0ee30a4",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 6,
			Changed = "6.2 had Anemora drop the books at your feet, which never happens."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDeskariPesticideMockery",
			Scene = "c5/Iz/DeskariFight",
			ListGuid = "7d877541fed521c489e6b33845647ad3",
			ExpCr = 8,
			Changed = "6.2 fried Deskari's swarms before the battle begins."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDeskariGodEmperorSolarJudgment",
			Scene = "c5/Iz/DeskariFight",
			ListGuid = "7d877541fed521c489e6b33845647ad3",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8,
			Changed = "6.2 struck Deskari's torso before the battle begins."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDeskariMartialGodScytheSunder",
			Scene = "c5/Iz/DeskariFight",
			ListGuid = "7d877541fed521c489e6b33845647ad3",
			Gate = "MartialGodProficiencies",
			MythicPath = "Legend",
			ExpCr = 8,
			Changed = "6.2 sheared the teeth off Deskari's scythe before the battle begins."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDeskariOverlordSwarmUsurpation",
			Scene = "c5/Iz/DeskariFight",
			ListGuid = "7d877541fed521c489e6b33845647ad3",
			Gate = "OverlordProficiencies",
			MythicPath = "Demon",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 8,
			Changed = "6.2 turned Deskari's swarm on him before the battle begins."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDeskariMastermindAeonChitinShatter",
			Scene = "c5/Iz/DeskariFight",
			ListGuid = "7d877541fed521c489e6b33845647ad3",
			Gate = "MastermindProficiencies",
			MythicPath = "Aeon",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 8,
			Changed = "6.2 shattered Deskari's exoskeleton before the battle begins."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiDeskariHeroAzataDawnOfMankind",
			Scene = "c5/Iz/DeskariFight",
			ListGuid = "7d877541fed521c489e6b33845647ad3",
			Gate = "HeroProficiencies",
			MythicPath = "Azata",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8,
			Changed = "6.2 had Deskari recoil in fear, which he never does."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzBannerResonantAnchors",
			Scene = "c5/Iz/Banner",
			ListGuid = "20a1be61aa7680e45890656b204e6fe8",
			ExpCr = 7,
			Changed = "6.2 held the banner against the gale; the camp is destroyed and the flagpole stands empty.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzBannerGodEmperorSolarBastion",
			Scene = "c5/Iz/Banner",
			ListGuid = "20a1be61aa7680e45890656b204e6fe8",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 8,
			Changed = "6.2 set a standard ablaze that is no longer there.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzBannerMartialGodUnbreakableLine",
			Scene = "c5/Iz/Banner",
			ListGuid = "20a1be61aa7680e45890656b204e6fe8",
			Gate = "MartialGodProficiencies",
			ExpCr = 7,
			Changed = "6.2 guarded a banner that is no longer there.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzBannerOverlordDarkConsecration",
			Scene = "c5/Iz/Banner",
			ListGuid = "20a1be61aa7680e45890656b204e6fe8",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 7,
			Changed = "6.2 had the dead defend a banner that is no longer there.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzBannerMastermindDefensiveCalculus",
			Scene = "c5/Iz/Banner",
			ListGuid = "20a1be61aa7680e45890656b204e6fe8",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7,
			Changed = "6.2 warded a banner that is no longer there.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiIzBannerHeroEternalOath",
			Scene = "c5/Iz/Banner",
			ListGuid = "20a1be61aa7680e45890656b204e6fe8",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7,
			Changed = "6.2 had defenders roaring round a banner that is no longer there; there are no survivors.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAlderpashOverlordTreaty",
			Scene = "c5/LabyrinthOfBaphometh/Alderpash",
			ListGuid = "a120eec4aaca4d2398cd863e6245a551",
			Gate = "OverlordProficiencies",
			ExpCr = 8,
			Changed = "6.2 had Alderpash kneel in fealty; he stays caged and unbowed."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAlderpashAeonContractAxiom",
			Scene = "c5/LabyrinthOfBaphometh/Alderpash",
			ListGuid = "a120eec4aaca4d2398cd863e6245a551",
			Gate = "MastermindProficiencies",
			MythicPath = "Aeon",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 8,
			Changed = "6.2 dissolved the prison; he stays caged."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAlderpashLegendMartialCageShatter",
			Scene = "c5/LabyrinthOfBaphometh/Alderpash",
			ListGuid = "a120eec4aaca4d2398cd863e6245a551",
			Gate = "MartialGodProficiencies",
			MythicPath = "Legend",
			ExpCr = 8,
			Changed = "6.2 shattered the prison; he stays caged."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAlderpashHeroRedemptionCondition",
			Scene = "c5/LabyrinthOfBaphometh/Alderpash",
			ListGuid = "a120eec4aaca4d2398cd863e6245a551",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 8,
			Changed = "6.2 promised to break the cage there and then; he stays caged."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPuluraAct5FinalDisruption",
			Scene = "c5/PuluraFallsC5/Mutasafen",
			ListGuid = "6278129d95bb73744a60b07fc6612e45",
			ExpCr = 7,
			Changed = "6.2 took his syringes; Mutasafen escapes with them in vanilla."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPuluraAct5MartialGodDecisiveStrike",
			Scene = "c5/PuluraFallsC5/Mutasafen",
			ListGuid = "6278129d95bb73744a60b07fc6612e45",
			Gate = "MartialGodProficiencies",
			ExpCr = 7,
			Changed = "6.2 pinned Mutasafen to the wall; he escapes in vanilla."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPuluraAct5AngelSolarIncineration",
			Scene = "c5/PuluraFallsC5/Mutasafen",
			ListGuid = "6278129d95bb73744a60b07fc6612e45",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7,
			Changed = "6.2 burned all his research; he escapes with it."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPuluraAct5OverlordSoulDissection",
			Scene = "c5/PuluraFallsC5/Mutasafen",
			ListGuid = "6278129d95bb73744a60b07fc6612e45",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 7,
			Changed = "6.2 claimed his soul; he escapes."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPuluraAct5MastermindTriage",
			Scene = "c5/PuluraFallsC5/Mutasafen",
			ListGuid = "6278129d95bb73744a60b07fc6612e45",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7,
			Changed = "6.2 collapsed his escape tunnels; he escapes."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiPuluraAct5HeroAegis",
			Scene = "c5/PuluraFallsC5/Mutasafen",
			ListGuid = "6278129d95bb73744a60b07fc6612e45",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7,
			Changed = "6.2 declared the nightmare over while Mutasafen is still forcing the choice."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct5ChunPaperworkBacklog",
			Scene = "c5/DrezenMain_C5/Chun",
			ListGuid = "6b8b71866da1eee44bdd1d5d93a2a0f3",
			ExpCr = 6,
			ClaimKey = "Port62:Act5ChunPaperworkBacklog",
			Coins = 400,
			SixTwoSponsor = "The Grand Arbiter",
			Gold = 1500
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct5ChunMastermindMatrix",
			Scene = "c5/DrezenMain_C5/Chun",
			ListGuid = "6b8b71866da1eee44bdd1d5d93a2a0f3",
			Gate = "MastermindProficiencies",
			ExpCr = 6
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct5CiarHoldingDrezen",
			Scene = "c5/DrezenMain_C5/Ciar",
			ListGuid = "234a43b05125963409b93aded03a5125",
			ExpCr = 6,
			ClaimKey = "Port62:Act5CiarHoldingDrezen",
			Coins = 350,
			SixTwoSponsor = "The Inheritor"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct5AneviaDemonManhattan",
			Scene = "c5/DrezenMain_C5/KTC_AneviaStart",
			ListGuid = "46f6b0263c6992f43b33a2fda5518983",
			ExpCr = 6,
			ClaimKey = "Port62:Act5AneviaDemonManhattan",
			Coins = 350,
			SixTwoSponsor = "The Song of the Spheres"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct5AneviaHeroRelief",
			Scene = "c5/DrezenMain_C5/KTC_AneviaStart",
			ListGuid = "46f6b0263c6992f43b33a2fda5518983",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2 promised to bring Irabeth back from Iz, but Irabeth is standing in this scene."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct5AnemoraCurator",
			Scene = "c5/Iz/Anemora",
			ListGuid = "c6d5321c350bf7c4fa364e34c0ee30a4",
			ExpCr = 6,
			ClaimKey = "Port62:Act5AnemoraCurator",
			Coins = 400,
			SixTwoSponsor = "The Hermit"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct5AlderpashAcademic",
			Scene = "c5/LabyrinthOfBaphometh/Alderpash",
			ListGuid = "a120eec4aaca4d2398cd863e6245a551",
			ExpCr = 8,
			ClaimKey = "Port62:Act5AlderpashAcademic",
			Coins = 450,
			SixTwoSponsor = "The Hermit"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct5TavernSiegeComfort",
			Scene = "c5/Drezen_Under_Siedge/VendorDangerousTavern",
			ListGuid = "15abc4d1f76f3cc418b593c33c41dda1",
			ExpCr = 6,
			ClaimKey = "Port62:Act5TavernSiegeComfort",
			Coins = 350,
			SixTwoSponsor = "The Lucky Drunk"
		}
	};

	internal static Port62LateActs.Graft Find(string answer)
	{
		return Grafts.FirstOrDefault((Port62LateActs.Graft g) => g.Answer == answer);
	}

	public static void Add()
	{
		Port(DialogueBeats.CiarWelcomesYouBack, "IsekaiDrezenReturnGodEmperorMajesty", "(God Emperor) [Angel] [The Emperor's Sun Returns] \"I have walked through the depths of hell and brought back victory. Drezen, rise and bask in the radiant dawn of mortal triumph!\"", "{n}Golden light spills across the courtyard, and Ciar bows his gray head in its warmth.{/n}");
		Port(DialogueBeats.CiarWelcomesYouBack, "IsekaiDrezenReturnMartialGodPoise", "(Martial God) [The Return of the Unconquered] \"The demons of Alushinyrra tested my blade and broke. Anyone who thought Drezen would fall in my absence will now see true mastery.\"", "{n}Crusader veterans stand at rigid attention, awe-struck by your serene, terrifying combat aura.{/n}");
		Port(DialogueBeats.CiarWelcomesYouBack, "IsekaiDrezenReturnOverlordDominion", "(Overlord) [Iron Dominion Reasserted] \"My absence was merely a brief recess. Drezen belongs to me, and any traitor who wavered will answer for their treason.\"", "{n}Ciar stiffens as your suffocating aura rolls over the courtyard, and the soldiers nearby fall silent.{/n}");
		Port(DialogueBeats.CiarWelcomesYouBack, "IsekaiDrezenReturnMastermindReorganization", "(Mastermind) [Crusade Grid Reboot] \"Logistical deficit: 31%. Morale attrition: 19%. By executing emergency reallocation protocol Alpha-7, Drezen will be at 100% operational readiness in 48 hours.\"", "{n}Ciar blinks in disbelief as your calculations lay out a plan for weeks of supply chaos in a single breath.{/n}");
		Port(DialogueBeats.CiarWelcomesYouBack, "IsekaiDrezenReturnHeroTriumphantEmbrace", "(Hero) [We Survived Together] \"I swore I would return for all of you! Hold your heads high, crusaders; the end of the Worldwound is within our grasp!\"", "{n}Tears of relief shine in Ciar's tired eyes, and the soldiers nearby raise a ragged cheer at your return.{/n}");
		PortNarrated(DialogueBeats.AneviaAfterTheQueenFell, "IsekaiCoronationProtagonistSovereignty", "(Isekai Protagonist) [Narrative Climax - The True Sovereign] \"Then I'll carry what she carried. I am the protagonist of this tale, Anevia, and no crown and no demon is taking this crusade from the people who bled for it!\"", "{n}Anevia's jaw sets, and the soldiers within earshot straighten as your words carry through the hall with multiversal certainty.{/n}");
		PortNarrated(DialogueBeats.AneviaAfterTheQueenFell, "IsekaiCoronationGodEmperorSelfCoronation", "(God Emperor) [Angel] [The Imperial Mandate] \"By divine authority and the will of the mortal realm, I take up the Queen's burden. Mendev will not stand leaderless for a single day!\"", "{n}A celestial halo flares at your brow in solar brilliance, and every soldier in the hall bows their head.{/n}");
		PortNarrated(DialogueBeats.AneviaAfterTheQueenFell, "IsekaiCoronationMartialGodEdictSunder", "(Martial God) [The Blade That Remains] \"My power was never granted by a crown, Anevia, and it does not leave with one. The crusade still has a blade, and it is mine.\"", "{n}You draw your weapon in a single soundless motion, and the grieving soldiers take heart at the sight of its edge.{/n}");
		PortNarrated(DialogueBeats.AneviaAfterTheQueenFell, "IsekaiCoronationOverlordAbsoluteSubmission", "(Overlord) [Lich] [The Living Shall Obey] \"Grief is a luxury for the living, Anevia. The Queen is gone; the crusade answers to the master of life and death now.\"", "{n}Chilling frost creeps along the walls as the dead whisper your dread name, and the guards at the doors do not dare to meet your eyes.{/n}");
		PortNarrated(DialogueBeats.AneviaAfterTheQueenFell, "IsekaiCoronationMastermindConstitutionalAudit", "(Mastermind) [Chain of Command Audit] \"Under Article IV of the First Crusade Charter, command of the crusade passes to the Knight-Commander in the field. No council has to ratify it. The army is mine to lead, and I will lead it.\"", "{n}Anevia lets out a tired breath of relief as your ironclad legal logic settles the question before anyone can raise it.{/n}");
		PortNarrated(DialogueBeats.AneviaAfterTheQueenFell, "IsekaiCoronationHeroShieldOfThePeople", "(Hero) [Champion of the People] \"I never fought for glory or crowns, Anevia. I fight for the people of Golarion, the way she did, and I will protect them until the very end!\"", "{n}A murmur of deep respect ripples through the crusader bodyguards, many of them bowing their heads in reverence.{/n}");
		Port(DialogueBeats.GalfreyBriefsYouForIz, "IsekaiIzPlagueBombsTriadSalvation", "(Isekai Protagonist) [Dimensional Sprint - Triad Salvation] \"If Iz is a trap, I'll be faster than it. Flash-step, fuse cut, hostages out of the blast radius. Nobody we bring in is staying there.\"", "{n}Galfrey studies you for a long moment, then gives a short, approving nod.{/n} \"Then plan for it, Commander. Irabeth and I will hold our lines until you reach us.\"");
		Port(DialogueBeats.GalfreyBriefsYouForIz, "IsekaiIzMartialGodKiFlash", "(Martial God) [Supersonic Ki Flash] \"If Mutasafen has left us any of his concoctions in Iz, a supersonic step will cut the detonator before sound can travel.\"", "{n}Galfrey's lips twitch in the ghost of a smile.{/n} \"I will hold you to that. Mutasafen has left traps for better crusaders than us.\"");
		Port(DialogueBeats.GalfreyBriefsYouForIz, "IsekaiIzAngelSolarAegisSave", "(God Emperor) [Angel] [Solar Aegis of the Triad] \"Whatever waits in Iz, Heaven's barrier will stand over the Sword of Valor, the Queen, and the library at once. No foul poison will reach them while I live!\"", "{n}Golden light flares around you, and even the Queen of Mendev has to shield her eyes.{/n} \"A bold vow. See that Iz lets you keep it.\"");
		Port(DialogueBeats.GalfreyBriefsYouForIz, "IsekaiIzMastermindTemporalDisplacement", "(Mastermind) [Temporal Phase Displacement] \"I've prepared a calibrated chronomantic delay. If anything in Iz is rigged to go off, its fuse runs thirty seconds late, and that is all the time an evacuation needs.\"", "{n}Galfrey nods slowly, weighing your calculations.{/n} \"Thirty seconds can decide a battle. Keep them in reserve, Commander.\"");
		Port(DialogueBeats.GalfreyBriefsYouForIz, "IsekaiIzHeroUltimateShield", "(Hero) [The Indomitable Shield] \"If anything in Iz blows up, I'll take the blast myself. You and Irabeth stay behind me, Your Majesty. That's not a request.\"", "{n}Galfrey's stern face softens for a heartbeat.{/n} \"Spoken like a true crusader. Let us all come back from Iz, then.\"");
		Port(DialogueBeats.AreeluAtTheGatesOfIz, "IsekaiTerendelevMemoryResonance", "(Isekai Protagonist) [Soul Resonance: The Scale of Kenabres] \"If Deskari has made Terendelev his butcher somewhere in this city, I'll show her this scale from Kenabres. She protected this world for centuries; she deserves to remember it.\"", "{n}Areelu glances at the scale, and something unreadable crosses her face.{/n} \"Sentiment. Carry it if you must, but do not expect the dead to thank you.\"");
		Port(DialogueBeats.AreeluAtTheGatesOfIz, "IsekaiTerendelevAngelSolarRedemption", "(God Emperor) [Angel] [Heaven's Dragon Rest] \"If Deskari has defiled Terendelev's remains, her long watch is ended. I will send her back to Heaven's golden skies with honor and dignity.\"", "{n}Areelu's crimson eyes narrow at the golden light rising around you.{/n} \"Heaven's promises. You make them so easily.\"");
		Port(DialogueBeats.AreeluAtTheGatesOfIz, "IsekaiTerendelevMartialGodMercifulRelease", "(Martial God) [The Warrior's Requiem] \"If the dragon who fought for Kenabres stands against us here, my blade will grant her a swift, honorable release worthy of a warrior.\"", "{n}Areelu shrugs, uninterested.{/n} \"Swift or slow, the end is the same. Mind your own.\"");
		Port(DialogueBeats.AreeluAtTheGatesOfIz, "IsekaiTerendelevLichDominionClaim", "(Overlord) [Lich] [The Dragon Kneels to the Sovereign] \"If Deskari's crude puppetry has raised Terendelev, it ends with me. A dragon that dead should serve a true master of undeath.\"", "{n}For the first time, Areelu looks at you with something like professional interest.{/n} \"Ambitious. We shall see whether the Abyss agrees.\"");
		Port(DialogueBeats.AreeluAtTheGatesOfIz, "IsekaiTerendelevMastermindRuneSever", "(Mastermind) [Necromantic Matrix Severance] \"Any undead Deskari animates here will be anchored through a control focus. Find the focus, shatter it, and the reanimation collapses.\"", "{n}Areelu inclines her head.{/n} \"Correct, in principle. The practice is always messier.\"");
		Port(DialogueBeats.AreeluAtTheGatesOfIz, "IsekaiTerendelevHeroSacredVow", "(Hero) [We Will Avenge You] \"Terendelev gave her life to protect Kenabres. Whatever Deskari has done with her, we will defeat him and save the world she died for. That is my sacred promise!\"", "{n}Areelu says nothing, but for a moment she looks toward the ruins as if listening for something.{/n}");
		Port(DialogueBeats.AnemoraRevealsHerLord, "IsekaiStoneManuscriptsTemporalStasis", "(Isekai Protagonist) [Temporal Stasis - Rapid Acquisition] \"If you so much as reach for a torch, time will slow and my dimensional grasp will pull the Glass Key and the Stone Manuscripts clear before the flames touch them.\"", "{n}Anemora's dead eyes follow yours to the stone manuscripts, and she smiles thinly, as if you had missed the point entirely.{/n}");
		Port(DialogueBeats.AnemoraRevealsHerLord, "IsekaiStoneManuscriptsAeonArchivalPreservation", "(Mastermind) [Aeon] [Axiomatic Data Reconstruction] \"Information cannot be destroyed under fundamental cosmic conservation laws. Burn whatever you like; I can read these manuscripts back from their echoes.\"", "{n}Anemora hisses as the air around the stone tablets shimmers with faint sapphire traces of every word carved into them.{/n}");
		Port(DialogueBeats.AnemoraRevealsHerLord, "IsekaiStoneManuscriptsMartialGodSpeedSnatch", "(Martial God) [Zero-Point Draw: Archive Retrieval] \"Faster than the ignition of oil, my blade-wind will put out every torch in here before a single spark reaches the documents.\"", "{n}A localized vacuum snuffs out the nearest torches in a single heartbeat, and the temple falls into gloom.{/n}");
		Port(DialogueBeats.AnemoraRevealsHerLord, "IsekaiStoneManuscriptsGodEmperorSanctum", "(God Emperor) [Solar Preservation] \"No profane flame of the Abyss shall touch this sacred history. By imperial will, this archive is preserved for eternity!\"", "{n}Solar light settles over the shelves of stone manuscripts like a shield, and Anemora's cultists step back from it.{/n}");
		Port(DialogueBeats.AnemoraRevealsHerLord, "IsekaiStoneManuscriptsOverlordTomeHarvest", "(Overlord) [Forbidden Archive Acquisition] \"These ancient secrets belong to my personal library. Anemora, surrender the records or be flayed alive where you stand.\"", "{n}The cultist priestess shivers under your gaze, but she keeps her place before her lord's records.{/n}");
		Port(DialogueBeats.DeskariLooksDown, "IsekaiDeskariPesticideMockery", "(Isekai Protagonist) [Pesticide Protocol: Apex Bug-Zapper] \"Deskari! You took Kenabres with a sneak attack, but today you face an Otherworlder prepared for a raid boss! Time for the ultimate bug zapper!\"", "{n}Deskari's laughter booms over the ruins as a crackling grid of multiversal mana flares around your party, and the nearest locusts veer away from it.{/n}");
		Port(DialogueBeats.DeskariLooksDown, "IsekaiDeskariGodEmperorSolarJudgment", "(God Emperor) [Angel] [The Dawn That Burns the Swarm] \"Lord of Locusts! For every drop of innocent blood shed on Golarion, heaven's eternal dawn sentences you to total obliteration!\"", "{n}A pillar of solar wrath blazes up from where you stand, lighting the ruins of Iz, and for a moment the Lord of Locusts has to look away.{/n}");
		Port(DialogueBeats.DeskariLooksDown, "IsekaiDeskariMartialGodScytheSunder", "(Martial God) [Legend] [The God-Slaying Counter] \"You cleaved Kenabres with your scythe, insect. Now witness a blade that cuts through gods!\"", "{n}A sonic blast of peerless martial force rolls across the ruins, and the towering demon lord turns his scythe toward you with new attention.{/n}");
		Port(DialogueBeats.DeskariLooksDown, "IsekaiDeskariOverlordSwarmUsurpation", "(Overlord) [Demon] [Swarm Usurper] \"You called yourself a Demon Lord, Deskari? You are merely vermin. Your locusts will learn who their true master is!\"", "{n}The locusts around Deskari falter, some of them hesitating in the air as your dominant abyssal will pulls at them. Deskari roars in fury.{/n}");
		Port(DialogueBeats.DeskariLooksDown, "IsekaiDeskariMastermindAeonChitinShatter", "(Mastermind) [Aeon] [Axiomatic Resonant Frequency] \"Your chitin matrix has a resonant shear frequency of 14,200 Hertz. When I emit the counter-wave, your carapace dissolves.\"", "{n}Deskari's vast carapace hums in answer to the probing tone, and the demon lord's laughter takes on an edge of irritation.{/n}");
		Port(DialogueBeats.DeskariLooksDown, "IsekaiDeskariHeroAzataDawnOfMankind", "(Hero) [Azata] [The Song of Kenabres Avenged] \"For everyone who lost someone in Kenabres! For every family torn apart! We stand together, and we bring back the dawn!\"", "{n}A glorious chorus of celestial harmonies rings across Iz, filling your companions with unstoppable courage as they face the demon lord.{/n}");
		PortNarrated(DialogueBeats.TheEmptyFlagpole, "IsekaiIzBannerResonantAnchors", "(Isekai Protagonist) [Multiversal Banner Leyline Anchor] \"The Sword of Valor flew high over Drezen, and it will fly again. I'm marking this flagpole with a mana anchor so we can find our way back and raise it here!\"", "{n}A faint resonant pulse runs up the empty flagpole and lingers there, a small point of light amid the carnage.{/n}");
		PortNarrated(DialogueBeats.TheEmptyFlagpole, "IsekaiIzBannerGodEmperorSolarBastion", "(God Emperor) [Angel] [The Emperor's Sun Banner] \"By holy decree, this pole will bear the standard again. Until then, let the dawn of mortal victory burn where it stood!\"", "{n}Golden flames leap along the bare flagpole and burn there without consuming it, keeping the carrion birds away from the fallen.{/n}");
		PortNarrated(DialogueBeats.TheEmptyFlagpole, "IsekaiIzBannerMartialGodUnbreakableLine", "(Martial God) [The Unbreakable Sentinel] \"They took the banner over the bodies of good knights. Any fiend that comes back for this ground will have to step through the storm of my blade.\"", "{n}You plant your weapon beside the empty flagpole, and the scorched ground fractures under your supreme martial pressure.{/n}");
		PortNarrated(DialogueBeats.TheEmptyFlagpole, "IsekaiIzBannerOverlordDarkConsecration", "(Overlord) [Consecration of the Dread Banner] \"The martyrs who fell here will not rest until their banner is returned. Rise, fallen crusaders, and guard this ground for me!\"", "{n}Spectral knights stir among the ash and salute your dread majesty before fading into the smoke, waiting for their standard.{/n}");
		PortNarrated(DialogueBeats.TheEmptyFlagpole, "IsekaiIzBannerMastermindDefensiveCalculus", "(Mastermind) [Recovery Calculus] \"Banner lost, camp destroyed, no survivors in sight. Probability that the standard is lost for good: low, if we move now.\"", "{n}Your calculations cut through the horror of the scene, and your companions ready themselves to move on.{/n}");
		PortNarrated(DialogueBeats.TheEmptyFlagpole, "IsekaiIzBannerHeroEternalOath", "(Hero) [The Banner of the Living] \"For everyone who bled for Mendev here, we will take that banner back! We will not take a single step backward!\"", "{n}A warm, stubborn light fills your companions' eyes as they look from the fallen knights to the empty pole.{/n}");
		Port(DialogueBeats.AlderpashInHisPrison, "IsekaiAlderpashOverlordTreaty", "(Overlord) [Sovereign Planar Treaty] \"Runelord of Wrath, remember who stands outside your cage. Swear fealty to the Great Tomb, and perhaps one day I will shatter it.\"", "{n}Alderpash studies you through the shimmering wall of his prison, measuring you as one tyrant measures another.{/n} \"Perhaps. Ten thousand years have taught me patience, if nothing else.\"");
		Port(DialogueBeats.AlderpashInHisPrison, "IsekaiAlderpashAeonContractAxiom", "(Mastermind) [Aeon] [Axiomatic Sentence Review] \"Ten thousand years exceeds the statutory maximum sentence for planar wrath transgressions. By the authority of the cosmic scales, your confinement is under review.\"", "{n}The glowing runes of the sphere flicker crystalline blue for a moment, then settle back into their ancient pattern. The cage holds, but Alderpash watches you with new interest.{/n}");
		Port(DialogueBeats.AlderpashInHisPrison, "IsekaiAlderpashLegendMartialCageShatter", "(Martial God) [Legend] [Transcendent Sunder: Prison Test] \"An unbreakable magical prison? There is no concept in existence my blade cannot sever. Stand back, old wizard, I want to see how it holds.\"", "{n}Your strike rings off the sphere with a thunderous crack that shakes the whole labyrinth. A hairline fracture races across the force wall, then seals itself.{/n} \"Impressive,\" {n}Alderpash murmurs.{/n} \"Baphomet builds his cages well.\"");
		Port(DialogueBeats.AlderpashInHisPrison, "IsekaiAlderpashHeroRedemptionCondition", "(Hero) [Freedom with a Conscience] \"If I ever break this cage, Alderpash, it will only be after you swear on your ancient honor never to bring war or tyranny to the mortal realms again.\"", "{n}The ancient Runelord bows his head with genuine solemnity.{/n} \"You would have my word, young champion. Ten thousand years has taught me the futility of mortal tyrannies.\"");
		Port(DialogueBeats.MutasafenForcesTheChoice, "IsekaiPuluraAct5FinalDisruption", "(Isekai Protagonist) [Checkmate: The Final Save] \"Mutasafen, how many times must I humiliate your research before you realize you cannot outmaneuver an Otherworlder? It's over!\"", "{n}Mutasafen screeches in panicked hysteria, clutching his last batch of nahyndrian syringes as your telekinetic ward snaps shut an inch from his claws.{/n}");
		Port(DialogueBeats.MutasafenForcesTheChoice, "IsekaiPuluraAct5MartialGodDecisiveStrike", "(Martial God) [Instant Severance: Escape Intercept] \"Crawl into your escape tunnel if you like. One step to close the distance, one stroke to end your experiments, the day I catch you.\"", "{n}A flash of steel bites into the stone where Mutasafen stood a heartbeat before; he is already scrambling for his escape flask.{/n}");
		Port(DialogueBeats.MutasafenForcesTheChoice, "IsekaiPuluraAct5AngelSolarIncineration", "(God Emperor) [Angel] [Solar Purification of the Blight] \"Your grotesque experiments have desecrated the stars for long enough. Burn in heaven's cleansing dawn!\"", "{n}Golden celestial fire sweeps across the cavern, and Mutasafen snatches his research notes out of its path with a shriek.{/n}");
		Port(DialogueBeats.MutasafenForcesTheChoice, "IsekaiPuluraAct5OverlordSoulDissection", "(Overlord) [Soul Extraction] \"Your twisted intellect will serve my grand library. Yield your soul, alchemist, or suffer eternal torment in my soul crucible.\"", "{n}Mutasafen's feathers stand on end as your shadow reaches for his essence, and his voice cracks as he backs toward the exit.{/n}");
		Port(DialogueBeats.MutasafenForcesTheChoice, "IsekaiPuluraAct5MastermindTriage", "(Mastermind) [Predictive Starlight Lockdown] \"Your escape vector was mapped before we entered the canyon. Run if you like; I know every tunnel you can take.\"", "{n}Mutasafen glances at the cavern exits, and for once the alchemist does not have a clever answer.{/n}");
		Port(DialogueBeats.MutasafenForcesTheChoice, "IsekaiPuluraAct5HeroAegis", "(Hero) [Sanctuary of the Stars] \"Priestesses of Pulura, hold on! As long as we stand here, no demon will harm this sanctuary!\"", "{n}Tears of gratitude fill the priestesses' eyes as your heroic aura steadies them against the chaos.{/n}");
		Port(DialogueBeats.ChunHub, "IsekaiAct5ChunPaperworkBacklog", "(Isekai Protagonist) [Six Months of Overdue Citadel Invoices] \"Chun, I survived the Abyss, but staring at this towering mountain of six months of backlogged supply requisitions and unpaid blacksmith invoices might actually kill me. Where is the 'mark all as read' button?!\"", "{n}Chun lets out a dry, exhausted chuckle, frantically pushing stacks of parchment aside to uncover an inkwell.{/n} \"A 'mark all as read button'?! Ha! If only the heavens were so merciful, Commander! Half the merchant cartels in Mendev thought you were dead and tried to double-charge for rations! But now that you're back... we'll balance these books in three days flat!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"The true final boss of any crusade: catastrophic bureaucratic backlogs!\"</i>"));
		Port(DialogueBeats.ChunHub, "IsekaiAct5ChunMastermindMatrix", "(Mastermind) [Quad-Quadrant Organizational Matrix] \"Color-code the requisitions: green for immediate grain, blue for cold iron, red for royal taxes to be disputed. This backlog clears by nightfall.\"", "{n}Chun stares at the quadrant system in religious awe.{/n} \"It's... it's pure mathematical genius!\"");
		Port(DialogueBeats.CiarWelcomesYouBack, "IsekaiAct5CiarHoldingDrezen", "(Isekai Protagonist) [Commending the Wall Defenders] \"Sir Ciar, you and the Everbright knights held these walls while the Queen took the army to Iz and I was lost in the Abyss. Whatever anyone says, you saved this city from becoming a smoking crater. Outstanding work, knight.\"", "{n}Sir Ciar stands rigid, his scarred jaw clenching as he swallows a surge of hard-won pride, executing a slow, solemn bow of absolute respect.{/n} \"We held our ground because we believed you would return, Commander. When the demons pounded on the lower gate, we reminded each other that as long as the Otherworlder lives, Drezen will not fall. Welcome home.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"Honor and unyielding steadfastness rewarded! The bastion stands!\"</i>"));
		Port(DialogueBeats.AneviaDrezenReturnHub, "IsekaiAct5AneviaDemonManhattan", "(Isekai Protagonist) [Catching Up on Gossip from Demon Manhattan] \"Anevia, give me the debrief: while I was dodging demon kingpins in the Abyss, who made a mess of my city? And more importantly, are you and Irabeth holding up?\"", "{n}Anevia laughs--a warm, tired, deeply relieved laugh as she leans against the map table.{/n} \"'Demon Manhattan'?! Only you could survive the Midnight Isles and treat it like a bad business trip! Beth and I are still breathing, thanks to you. And now that you're back... let's clean up the Queen's mess and finish this war.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"The reunion of faithful friends brings light back to Drezen!\"</i>"));
		Port(DialogueBeats.AneviaDrezenReturnHub, "IsekaiAct5AneviaHeroRelief", "(Hero) [Good] [Unbroken Bond] \"Seeing you and Irabeth alive and well is the greatest victory I could ask for, Anevia. Whatever comes next, we face it together; I swear it.\"", "{n}Anevia nods with tight, grateful determination.{/n} \"I know we will, Commander. Just come back in one piece.\"");
		Port(DialogueBeats.AnemoraRevealsHerLord, "IsekaiAct5AnemoraCurator", "(Isekai Protagonist) [Museum Curator Appraisal of Ancient Sarkoris] \"Anemora, scribbling demonic graffiti over ten-thousand-year-old Sarkorian stone tablets is an unforgivable crime against multiversal archaeology. In my world, museum curators would have you jailed for defacing historical artifacts, even before we execute you for treason!\"", "{n}Anemora shrieks in furious derision, her desiccated hands clawing the stone manuscripts.{/n} \"'Museum curators'?! 'Defacing artifacts'?! Foolish Otherworlder, the glory of Deskari drowns all your petty history in swarms and ash!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Defending ancient historical relics against abyssal vandalism! A true scholar!\"</i>"));
		Port(DialogueBeats.AlderpashInHisPrison, "IsekaiAct5AlderpashAcademic", "(Isekai Protagonist) [Ancient Academic Boredom & Prison Hobbies] \"Runelord Alderpash, spending ten millennia trapped in a demonic maze would drive anyone mad. Back home, long-term prisoners take up wood carving, write ten-volume autobiographies, or do calisthenics. Did you at least invent a new branch of mathematics to pass the time?\"", "{n}The ancient Runelord of Wrath stares at you, his withered face cracking into an incredulous, cackling laugh that echoes off the iron bars.{/n} \"'Wood carving' and 'calisthenics'?! Hahaha! Mortal, you are speaking to the sovereign master of Thassilonian wrath magic, not an indolent convict! But... I did indeed derive forty-seven new runic theorems while waiting for Baphomet to rot! Tell me of your world's magic, stranger!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Debating high arcane theory with an ancient Runelord. Superb intellectual parley.\"</i>"));
		Port(DialogueBeats.DangerousTavernHub, "IsekaiAct5TavernSiegeComfort", "(Isekai Protagonist) [Tavern Defense Reassurance] \"Keep behind the barricade, barkeep. I didn't return from the Abyss just to let demon locusts drink our cold brew. The vanguard is here; stay low and pour the celebration ale once the dust settles.\"", "{n}The terrified barkeep nods frantically, clutching a heavy iron ladle like a warhammer.{/n} \"You heard the Commander! Hold the doors! Pour the victory ale!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"Protecting the sacred taps from abyssal defilement! Charge!\"</i>"));
	}

	private static void Port(GraftBeat beat, string name, string answerText, string replyText, string line = null)
	{
		Port62LateActs.Graft graft = Port62LateActs.Find(Grafts, name, beat, "Port62Act5", Port62LateActs.ReplyShape.DialogSpeaker);
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
		Port62LateActs.Graft graft = Port62LateActs.Find(Grafts, name, beat, "Port62Act5", Port62LateActs.ReplyShape.Narrated);
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
}
