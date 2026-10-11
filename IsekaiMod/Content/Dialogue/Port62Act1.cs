using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62Act1
{
	private const string CureLightWounds = "d52566ae8cbe8dc4dae977ef51c27d91";

	private const string CureModerateWounds = "f991f3051c3b9e64fabc87891077b613";

	private const string BullsStrengthPotion = "2983c97b71ba60443b1c749854ef53a1";

	private const string InvisibilityPotion = "5f9fe6030695f48489b59887536748e8";

	private const string NeutralizePoisonPotion = "b46b711ee354a134496c080b720bbb16";

	private const string ColdIronArrowsQuiver = "464ecede228b0f745a578f69a968226d";

	private const string Chapter01 = "df17ab913c348644b9bd3fe3f9781a84";

	private const string DHDefended = "b4f7860936e1248468d7ec3583b7441e";

	private const string DHLost = "11f47a92b8a99bf47a2a9a487433befd";

	internal static readonly Port62Graft[] Grafts = new Port62Graft[38]
	{
		WoljifSyndicateContract(),
		DefendersHeartCouncilDoctrine(),
		FaxonPreemptiveAmbush(),
		DaeranAristocraticWit(),
		MeetEmberGuardianAegis(),
		DesnaTempleSanctuaryHarmony(),
		BlackWingLibraryInquisitorExpose(),
		BlackWingHeroRighteousness(),
		TopazChemicalProtocol(),
		TopazSlimeAbsorption(),
		SilkenThreadEvacuation(),
		MarketSquareReconcile(),
		MarketSquareHeroUnity(),
		ThaberdineHangoverPanacea(),
		ThaberdineSlimeGastricFilter(),
		JoranColdIronMetallurgy(),
		MinaghoShieldMazeDefiance(),
		NenioBiologyExam(),
		NenioScholarMeta(),
		NenioMeetCollaborator(),
		WoljifCurioGolem(),
		WoljifTraitorExpose(),
		WoljifSisterKerismeiAudit(),
		GwermSecretCryptDiscretion(),
		ArankaSongOfTheStars(),
		TrappedThieflingsLeverage(),
		StorytellerMultiversalChronicles(),
		StauntonVhaneRedemptionSeed(),
		KaylessaCellularPurification(),
		DefendersHeartPreBattleSpeech(),
		DefendersHeartPostBattleToast(),
		GrayGarrisonSuccubusShatter(),
		DefendersHeartMessengerAlarm(),
		ChiefSullAuxiliaryIntegration(),
		KlaemTraumaDecompression(),
		QueenGalfreyAfterActionReview(),
		QueenGalfreyGodEmperorHegemony(),
		BartenderOtherworldDistillation()
	};

	internal static readonly Port62BookGraft[] BookGrafts = new Port62BookGraft[3]
	{
		new Port62BookGraft
		{
			Answer = "IsekaiWardstoneResonantHarmony",
			Scene = "c1/GreyGarrison_ChapterEnd/Wardstone_BookEvent",
			PageGuid = "c297ea885ef96b8408b9bc236a192c63",
			NextPageGuid = "8cd54df3593a1e644b8f2ca018d3e40c",
			MirrorsAnswer = "91a0b72bdcd5809409760cb25ad7d8c3",
			ExpCr = 6,
			ClaimKey = "Port62:WardstoneResonantHarmony",
			Coins = 1000,
			SixTwoSponsor = "The Grand Arbiter",
			Feature = "ResonantMythicSparkFeature"
		},
		new Port62BookGraft
		{
			Answer = "IsekaiAct1MarketSquareAeonRiftBookEvent",
			Scene = "c1/KenabresBurning/BookEvent_Aeon",
			PageGuid = "ffd65d469961ff74c822be943a521775",
			NextPageGuid = "ffd65d469961ff74c822be943a521775",
			ClaimKey = "Port62:Act1MarketSquareAeonRiftBookEvent",
			Coins = 500,
			SixTwoSponsor = "The Grand Arbiter"
		},
		new Port62BookGraft
		{
			Answer = "IsekaiAct1MarketSquareDesnaHymnBookEvent",
			Scene = "c1/KenabresBurning/CravingForElysium",
			PageGuid = "b3f4b3f4de3494e4fb60d82d3f7c187e",
			NextPageGuid = "b3f4b3f4de3494e4fb60d82d3f7c187e",
			ClaimKey = "Port62:Act1MarketSquareDesnaHymnBookEvent",
			Coins = 450,
			SixTwoSponsor = "The Song of the Spheres"
		}
	};

	private static readonly Dictionary<string, BlueprintAnswer> Built = new Dictionary<string, BlueprintAnswer>(StringComparer.Ordinal);

	private static string SpeakerOf(string unitGuid)
	{
		return unitGuid;
	}

	private static Port62Graft WoljifSyndicateContract()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiWoljifSyndicateContract";
		port62Graft.Scene = "c1/DefendersHeart/WoljifHeyYou";
		port62Graft.ListGuid = "a9e697b9ad6fa1c4d9c2ae0895bd56d2";
		port62Graft.Beat = DialogueBeats.WoljifInChains;
		port62Graft.ExpCr = 3;
		port62Graft.ClaimKey = "Port62:WoljifSyndicateContract";
		port62Graft.Coins = 350;
		port62Graft.SixTwoSponsor = "The Laughing King";
		port62Graft.ModItems = new string[1] { "ItemThieflingCharm" };
		return port62Graft;
	}

	private static Port62Graft DefendersHeartCouncilDoctrine()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiDefendersHeartCouncilDoctrine";
		port62Graft.Scene = "NPC_Common/Irabeth";
		port62Graft.ListGuid = "871af36f2ab2b1f40b5de77976c54276";
		port62Graft.Beat = DialogueBeats.IrabethDefendersHeartCouncil;
		port62Graft.ExpCr = 4;
		port62Graft.WhilePlaying = new string[1] { "df17ab913c348644b9bd3fe3f9781a84" };
		port62Graft.UnlessPlaying = new string[2] { "b4f7860936e1248468d7ec3583b7441e", "11f47a92b8a99bf47a2a9a487433befd" };
		port62Graft.ClaimKey = "Port62:DefendersHeartCouncilDoctrine";
		port62Graft.Coins = 400;
		port62Graft.SixTwoSponsor = "The Inheritor";
		port62Graft.Gold = 400;
		return port62Graft;
	}

	private static Port62Graft FaxonPreemptiveAmbush()
	{
		return new Port62Graft
		{
			Answer = "IsekaiFaxonPreemptiveAmbush",
			Scene = "c1/EstrodTower/Faxon",
			ListGuid = "0637eb6a25ff1f34c8cc800e53604705",
			Beat = DialogueBeats.FaxonCornered,
			ExpCr = 4,
			ClaimKey = "Port62:FaxonPreemptiveAmbush",
			Coins = 400,
			SixTwoSponsor = "The Inheritor",
			Gold = 500
		};
	}

	private static Port62Graft DaeranAristocraticWit()
	{
		return new Port62Graft
		{
			Answer = "IsekaiDaeranAristocraticWit",
			Scene = "c1/KenabresBurning/OraclePartyHouse/Daeran_FirstMeet",
			ListGuid = "583314478921a19419304d5f83b72109",
			Beat = DialogueBeats.DaeranAsksAboutTheDemons,
			ExpCr = 4,
			ClaimKey = "Port62:DaeranAristocraticWit",
			Coins = 350,
			SixTwoSponsor = "The Laughing King"
		};
	}

	private static Port62Graft MeetEmberGuardianAegis()
	{
		return new Port62Graft
		{
			Answer = "IsekaiMeetEmberGuardianAegis",
			Scene = "c1/KenabresBurning/MeetEmber",
			ListGuid = "4bba85261e5f2064989ebdc878c0228a",
			Beat = DialogueBeats.KnightsAboutToSacrificeEmber,
			ExpCr = 4,
			ReplySpeaker = SpeakerOf("a0096331dbe6a204090677301ffbe3d3"),
			ContinueCueGuid = "7a0b2ad7452e75242a8f4326e9cec69a",
			ClaimKey = "Port62:MeetEmberGuardianAegis",
			Coins = 400,
			SixTwoSponsor = "The Dawnflower",
			Buff = "EmberSparkBuff"
		};
	}

	private static Port62Graft DesnaTempleSanctuaryHarmony()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiDesnaTempleSanctuaryHarmony";
		port62Graft.Scene = "c1/KenabresBurning/DesnaTempleFinal";
		port62Graft.ListGuid = "16d9e0a48d5746a4fbb1497e4ee4cf81";
		port62Graft.Beat = DialogueBeats.DesnaAdeptsAskAboutTheSong;
		port62Graft.ExpCr = 3;
		port62Graft.ClaimKey = "Port62:DesnaTempleSanctuaryHarmony";
		port62Graft.Coins = 350;
		port62Graft.SixTwoSponsor = "The Song of the Spheres";
		port62Graft.VanillaItems = new string[2] { "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" };
		return port62Graft;
	}

	private static Port62Graft BlackWingLibraryInquisitorExpose()
	{
		return new Port62Graft
		{
			Answer = "IsekaiBlackWingLibraryInquisitorExpose",
			Scene = "c1/BlackWingRuins/Caleb_MainDialogue",
			ListGuid = "497b79c111a66034c920842d33b9ce71",
			Beat = DialogueBeats.ChalebAtThePyre,
			ExpCr = 3,
			ContinueCueGuid = "2bd1fd06cb6699e4c8f737359aa993dd",
			ContinueFallbackGuid = "40d1c291e756ce14eb0fd243770627e5",
			FallBackToList = true
		};
	}

	private static Port62Graft BlackWingHeroRighteousness()
	{
		return new Port62Graft
		{
			Answer = "IsekaiBlackWingHeroRighteousness",
			Scene = "c1/BlackWingRuins/Caleb_MainDialogue",
			ListGuid = "497b79c111a66034c920842d33b9ce71",
			Beat = DialogueBeats.ChalebAtThePyre,
			Gate = "HeroProficiencies",
			ExpCr = 4,
			Narrated = true
		};
	}

	private static Port62Graft TopazChemicalProtocol()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiTopazChemicalProtocol";
		port62Graft.Scene = "c1/KenabresBurning/Theatre_Trapped";
		port62Graft.ListGuid = "833a8b5997b48b645b386b4fcb518134";
		port62Graft.Beat = DialogueBeats.ActorsUnderTheJunk;
		port62Graft.ExpCr = 3;
		port62Graft.Narrated = true;
		port62Graft.ContinueCueGuid = "a6521a499ecbb604eb688a6fce97850b";
		port62Graft.VanillaActionsFrom = new string[1] { "979c0288728143d4cadd65e43eb70f77" };
		port62Graft.ExclusiveWith = new string[1] { "979c0288728143d4cadd65e43eb70f77" };
		port62Graft.ExclusiveWithPorted = new string[1] { "IsekaiTopazSlimeAbsorption" };
		return port62Graft;
	}

	private static Port62Graft TopazSlimeAbsorption()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiTopazSlimeAbsorption";
		port62Graft.Scene = "c1/KenabresBurning/Theatre_Trapped";
		port62Graft.ListGuid = "833a8b5997b48b645b386b4fcb518134";
		port62Graft.Beat = DialogueBeats.ActorsUnderTheJunk;
		port62Graft.Gate = "DevourerProficiencies";
		port62Graft.ExpCr = 4;
		port62Graft.Narrated = true;
		port62Graft.ContinueCueGuid = "a6521a499ecbb604eb688a6fce97850b";
		port62Graft.VanillaActionsFrom = new string[1] { "979c0288728143d4cadd65e43eb70f77" };
		port62Graft.ExclusiveWith = new string[1] { "979c0288728143d4cadd65e43eb70f77" };
		port62Graft.ExclusiveWithPorted = new string[1] { "IsekaiTopazChemicalProtocol" };
		return port62Graft;
	}

	private static Port62Graft SilkenThreadEvacuation()
	{
		return new Port62Graft
		{
			Answer = "IsekaiSilkenThreadEvacuation",
			Scene = "c1/KenabresBurning/Theatre_Trapped",
			ListGuid = "32a8b708ceab50b49ab5696300a893cf",
			Beat = DialogueBeats.TheatreTroupeFreed,
			ExpCr = 3,
			Narrated = true,
			ContinueCueGuid = "e7c02b48a32a25947963e43d6c40e164",
			FallBackToList = true
		};
	}

	private static Port62Graft MarketSquareReconcile()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiMarketSquareReconcile";
		port62Graft.Scene = "c1/KenabresBurning/HurlunRamien_Early";
		port62Graft.ListGuid = "cc0bf28eab0e60b429dac7b53ec8e4d2";
		port62Graft.Beat = DialogueBeats.HulrunTurnsFromRamien;
		port62Graft.ExpCr = 5;
		port62Graft.ReconcileHulrunAndRamien = true;
		port62Graft.ExclusiveWithPorted = new string[1] { "IsekaiMarketSquareHeroUnity" };
		return port62Graft;
	}

	private static Port62Graft MarketSquareHeroUnity()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiMarketSquareHeroUnity";
		port62Graft.Scene = "c1/KenabresBurning/HurlunRamien_Early";
		port62Graft.ListGuid = "cc0bf28eab0e60b429dac7b53ec8e4d2";
		port62Graft.Beat = DialogueBeats.HulrunTurnsFromRamien;
		port62Graft.Gate = "HeroProficiencies";
		port62Graft.ExpCr = 5;
		port62Graft.Narrated = true;
		port62Graft.ReconcileHulrunAndRamien = true;
		port62Graft.ExclusiveWithPorted = new string[1] { "IsekaiMarketSquareReconcile" };
		return port62Graft;
	}

	private static Port62Graft ThaberdineHangoverPanacea()
	{
		return new Port62Graft
		{
			Answer = "IsekaiThaberdineHangoverPanacea",
			Scene = "c3/Mythic_Trickster/FoolKing_Tavern",
			ListGuid = "1a17d8053a3be7f47a7908eb6706f2fe",
			Beat = DialogueBeats.ThaberdineHub,
			ExpCr = 3
		};
	}

	private static Port62Graft ThaberdineSlimeGastricFilter()
	{
		return new Port62Graft
		{
			Answer = "IsekaiThaberdineSlimeGastricFilter",
			Scene = "c3/Mythic_Trickster/FoolKing_Tavern",
			ListGuid = "1a17d8053a3be7f47a7908eb6706f2fe",
			Beat = DialogueBeats.ThaberdineHub,
			Gate = "DevourerProficiencies",
			ExpCr = 4
		};
	}

	private static Port62Graft JoranColdIronMetallurgy()
	{
		return new Port62Graft
		{
			Answer = "IsekaiJoranColdIronMetallurgy",
			Scene = "c1/DefendersHeart/Joran",
			ListGuid = "38adb0a43a5ac3043ad0fe59fdcba6b8",
			Beat = DialogueBeats.JoranHub,
			ExpCr = 3
		};
	}

	private static Port62Graft MinaghoShieldMazeDefiance()
	{
		return new Port62Graft
		{
			Answer = "IsekaiMinaghoShieldMazeDefiance",
			Scene = "c1/GreyGarrison_ChapterStart/Minagho_First",
			ListGuid = "5acd8001d9f7d2443bd57fb1291a03e4",
			Beat = DialogueBeats.IrabethNamesMinagho,
			ExpCr = 4,
			ClaimKey = "Port62:MinaghoShieldMazeDefiance",
			Coins = 400,
			SixTwoSponsor = "The Laughing King"
		};
	}

	private static Port62Graft NenioBiologyExam()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1NenioBiologyExam";
		port62Graft.Scene = "c1/MeetNenioSE/CultistExam";
		port62Graft.ListGuid = "2e4be005071707640b4b5cac95c23d06";
		port62Graft.Beat = DialogueBeats.NenioExaminesTheCultists;
		port62Graft.ExpCr = 4;
		port62Graft.ReplySpeaker = SpeakerOf("b9da69e83235c99429ea4631a7313b63");
		port62Graft.ClaimKey = "Port62:Act1NenioBiologyExam";
		port62Graft.Coins = 450;
		port62Graft.SixTwoSponsor = "The Hermit";
		port62Graft.ModItems = new string[1] { "ItemNenioInscribedJournal" };
		return port62Graft;
	}

	private static Port62Graft NenioScholarMeta()
	{
		return new Port62Graft
		{
			Answer = "IsekaiAct1NenioScholarMeta",
			Scene = "c1/MeetNenioSE/CultistExam",
			ListGuid = "2e4be005071707640b4b5cac95c23d06",
			Beat = DialogueBeats.NenioExaminesTheCultists,
			ReplySpeaker = SpeakerOf("b9da69e83235c99429ea4631a7313b63"),
			Gate = "HeroProficiencies",
			ExpCr = 4
		};
	}

	private static Port62Graft NenioMeetCollaborator()
	{
		return new Port62Graft
		{
			Answer = "IsekaiAct1NenioMeetCollaborator",
			Scene = "c1/MeetNenioSE/MeetNenio",
			ListGuid = "28a0d7c556d73e84c89211458a9adbc5",
			Beat = DialogueBeats.NenioNoticesYou,
			ExpCr = 3,
			ReplySpeaker = SpeakerOf("b9da69e83235c99429ea4631a7313b63")
		};
	}

	private static Port62Graft WoljifCurioGolem()
	{
		return new Port62Graft
		{
			Answer = "IsekaiAct1WoljifCurioGolem",
			Scene = "Companions/CompanionQuests/Woljif/Q1WoljifShopGolem",
			ListGuid = "8c13ea607012b5c40950191b24dfe2d5",
			Beat = DialogueBeats.ShopGolemAsksForTheCodeword,
			ExpCr = 3,
			ReplySpeaker = SpeakerOf("766435873b1361c4287c351de194e5f9"),
			CompanionSpeaker = true,
			ClaimKey = "Port62:Act1WoljifCurioGolem",
			Coins = 400,
			SixTwoSponsor = "The Laughing King",
			Gold = 400
		};
	}

	private static Port62Graft WoljifTraitorExpose()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1WoljifTraitorExpose";
		port62Graft.Scene = "Companions/CompanionQuests/Woljif/Q1WoljifTraitor";
		port62Graft.ListGuid = "a586818461e82c64c9a2f9249795b676";
		port62Graft.Beat = DialogueBeats.KerismeiDemandsTheTraitor;
		port62Graft.ExpCr = 4;
		port62Graft.ClaimKey = "Port62:Act1WoljifTraitorExpose";
		port62Graft.Coins = 450;
		port62Graft.SixTwoSponsor = "The Laughing King";
		port62Graft.Gold = 800;
		port62Graft.ModItems = new string[1] { "ItemThieflingMasterLockpicks" };
		return port62Graft;
	}

	private static Port62Graft WoljifSisterKerismeiAudit()
	{
		return new Port62Graft
		{
			Answer = "IsekaiAct1WoljifSisterKerismeAudit",
			Scene = "Companions/CompanionQuests/Woljif/Q1WoljifAfterTraitor",
			ListGuid = "1f1985a501be18a409aa22c31b268150",
			Beat = DialogueBeats.KerismeiAfterTheTraitor,
			ExpCr = 3
		};
	}

	private static Port62Graft GwermSecretCryptDiscretion()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1GwermSecretCryptDiscretion";
		port62Graft.Scene = "c1/HorgusManor/HorgusSecret";
		port62Graft.ListGuid = "82da27c4efd945bab5ec9c3cb977dcb2";
		port62Graft.Beat = DialogueBeats.HorgusAsksIfYouThreatenHim;
		port62Graft.ExpCr = 4;
		port62Graft.ClaimKey = "Port62:Act1GwermSecretCryptDiscretion";
		port62Graft.Coins = 450;
		port62Graft.SixTwoSponsor = "The Dark Prince";
		port62Graft.Gold = 1000;
		port62Graft.ModItems = new string[1] { "ItemGwermAncestralRapier" };
		return port62Graft;
	}

	private static Port62Graft ArankaSongOfTheStars()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1ArankaSongOfTheStars";
		port62Graft.Scene = "c1/KenabresBurning/OraclePartyHouse/DesnaAdept2";
		port62Graft.ListGuid = "2373bef62ea65d741a8f54ba95f9a8b2";
		port62Graft.Beat = DialogueBeats.ArankaAtTheParty;
		port62Graft.ExpCr = 3;
		port62Graft.ClaimKey = "Port62:Act1ArankaSongOfTheStars";
		port62Graft.Coins = 400;
		port62Graft.SixTwoSponsor = "The Song of the Spheres";
		port62Graft.VanillaItems = new string[2] { "5f9fe6030695f48489b59887536748e8", "5f9fe6030695f48489b59887536748e8" };
		return port62Graft;
	}

	private static Port62Graft TrappedThieflingsLeverage()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1TrappedThieflingsLeverage";
		port62Graft.Scene = "c1/KenabresBurning/Thieflings_Early";
		port62Graft.ListGuid = "58bfbb095cc338a4cb79612cf5acf380";
		port62Graft.Beat = DialogueBeats.TieflingUnderTheRubble;
		port62Graft.ExpCr = 3;
		port62Graft.ContinueCueGuid = "b3474f2297f5a0f449d0ecd2f0cc218c";
		port62Graft.FallBackToList = true;
		port62Graft.VanillaActionsFrom = new string[2] { "cf1a2f5320c41224d815f519af0acaee", "9e7473165b560584aa252ba0308dc69c" };
		port62Graft.ExclusiveWith = new string[2] { "cf1a2f5320c41224d815f519af0acaee", "5aeb1de784bba7b41bc7ee01e66ea967" };
		port62Graft.ClaimKey = "Port62:Act1TrappedThieflingsLeverage";
		port62Graft.Coins = 350;
		port62Graft.SixTwoSponsor = "The Laughing King";
		port62Graft.Gold = 350;
		port62Graft.VanillaItems = new string[2] { "5f9fe6030695f48489b59887536748e8", "f991f3051c3b9e64fabc87891077b613" };
		return port62Graft;
	}

	private static Port62Graft StorytellerMultiversalChronicles()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1StorytellerMultiversalChronicles";
		port62Graft.Scene = "c1/BlackWingRuins/StoryTeller_BlackWing";
		port62Graft.ListGuid = "7789c0a35cd866b41896f74f6f6b3c82";
		port62Graft.Beat = DialogueBeats.StorytellerRescued;
		port62Graft.ExpCr = 4;
		port62Graft.ClaimKey = "Port62:Act1StorytellerMultiversalChronicles";
		port62Graft.Coins = 500;
		port62Graft.SixTwoSponsor = "The Song of the Spheres";
		port62Graft.ModItems = new string[1] { "ItemRingOfTheAncientChronicler" };
		return port62Graft;
	}

	private static Port62Graft StauntonVhaneRedemptionSeed()
	{
		return new Port62Graft
		{
			Answer = "IsekaiAct1StauntonVhaneRedemptionSeed",
			Scene = "c1/DefendersHeart/Staunton_DefHeart",
			ListGuid = "1c39502dd11dbd046a7fc212702ad3fb",
			Beat = DialogueBeats.StauntonOnHisPenance,
			ExpCr = 4,
			ClaimKey = "Port62:Act1StauntonVhaneRedemptionSeed",
			Coins = 400,
			SixTwoSponsor = "The Inheritor"
		};
	}

	private static Port62Graft KaylessaCellularPurification()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1KaylessaCellularPurification";
		port62Graft.Scene = "NPC_Common/Kaylessa/Forn_main";
		port62Graft.ListGuid = "cb8da702f652de9488abb22f0ae88881";
		port62Graft.Beat = DialogueBeats.FornSpeaksOfThePoison;
		port62Graft.ExpCr = 3;
		port62Graft.ClaimKey = "Port62:Act1KaylessaCellularPurification";
		port62Graft.Coins = 400;
		port62Graft.SixTwoSponsor = "The Song of the Spheres";
		port62Graft.VanillaItems = new string[2] { "b46b711ee354a134496c080b720bbb16", "b46b711ee354a134496c080b720bbb16" };
		return port62Graft;
	}

	private static Port62Graft DefendersHeartPreBattleSpeech()
	{
		return new Port62Graft
		{
			Answer = "IsekaiAct1DefendersHeartPreBattleSpeech",
			Scene = "c1/DefendersHeart/BeforeAttackIrabeth",
			ListGuid = "65e2ec6ff2d50884b818df2a35700e2c",
			Beat = DialogueBeats.IrabethBeforeTheAssault,
			ExpCr = 4,
			ClaimKey = "Port62:Act1DefendersHeartPreBattleSpeech",
			Coins = 450,
			SixTwoSponsor = "The World Sovereign",
			Buff = "TavernDefianceMoraleBuff",
			BuffMinutes = 120
		};
	}

	private static Port62Graft DefendersHeartPostBattleToast()
	{
		return new Port62Graft
		{
			Answer = "IsekaiAct1DefendersHeartPostBattleToast",
			Scene = "c1/DefendersHeart/AfterAttackIrabeth",
			ListGuid = "aab7657377a11d847a5ad73bf633e8e8",
			Beat = DialogueBeats.IrabethAfterTheVictory,
			ExpCr = 4,
			ReplySpeaker = SpeakerOf("48cc2d6daf65f014781f6ea76c2565bc"),
			ClaimKey = "Port62:Act1DefendersHeartPostBattleToast",
			Coins = 450,
			SixTwoSponsor = "The Lucky Drunk",
			Gold = 500
		};
	}

	private static Port62Graft GrayGarrisonSuccubusShatter()
	{
		return new Port62Graft
		{
			Answer = "IsekaiAct1GrayGarrisonSuccubusShatter",
			Scene = "c1/GreyGarrison_ChapterEnd/SuccubusKnights",
			ListGuid = "f6572cb37f42aa841808b2bbc293f3b0",
			Beat = DialogueBeats.SuccubusTakesTheOffering,
			ExpCr = 4,
			ContinueCueGuid = "81c1fc4731b030e4d80a4fa4355bb699",
			ContinueFallbackGuid = "c09de2256ada6744698efe28bb6c2de2",
			ClaimKey = "Port62:Act1GrayGarrisonSuccubusShatter",
			Coins = 450,
			SixTwoSponsor = "The Grand Arbiter"
		};
	}

	private static Port62Graft DefendersHeartMessengerAlarm()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1DefendersHeartMessengerAlarm";
		port62Graft.Scene = "c1/AlarmSE/Dialogue";
		port62Graft.ListGuid = "ab362e5b1af4b354ea59d583376cf661";
		port62Graft.Beat = DialogueBeats.MessengerRaisesTheAlarm;
		port62Graft.ExpCr = 3;
		port62Graft.ClaimKey = "Port62:Act1DefendersHeartMessengerAlarm";
		port62Graft.Coins = 350;
		port62Graft.SixTwoSponsor = "The Inheritor";
		port62Graft.VanillaItems = new string[1] { "2983c97b71ba60443b1c749854ef53a1" };
		return port62Graft;
	}

	private static Port62Graft ChiefSullAuxiliaryIntegration()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1ChiefSullAuxiliaryIntegration";
		port62Graft.Scene = "c1/KenabresBurning/MongrelsOutside";
		port62Graft.ListGuid = "07d811c04bc82a34ba54c253e3ca2455";
		port62Graft.Beat = DialogueBeats.MongrelsReachTheSurface;
		port62Graft.ExpCr = 3;
		port62Graft.ReplySpeaker = SpeakerOf("f3d64e3f08a86494bbda4618aa171598");
		port62Graft.ClaimKey = "Port62:Act1ChiefSullAuxiliaryIntegration";
		port62Graft.Coins = 450;
		port62Graft.SixTwoSponsor = "The World Sovereign";
		port62Graft.VanillaItems = new string[4] { "5f9fe6030695f48489b59887536748e8", "5f9fe6030695f48489b59887536748e8", "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" };
		return port62Graft;
	}

	private static Port62Graft KlaemTraumaDecompression()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1KlaemTraumaDecompression";
		port62Graft.Scene = "c1/DefendersHeart/SavedCrusader_Tavern";
		port62Graft.ListGuid = "b837a26d7444dd442a45ee54da358cc9";
		port62Graft.Beat = DialogueBeats.KlaemInTheTavern;
		port62Graft.ExpCr = 3;
		port62Graft.ClaimKey = "Port62:Act1KlaemTraumaDecompression";
		port62Graft.Coins = 350;
		port62Graft.SixTwoSponsor = "The Inheritor";
		port62Graft.VanillaItems = new string[2] { "464ecede228b0f745a578f69a968226d", "2983c97b71ba60443b1c749854ef53a1" };
		return port62Graft;
	}

	private static Port62Graft QueenGalfreyAfterActionReview()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1QueenGalfreyAfterActionReview";
		port62Graft.Scene = "c1/DefendersHeart/ChapterEnd_QueenVisit";
		port62Graft.ListGuid = "8c7d0b0b6a1701d49835740d65d3d922";
		port62Graft.Beat = DialogueBeats.QueenAtTheDefendersHeart;
		port62Graft.ExpCr = 5;
		port62Graft.ReplySpeaker = SpeakerOf("ac2060f96475ca54e8238cb1d8fc20a3");
		port62Graft.ClaimKey = "Port62:Act1QueenGalfreyAfterActionReview";
		port62Graft.Coins = 600;
		port62Graft.SixTwoSponsor = "The Grand Arbiter";
		port62Graft.Gold = 1000;
		port62Graft.ExclusiveWithPorted = new string[1] { "IsekaiQueenGalfreyGodEmperorHegemony" };
		return port62Graft;
	}

	private static Port62Graft QueenGalfreyGodEmperorHegemony()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1QueenGalfreyGodEmperorHegemony";
		port62Graft.Scene = "c1/DefendersHeart/ChapterEnd_QueenVisit";
		port62Graft.ListGuid = "8c7d0b0b6a1701d49835740d65d3d922";
		port62Graft.Beat = DialogueBeats.QueenAtTheDefendersHeart;
		port62Graft.ReplySpeaker = SpeakerOf("ac2060f96475ca54e8238cb1d8fc20a3");
		port62Graft.Gate = "GodEmperorProficiencies";
		port62Graft.ExpCr = 5;
		port62Graft.ClaimKey = "Port62:Act1QueenGalfreyGodEmperorHegemony";
		port62Graft.Coins = 700;
		port62Graft.SixTwoSponsor = "The Grand Arbiter";
		port62Graft.Gold = 1500;
		port62Graft.ExclusiveWithPorted = new string[1] { "IsekaiQueenGalfreyAfterActionReview" };
		return port62Graft;
	}

	private static Port62Graft BartenderOtherworldDistillation()
	{
		Port62Graft port62Graft = new Port62Graft();
		port62Graft.Answer = "IsekaiAct1BartenderOtherworldDistillation";
		port62Graft.Scene = "c1/DefendersHeart/Bartender_DefHeart";
		port62Graft.ListGuid = "6c6ca2d78b121504ca4bedf0a1c6f07f";
		port62Graft.Beat = DialogueBeats.GemylHub;
		port62Graft.ExpCr = 2;
		port62Graft.ClaimKey = "Port62:Act1BartenderOtherworldDistillation";
		port62Graft.Coins = 350;
		port62Graft.SixTwoSponsor = "The Lucky Drunk";
		port62Graft.VanillaItems = new string[5] { "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91" };
		return port62Graft;
	}

	internal static Port62Graft Find(string answer)
	{
		return Grafts.FirstOrDefault((Port62Graft g) => g.Answer == answer);
	}

	internal static Port62BookGraft FindBook(string answer)
	{
		return BookGrafts.FirstOrDefault((Port62BookGraft g) => g.Answer == answer);
	}

	public static void Add()
	{
		Port("IsekaiWoljifSyndicateContract", "(Isekai Protagonist) [Otherworlder's Syndicate Contract] \"I know exceptional talent when I see it, Woljif. A gifted scoundrel with fiendish arcane cunning is far too valuable to waste on a damp dungeon floor. Sign a personal retainer contract with me right now: lend your daggers and lockpicks to my vanguard, and I will personally guarantee your freedom from Irabeth's gallows and the Thieflings' blades.\"", "{n}Woljif's eyes light up with unbridled joy, his tail swishing wildly as he eagerly thrusts his clawed hand through the iron bars to seal the pact.{/n} \"A real contract?! With personal protection from the top brass?! Deal, boss! A thousand times deal! You've got an eye for diamond-grade talent! Irabeth's jaw is gonna hit the tavern floor when she sees you walking me out!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Recruiting the trickster right from under the inquisitors' noses! Glorious!\"</i>"));
		Port("IsekaiDefendersHeartCouncilDoctrine", "(Isekai Protagonist) [Otherworlder's Urban Defense Doctrine] \"Irabeth, despair will cost us this city faster than any demon blade. Where I come from, we defend fortified positions through interlocking barricades, reserve strike teams, and controlled choke points. Put the barricades where I show you and the reserves where the demons will strike; we will hold Defender's Heart together.\"", "{n}Irabeth's tired shoulders straighten, a spark of genuine hope kindling in her heavy eyes as she studies your defensive layout and presses a purse into your hand.{/n} \"Interlocking barricades and reserve runners... by the gods, you speak with the authority of a seasoned fortress commander! Four hundred from what is left of the crusade's war chest; a doctrine like this is worth paying for. Defender's Heart will not break! Thank you, friend.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"A commander who turns panic into ironclad discipline. Defender's Heart shall stand!\"</i>"));
		Port("IsekaiFaxonPreemptiveAmbush", "(Isekai Protagonist) [Pre-emptive Tactical Deconstruction] \"Save your theatrical chanting, Faxon. Your clandestine timetable for the assault on Defender's Heart was leaked hours ago. We've already inverted your defensive perimeter and sealed the cellar escapes. Relinquish your orders and surrender the tower, or face immediate annihilation.\"", "{n}Faxon's fanatical grin evaporates into sheer, blood-chilling terror. He frantically glances toward the shadowed rafters as his cultist acolytes begin to murmur in panicked paranoia.{/n} \"Our timetable?! The sealed cellars?! How could you know our exact deployment?! Traitors! There are heretics in our ranks!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"The fortress of righteousness strikes before the serpent can strike!\"</i>"));
		Port("IsekaiDaeranAristocraticWit", "(Isekai Protagonist) [Aristocratic Meta-Wit] \"Hosting an extravagant masquerade banquet in the midst of a demonic siege while cutthroats linger behind the velvet curtains? Count Arendae, your flair for theatrical decadence is eclipsed only by your spectacular indifference to mortal peril.\"", "{n}Daeran swirls the vintage wine in his golden goblet, a dazzling, amused smile breaking across his handsome features as he looks you up and down.{/n} \"Oh, delightful! An uninvited guest who speaks fluent insolence rather than suffocating crusader righteousness! Tell me, my sudden savior, did you kick down my door to lecture me on civic duty, or simply to admire the exquisite irony of my final vintage?\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Two masters of insolence meeting in the ruins. Pure entertainment!\"</i>"));
		Port("IsekaiMeetEmberGuardianAegis", "(Isekai Protagonist) [Otherworlder's Guardian Intervention] \"Stand down and lower your halberds, crusaders, before you stain what remains of your honor! Murdering a defenseless, barefoot child in the name of the Inheritor is the swiftest road to becoming the very monsters you fight!\"", "{n}The crusaders blanch beneath the sheer, suffocating authority radiating from your voice. Shamed and shaken, they slowly lower their weapons and retreat into the rubble. Ember looks up with enormous, luminous eyes, clutching her soot-stained ragdoll to her chest with a wondrous smile.{/n} \"Your heart... it glows with such a warm, golden fire, stranger. Like sunshine after a long winter night. Can I walk with you? The street was getting so cold...\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Everbloom']</b></color>: <i>\"Blessed are the guardians of the innocent. Her spark is safe in your warmth.\"</i>"));
		Port("IsekaiDesnaTempleSanctuaryHarmony", "(Isekai Protagonist) [Resonance of the Starlit Void] \"The music of the Song of the Spheres carries across all worlds and dimensions. Be at peace, faithful of Desna; your sanctuary shall not fall to paranoia or demon claws while I draw breath.\"", "{n}A celestial bell rings through the ruined shrine, a chorus of spectral starlight swirling around your shoulders as the nervous acolytes bow in profound gratitude.{/n} \"The stars have brought us a protector who hears the heavenly song! Take our blessings and these draughts, starlit traveler!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"The celestial chimes resonate with your otherworldly presence!\"</i>"));
		Port("IsekaiBlackWingLibraryInquisitorExpose", "(Isekai Protagonist) [Inquisitorial Jurisdiction Expose] \"You can drop the pious facade, Chaleb. Prelate Hulrun's official inquisitorial warrants bear the crimson wax seal of the Third Crusade, which your forged parchment noticeably lacks. Step away from the scholars and put down that torch before I remove your head from your shoulders.\"", "{n}Chaleb's self-righteous sneer freezes in terror, the torch trembling in his grasp as he realizes his ruse has been completely dismantled.{/n} \"The wax seal...?! How could a wretched vagabond know the internal seals of the Inquisition?! Kill them! Kill them before they ruin everything!\"");
		Port("IsekaiBlackWingHeroRighteousness", "(Hero) [Voice of Righteousness] \"By the light of Heaven, stand down, false inquisitor! No true servant of justice burns books and slaughters defenseless scholars!\"", "{n}The terrified cultists falter under your radiant conviction, several dropping their torches in panic.{/n}");
		Port("IsekaiTopazChemicalProtocol", "(Isekai Protagonist) [Structural Threat Deconstruction] \"Hold your step. That pile of junk is leaning on the cellar door, and pull the wrong beam and the whole lot comes down on whoever is underneath. Step aside while I take it apart from the top, piece by piece.\"", "{n}With a steady hand you lift the junk away piece by piece, until the cellar door is clear and nothing has crashed down on the actors below.{/n}");
		Port("IsekaiTopazSlimeAbsorption", "(Slime) [Debris Absorption] \"A heap of junk on a cellar door? My slime body can flow through it and dissolve the splintered beams holding it together.\"", "{n}You seep through the pile in seconds, dissolving the rotten beams, and the junk slides harmlessly off the cellar door.{/n}");
		Port("IsekaiSilkenThreadEvacuation", "(Isekai Protagonist) [Calculated Alley Evacuation] \"Gather your tools, stay low, and follow my lead. The demonic prowlers in the adjacent plaza operate on a forty-second patrol loop; we have a clear window through the eastern courtyard right now. Move!\"", "{n}The terrified actors hustle quietly along your guided route, slipping through the ruined alleys without alerting a single demon scout.{/n}");
		Port("IsekaiMarketSquareReconcile", "(Isekai Protagonist) [Sovereign Reconciliation] \"Lower your steel, both of you! Hulrun, your obsession with hidden demons is blinding you to real allies. Ramien, your secretiveness is feeding his paranoia. Kenabres is on fire, and you two are squabbling like children in the ashes! You will lay down your grudges and unite your faiths under my command right now, or I will remove you both from this city's defense!\"", "{n}Both the iron prelate and the Desnan priest freeze in stunned disbelief under the crushing authority of your voice. Hulrun's jaw clenches tightly before he slowly grounds his halberd, while Ramien lets out a long, trembling breath of relief.{/n} \"Under your command... such audacity, yet your conviction cuts deeper than any demon's blade. Very well, stranger. For the survival of Kenabres, the Inquisition and the clergy shall stand together.\"");
		Port("IsekaiMarketSquareHeroUnity", "(Hero) [Unifier of the Despairing] \"Crusaders of Iomedae and worshippers of Desna! Our enemy is the Abyss, not each other! Lay down your pride and unite for Kenabres!\"", "{n}A warm, celestial luminescence envelops the square. Hulrun grunts in begrudging respect, while Ramien smiles through tears.{/n}");
		Port("IsekaiThaberdineHangoverPanacea", "(Isekai Protagonist) [Otherworlder's Hangover Panacea] \"Drink this effervescent blend of activated charcoal, willow bark, and electrolyte tonic, your majesty. It will flush the toxic spirits from your bloodstream and banish your headache in ten heartbeats.\"", "{n}Thaberdine gags, chugs the fizzing draught, and gasps as color rushes back into his sallow face. His rheumy eyes widen in absolute clarity.{/n} \"By Torag's heavy anvil! The pounding inside my skull... it vanished! My head feels lighter than a newly polished crown! What sorcery is this glorious nectar?!\"");
		Port("IsekaiThaberdineSlimeGastricFilter", "(Slime) [Gastric Fermentation Filter] \"Allow my slime catalyst to consume the toxic fusel oils in your blood. In five seconds, you will be sober enough to recite ancient history.\"", "{n}A cool blue droplet touches Thaberdine's tongue, absorbing all alcohol toxicity in seconds.{/n} \"Incredible! My head is as sharp as a newly honed waraxe!\"");
		Port("IsekaiJoranColdIronMetallurgy", "(Isekai Protagonist) [Advanced Metallurgy - Cold Iron Cryo-Quenching] \"Master Vhane, standard oil-quenching leaves microscopic fractures in cold iron. If you rapidly quench the heated blade in glacial mineral brine, the crystal lattice stabilizes, doubling its cutting edge against fiendish hide.\"", "{n}Joran Vhane pauses his hammer mid-swing, examining your sketches with burgeoning smithing fervor.{/n} \"Glacial brine quenching... to prevent lattice stress?! By the Great Smith's beard, you've solved the brittle-grain dilemma! Any weapon you bring to my forge will receive the finest craftsmanship I have to give!\"");
		Port("IsekaiMinaghoShieldMazeDefiance", "(Isekai Protagonist) [Otherworlder's Defiance] \"You boast of slaughtering Kenabres, Minagho, but your reign ends right here. I am an Otherworlder who crossed the cosmic void; your abyssal masters hold no terror for me. Ready your scythe--you're facing someone whose script you cannot predict.\"", "{n}Minagho's cruel smile falters as an inexplicable, icy dread washes over her demonic flesh. She bares her fangs in a frantic snarl.{/n} \"Another world?! What meaningless drivel! I will carve your heart out and offer your soul to Deskari!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Defying the general of the Abyss with unflinching insolence! Magnifique!\"</i>"));
		Port("IsekaiAct1NenioBiologyExam", "(Isekai Protagonist) [Multiversal Scientific Taxonomy] \"Allow me to contribute to your encyclopedic inquiry, scholar. In my home dimension, cellular biology, genetic recombination, and extradimensional physics operate on rigorous empirical frameworks far beyond primitive cult dogma.\"", "{n}Nenio's eyes dilate in breathless academic ecstasy. Her quill scratches across parchment with blurring speed, sending flecks of ink flying.{/n} \"Empirical frameworks?! Extradimensional cellular genetics?! Fascinating! Utterly magnificent! A completely unique specimen with unprecedented biological taxonomy! You must accompany me immediately for further cranial measurements! Take this inscribed journal of my foundational hypotheses!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"A meeting of unfathomable intellects. Knowledge transcends dimensional boundaries!\"</i>"));
		Port("IsekaiAct1NenioScholarMeta", "(Hero) [Unwavering Axiom of Truth] \"Demonic falsehoods collapse beneath objective inquiry. Observe how the demonic humors decompose when subjected to celestial resonance.\"", "{n}Nenio nods frantically, scribbling notes without looking.{/n} \"Celestial resonance as an antiseptic agent! Extraordinary empirical verification!\"");
		Port("IsekaiAct1NenioMeetCollaborator", "(Isekai Protagonist) [Academic Collaboration] \"We can debate extradimensional thermodynamics once we reach safety, Nenio. Travel with my company; you will have front-row observations of phenomena no Golarion text has ever documented.\"", "{n}Nenio snaps her notebook shut with decisive authority.{/n} \"A sound proposal. An itinerant field expedition with an anomalous specimen provides superior data density. Lead the way!\"");
		PortWithCompanion("IsekaiAct1WoljifCurioGolem", "(Isekai Protagonist) [Golem Logic Override] \"Standard runic automatons operate on binary heuristic directives. Observe: by inverting the polarity of the command matrix etched into this broken clay skull, its guardian subroutine goes quiet.\"", "{n}With a calm gesture and a hum of celestial energy, the shattered clay head crackles, its glowing eyes dimming for a heartbeat before they flare again and it rasps for its codeword.{/n} \"Runic polarity inversion?! Boss, you nearly talked a broken pot out of its password! Still wants the codeword, though. Look at the shiny salvage lying around it!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Outsmarting ancient magic with modern logic! Simply delightful!\"</i>"));
		Port("IsekaiAct1WoljifTraitorExpose", "(Isekai Protagonist) [Forensic Syndicate Audit] \"Sister Kerismei, save the veiled threats. I have already cross-referenced the ledger discrepancies with street informant reports. The traitor isn't Woljif; it's the compromised middlemen selling tips to the cultists. Woljif operates under my personal retainer now--clear his name and compensate him properly, or I'll audit every illicit operation under this tavern.\"", "{n}Sister Kerismei's sly smile freezes, her eyes widening as she sizes up your unwavering stance and razor-sharp deduction. She exhales softly, tossing a pouch of coins and a velvet roll of tools onto the table.{/n} \"An auditor with teeth... I like that. Very well: the coin and these masterwork picks are yours for the lead. But the Family still wants a name, boy, not a theory. Bring me the traitor and your slate is clean.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"A forensic audit that leaves the underworld speechless! Outstanding work!\"</i>"));
		Port("IsekaiAct1WoljifSisterKerismeAudit", "(Isekai Protagonist) [Syndicate Retainer Settlement] \"Our business here is concluded, Sister Kerismei. Woljif's talents are contracted to the crusade now. See that your Thieflings stay out of our vanguard's way, and we won't need to return.\"", "{n}Kerismei smirks respectfully and tips two fingers to her temple.{/n} \"Understood, Commander. The Thieflings know who holds the winning hand.\"");
		Port("IsekaiAct1GwermSecretCryptDiscretion", "(Isekai Protagonist) [Corporate Confidentiality & Bloodline Discretion] \"Master Gwerm, your private crypt and family secrets remain strictly confidential; you have my word. Whatever shadows lie in this cellar are between you and your conscience; my concern is securing your estate and honoring our pact.\"", "{n}Horgus Gwerm exhales a shaky sigh of immense relief, his trembling hands lowering his lantern. For the first time, his gaze holds genuine, profound respect.{/n} \"Confidentiality... without sanctimonious preaching or extortionate threats. You are truly unlike any crusader I have ever known. Take this ancestral rapier from my family vault, and the full bounty I promised. You have earned every coin.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>: <i>\"A gentleman's discretion rewarded in cold steel and gold. Exquisite contract adherence.\"</i>"));
		Port("IsekaiAct1ArankaSongOfTheStars", "(Isekai Protagonist) [Harmonic Resonance of the Spheres] \"Sing on, Aranka. The melodies of the cosmos echo through every realm and world. Hulrun's inquisitors will not touch you; my vanguard will guide your song safely past the checkpoints.\"", "{n}Aranka's fingers dance across her harp strings, a celestial glow illuminating her tear-streaked face.{/n} \"You hear it too... the song that dreams of freedom beneath the night sky! Take these traveler's draughts, noble protector. May the stars forever watch over your path!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"The song of Elysium swells in harmony with your otherworldly soul!\"</i>"));
		Port("IsekaiAct1TrappedThieflingsLeverage", "(Isekai Protagonist) [Structural Physics: Safe Extraction] \"Hold still under there! Don't push against the keystone beam or the entire ceiling will collapse. Stand clear while we lever the fulcrum from this angle and clear the debris safely.\"", "{n}With precise leverage and a surge of otherworldly strength, the heavy masonry is shifted aside, freeing the trapped scoundrels without a single scratch.{/n} \"Whoa! You moved that boulder like it was made of balsa wood! Thanks, friend! Here--take our stash of shiny trinkets and potions before the guards show up!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Clever leverage saves the day and fills your pockets!\"</i>"));
		Port("IsekaiAct1StorytellerMultiversalChronicles", "(Isekai Protagonist) [Multiversal Chronicles] \"Greetings, Storyteller. The memories you read from relics are not confined to Golarion alone. In my soul linger echoes of other skies, distant stars, and forgotten civilizations that traversed the cosmic void. When Kenabres is secure, I would share these tales with you.\"", "{n}The blind elf tilts his head, his milky eyes widening as he reaches out a trembling hand toward your aura. A look of ancient, reverent awe settles upon his weathered face.{/n} \"Echoes... of other skies? Yes... I can feel the boundless resonance radiating from you, child of distant cosmoses. The threads of reality bend around your journey. Wear this ring, chronicler of the stars, and may your tales endure when all else turns to dust.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"A bond forged between chroniclers of infinite worlds! +500 Cosmic Coins!\"</i>"));
		Port("IsekaiAct1StauntonVhaneRedemptionSeed", "(Isekai Protagonist) [Insight into Guilt and Damnation] \"Drinking yourself to numbness won't wash seventy years of shame from your hands, Staunton. In the world I hail from, true redemption isn't granted by a forgiving crowd--it is carved out of the future with your own blood and steel. When the crusade marches again, don't throw your life away in despair; look to the horizon and choose who you want to be.\"", "{n}Staunton Vhane freezes, his tankard gripped so tightly the tin bends under his calloused fingers. He lifts bloodshot eyes to meet your gaze, a tremor running through his scarred dwarf frame.{/n} \"Carve it out of the future...? You talk like a madman who hasn't felt the weight of seventy winters of curses. ...Yet no one has looked at me without disgust in half a century. I won't forget what you said, Otherworlder.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"A seed of true redemption planted in hardened soil. The Inheritor watches with hope.\"</i>"));
		Port("IsekaiAct1KaylessaCellularPurification", "(Isekai Protagonist) [Cellular Toxin Neutralization] \"Rest easy, Forn. I recognize the necrotizing venom coursing through your veins. Drink this concentrated antitoxin infusion while I neutralize the dark humors in your bloodstream.\"", "{n}Forn studies the vial for a long moment, then drinks it with a grimace.{/n} \"The burning in my veins is easing. You have handled drow venom before, or something very like it. I told you I would fight this off alone, and I meant it, but I will not refuse help that works. Who are you, stranger? Take these antidotes; you may need them against those who hunt me.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"Mercy shown to the hunted wanderer beneath the shadows.\"</i>"));
		Port("IsekaiAct1DefendersHeartPreBattleSpeech", "(Isekai Protagonist) [Otherworlder Vanguard Rally] \"Crusaders of Kenabres! The demons believe we are trapped rats in this tavern, waiting to be butchered in the dark! But they do not know what kind of hunter is waiting inside! Steel your hearts, hold your barricades, and fight for every inch of stone! Tonight, we show them that Kenabres will NEVER kneel!\"", "{n}A deafening roar of defiance erupts from every throat in the tavern, weapons clashing against shields in a thunderous rhythm of indomitable courage. Irabeth raises her sword high, tears of fierce pride shining in her eyes.{/n} \"To the barricades! FOR KENABRES! FOR THE COMMANDER!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"An inspiring rally that ignites the flame of victory in mortal hearts!\"</i>"));
		Port("IsekaiAct1DefendersHeartPostBattleToast", "(Isekai Protagonist) [The Victor's Toast] \"Raise your cups, defenders of Kenabres! Tonight we stood against the fury of the Abyss, and it was the demons who broke against our wall! To fallen comrades, to enduring hope, and to the dawn that will see our city reclaimed!\"", "{n}Mugs clink and cheer after cheer shakes the tavern rafters as soldiers drink deep in joyous relief. Irabeth claps you on the shoulder with a tired, glowing smile.{/n} \"You turned what should have been our slaughter into our finest hour. Take this victory purse collected from the grateful merchants--you've earned every single copper.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"A victory toast that will be sung of for centuries! Cheers, hero!\"</i>"));
		Port("IsekaiAct1GrayGarrisonSuccubusShatter", "(Isekai Protagonist) [Dimensional Resonance: Charm Shatter] \"Your cheap pheromones and seductive enchantments might ensnare mortal senses, demon, but against an Otherworlder anchored across realities, they are as brittle as glass. Crusaders of Mendev, AWAKEN!\"", "{n}A piercing chord of celestial starlight reverberates through the hall. The charmed crusaders gasp, violently shaking their heads as the succubus's psychic webs shatter into iridescent motes.{/n} \"My enchantment?! How dare you break my playthings, filthy worm! Cut them to pieces!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Demonic psychic enthrallment cleanly severed by dimensional disruption!\"</i>"));
		Port("IsekaiAct1DefendersHeartMessengerAlarm", "(Isekai Protagonist) [Emergency Defense Deployment] \"Calm yourself, runner! Panic will break our lines before the first demon reaches the barricades. Tell Irabeth to seal the cellar hatches and position the archers on the upper gallery. Where I come from, ambushes are turned into kill zones by disciplined choke points. Kenabres stands ready!\"", "{n}The frantic messenger catches his breath, his panicked eyes steadying as he repeats your orders back to you.{/n} \"Understood, Commander! Archery squads to the gallery! Reinforce the barricades! Defender's Heart shall not fall!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"Calm leadership under sudden alarm turns chaos into resolute defiance.\"</i>"));
		Port("IsekaiAct1ChiefSullAuxiliaryIntegration", "(Isekai Protagonist) [Tactical Auxiliary Integration: The Subterranean Pioneers] \"Stand tall, hunters of Neathholm. In my world, elite subterranean shock troops with night-vision and poison resistance are invaluable command assets, not monsters. Your hunters form our stealth vanguard beneath Kenabres's shattered streets; any crusader who questions your bloodline answers directly to me.\"", "{n}The young mongrel stares in disbelief, his eyes glistening as he straightens his curved back with pride. Behind him, the other hunters let out a fierce, guttural cheer.{/n} \"Shock troops... vanguard?! You hear that, children of the stone?! The surface commander sees us as warriors, not vermin! We will scour the alleys and flay every demon that dares cross your path!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"A true sovereign sees strength and loyalty where petty zealots see only fear.\"</i>"));
		Port("IsekaiAct1KlaemTraumaDecompression", "(Isekai Protagonist) [Combat Trauma Decompression & Forward Re-Armament] \"Drink this warm restorative tonic, Klaem. You read of saints while the city burns around us, and that calm steadies everyone in this tavern. Rest your hands, re-align your bowstring, and be ready when the demon horns sound on our barricades.\"", "{n}Klaem marks his page, sets the dog-eared book aside and drinks the tonic, as unhurried as ever.{/n} \"Saint Lymirin would approve of a tonic shared in a siege, Commander. Reading steadies the soul, but tonight the soul will need a steady hand too. Give me a quiver of cold iron and point me at the gate!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"Honor for the fallen, strength for the living. The scout returns to the fight!\"</i>"));
		Port("IsekaiAct1QueenGalfreyAfterActionReview", "(Isekai Protagonist) [Comprehensive After-Action Review & Fifth Crusade Strategic Doctrine] \"Welcome to reclaimed Kenabres, Your Majesty. Review these detailed battle logs: the Wardstone has been dealt with, the demonic supply lines are severed, and our vanguard has maintained full squad cohesion. Kenabres was merely the defensive phase; give me command of the Fifth Crusade, and we march on Drezen.\"", "{n}Queen Galfrey pores over your concise, impeccably organized battle logs with rising astonishment. The high prelates and generals exchange wide-eyed glances.{/n} \"The Wardstone settled... strategic supply interdiction... and not a single lost regiment in the garrison assault. In a hundred years of crusade warfare, I have never seen a military mind grasp planar battlefield logistics with such clarity. Commander, Mendev is in your debt. The sword of the Fifth Crusade is yours to wield.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Exemplary strategic doctrine commands immediate imperial recognition! +600 Cosmic Coins!\"</i>"));
		Port("IsekaiAct1QueenGalfreyGodEmperorHegemony", "(God Emperor) [Imperial Hegemony Briefing] \"Queen Galfrey, behold a city that did not merely survive, but triumphed under an otherworldly sovereign. The Fifth Crusade requires neither pious hesitation nor bureaucratic doubt. Grant me the mandate, and the Worldwound will bend.\"", "{n}Queen Galfrey's royal breath hitches as a majestic, undeniable aura of cosmic sovereignty radiates from your presence, leaving the gathered court in stunned silence.{/n} \"Such overwhelming authority... It feels as though an emperor of the heavens stands before me. Lead the Fifth Crusade, Sovereign. May the Worldwound shatter beneath your heel.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Imperial majesty acknowledged by mortal royalty. The crusade marches under your sovereign will!\"</i>"));
		Port("IsekaiAct1BartenderOtherworldDistillation", "(Isekai Protagonist) [High-Proof Fractional Distillation & Carbonation Technique] \"Master Gemyl, your ale is watered down by panic. Let me demonstrate a simple fractional freeze-distillation and effervescent herb carbonation. One barrel of this 'Otherworld IPA' will double soldier morale and clean their field wounds in a pinch.\"", "{n}Gemyl takes a cautious sip from the effervescent wooden tankard. His eyes widen in pure ecstasy as the crisp, aromatic hops and sparkling bubbles hit his tongue.{/n} \"By Cayden's blessed beard! It's crisp, it's fizzing like lightning, and the aftertaste warms you right down to your boots! You're a genius, friend! Take these cure draughts from behind the bar--any drink you ever order in my tavern is on the house!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"A master brewmaster from another reality! Cayden Cailean raises his mug in celestial salute!\"</i>"));
		Port62Graft[] grafts = Grafts;
		foreach (Port62Graft port62Graft in grafts)
		{
			if (Built.TryGetValue(port62Graft.Answer, out var value))
			{
				Port62Grafting.Exclude(port62Graft, value, Built);
			}
		}
		AddBookAnswers();
	}

	private static void Port(string name, string answerText, string replyText, string line = null)
	{
		Port62Graft graft = Find(name);
		string text = Port62Grafting.Problem(graft);
		if (text != null)
		{
			Main.IsekaiContext.Logger.LogError("Port62Act1: '" + name + "' " + text + ", skipping.");
			return;
		}
		Condition[] array = Port62Grafting.Conditions(graft);
		if (array == null)
		{
			Main.IsekaiContext.Logger.LogWarning("Port62Act1: the gate of " + name + " could not be built, skipping.");
			return;
		}
		BlueprintCueBase blueprintCueBase = (string.IsNullOrEmpty(graft.ContinueCueGuid) ? null : BlueprintTools.GetBlueprint<BlueprintCueBase>(graft.ContinueCueGuid));
		BlueprintCueBase blueprintCueBase2 = (string.IsNullOrEmpty(graft.ContinueFallbackGuid) ? null : BlueprintTools.GetBlueprint<BlueprintCueBase>(graft.ContinueFallbackGuid));
		if ((graft.ContinueCueGuid != null && blueprintCueBase == null) || (graft.ContinueFallbackGuid != null && blueprintCueBase2 == null))
		{
			Main.IsekaiContext.Logger.LogWarning("Port62Act1: a continuation of " + name + " was not found, skipping.");
			return;
		}
		BlueprintUnit blueprintUnit = (string.IsNullOrEmpty(graft.ReplySpeaker) ? null : BlueprintTools.GetBlueprint<BlueprintUnit>(graft.ReplySpeaker));
		GraftedAnswer graftedAnswer = GraftedAnswer.Named(name).At(graft.Beat).Says(answerText)
			.ShownWhen(array)
			.OnSelect(Port62Grafting.OnSelect(graft, line))
			.Configure(delegate(BlueprintAnswer bp)
			{
				Port62Grafting.Configure(graft, bp);
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
		if (blueprintCueBase == null || graft.FallBackToList)
		{
			graftedAnswer.ThenBackToTheList();
		}
		if (blueprintCueBase != null)
		{
			if (blueprintCueBase2 != null)
			{
				graftedAnswer.ThenContinueTo(blueprintCueBase, blueprintCueBase2);
			}
			else
			{
				graftedAnswer.ThenContinueTo(blueprintCueBase);
			}
		}
		BlueprintAnswer blueprintAnswer = graftedAnswer.Graft();
		if (blueprintAnswer != null)
		{
			Built[name] = blueprintAnswer;
		}
	}

	private static void PortWithCompanion(string name, string answerText, string replyText, string line)
	{
		Port62Graft port62Graft = Find(name);
		string text = Port62Grafting.Problem(port62Graft);
		Condition[] array = ((text == null) ? Port62Grafting.Conditions(port62Graft) : null);
		BlueprintUnit blueprintUnit = ((text == null) ? BlueprintTools.GetBlueprint<BlueprintUnit>(port62Graft.ReplySpeaker) : null);
		if (array == null || blueprintUnit == null)
		{
			Main.IsekaiContext.Logger.LogError("Port62Act1: '" + name + "' cannot be built (" + (text ?? "gate or companion missing") + "), skipping.");
		}
		else
		{
			BlueprintAnswer blueprintAnswer = GraftedAnswer.Named(name).At(port62Graft.Beat).Says(answerText)
				.ShownWhen(array)
				.OnSelect(Port62Grafting.OnSelect(port62Graft, line))
				.RepliedToBy(name + "Reply", replyText)
				.SpokenByCompanion(blueprintUnit)
				.WithSpeakerlessTwin(name + "ReplyNoSpeaker")
				.ThenBackToTheList()
				.Graft();
			if (blueprintAnswer != null)
			{
				Built[name] = blueprintAnswer;
			}
		}
	}

	private static void AddBookAnswers()
	{
		BookAnswer("IsekaiWardstoneResonantHarmony", "(Isekai Protagonist) [Resonant Harmonization: Cleanse and Preserve] \"I will not shatter the crusaders' ancient aegis, nor let the Abyss corrupt it! I channel the boundless mana of the Otherworlder directly into the Wardstone's beating heart! Purge the demonic rot from its veins and unleash a tide of cleansing fire across Kenabres while anchoring the celestial barrier for eternity!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"EPIC TRIUMPH! The Wardstone is cleansed without loss of celestial barrier integrity! +1000 Cosmic Coins awarded!\"</i>"));
		BookAnswer("IsekaiAct1MarketSquareAeonRiftBookEvent", "(Isekai Protagonist) [Cosmic Equilibrium: Planar Rift Stabilization] Step before the tearing planar anomaly. As an entity whose very existence spans disparate dimensions, reach out with focused will and weave cosmic threads of equilibrium across the rift, sealing the abyssal fracture with the absolute authority of the cosmos.", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Planar tear stabilized through primordial multiversal equilibrium! +500 Cosmic Coins!\"</i>"));
		BookAnswer("IsekaiAct1MarketSquareDesnaHymnBookEvent", "(Isekai Protagonist) [Harmonic Resonance: Celestial Hymn of the Spheres] Touch the ancient altar of the Song of the Spheres, blending your otherworldly mana with the sacred melody of Elysium. Chime the bells in perfect octave resonance, suffusing the ruined square with celestial starlight.", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"The celestial chimes echo across Kenabres, heralding the triumph of dreams!\"</i>"));
	}

	private static void BookAnswer(string name, string text, string line)
	{
		Port62BookGraft graft = FindBook(name);
		BlueprintBookPage blueprintBookPage = ((graft == null) ? null : BlueprintTools.GetBlueprint<BlueprintBookPage>(graft.PageGuid));
		BlueprintBookPage next = ((graft == null) ? null : BlueprintTools.GetBlueprint<BlueprintBookPage>(graft.NextPageGuid));
		if (blueprintBookPage == null || next == null)
		{
			Main.IsekaiContext.Logger.LogWarning("Port62Act1: the page or next page of book answer " + name + " was not found, skipping.");
			return;
		}
		BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
		{
			bp.SetText(Main.IsekaiContext, text);
			bp.NextCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference> { next.ToReference<BlueprintCueBaseReference>() },
				Strategy = Strategy.First
			};
			bp.ShowOnce = true;
			bp.OnSelect = Helpers.CreateActionList(Port62Grafting.BookOnSelect(graft, line));
			bp.RequirePlotArmor();
			bp.ShowConditions.Conditions = bp.ShowConditions.Conditions.AppendToArray(Port62Grafting.MirroredConditions(graft));
		});
		BlueprintBookPage blueprintBookPage2 = blueprintBookPage;
		if (blueprintBookPage2.Answers == null)
		{
			blueprintBookPage2.Answers = new List<BlueprintAnswerBaseReference>();
		}
		if (!blueprintBookPage.Answers.Any((BlueprintAnswerBaseReference r) => r != null && ((BlueprintReferenceBase)r).deserializedGuid == answer.AssetGuid))
		{
			blueprintBookPage.Answers.Insert(0, answer.ToReference<BlueprintAnswerBaseReference>());
		}
	}
}
