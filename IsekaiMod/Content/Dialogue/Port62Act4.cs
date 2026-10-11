using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62Act4
{
	internal static readonly Port62LateActs.Graft[] Grafts = new Port62LateActs.Graft[69]
	{
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNexusCampTricksterArrival",
			Scene = "c4/Nexus_Camp/HeraldIntro",
			ListGuid = "91097a9b90537a34fbe379d5aa4d9cbe",
			ExpCr = 5
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNexusCampAngelConsecration",
			Scene = "c4/Nexus_Camp/HeraldIntro",
			ListGuid = "91097a9b90537a34fbe379d5aa4d9cbe",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2 said the Nexus became a permanent oasis of celestial peace; nothing in the camp changes."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNexusCampMartialGodAcclimation",
			Scene = "c4/Nexus_Camp/HeraldIntro",
			ListGuid = "91097a9b90537a34fbe379d5aa4d9cbe",
			Gate = "MartialGodProficiencies",
			ExpCr = 5
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNexusCampOverlordTerritoryClaim",
			Scene = "c4/Nexus_Camp/HeraldIntro",
			ListGuid = "91097a9b90537a34fbe379d5aa4d9cbe",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 5
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNexusCampMastermindPlanarMapping",
			Scene = "c4/Nexus_Camp/HeraldIntro",
			ListGuid = "91097a9b90537a34fbe379d5aa4d9cbe",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 5
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNexusCampHeroSteadfastHeart",
			Scene = "c4/Nexus_Camp/HeraldIntro",
			ListGuid = "91097a9b90537a34fbe379d5aa4d9cbe",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 5
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiFleshmarketTricksterLiquidation",
			Scene = "c4/FleshMarket/KingpinSlaver_dialogue",
			ListGuid = "c896137fe31c15445bba592672ebfbc9",
			ExpCr = 6,
			Changed = "6.2's reply shattered every slave collar in the plaza; the conversation goes on with every slave still chained."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiFleshmarketAngelHolyLiberation",
			Scene = "c4/FleshMarket/KingpinSlaver_dialogue",
			ListGuid = "c896137fe31c15445bba592672ebfbc9",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2's reply burned every manacle to vapor; nobody is freed in this conversation."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiFleshmarketMartialGodSunderSlavers",
			Scene = "c4/FleshMarket/KingpinSlaver_dialogue",
			ListGuid = "c896137fe31c15445bba592672ebfbc9",
			Gate = "MartialGodProficiencies",
			ExpCr = 6
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiFleshmarketOverlordTitheDominion",
			Scene = "c4/FleshMarket/KingpinSlaver_dialogue",
			ListGuid = "c896137fe31c15445bba592672ebfbc9",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 6,
			Changed = "6.2 had Dyunk prostrate himself and hand over the market, which the conversation then ignores."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiFleshmarketMastermindCartelAudit",
			Scene = "c4/FleshMarket/KingpinSlaver_dialogue",
			ListGuid = "c896137fe31c15445bba592672ebfbc9",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 6,
			Changed = "6.2 emptied Dyunk's accounts, which never happens; it is now a threat."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiFleshmarketHeroAegisVow",
			Scene = "c4/FleshMarket/KingpinSlaver_dialogue",
			ListGuid = "c896137fe31c15445bba592672ebfbc9",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2 had the slaves cheering behind a barrier pushing the guards back; no fight happens here."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBattleblissTournamentArcMockery",
			Scene = "c4/BattleBliss/Atselm_dialogue",
			ListGuid = "6edb974ede9417542abc4f2fc7c1bade",
			ExpCr = 6,
			Changed = "6.2's reply was Zeklex's, who is not in Atselm's conversation."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBattleblissMartialGodApexGladiator",
			Scene = "c4/BattleBliss/Atselm_dialogue",
			ListGuid = "6edb974ede9417542abc4f2fc7c1bade",
			Gate = "MartialGodProficiencies",
			MythicPath = "Legend",
			ExpCr = 7,
			Changed = "6.2 silenced forty thousand spectators; this is a quiet talk with Atselm, not a fight in the arena."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBattleblissGodEmperorGladiatorLiberation",
			Scene = "c4/BattleBliss/Atselm_dialogue",
			ListGuid = "6edb974ede9417542abc4f2fc7c1bade",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2 burned away the gladiators' brands in a scene where there are none; Atselm now answers."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBattleblissOverlordBloodDomain",
			Scene = "c4/BattleBliss/Atselm_dialogue",
			ListGuid = "6edb974ede9417542abc4f2fc7c1bade",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 6,
			Changed = "6.2 addressed Zeklex and a crowd; the scene is Atselm's."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBattleblissMastermindTacticalAnalysis",
			Scene = "c4/BattleBliss/Atselm_dialogue",
			ListGuid = "6edb974ede9417542abc4f2fc7c1bade",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 6,
			Changed = "6.2's reply was Zeklex's, who is not in Atselm's conversation."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBattleblissHeroAzataFreedomSong",
			Scene = "c4/BattleBliss/Atselm_dialogue",
			ListGuid = "6edb974ede9417542abc4f2fc7c1bade",
			Gate = "HeroProficiencies",
			MythicPath = "Azata",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2 started a gladiator revolt that never happens."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHerraxSocialDeduction",
			Scene = "c4/TenThousandDelights/Herraxa_dialogue",
			ListGuid = "43f93812d6216c94db356622859397f1",
			ExpCr = 5,
			Changed = "6.2 had Herrax hand over her guest ledger, which she never does."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHerraxGodEmperorSolarPurge",
			Scene = "c4/TenThousandDelights/Herraxa_dialogue",
			ListGuid = "43f93812d6216c94db356622859397f1",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2 had Herrax burning and confessing; the conversation goes on as before."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHerraxMartialGodBladeWard",
			Scene = "c4/TenThousandDelights/Herraxa_dialogue",
			ListGuid = "43f93812d6216c94db356622859397f1",
			Gate = "MartialGodProficiencies",
			ExpCr = 5,
			Changed = "6.2 killed three assassins in a scene where no fight happens."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHerraxOverlordDarkCoercion",
			Scene = "c4/TenThousandDelights/Herraxa_dialogue",
			ListGuid = "43f93812d6216c94db356622859397f1",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 5,
			Changed = "6.2 handed Herrax's network over to you, which nothing in the game follows up."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHerraxHeroAzataDreamLiberation",
			Scene = "c4/TenThousandDelights/Herraxa_dialogue",
			ListGuid = "43f93812d6216c94db356622859397f1",
			Gate = "HeroProficiencies",
			MythicPath = "Azata",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2 opened every locked door and freed the captives, which never happens."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiMageTowerPortalAlignment",
			Scene = "c4/Arcs/AlushPortals",
			ListGuid = "e76902cd78ea1d24687eed6a97c51598",
			ExpCr = 5,
			Changed = "6.2 wrote these for a rotating mage tower; the list is the Alushinyrra portal arches.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiMageTowerAeonGravitationalOrder",
			Scene = "c4/Arcs/AlushPortals",
			ListGuid = "e76902cd78ea1d24687eed6a97c51598",
			Gate = "MastermindProficiencies",
			MythicPath = "Aeon",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 6,
			Changed = "6.2 wrote these for a rotating mage tower; the list is the Alushinyrra portal arches.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiMageTowerLegendMartialSunder",
			Scene = "c4/Arcs/AlushPortals",
			ListGuid = "e76902cd78ea1d24687eed6a97c51598",
			Gate = "MartialGodProficiencies",
			MythicPath = "Legend",
			ExpCr = 6,
			Changed = "6.2 cut a rift to a wizard's sanctum; the list is the portal arches, which still need their keys.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiMageTowerLichLeylineDrain",
			Scene = "c4/Arcs/AlushPortals",
			ListGuid = "e76902cd78ea1d24687eed6a97c51598",
			Gate = "OverlordProficiencies",
			MythicPath = "Lich",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 6,
			Changed = "6.2 drained a tower and forced its doors; the list is the portal arches.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiMageTowerHeroPathfinderGuide",
			Scene = "c4/Arcs/AlushPortals",
			ListGuid = "e76902cd78ea1d24687eed6a97c51598",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 5,
			Changed = "6.2 guided the party through a tower; the list is the portal arches.",
			Narrated = true
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNocticulaExitStrategyPsychology",
			Scene = "c4/Nocticula_main",
			ListGuid = "75678bafb5e33854baaee17ee6eaf69c",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNocticulaGodEmperorSovereignParity",
			Scene = "c4/Nocticula_main",
			ListGuid = "75678bafb5e33854baaee17ee6eaf69c",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNocticulaMartialGodBladeWarning",
			Scene = "c4/Nocticula_main",
			ListGuid = "75678bafb5e33854baaee17ee6eaf69c",
			Gate = "MartialGodProficiencies",
			ExpCr = 6
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNocticulaOverlordPlanarPact",
			Scene = "c4/Nocticula_main",
			ListGuid = "75678bafb5e33854baaee17ee6eaf69c",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 6
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNocticulaMastermindAeonForensics",
			Scene = "c4/Nocticula_main",
			ListGuid = "75678bafb5e33854baaee17ee6eaf69c",
			Gate = "MastermindProficiencies",
			MythicPath = "Aeon",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7,
			Changed = "6.2 revealed a plot to assassinate Nocticula within 48 hours, a story fact the game never has."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiNocticulaHeroAzataRedemptionWhisper",
			Scene = "c4/Nocticula_main",
			ListGuid = "75678bafb5e33854baaee17ee6eaf69c",
			Gate = "HeroProficiencies",
			MythicPath = "Azata",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiChivarroVanityDeconstruction",
			Scene = "c4/TenThousandDelights/Chivarro_dialogue",
			ListGuid = "abf7287a88afc554da91b3cd5d7c5bd1",
			ExpCr = 5
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiChivarroMartialGodInstantSever",
			Scene = "c4/TenThousandDelights/Chivarro_dialogue",
			ListGuid = "abf7287a88afc554da91b3cd5d7c5bd1",
			Gate = "MartialGodProficiencies",
			ExpCr = 5,
			Changed = "6.2 disarmed Chivarro and demanded keys to a portal; the scene is her asking you for a favor."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiChivarroGodEmperorImperialExile",
			Scene = "c4/TenThousandDelights/Chivarro_dialogue",
			ListGuid = "abf7287a88afc554da91b3cd5d7c5bd1",
			Gate = "GodEmperorProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 5,
			Changed = "6.2 drove the guards into retreat; the conversation goes on with everyone in place."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiChivarroOverlordEnslavement",
			Scene = "c4/TenThousandDelights/Chivarro_dialogue",
			ListGuid = "abf7287a88afc554da91b3cd5d7c5bd1",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 5,
			Changed = "6.2 had Chivarro kneel; she then goes on bargaining as before."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiChivarroHeroMercyChoice",
			Scene = "c4/TenThousandDelights/Chivarro_dialogue",
			ListGuid = "abf7287a88afc554da91b3cd5d7c5bd1",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 5,
			Changed = "6.2 had Chivarro flee into the shadows; she stays and the conversation goes on."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiChivarroMastermindNetworkSubversion",
			Scene = "c4/TenThousandDelights/Chivarro_dialogue",
			ListGuid = "abf7287a88afc554da91b3cd5d7c5bd1",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 5,
			Changed = "6.2 had Chivarro surrender her signet ring, which never happens."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHepzamirahAntagonistDeconstruction",
			Scene = "c4/FinalDungeon_Colyphyr/Hepzamirah_main",
			ListGuid = "995aaa29e772b594a966a2f133be702c",
			ExpCr = 6
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHepzamirahAngelHolySmite",
			Scene = "c4/FinalDungeon_Colyphyr/Hepzamirah_main",
			ListGuid = "995aaa29e772b594a966a2f133be702c",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7,
			Changed = "6.2 burned the bodyguards to ash before the fight has begun."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHepzamirahMartialGodHornSever",
			Scene = "c4/FinalDungeon_Colyphyr/Hepzamirah_main",
			ListGuid = "995aaa29e772b594a966a2f133be702c",
			Gate = "MartialGodProficiencies",
			ExpCr = 6,
			Changed = "6.2 cut off Hepzamirah's horn in the middle of the conversation; she fights you whole."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHepzamirahLichNahyndrianDrain",
			Scene = "c4/FinalDungeon_Colyphyr/Hepzamirah_main",
			ListGuid = "995aaa29e772b594a966a2f133be702c",
			Gate = "OverlordProficiencies",
			MythicPath = "Lich",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 7,
			Changed = "6.2 drained the mine into your phylactery; the mines are untouched."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHepzamirahMastermindStructuralDisruption",
			Scene = "c4/FinalDungeon_Colyphyr/Hepzamirah_main",
			ListGuid = "995aaa29e772b594a966a2f133be702c",
			Gate = "MastermindProficiencies",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 6,
			Changed = "6.2 buried her reinforcements under granite before the fight."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiHepzamirahHeroSlaveEvacuation",
			Scene = "c4/FinalDungeon_Colyphyr/Hepzamirah_main",
			ListGuid = "995aaa29e772b594a966a2f133be702c",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 6,
			Changed = "6.2 had the miners pour out of the pits mid-conversation."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBaphometDimensionalWard",
			Scene = "c4/FinalDungeon_Colyphyr/Baphomet",
			ListGuid = "9ab9ad8f6e11d67499b48fd595e50972",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBaphometAngelDivineChallenge",
			Scene = "c4/FinalDungeon_Colyphyr/Baphomet",
			ListGuid = "9ab9ad8f6e11d67499b48fd595e50972",
			Gate = "GodEmperorProficiencies",
			MythicPath = "Angel",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBaphometLegendMartialChallenger",
			Scene = "c4/FinalDungeon_Colyphyr/Baphomet",
			ListGuid = "9ab9ad8f6e11d67499b48fd595e50972",
			Gate = "MartialGodProficiencies",
			MythicPath = "Legend",
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBaphometOverlordLabyrinthClaim",
			Scene = "c4/FinalDungeon_Colyphyr/Baphomet",
			ListGuid = "9ab9ad8f6e11d67499b48fd595e50972",
			Gate = "OverlordProficiencies",
			Shift = AlignmentShiftDirection.Evil,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBaphometMastermindAeonGeometrySunder",
			Scene = "c4/FinalDungeon_Colyphyr/Baphomet",
			ListGuid = "9ab9ad8f6e11d67499b48fd595e50972",
			Gate = "MastermindProficiencies",
			MythicPath = "Aeon",
			Shift = AlignmentShiftDirection.Lawful,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiBaphometHeroAzataHeartOfFreedom",
			Scene = "c4/FinalDungeon_Colyphyr/Baphomet",
			ListGuid = "9ab9ad8f6e11d67499b48fd595e50972",
			Gate = "HeroProficiencies",
			MythicPath = "Azata",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 7
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4BadLuckHealthViolation",
			Scene = "c4/TavernBadLuck/InnKeeper_mainDialogue",
			ListGuid = "772496e11e1b6804da503d2883417f97",
			ExpCr = 5,
			ClaimKey = "Port62:Act4BadLuckHealthViolation",
			Coins = 400,
			SixTwoSponsor = "The Dark Prince",
			Gold = 600,
			ModItems = new string[1] { "ItemAmuletOfOtherworldEquilibrium" },
			Changed = "The innkeeper has one eye, not three, and 6.2 had him take your coin while the reward pays you gold. The Amulet of Otherworld Equilibrium is also the reward of Arsinoe's Act 3 tithe answer, and a save is handed only one."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4BadLuckGodEmperorTip",
			Scene = "c4/TavernBadLuck/InnKeeper_mainDialogue",
			ListGuid = "772496e11e1b6804da503d2883417f97",
			Gate = "GodEmperorProficiencies",
			ExpCr = 5
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4BadLuckHalflingTurnaround",
			Scene = "c4/TavernBadLuck/Blind_Halfling",
			ListGuid = "b45c22db4bfe8ad4ab106941ede45b7d",
			ExpCr = 4,
			ClaimKey = "Port62:Act4BadLuckHalflingTurnaround",
			Coins = 300,
			SixTwoSponsor = "The Lucky Drunk",
			Changed = "6.2 had you hand the halfling a pouch of coin while the reward pays you; his eyes are crimson, not cloudy."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4BadLuckHalflingHero",
			Scene = "c4/TavernBadLuck/Blind_Halfling",
			ListGuid = "b45c22db4bfe8ad4ab106941ede45b7d",
			Gate = "HeroProficiencies",
			Shift = AlignmentShiftDirection.Good,
			ExpCr = 4
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4HerraxHotSprings",
			Scene = "c4/TenThousandDelights/Herraxa_dialogue",
			ListGuid = "43f93812d6216c94db356622859397f1",
			ExpCr = 5,
			ClaimKey = "Port62:Act4HerraxHotSprings",
			Coins = 350,
			SixTwoSponsor = "The Song of the Spheres"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4HerraxMastermindIntel",
			Scene = "c4/TenThousandDelights/Herraxa_dialogue",
			ListGuid = "43f93812d6216c94db356622859397f1",
			Gate = "MastermindProficiencies",
			ExpCr = 5
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4ChivarroRetention",
			Scene = "c4/TenThousandDelights/Chivarro_dialogue",
			ListGuid = "5ce68965a500a3142a864c7009b56799",
			ExpCr = 5,
			ClaimKey = "Port62:Act4ChivarroRetention",
			Coins = 350,
			SixTwoSponsor = "The Dark Prince"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4BatteSlaverAutomation",
			Scene = "c4/FleshMarket/BatteSlaver_dialogue",
			ListGuid = "c67f95deb80bb324281796413b5d28ae",
			ExpCr = 5,
			ClaimKey = "Port62:Act4BatteSlaverAutomation",
			Coins = 400,
			SixTwoSponsor = "The Grand Arbiter",
			Changed = "6.2 named the trainer 'Batte'; the dialog has no one of that name."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4BatteSlaverOverlordScorn",
			Scene = "c4/FleshMarket/BatteSlaver_dialogue",
			ListGuid = "c67f95deb80bb324281796413b5d28ae",
			Gate = "OverlordProficiencies",
			ExpCr = 5,
			Changed = "6.2 named the trainer 'Batte'; the dialog has no one of that name."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4MeatSlavesDisgust",
			Scene = "c4/FleshMarket/MeatSlaves_dialogue",
			ListGuid = "642c54b135c66dd448373875b677a25a",
			ExpCr = 5,
			ClaimKey = "Port62:Act4MeatSlavesDisgust",
			Coins = 300,
			SixTwoSponsor = "The Laughing King",
			Changed = "6.2's reply was spoken by a butcher demon who is not in this scene: the list belongs to Dyunk's freed slaves."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4AasimarSlavesPromise",
			Scene = "c4/FleshMarket/AasimarSlaves_dialogue",
			ListGuid = "67823011274209e4882b60f4b95d103f",
			ExpCr = 5,
			ClaimKey = "Port62:Act4AasimarSlavesPromise",
			Coins = 350,
			SixTwoSponsor = "The Dawnflower"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4ArenaHealerSterile",
			Scene = "c4/BattleBliss/ArenaHealer_dialogue",
			ListGuid = "dd04ec68821f0ac4db58a756d323d213",
			ExpCr = 5,
			ClaimKey = "Port62:Act4ArenaHealerSterile",
			Coins = 350,
			SixTwoSponsor = "The Grand Arbiter",
			Changed = "6.2 set the bone of a wounded gladiator who is not in the scene, and gave a combat draught the reward never pays."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4ArenaHealerSlime",
			Scene = "c4/BattleBliss/ArenaHealer_dialogue",
			ListGuid = "dd04ec68821f0ac4db58a756d323d213",
			Gate = "DevourerProficiencies",
			ExpCr = 5,
			Changed = "6.2 sealed a wound on nobody in particular; there is no patient in the scene."
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4BattleblissEsports",
			Scene = "c4/BattleBliss/ZeklexAfterImprisonment_dialogue",
			ListGuid = "da29db4239d1c9945ba3440bd1a01141",
			ExpCr = 5,
			ClaimKey = "Port62:Act4BattleblissEsports",
			Coins = 350,
			SixTwoSponsor = "The Laughing King"
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4ShamiraVillainessMelodrama",
			Scene = "c4/HaremOfArdentDream/Shamira_dialogue",
			ListGuid = "d138954fd7cdb2d4e90bb28cbd76235e",
			ExpCr = 6,
			ClaimKey = "Port62:Act4ShamiraVillainessMelodrama",
			Coins = 500,
			SixTwoSponsor = "The World Sovereign",
			Gold = 1000,
			ModItems = new string[1] { "ItemSovereignsImperialSignet" }
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4ShamiraGodEmperorPoise",
			Scene = "c4/HaremOfArdentDream/Shamira_dialogue",
			ListGuid = "d138954fd7cdb2d4e90bb28cbd76235e",
			Gate = "GodEmperorProficiencies",
			ExpCr = 6
		},
		new Port62LateActs.Graft
		{
			Answer = "IsekaiAct4NexusCyberpunk",
			Scene = "c4/Nexus_Camp/HeraldIntro",
			ListGuid = "91097a9b90537a34fbe379d5aa4d9cbe",
			ExpCr = 5,
			ClaimKey = "Port62:Act4NexusCyberpunk",
			Coins = 350,
			SixTwoSponsor = "The Inheritor"
		}
	};

	internal static Port62LateActs.Graft Find(string answer)
	{
		return Grafts.FirstOrDefault((Port62LateActs.Graft g) => g.Answer == answer);
	}

	public static void Add()
	{
		Port(DialogueBeats.NexusCampArrival, "IsekaiNexusCampTricksterArrival", "(Isekai Protagonist) [Planar Beachhead Meta] \"So this is the Abyss! A bit gloomy, sure, but nothing an Otherworlder can't turn into a forward operating base. Time to explore!\"", "{n}The Hand of the Inheritor looks upon your unfazed demeanor with awe and renewed hope.{/n} \"Your courage in this cursed realm is a beacon to us all, Commander.\"");
		Port(DialogueBeats.NexusCampArrival, "IsekaiNexusCampAngelConsecration", "(God Emperor) [Angel] [Planar Consecration] \"Heaven's light does not retreat before the Abyss! By my imperial mandate, this Nexus is consecrated as holy ground!\"", "{n}Blinding golden light suffuses the volcanic cavern for a long moment, driving the abyssal shadows back from the camp.{/n}");
		Port(DialogueBeats.NexusCampArrival, "IsekaiNexusCampMartialGodAcclimation", "(Martial God) [Vanguard Acclimation] \"The gravity is heavy and the air tastes of brimstone. Perfect. Every swing of my blade will be forged twice as sharp in this crucible.\"", "{n}You execute a few effortless practice draws, your blade slicing the sulfuric air with melodic, lethal precision.{/n}");
		Port(DialogueBeats.NexusCampArrival, "IsekaiNexusCampOverlordTerritoryClaim", "(Overlord) [First Territory Claim] \"The Midnight Isles do not know it yet, but this camp is merely the first stone in my new abyssal dominion.\"", "{n}Dark shadows swirl obediently across the cave entrance, warding the camp with chilling dread.{/n}");
		Port(DialogueBeats.NexusCampArrival, "IsekaiNexusCampMastermindPlanarMapping", "(Mastermind) [Planar Topology Audit] \"Abyssal gravitational drift and floating island resonance calculated. I have mapped three optimal infiltration vectors into Alushinyrra.\"", "{n}The Herald studies your geometric crystalline maps in astonishment.{/n} \"You navigate the labyrinthine Abyss as if you have lived here for centuries, Commander.\"");
		Port(DialogueBeats.NexusCampArrival, "IsekaiNexusCampHeroSteadfastHeart", "(Hero) [Unwavering Hope in Hell] \"No matter how dark or terrifying this realm is, we stand united. We will find the source of the Nahyndrian crystals and save our world!\"", "{n}Your companions nod with fierce, renewed resolve as your heroic warmth dispels their doubts.{/n}");
		Port(DialogueBeats.KingpinAsksAboutSlavery, "IsekaiFleshmarketTricksterLiquidation", "(Isekai Protagonist) [Hostile Commercial Liquidation] \"Selling mortals by the pound? Your markup margins are absurd and your customer service is atrocious. This market is headed for new management: bankruptcy and liberation!\"", "{n}Dyunk stutters in enraged bewilderment, glancing at his chained wares as if he expects them to rise on the spot.{/n} \"Bankruptcy?! Liberation?! Nobody buys that kind of talk in the Fleshmarket, stranger!\"");
		Port(DialogueBeats.KingpinAsksAboutSlavery, "IsekaiFleshmarketAngelHolyLiberation", "(God Emperor) [Angel] [Solar Emancipation] \"Every chain forged in this wretched pit will melt before heaven's righteous fury! Mark my words: every captive here will walk free under my aegis!\"", "{n}Blinding golden light flares around you, and the slavers shrink back from it while their chained wares stare at you with sudden, desperate hope.{/n}");
		Port(DialogueBeats.KingpinAsksAboutSlavery, "IsekaiFleshmarketMartialGodSunderSlavers", "(Martial God) [Apex Sunder: Slaver Cleave] \"You trade in flesh because you are too cowardly to test your steel against a real warrior. Draw your weapons, vermin; let's see how much your own meat fetches.\"", "{n}A sonic detonation of supreme killing intent drops the surrounding slaver brutes to their knees, vomiting black bile in terror.{/n}");
		Port(DialogueBeats.KingpinAsksAboutSlavery, "IsekaiFleshmarketOverlordTitheDominion", "(Overlord) [Sovereign Confiscation] \"The Fleshmarket now belongs to my dark dominion. Hand over the ledger keys and kneel, or your flesh will be the first item placed on the auction block.\"", "{n}Dyunk pales to the color of curdled milk as your dread aura presses down on him, and his bluster dies in his throat.{/n}");
		Port(DialogueBeats.KingpinAsksAboutSlavery, "IsekaiFleshmarketMastermindCartelAudit", "(Mastermind) [Abyssal Macroeconomic Collapse] \"By manipulating Alushinyrra's soul-token arbitrage and flashing counter-trade promissory writs, I could make your cartel's assets vanish into thin air. Think about how insolvent you would be, Dyunk.\"", "{n}Dyunk glances at his enchanted transaction stones and back at you, and for the first time his grin falters.{/n}");
		Port(DialogueBeats.KingpinAsksAboutSlavery, "IsekaiFleshmarketHeroAegisVow", "(Hero) [No One Left in Chains] \"We swore to protect the innocent wherever the crusade marches! Slavers, step back from those cages, or you will not survive the next ten seconds!\"", "{n}The enslaved crusaders and tieflings stir in their cages, and the demonic guards take a wary step back from you.{/n}");
		Port(DialogueBeats.AtselmHub, "IsekaiBattleblissTournamentArcMockery", "(Isekai Protagonist) [Anime Tournament Arc Meta] \"You call this chaotic brawl an arena? Back home, a proper tournament arc has tiered brackets, dramatic backstories, and actual choreography. Time to show this city a real championship fight!\"", "{n}The three Atselms exchange a delighted look, and the youngest claps her hands.{/n} \"Choreography! Backstories! Oh, we would pay to see that, Golarian.\"");
		Port(DialogueBeats.AtselmHub, "IsekaiBattleblissMartialGodApexGladiator", "(Martial God) [Legend] [The God of the Arena] \"Gelderfang? He telegraphs his heavy swing by three whole frames. Send every gladiator in the pits at once; maybe then I won't fall asleep from boredom.\"", "{n}A shockwave of pure martial dominance rolls out from you, and all three Atselms fall silent at once, measuring you with new respect.{/n}");
		Port(DialogueBeats.AtselmHub, "IsekaiBattleblissGodEmperorGladiatorLiberation", "(God Emperor) [Angel] [Sovereign of the Sands] \"Mortal blood will no longer be spilled for demonic amusements! Gladiators of Battlebliss, lay down your chains and rally behind the golden dawn!\"", "{n}Holy luminescence spills over the stones around you, and the old Atselm squints at it sourly.{/n} \"Pretty. The gladiators will not lay down anything for pretty.\"");
		Port(DialogueBeats.AtselmHub, "IsekaiBattleblissOverlordBloodDomain", "(Overlord) [Reign of Blood and Iron] \"This arena will be consecrated in the entrails of its masters. Tell your lord to prepare the floor, Atselm: when I fight, the blood runs until Alushinyrra drowns.\"", "{n}Shadows writhe around you as your demonic bloodlust chills the air, and even the fey triplets stop smiling.{/n}");
		Port(DialogueBeats.AtselmHub, "IsekaiBattleblissMastermindTacticalAnalysis", "(Mastermind) [Predictive Combat Calculus] \"Gelderfang relies on inertial momentum for his secondary cleave. A simple side-step feint at 0.4 seconds exposes his cervical spine to instant lethality.\"", "{n}All three Atselms tilt their heads in unison, committing your observations to memory.{/n}");
		Port(DialogueBeats.AtselmHub, "IsekaiBattleblissHeroAzataFreedomSong", "(Hero) [Azata] [Song of the Unbroken] \"Hear me, fighters! You are not beasts to be slaughtered! Fight for your freedom, fight for tomorrow! We break these walls together!\"", "{n}Your stirring battle-hymn rings through the pits, and the youngest Atselm hums along under her breath.{/n} \"A lovely song. Sing it in the arena, and see who joins in.\"");
		Port(DialogueBeats.HerraxHub, "IsekaiHerraxSocialDeduction", "(Isekai Protagonist) [Social Deduction - The Mastermind's Gambit] \"Herrax, your girls aren't gathering coin; they are gathering scrying frequencies for the Shadow Demon cabal. Play straight with me, or your establishment becomes rubble.\"", "{n}Herrax's smile does not waver, but her eyes flick toward the curtains.{/n} \"My lord is so very suspicious! Play straight? Darling, I play every way there is. But I will remember that you noticed.\"");
		Port(DialogueBeats.HerraxHub, "IsekaiHerraxGodEmperorSolarPurge", "(God Emperor) [Angel] [Sanctification of Shadows] \"Your den of lust and extortion is an insult to mortal dignity. Speak the truth of your masters before my golden aura burns this den to cinders!\"", "{n}Herrax shields her eyes from the celestial radiance with a silk fan, laughing a little too loudly.{/n} \"So bright! You will scare off my guests, my love. Ask what you wish, just lower the light.\"");
		Port(DialogueBeats.HerraxHub, "IsekaiHerraxMartialGodBladeWard", "(Martial God) [Vanish-Cut: Assassin Warning] \"Tell the hidden shadow demons behind the drapery to sheathe their daggers, or my blade will cut their shadows from their spines.\"", "{n}A faint rustle comes from behind the drapery, and then nothing at all. Herrax laughs lightly, as if you had made a charming joke.{/n}");
		Port(DialogueBeats.HerraxHub, "IsekaiHerraxOverlordDarkCoercion", "(Overlord) [Subjugation of the Courtesans] \"You will serve as my eyes and ears in Alushinyrra, Herrax. Every drop of information passing through these halls belongs to me, or your soul will feed my hounds.\"", "{n}Herrax sinks into a deep, theatrical bow, but there is real fear under the performance.{/n} \"Such a commanding guest! My ears are always open to you, Dread Sovereign.\"");
		Port(DialogueBeats.HerraxHub, "IsekaiHerraxHeroAzataDreamLiberation", "(Hero) [Azata] [Cleansing the Stolen Dreams] \"The mortals trapped here deserve their own lives, not to be drained for demonic vices. I will see every captive here set free!\"", "{n}A breeze of sweet wildflowers sweeps through the perfumed corridors, and for a moment the heavy air of the Delights feels clean. Herrax wrinkles her nose.{/n} \"How very... wholesome.\"");
		PortNarrated(DialogueBeats.AlushinyrraPortalArch, "IsekaiMageTowerPortalAlignment", "(Isekai Protagonist) [Dimensional Compass] \"A portal network that only opens for the right key? The routing runes are aligned in a 3-2-4 sequence; I can read every destination from here.\"", "{n}The runes of the arch flash gold one after another as you trace them, and the tangle of tunnels above the city settles into a map in your mind. The arch still wants its key, but you know now where each one would take you.{/n}");
		PortNarrated(DialogueBeats.AlushinyrraPortalArch, "IsekaiMageTowerAeonGravitationalOrder", "(Mastermind) [Aeon] [Axiomatic Space Stabilization] \"These arches fling travelers through chaos with no order at all. Holding this one steady at the Prime Material harmonic.\"", "{n}Crystalline geometric grids snap across the arch, and the whirlwind inside it stops lurching and spins in a slow, predictable circle.{/n}");
		PortNarrated(DialogueBeats.AlushinyrraPortalArch, "IsekaiMageTowerLegendMartialSunder", "(Martial God) [Legend] [Dimensional Cleave Test] \"Who needs a key? One vertical strike should cut straight through the spatial barrier inside this arch!\"", "{n}With a thunderous crack your blade bites into the whirlwind, and the arch shrieks in protest. The rift seals itself a heartbeat later: whoever built this network made it to answer keys, not steel.{/n}");
		PortNarrated(DialogueBeats.AlushinyrraPortalArch, "IsekaiMageTowerLichLeylineDrain", "(Overlord) [Lich] [Abyssal Leyline Siphon] \"The arcane power circulating through these arches will fuel my own soul forge. Siphoning a thread of it into my personal focus!\"", "{n}Necrotic tendrils drink a thin thread of power from the glowing runes, and a cold strength settles in your chest. The arch keeps humming, none the poorer.{/n}");
		PortNarrated(DialogueBeats.AlushinyrraPortalArch, "IsekaiMageTowerHeroPathfinderGuide", "(Hero) [Safe Passage for the Party] \"Stay close to me, everyone! Whatever these arches do to travelers, none of our companions will be lost to the void!\"", "{n}Your protective aura envelops your friends as the arch's nauseating vibration washes over you, and every one of them stays steady on their feet.{/n}");
		Port(DialogueBeats.NocticulaFirstAudience, "IsekaiNocticulaExitStrategyPsychology", "(Isekai Protagonist) [Narrative Psychology - The Demon Lord's Exit] \"Lady Nocticula, let's skip the theatrical seductions. You are exhausted by the petty savagery of the Abyss and looking for an exit strategy into true divinity. Am I warm?\"", "{n}Nocticula's seductive smile freezes for an imperceptible fraction of a second, her dark eyes narrowing with razor-sharp fascination.{/n} \"How dreadfully perceptive of you, mortal... Perhaps you are far more than another disposable champion of the crusade.\"");
		Port(DialogueBeats.NocticulaFirstAudience, "IsekaiNocticulaGodEmperorSovereignParity", "(God Emperor) [Angel] [Sovereign Parity] \"I stand in your palace not as a supplicant, Lady of Shadows, but as the supreme sovereign of the Crusade. We dictate terms to the Abyss, not the reverse!\"", "{n}Golden imperial halos pulse behind you, resisting Nocticula's intoxicating aura with effortless majesty. She leans forward on her throne, genuinely intrigued.{/n}");
		Port(DialogueBeats.NocticulaFirstAudience, "IsekaiNocticulaMartialGodBladeWarning", "(Martial God) [Apex Stance: God-Slaying Intent] \"Keep your shadows where they belong, Nocticula. My blade has cut through concepts and dimensions; do not make the mistake of thinking your demon flesh is immortal.\"", "{n}A razor-thin line of vacuum chills the royal chamber, causing Nocticula's shadow-guards to gasp in sudden, frantic alarm.{/n}");
		Port(DialogueBeats.NocticulaFirstAudience, "IsekaiNocticulaOverlordPlanarPact", "(Overlord) [Equal Exploitation] \"You rule Alushinyrra, Nocticula, but my reach expands across realities. Assist my crusade against Baphomet and Deskari, and I shall allow your realm to survive my ascension.\"", "{n}Nocticula laughs, a dark, melodic sound that echoes with mutual ambition.{/n} \"Such delightful, brazen audacity! I do appreciate a creature with grand appetites.\"");
		Port(DialogueBeats.NocticulaFirstAudience, "IsekaiNocticulaMastermindAeonForensics", "(Mastermind) [Aeon] [Nahyndrian Forensics Delivery] \"Here is the actuarial balance of the Nahyndrian harvests from Colyphyr. My calculations say Baphomet's servants are skimming from every shipment that should reach you.\"", "{n}Nocticula reviews your crystalline data projections with chilling intensity, nodding slowly.{/n} \"Interesting arithmetic, Commander. The horned beast's servants will find their ambitions severely punished.\"");
		Port(DialogueBeats.NocticulaFirstAudience, "IsekaiNocticulaHeroAzataRedemptionWhisper", "(Hero) [Azata] [The Whisper of Elysium] \"Even in the darkest abyss, a soul is never permanently chained to evil. When you are ready to seek the stars and leave this shadow behind, remember this conversation.\"", "{n}For a fleeting heartbeat, the Lady of Shadows gazes into your eyes with profound, melancholic silence before turning away with a faint smile.{/n}");
		Port(DialogueBeats.ChivarroDemandsAFavor, "IsekaiChivarroVanityDeconstruction", "(Isekai Protagonist) [Ego Shatter] \"Your entire hierarchy is built on pathetic vanity and mutual deceit. In the grand multiversal scale, your petty squabbles wouldn't even warrant a B-plot.\"", "{n}Chivarro shrieks in wounded pride as your casual dismissal stings far worse than any holy water.{/n}");
		Port(DialogueBeats.ChivarroDemandsAFavor, "IsekaiChivarroMartialGodInstantSever", "(Martial God) [Zero-Frame Draw] \"Your vanity charms won't save you. Draw your steel, or tell me what I came to learn.\"", "{n}Your hand rests on your hilt, and the whip at Chivarro's belt twitches as if it wants to flee on its own. She laughs, a little too late.{/n}");
		Port(DialogueBeats.ChivarroDemandsAFavor, "IsekaiChivarroGodEmperorImperialExile", "(God Emperor) [Imperial Condemnation] \"Your reign over these corrupted pleasure dens is terminated. Begone from my sight before divine sunlight reduces you to ash!\"", "{n}Solar brilliance flares from your silhouette, and the succubus guards flinch back from it. Chivarro only narrows her eyes against the glare.{/n}");
		Port(DialogueBeats.ChivarroDemandsAFavor, "IsekaiChivarroOverlordEnslavement", "(Overlord) [Subservience Under Penalty of Death] \"You will crawl before my throne and serve as an informant, Chivarro. Cross me once, and I will feed your soul to the outer void.\"", "{n}Chivarro's smile stiffens under the weight of your suffocating malice, but she keeps her feet.{/n} \"Bold words for a guest, mortal.\"");
		Port(DialogueBeats.ChivarroDemandsAFavor, "IsekaiChivarroHeroMercyChoice", "(Hero) [The Choice of Mercy] \"When this is over, leave Alushinyrra and never harm another mortal again, Chivarro. This is the only warning you will receive.\"", "{n}Chivarro blinks at your unexpected mercy, then laughs softly.{/n} \"A warning, from a guest? How quaint.\"");
		Port(DialogueBeats.ChivarroDemandsAFavor, "IsekaiChivarroMastermindNetworkSubversion", "(Mastermind) [Espionage Grid Hijack] \"Your entire communications network is compromised. Help me, and I might forget which of your people talk too much.\"", "{n}Chivarro's eyes narrow as she weighs how much you really know. For once, she has no ready answer.{/n}");
		Port(DialogueBeats.HepzamirahConfrontation, "IsekaiHepzamirahAntagonistDeconstruction", "(Isekai Protagonist) [Antagonist Deconstruction] \"Hepzamirah, you think yourself Baphomet's cherished daughter. You are merely his sacrificial goat to pave his planar incursion.\"", "{n}Hepzamirah's fanatical fury turns to bewildered horror as your words strike the hollow core of her devotion.{/n}");
		Port(DialogueBeats.HepzamirahConfrontation, "IsekaiHepzamirahAngelHolySmite", "(God Emperor) [Angel] [Heaven's Execution] \"Your defiled mines end here, daughter of Baphomet! Consecrating these caverns with heaven's wrath!\"", "{n}Celestial fire gathers around you, and Hepzamirah's cultist bodyguards shrink back from its glare.{/n}");
		Port(DialogueBeats.HepzamirahConfrontation, "IsekaiHepzamirahMartialGodHornSever", "(Martial God) [Horn-Severing Horizon] \"Your giant blood and demonic lineage are nothing before true mastery. One strike, and your prized horn is mine!\"", "{n}Hepzamirah tosses her head and lowers her curved horns toward you, laughing.{/n} \"Come and take it, then!\"");
		Port(DialogueBeats.HepzamirahConfrontation, "IsekaiHepzamirahLichNahyndrianDrain", "(Overlord) [Lich] [The True Nahyndrian Heir] \"These Nahyndrian crystal veins belong to me now, Hepzamirah. Your soul will serve as the keystone of my abyssal throne!\"", "{n}Black soul-frost gathers in your hand, and the Nahyndrian veins in the walls dim for a moment as if in answer. Hepzamirah hisses.{/n}");
		Port(DialogueBeats.HepzamirahConfrontation, "IsekaiHepzamirahMastermindStructuralDisruption", "(Mastermind) [Mine Shaft Demolition Matrix] \"The support struts of your crystal refinery are under extreme tectonic tension. A single targeted shockwave would collapse your escape route.\"", "{n}Hepzamirah follows your gaze to the straining struts, and for the first time she hesitates.{/n}");
		Port(DialogueBeats.HepzamirahConfrontation, "IsekaiHepzamirahHeroSlaveEvacuation", "(Hero) [Miners' Emancipation] \"Crusaders! When this is over, free the miners and secure the skiffs! We hold the line against Hepzamirah until they are safe!\"", "{n}Your companions nod grimly, and a murmur of hope rises from the pits below.{/n}");
		Port(DialogueBeats.BaphometTaunts, "IsekaiBaphometDimensionalWard", "(Isekai Protagonist) [Dimensional Ward] \"A Demon Lord arrives? We stand our ground! Multiversal ward deployed!\"", "{n}A crystalline barrier of multiversal mana negates Baphomet's suffocating fear aura across your entire party!{/n}");
		Port(DialogueBeats.BaphometTaunts, "IsekaiBaphometAngelDivineChallenge", "(God Emperor) [Angel] [The Dawn of Judgment] \"Baphomet! The Lord of the Ivory Labyrinth cowers before the righteous dawn! You will answer for a century of crusader blood!\"", "{n}Blinding solar wings unfurl from your back, forcing the goat-headed demon lord to shield his glowing red eyes in rage and disgust!{/n}");
		Port(DialogueBeats.BaphometTaunts, "IsekaiBaphometLegendMartialChallenger", "(Martial God) [Legend] [The God-Slayer's Roar] \"A Demon Lord in the flesh! Finally, a neck thick enough to test the full power of my blade! Come, beast, show me your labyrinth!\"", "{n}Your weapon roars with a hurricane of pressurized air, cutting deep gouges into the cavern walls before you even take a step forward!{/n}");
		Port(DialogueBeats.BaphometTaunts, "IsekaiBaphometOverlordLabyrinthClaim", "(Overlord) [Sovereign of Nightmares] \"You rule a maze of cattle, Baphomet. When I tear your horns from your skull, the Ivory Labyrinth will be annexed into my eternal empire!\"", "{n}Suffocating dread rolls off your form, meeting Baphomet's abyssal aura in a shrieking clash of dark titans!{/n}");
		Port(DialogueBeats.BaphometTaunts, "IsekaiBaphometMastermindAeonGeometrySunder", "(Mastermind) [Aeon] [Axiomatic Sunder of the Maze] \"The geometric topology of your labyrinth is an obsolete fractal. Every corridor in your realm now leads directly to your inevitable doom!\"", "{n}Ribbons of crystalline cosmic law fracture Baphomet's planar halo, exposing his metaphysical core to reality's judgment!{/n}");
		Port(DialogueBeats.BaphometTaunts, "IsekaiBaphometHeroAzataHeartOfFreedom", "(Hero) [Azata] [The Song That Breaks All Mazes] \"No labyrinth can trap a heart that flies free! For everyone you butchered in the crusade, we stand unbroken and we will never bow!\"", "{n}Celestial song cascades through the volcanic caverns, shattering Baphomet's despair curse across your entire retinue!{/n}");
		Port(DialogueBeats.BadLuckInnkeeperHub, "IsekaiAct4BadLuckHealthViolation", "(Isekai Protagonist) [Municipal Health Code Violations in the Abyss] \"Barkeep, I've conducted health inspections in some sketchy dive bars, but this establishment fails on fourteen consecutive health code violations: raw demon viscera on the cutting board, acidic sludge leaking into the grog cask, and whatever is breathing inside that mug. Wipe the counter down before the health department condemns the whole block!\"", "{n}The demonic innkeeper blinks his one bloodshot eye, scratching his bulbous horn in utter, dumbfounded incomprehension.{/n} \"'Health code violations'?! 'Municipal department'?! Mortal, this is Alushinyrra! We serve pure distilled misery and poison! But... if a guest this loud is going to bring inspectors down on my head, I'd better pay you to keep quiet. Here, take this purse, and this pendant from the last fool who choked on the house brew!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>: <i>\"Demanding sanitary standards from abyssal dive bars! Superbly insolent pragmatism!\"</i>"));
		Port(DialogueBeats.BadLuckInnkeeperHub, "IsekaiAct4BadLuckGodEmperorTip", "(God Emperor) [Imperial Coin of Cleanliness] \"Clean this table, fiend. The sovereign vanguard does not rest where filth festers.\"", "{n}The demon innkeeper scurries like a frightened rat, violently polishing the dark stone with both sleeves.{/n} \"Right away, glorious majesty! Immediately!\"");
		Port(DialogueBeats.BlindHalflingHub, "IsekaiAct4BadLuckHalflingTurnaround", "(Isekai Protagonist) [Underdog Protagonist Reversal Charm] \"Keep your head down and stay alive, friend. In my world, stories of blind wanderers surviving in demon cities always lead to the ultimate third-act underdog comeback. The tide in Alushinyrra is about to turn.\"", "{n}The halfling lifts his burned face, and behind his crimson eyes a sudden spark of long-dead hope flares to life.{/n} \"A comeback... You speak like someone who actually intends to burn this city to the ground. May whatever lucky star brought you here shield your back from their daggers!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"A generous hand tossed to a battered soul in the pit of the Abyss. Cayden salutes you!\"</i>"));
		Port(DialogueBeats.BlindHalflingHub, "IsekaiAct4BadLuckHalflingHero", "(Hero) [Good] [No One is Forgotten] \"Even in the deepest shadow of the Abyss, you are not forgotten. Hold on; light is returning to these broken streets.\"", "{n}The halfling weeps quietly into his cloak, bowing his head in heartfelt prayer.{/n}");
		Port(DialogueBeats.HerraxHub, "IsekaiAct4HerraxHotSprings", "(Isekai Protagonist) [Luxury Hospitality & Bathhouse Inquiries] \"Madame Herrax, you advertise ten thousand delights, but do you have an authentic volcanic hot spring bathhouse, an aromatic cedar sauna, or a karaoke soundstage? True relaxation requires steam and song, not just brooding incubus guards in velvet shadows.\"", "{n}Herrax arches a dark, painted eyebrow, a throaty, purring laugh escaping her lips as she fans herself with jeweled feathers.{/n} \"'Volcanic hot spring bathhouses' and 'cedar saunas'?! What delightfully decadent and refined tastes you possess, mortal traveler! Perhaps the upper planes have far more imaginative pleasures than we credit them for. I may have to commission our incubus artisans to construct a steam bathhouse just for you!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"Demanding luxury bathhouse amenities in the City of Screams! A true vacation connoisseur!\"</i>"));
		Port(DialogueBeats.HerraxHub, "IsekaiAct4HerraxMastermindIntel", "(Mastermind) [Brothel Information Broker Network] \"I see through the silk veils, madame. Every courtesan is an informational listening post routing secrets to the Upper City. Let's discuss pricing for your archives.\"", "{n}Herrax's smile tightens with professional respect.{/n} \"Sharp eyes, traveler. You understand how true power flows through Alushinyrra.\"");
		Port(DialogueBeats.ChivarroHub, "IsekaiAct4ChivarroRetention", "(Isekai Protagonist) [Customer Retention Metrics in Alushinyrra] \"Chivarro, murdering high-paying patrons every second Tuesday is terrible for customer lifetime value. In my world, luxury entertainment venues focus on repeat subscriptions, VIP reward tiers, and non-fatal hospitality. Think of the compounding quarterly revenue!\"", "{n}Chivarro stares at you in complete, speechless bewilderment before letting out a sharp, amused hiss.{/n} \"'Quarterly compounding revenue'?! You speak of the flesh trade as though it were an account ledger in a merchant guild! How wonderfully twisted you are, Otherworlder! But I suppose there is logic in not devouring the cows that give the richest milk...\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>: <i>\"Optimizing customer lifetime value in an abyssal pleasure palace! Exquisite capitalism!\"</i>"));
		Port(DialogueBeats.BatteSlaverHub, "IsekaiAct4BatteSlaverAutomation", "(Isekai Protagonist) [Economic Invalidation of Chattel Slavery] \"You boast of your slave pens, demon, but your business model is hopelessly obsolete. In advanced civilizations, automated steam engines, hydraulic cranes, and magical assembly lines produce ten thousand times more output at zero feeding and security costs. You aren't a fearsome kingpin; you're an economically illiterate savage trading in meat.\"", "{n}The gladiator trainer turns a violent shade of crimson, his many weapons clattering as he stomps the stone.{/n} \"'Economically illiterate'?! 'Automated assembly lines'?! How dare you insult the proud, ancient tradition of abyssal flesh trading, mortal scum! Say that again, and I'll have you chained in my pens and sold to the highest bidder!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Demolishing the abyssal slave trade through basic macroeconomic theory. Absolutely devastating.\"</i>"));
		Port(DialogueBeats.BatteSlaverHub, "IsekaiAct4BatteSlaverOverlordScorn", "(Overlord) [Insects in the Dust] \"You trade in screaming cattle and call yourself powerful. Before a true sovereign, your entire bazaar is merely kindling.\"", "{n}The trainer recoils instinctively as a crushing dark aura presses down on him.{/n} \"What... what kind of monster are you?!\"");
		Port(DialogueBeats.FreedMeatSlaves, "IsekaiAct4MeatSlavesDisgust", "(Isekai Protagonist) [Deadpan Health Inspection Horror] \"Selling humanoid meat in an open-air market under sulfuric ash without refrigeration or health seals? Even for the Abyss, this is disgusting. I'm going to cleanse this whole plaza just so I don't have to smell this stall anymore.\"", "{n}The freed slaves huddle closer and nod eagerly, though none of them seems to understand what a health seal is. The butcher who kept the stall is gone, and only the stink remains.{/n} \"Cleanse it? Yes, yes! Whatever our new {mf|master|mistress} wishes!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Health inspector protocol: purify with righteous steel!\"</i>"));
		Port(DialogueBeats.AasimarSlavesHub, "IsekaiAct4AasimarSlavesPromise", "(Isekai Protagonist) [Otherworlder's Promise of Liberation] \"Hold on to hope, celestial kin. I crossed the void between dimensions to tear this city's masters from their thrones. Your chains will not hold you for much longer.\"", "{n}The chained Aasimar weep in quiet reverence, their golden halos flickering back to life as your otherworldly presence cuts through the demonic miasma.{/n} \"An Otherworlder... We can feel the starlight in your soul! May the heavens guide your strike!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Everbloom']</b></color>: <i>\"The promise of redemption and freedom shines through the slave pens!\"</i>"));
		Port(DialogueBeats.ArenaHealerHub, "IsekaiAct4ArenaHealerSterile", "(Isekai Protagonist) [Field Medicine vs Rusty Bone-Saws] \"Put that rusty serrated bone-saw away, butcher! Where I come from, compound fractures are stabilized with traction splints and sterilized sutures, not hacked off like cheap firewood. Boil those bandages before you give the next gladiator sepsis!\"", "{n}The demon surgeon snorts in bafflement, but lowers the saw as you walk him through traction splints and clean sutures.{/n} \"A splint without an amputation?! Bah! You'd ruin all the blood and gore, mortal! But a gladiator who can fight again tomorrow is worth more to the arena... Zeklex would be pleased.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Modern orthopedic triage saves another combatant from crude butcher surgery.\"</i>"));
		Port(DialogueBeats.ArenaHealerHub, "IsekaiAct4ArenaHealerSlime", "(Slime) [Living Suture Matrix] \"Biomechanical cellular secretion knits torn muscle tissue in twelve seconds. Observe.\"", "{n}The demon healer stares in utter awe as the gash you open across your own arm seals seamlessly.{/n} \"A living biological suture?! Unheard of in the pits!\"");
		Port(DialogueBeats.ZeklexAfterImprisonmentHub, "IsekaiAct4BattleblissEsports", "(Isekai Protagonist) [Esports Tournament Seeding & Bracket Hype] \"Zeklex, your tournament formatting is hopelessly chaotic. In my world, championship arenas use double-elimination brackets, hype intro packages, and broadcast commentary. If you let me seed the grand finals, pay-per-view ticket sales will crash the city gates!\"", "{n}Zeklex rubs his claws together with demonic greed, his eyes glowing with raw spectacle lust.{/n} \"'Double-elimination brackets' and 'hype broadcast commentary'?! Oh, magnificent! Truly the mortal realms produce geniuses of violent commercial entertainment! If you survive the arena, you can produce the next championship season!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Bringing esports tournament production to the blood-pits of the Abyss! Superb!\"</i>"));
		Port(DialogueBeats.ShamiraHub, "IsekaiAct4ShamiraVillainessMelodrama", "(Isekai Protagonist) [Calling Out Anime Villainess Melodrama] \"Lady Shamira, the heavy velvet drapes, the smoky pipe, the calculated sultry languor... In my home world, you are basically the textbook archetype of the dramatic anime villainess who plots in the shadows. It's a fantastic aesthetic, but let's skip the seduction monologue and talk real terms.\"", "{n}Shamira pauses mid-drag on her long ivory pipe, a stunned, crystalline laugh escaping her lips before she leans back against her silk cushions with genuine, predatory fascination.{/n} \"'Dramatic anime villainess'?! How delightfully insolent! No mortal has ever entered my court and reduced the sublime art of abyssal seduction to a theatrical archetype! You possess an extraordinary, insolent spark, Otherworlder. Wear this imperial signet; anyone with the audacity to mock Lady Shamira to her face deserves to walk Alushinyrra as my personal guest!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"Disarming the seductress of the Abyss with unflinching Otherworlder swagger! Outstanding!\"</i>"));
		Port(DialogueBeats.ShamiraHub, "IsekaiAct4ShamiraGodEmperorPoise", "(God Emperor) [Sovereign Radiance] \"Your perfumed court is merely a gilded cage, Shamira. Before true imperial majesty, demonic illusions wither into dust.\"", "{n}Shamira shivers as your golden aura washes over her court, dimming her seductive glamers.{/n} \"What... what kind of power radiates from you?!\"");
		Port(DialogueBeats.NexusCampArrival, "IsekaiAct4NexusCyberpunk", "(Isekai Protagonist) [Alushinyrra as Neon Cyberpunk Dystopia] \"Herald, look at those floating islands, neon portals, and cutthroat corporate-style syndicates. This city looks like a dark cyberpunk metropolis where everyone traded their smartphones for poisoned daggers. Don't worry--I've hacked tougher systems than this.\"", "{n}The Hand of the Inheritor tilts his golden helmet, his celestial eyes glowing with calm resolve.{/n} \"A 'cyberpunk metropolis'? Your homeland must be a strange and perilous realm, Commander. But if you have navigated such treacherous labyrinths before, my sword is honored to follow your lead through these shadows.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"Unshakable confidence in the belly of the demon realm! Lead on, Commander!\"</i>"));
	}

	private static void Port(GraftBeat beat, string name, string answerText, string replyText, string line = null)
	{
		Port62LateActs.Graft graft = Port62LateActs.Find(Grafts, name, beat, "Port62Act4", Port62LateActs.ReplyShape.DialogSpeaker);
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
		Port62LateActs.Graft graft = Port62LateActs.Find(Grafts, name, beat, "Port62Act4", Port62LateActs.ReplyShape.Narrated);
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
