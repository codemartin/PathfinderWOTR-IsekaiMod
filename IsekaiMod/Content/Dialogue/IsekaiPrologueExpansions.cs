using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal static class IsekaiPrologueExpansions
{
	internal sealed class Graft
	{
		public string Answer;

		public string Scene;

		public string ListGuid;

		public GraftBeat Beat;

		public string ReplySpeaker;

		public bool Narrated;

		public string ContinueCueGuid;

		public string Gate;

		public bool AngelOnly;

		public string ExclusiveWith;

		public string AchievementFlag;

		public AlignmentShiftDirection Shift = AlignmentShiftDirection.TrueNeutral;

		public int ExpCr = 2;

		public string ClaimKey;

		public int Coins;

		public string SixTwoSponsor;

		public int Gold;

		public string[] VanillaItems = new string[0];

		public string[] ModItems = new string[0];

		public string Buff;

		public int BuffMinutes;

		public string Feature;

		public string Sponsor => Port62Rewards.Seat(SixTwoSponsor);

		public bool HasReward => ClaimKey != null;
	}

	internal sealed class BookGraft
	{
		public string Answer;

		public string PageGuid;

		public string NextPageGuid;

		public string Etude;

		public int ExpCr = 3;

		public string ClaimKey;

		public int Coins;

		public string SixTwoSponsor;

		public string[] ModItems = new string[0];

		public string Buff;

		public int BuffMinutes;

		public string Feature;

		public string Sponsor => Port62Rewards.Seat(SixTwoSponsor);
	}

	private const string CureLightWounds = "d52566ae8cbe8dc4dae977ef51c27d91";

	private const string CureModerateWounds = "f991f3051c3b9e64fabc87891077b613";

	private const string ShieldOfFaithPotion = "bc93a78d71bef084fa155e529660ed0d";

	private const string BullsStrengthPotion = "2983c97b71ba60443b1c749854ef53a1";

	private const string StandardLightCrossbow = "511c97c1ea111444aa186b1a58496664";

	private const string ColdIronArrowsQuiver = "464ecede228b0f745a578f69a968226d";

	private const string MasterworkHeavyCrossbow = "adc1a081ba26d2a439a62d702437a40d";

	private const string Salvation = "4cefa6f7338ff6744b04179320a3cecb";

	private const string Retribution = "893252fb235b69a47b86b404e1d8accd";

	internal static readonly Graft[] Grafts = new Graft[17]
	{
		SquareAwakening(),
		SquareHulrun(),
		SquareTerendelev(),
		StitchGreedyHoard(),
		WakingUp(),
		MartialGodWaking(),
		AneviaBoneMend(),
		AneviaSlimeCast(),
		CamelliaAppraisal(),
		CamelliaOverlord(),
		MeetLannDiplomat(),
		VendougMasterStrategist(),
		SulHarmoniousRevelation(),
		HorgusExecutiveRetainer(),
		CaveBickeringMediation(),
		DyraAppraisal(),
		HosillaInvalidation()
	};

	internal static readonly BookGraft[] BookGrafts = new BookGraft[3]
	{
		new BookGraft
		{
			Answer = "IsekaiStorybookPage1DualAction",
			PageGuid = "ee776f7d205f3484e9def0e007c5623f",
			NextPageGuid = "2212f276d642d1d4fb8f93a0b74cb320",
			Etude = "4cefa6f7338ff6744b04179320a3cecb",
			ClaimKey = "Port62:StorybookPage1DualAction",
			Coins = 350,
			SixTwoSponsor = "The Dawnflower",
			ModItems = new string[1] { "ItemLarielFeather" }
		},
		new BookGraft
		{
			Answer = "IsekaiStorybookPage12SoulAegis",
			PageGuid = "2212f276d642d1d4fb8f93a0b74cb320",
			NextPageGuid = "77ec7207718bf02409322455b1fbbf6c",
			ClaimKey = "Port62:StorybookPage12SoulAegis",
			Coins = 300,
			SixTwoSponsor = "The Grand Arbiter",
			Buff = "MindOfTheOtherworlderBuff"
		},
		new BookGraft
		{
			Answer = "IsekaiStorybookPage33DualCovenant",
			PageGuid = "a6df6ed440dce0e499eb66b832243229",
			NextPageGuid = "d1da445d30718914a8b7794864af8039",
			Etude = "893252fb235b69a47b86b404e1d8accd",
			ExpCr = 4,
			ClaimKey = "Port62:StorybookPage33DualCovenant",
			Coins = 400,
			SixTwoSponsor = "The Inheritor",
			Feature = "LarielsVowFeature"
		}
	};

	internal static string PageOneRewardLine => ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Everbloom']</b></color>: <i>\"Mercy and vigilance intertwined. The golden flame burns brightest in your hands.\"</i>");

	internal static string PageTwelveRewardLine => ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Demon psychic manipulation completely neutralized by dimensional displacement.\"</i>");

	internal static string PageThirtyThreeRewardLine => ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"A commander who refuses compromise between mercy and justice. Well chosen!\"</i>");

	private static string SpeakerOf(string unitGuid)
	{
		return unitGuid;
	}

	private static Graft SquareAwakening()
	{
		Graft graft = new Graft();
		graft.Answer = "IsekaiPrologueSquareAwakening";
		graft.Scene = "c0/KenabresSquare/WelcomeDialogue";
		graft.ListGuid = "87997a477e6a58d4aae46e26a3712825";
		graft.Beat = DialogueBeats.HulrunPraysOverYou;
		graft.ReplySpeaker = SpeakerOf("77fbcde261cd4396bba5d9fe0b2c57c6");
		graft.ClaimKey = "Port62:SquareAwakening";
		graft.Coins = 300;
		graft.SixTwoSponsor = "The Fading Dragon";
		graft.ModItems = new string[1] { "ItemSilverDragonShard" };
		return graft;
	}

	private static Graft SquareHulrun()
	{
		Graft graft = new Graft();
		graft.Answer = "IsekaiPrologueSquareHulrun";
		graft.Scene = "c0/KenabresSquare/WelcomeDialogue";
		graft.ListGuid = "e27807b731f3b1a4eb19c1a04fdfcf53";
		graft.Beat = DialogueBeats.HulrunAsksWhoYouAre;
		graft.ReplySpeaker = SpeakerOf("77fbcde261cd4396bba5d9fe0b2c57c6");
		graft.ExpCr = 3;
		graft.ClaimKey = "Port62:SquareHulrun";
		graft.Coins = 350;
		graft.SixTwoSponsor = "The Grand Arbiter";
		graft.Gold = 150;
		graft.VanillaItems = new string[3] { "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91" };
		return graft;
	}

	private static Graft SquareTerendelev()
	{
		Graft graft = new Graft();
		graft.Answer = "IsekaiPrologueSquareTerendelev";
		graft.Scene = "c0/KenabresSquare/WelcomeDialogue";
		graft.ListGuid = "0f7dddeb3f77a4f408e7dc843b9c66fb";
		graft.Beat = DialogueBeats.TerendelevHasHealedYou;
		graft.ReplySpeaker = SpeakerOf("045e2ea911644fa0abaa2f6c76b83feb");
		graft.ExpCr = 3;
		graft.ClaimKey = "Port62:SquareTerendelev";
		graft.Coins = 400;
		graft.SixTwoSponsor = "The Fading Dragon";
		graft.ModItems = new string[1] { "ItemTearOfSilverDragon" };
		graft.Buff = "SilverDragonAegisBuff";
		graft.BuffMinutes = 1440;
		return graft;
	}

	private static Graft StitchGreedyHoard()
	{
		Graft graft = new Graft();
		graft.Answer = "IsekaiPrologueStitchGreedyHoard";
		graft.Scene = "c0/KenabresSquare/StitchGivesCrossbow";
		graft.ListGuid = "a3e8939fe8354cb4698403e4256fe0d9";
		graft.Beat = DialogueBeats.StitchOffersTheCrossbow;
		graft.ReplySpeaker = SpeakerOf("90215a976d13b254ab905d0d6f747deb");
		graft.ClaimKey = "Port62:StitchGreedyHoard";
		graft.Coins = 350;
		graft.SixTwoSponsor = "The Lucky Drunk";
		graft.VanillaItems = new string[7] { "511c97c1ea111444aa186b1a58496664", "464ecede228b0f745a578f69a968226d", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "d52566ae8cbe8dc4dae977ef51c27d91", "bc93a78d71bef084fa155e529660ed0d", "bc93a78d71bef084fa155e529660ed0d" };
		graft.ModItems = new string[1] { "ItemCodexOfReincarnation" };
		graft.Buff = "StitchWardBuff";
		graft.BuffMinutes = 60;
		return graft;
	}

	private static Graft WakingUp()
	{
		return new Graft
		{
			Answer = "IsekaiPrologueWakingUp",
			Scene = "c0/CavesUnderKenabres/MeetSeelahAnevia",
			ListGuid = "a55fc20c6f0ff56439b40d6ba53cb8d7",
			Beat = DialogueBeats.SeelahAsksForHelpWithTheBoulder,
			ReplySpeaker = SpeakerOf("54be53f0b35bf3c4592a97ae335fe765")
		};
	}

	private static Graft MartialGodWaking()
	{
		return new Graft
		{
			Answer = "IsekaiPrologueMartialGodWaking",
			Scene = "c0/CavesUnderKenabres/MeetSeelahAnevia",
			ListGuid = "a55fc20c6f0ff56439b40d6ba53cb8d7",
			Beat = DialogueBeats.SeelahAsksForHelpWithTheBoulder,
			ReplySpeaker = SpeakerOf("54be53f0b35bf3c4592a97ae335fe765"),
			Gate = "MartialGodProficiencies",
			ExpCr = 3
		};
	}

	private static Graft AneviaBoneMend()
	{
		return new Graft
		{
			Answer = "IsekaiPrologueAneviaBoneMend",
			Scene = "c0/CavesUnderKenabres/MeetSeelahAnevia",
			ListGuid = "da6ca50574b6c714f9166f37b64b7b59",
			Beat = DialogueBeats.AneviaAsksWhoYouAre,
			ReplySpeaker = SpeakerOf("ea562adea1736874c9c5616d140fe773"),
			ExpCr = 3
		};
	}

	private static Graft AneviaSlimeCast()
	{
		return new Graft
		{
			Answer = "IsekaiPrologueAneviaSlimeCast",
			Scene = "c0/CavesUnderKenabres/MeetSeelahAnevia",
			ListGuid = "da6ca50574b6c714f9166f37b64b7b59",
			Beat = DialogueBeats.AneviaAsksWhoYouAre,
			ReplySpeaker = SpeakerOf("ea562adea1736874c9c5616d140fe773"),
			Gate = "DevourerProficiencies",
			ExpCr = 4
		};
	}

	private static Graft CamelliaAppraisal()
	{
		return new Graft
		{
			Answer = "IsekaiPrologueCamelliaAppraisal",
			Scene = "c0/CavesUnderKenabres/MeetCamelia",
			ListGuid = "1ca6cf08fceeac141a0df689cecc784a",
			Beat = DialogueBeats.CamelliaIntroducesHerself,
			ReplySpeaker = SpeakerOf("397b090721c41044ea3220445300e1b8"),
			ExpCr = 3
		};
	}

	private static Graft CamelliaOverlord()
	{
		return new Graft
		{
			Answer = "IsekaiPrologueCamelliaOverlord",
			Scene = "c0/CavesUnderKenabres/MeetCamelia",
			ListGuid = "1ca6cf08fceeac141a0df689cecc784a",
			Beat = DialogueBeats.CamelliaIntroducesHerself,
			ReplySpeaker = SpeakerOf("397b090721c41044ea3220445300e1b8"),
			Gate = "OverlordProficiencies",
			ExpCr = 4
		};
	}

	private static Graft MeetLannDiplomat()
	{
		Graft graft = new Graft();
		graft.Answer = "IsekaiPrologueMeetLannDiplomat";
		graft.Scene = "c0/CavesUnderKenabres/MeetLann";
		graft.ListGuid = "61ba231b3ad6b144a918c88ca49cb92a";
		graft.Beat = DialogueBeats.LannAndWenduagFirstSighting;
		graft.ExpCr = 3;
		graft.ClaimKey = "Port62:MeetLannDiplomat";
		graft.Coins = 350;
		graft.SixTwoSponsor = "The Song of the Spheres";
		graft.VanillaItems = new string[3] { "adc1a081ba26d2a439a62d702437a40d", "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" };
		return graft;
	}

	private static Graft VendougMasterStrategist()
	{
		Graft graft = new Graft();
		graft.Answer = "IsekaiVendougMasterStrategist";
		graft.Scene = "c0/CavesUnderKenabres/VendougDontTell";
		graft.ListGuid = "c4d71c5f339b4564da1f4a86d323249e";
		graft.Beat = DialogueBeats.WenduagAsksYouToHideTheLight;
		graft.ReplySpeaker = SpeakerOf("ae766624c03058440a036de90a7f2009");
		graft.ExpCr = 3;
		graft.ClaimKey = "Port62:VendougMasterStrategist";
		graft.Coins = 300;
		graft.SixTwoSponsor = "The Laughing King";
		graft.Gold = 350;
		graft.VanillaItems = new string[5] { "adc1a081ba26d2a439a62d702437a40d", "2983c97b71ba60443b1c749854ef53a1", "2983c97b71ba60443b1c749854ef53a1", "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613" };
		return graft;
	}

	private static Graft SulHarmoniousRevelation()
	{
		Graft graft = new Graft();
		graft.Answer = "IsekaiSulHarmoniousRevelation";
		graft.Scene = "c0/CavesUnderKenabres/NeathholmVillage/SulDialogue";
		graft.ListGuid = "5233c72929cda7c48b92ec6292a730ac";
		graft.Beat = DialogueBeats.SullAsksForTheLight;
		graft.Narrated = true;
		graft.ContinueCueGuid = "8d0e0c2e20c12c14b9287dca004e8dd7";
		graft.AngelOnly = true;
		graft.ExclusiveWith = "042f8bc164f99bd43aed8b3293569bf4";
		graft.AchievementFlag = "c1691692455b46228ba19ad9eb1d3b3d";
		graft.Shift = AlignmentShiftDirection.Lawful;
		graft.ExpCr = 4;
		graft.ClaimKey = "Port62:SulHarmoniousRevelation";
		graft.Coins = 450;
		graft.SixTwoSponsor = "The Song of the Spheres";
		graft.VanillaItems = new string[1] { "209960f8d60e23a4c8e75d14e2261263" };
		return graft;
	}

	private static Graft HorgusExecutiveRetainer()
	{
		return new Graft
		{
			Answer = "IsekaiHorgusExecutiveRetainer",
			Scene = "c0/CavesUnderKenabres/NeathholmVillage/Horgus_GetMeOut",
			ListGuid = "a6b5ef97dca04894a8cc7984bff5a592",
			Beat = DialogueBeats.HorgusAgreesToTheEscort,
			ReplySpeaker = SpeakerOf("c02e641bf8cf0984fb49604afa224563"),
			ExpCr = 3,
			ClaimKey = "Port62:HorgusExecutiveRetainer",
			Coins = 300,
			SixTwoSponsor = "The Dark Prince",
			Gold = 500
		};
	}

	private static Graft CaveBickeringMediation()
	{
		return new Graft
		{
			Answer = "IsekaiPrologueCaveBickeringMediation",
			Scene = "c0/CavesUnderKenabres/NeathholmVillage/HorgusAnevia",
			ListGuid = "65395d8277d3b9b4f82f068616de8a56",
			Beat = DialogueBeats.HorgusAndAneviaBicker,
			ReplySpeaker = SpeakerOf("ea562adea1736874c9c5616d140fe773"),
			ClaimKey = "Port62:CaveBickeringMediation",
			Coins = 300,
			SixTwoSponsor = "The World Sovereign"
		};
	}

	private static Graft DyraAppraisal()
	{
		Graft graft = new Graft();
		graft.Answer = "IsekaiPrologueDyraAppraisal";
		graft.Scene = "c0/CavesUnderKenabres/NeathholmVillage/Dyra_Vendor";
		graft.ListGuid = "4b2c98a24f2bb304f8ee50286eec3f90";
		graft.Beat = DialogueBeats.DyraHub;
		graft.ReplySpeaker = SpeakerOf("bd6d6b07f1682c14c9d0966ef7954648");
		graft.ClaimKey = "Port62:DyraAppraisal";
		graft.Coins = 300;
		graft.SixTwoSponsor = "The Lucky Drunk";
		graft.VanillaItems = new string[3] { "f991f3051c3b9e64fabc87891077b613", "f991f3051c3b9e64fabc87891077b613", "bc93a78d71bef084fa155e529660ed0d" };
		return graft;
	}

	private static Graft HosillaInvalidation()
	{
		Graft graft = new Graft();
		graft.Answer = "IsekaiPrologueHosillaInvalidation";
		graft.Scene = "c0/CavesUnderKenabres/MongrelLabyrinth/SavamelehHosillaDialogue";
		graft.ListGuid = "d35d5c76186af32468dc71952eb339ec";
		graft.Beat = DialogueBeats.SavamelehVision;
		graft.Narrated = true;
		graft.ExpCr = 3;
		graft.ClaimKey = "Port62:HosillaInvalidation";
		graft.Coins = 350;
		graft.SixTwoSponsor = "The Inheritor";
		graft.VanillaItems = new string[2] { "2983c97b71ba60443b1c749854ef53a1", "2983c97b71ba60443b1c749854ef53a1" };
		return graft;
	}

	internal static Graft Find(string answer)
	{
		return Grafts.FirstOrDefault((Graft g) => g.Answer == answer);
	}

	internal static BookGraft FindBook(string answer)
	{
		return BookGrafts.FirstOrDefault((BookGraft g) => g.Answer == answer);
	}

	public static void Add()
	{
		Port("IsekaiPrologueSquareAwakening", "(Isekai Protagonist) [Otherworlder's Instinct] \"Alright, deep breaths... let's test the classic protocol. 'Status Window, open!' ...Menu? Character sheet? System inventory? ...Nothing. No floating blue display, no cheerful tutorial fairy. Just cold cobblestones, church bells, and a terrifyingly real wound in my chest.\"", "{n}The grim man kneeling over you keeps his glowing palm pressed to your chest, and for a moment the prayer falters.{/n} \"Floating windows. Invisible menus. You are raving, and you are losing blood. Lie still. Iomedae has not finished with you yet.\" {n}He presses something cold and faceted into your hand.{/n} \"Hold on to that. A sliver of our dragon's scale, from the festival stalls. Let its sacred starlight steady your fevered spirit.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"A brave soul from beyond the threshold stirs! Accept this blessing, traveler.\"</i>"));
		Port("IsekaiPrologueSquareHulrun", "(Isekai Protagonist) [Otherworlder's Frank Confession] \"I didn't sneak through your checkpoints, Prelate. The truth is far stranger: I am an Otherworlder. In the realm I came from, there are spires of glass and steel that scrape the clouds, horseless carriages of metal, and nights illuminated by harnessed lightning. I went to sleep in my world and woke up bleeding on your cobblestones.\"", "{n}Hulrun's weathered face twitches with severe suspicion, his knuckles tightening on his halberd until the metal creaks. He stares at you in dumbfounded disbelief before letting out a harsh, rasping snort.{/n} \"'Towers of glass'? 'Harnessed lightning'? By the Inheritor's shield, the blow to your skull has scrambled your brains. You are no demonic saboteur. Even the most half-witted cultist of Baphomet would invent a less preposterous tale.\" {n}He drops a small purse and a few vials of salve beside your stretcher.{/n} \"The festival stipend for the wounded. Now, again, and properly this time: who are you?\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Honesty so absurd it completely disarms inquisitorial paranoia. Superb opening performance.\"</i>"));
		Port("IsekaiPrologueSquareTerendelev", "(Isekai Protagonist) [Awe of the Reincarnated] \"In the world I came from, dragons existed only in bedtime chronicles and faded tapestries. But I felt what just healed me, and it was not a priest's prayer. You are her, aren't you? The silver dragon of Kenabres, wearing a woman's face.\"", "{n}The silver-haired woman smiles, and for the space of a breath her eyes are molten gold and the air around her smells of mountain pine and clean frost.{/n} \"You see a great deal for someone who was dying a moment ago, child of another sky. I felt it as I healed you: a spark deep in your soul, alien to this world, yet untouched by the Abyss.\" {n}She presses a pearlescent teardrop into your palm.{/n} \"Take this silver tear as my keepsake. Whatever trials await you beneath these heavens, let my light shield you from the dark.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"My guardian flame shall watch over your footsteps, Otherworlder.\"</i>"));
		Port("IsekaiPrologueStitchGreedyHoard", "(Isekai Protagonist) [Pragmatic Survivalist: Take Both] \"Listen to me, little one! When the sky tears open and demonic locusts rain from the heavens, you don't choose between weapons and medicine, you take both or you die! Hand over the crossbow, the cold iron bolts, and every healing draught in that satchel! Right now!\"", "{n}Stitch shrieks in utter panic as demonic rubble crashes down across the square, frantically shoving his entire leather satchel into your arms with trembling claws.{/n} \"Take it! Take the bow, take the bolts, take Stitch's lucky draughts! Just don't let those winged bug monsters rip my skin off!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"Hahaha! That's the spirit! Squeeze every coin and arrow out of the universe!\"</i>"));
		Port("IsekaiPrologueWakingUp", "(Isekai Protagonist) [Steeled Otherworlder] \"One moment I was falling between dimensions, and the next I was tumbling down a fissure with a dragon battling a demon lord overhead. My head is pounding, but my pulse is steady and I'm still breathing. Check your gear, paladin; we're climbing out of this pit together.\"", "{n}Seelah blinks through the limestone dust coating her brow, then lets out a hearty, surprised laugh.{/n} \"Another world? Well, whatever distant realm you tumbled out of, they certainly breed folk with backbone! Name's Seelah. Now help me get my friend out from under these rocks.\"");
		Port("IsekaiPrologueMartialGodWaking", "(Martial God) [Ki Circulation] \"My meridians are clear and circulating smoothly. Tend to your friend; I will secure the perimeter.\"", "{n}Seelah nods respectfully at your effortless stance.{/n} \"Now that's what I call crusader discipline! But the perimeter can wait a moment. First, the rocks.\"");
		Port("IsekaiPrologueAneviaBoneMend", "(Isekai Protagonist) [Otherworlder's Field Medicine] \"Before introductions: bite on this strap and let me look at that leg. Where I come from, proper traction and a steady hand can set a fracture cleanly before the shock sets in.\"", "{n}With a firm, practiced twist and a pulse of channeled mana, the bone grinds back into line. Anevia gasps, trembling, as the searing pain eases into a cool, steady numbness.{/n} \"Sweet Desna... who taught you hands like that?! Thank you, friend. I'm still not dancing anytime soon, but I might not be dead weight in this grave after all.\"");
		Port("IsekaiPrologueAneviaSlimeCast", "(Slime) [Biomechanical Secretion Cast] \"My fluid secretion can form a semi-rigid cast around your fracture in five seconds. Hold still.\"", "{n}Anevia stares as the painless translucent splint locks her leg in place.{/n} \"A living splint?! That's the strangest thing anyone's ever done for me, and I'm married to Irabeth. It holds, though. Thanks!\"");
		Port("IsekaiPrologueCamelliaAppraisal", "(Isekai Protagonist) [Discerning Gaze] \"You can lower the delicate aristocratic mask, Lady Camellia. The tremble in your fingers isn't horror at this corpse, it's a thrill. I know a hidden hunger when I look into someone's eyes. Point it at the demons devouring this city, and we won't have an issue.\"", "{n}Camellia's composure stiffens, her knuckles whitening around her rapier as a dark spark flares in her eyes. For a heartbeat the mask slips entirely before she recovers with a tight, breathy chuckle.{/n} \"How remarkably perceptive... and how delightfully insolent. Very well. As long as our blades point at the same fiends, I suppose I can tolerate your company.\"");
		Port("IsekaiPrologueCamelliaOverlord", "(Overlord) [Gaze of the Sovereign] \"You tremble with amateur bloodlust, girl. Learn to master your hunger in the presence of a true sovereign of death, or be erased.\"", "{n}Camellia shivers as a chill tightens around her throat, and she lowers her eyes.{/n} \"Understood... my lord.\"");
		Port("IsekaiPrologueMeetLannDiplomat", "(Isekai Protagonist) [Otherworlder's Diplomatic Hand] \"Lower your bows, scouts. I am an Otherworlder who just survived a fall from the surface alongside these crusaders. We seek a way out, not a quarrel. Share your path, and we will share what we have and stand with you against whatever prowls these tunnels.\"", "{n}Lann and Wenduag exchange wary glances, their bowstrings slowly easing as they take in your open hands.{/n} \"An Otherworlder?\" {n}Lann scratches his horn.{/n} \"Surface folk usually scream at our scales or draw steel first. If you've got guts and good intentions, we can talk.\" {n}Wenduag says nothing, but she tosses you a crossbow from the scouts' cache and a pair of healing draughts, as if testing what you will do with them.{/n}", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"A peaceful hand extended in the darkness. The stars illuminate your path!\"</i>"));
		Port("IsekaiVendougMasterStrategist", "(Isekai Protagonist) [Master Strategist of the Otherworld] \"Lower your voice, Wenduag. I am neither a reckless zealot nor a fool. If the light is shown to Sull, I will see to it that the young hunters hold Neathholm's barricades instead of marching into a meat grinder. Your hunters survive, the tribe is not thrown away, and you and I take the Shield Maze together.\"", "{n}Wenduag freezes, her amber eyes narrowing as she studies your calm expression. A slow, predatory smile parts her fangs.{/n} \"You... you aren't an idealistic crusader puppy at all. You think like a chieftain, three moves ahead.\" {n}She presses a pouch of coin, a bundle of stoppered draughts and a masterwork crossbow from her ambush stash into your hands.{/n} \"My stash. If you can truly pull this off, my bow belongs to you.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Playing both sides like a seasoned guild grandmaster! Exquisite diplomacy!\"</i>"));
		Port("IsekaiSulHarmoniousRevelation", "(Isekai Protagonist) [The Harmonious Revelation] \"Look upon the sacred light of the First Crusaders, Chief Sull, and let hope return to the children of the stone! But heed my words: do not send your youth to die unprepared in the dark. Wenduag's hunters shall guard Neathholm's walls, while my companions and I carve a path through Hosilla's lair!\"", "{n}You raise your hand, and the light you carried out of the angel's memory answers, pouring from your palm across the cavern until every scaled face in Neathholm is turned toward it.{/n}", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"Hope rekindled beneath the stones. The stars smile upon Neathholm!\"</i>"));
		Port("IsekaiHorgusExecutiveRetainer", "(Isekai Protagonist) [Shrewd Executive Retainer] \"Master Gwerm, highborn titles and family crests won't parry a fiend's claws in these tunnels, but gold can certainly buy the finest protection in Kenabres. If you desire my personal vanguard to escort you safely to your mansion's doorstep, you will pay five hundred pieces of gold upfront as an executive retainer, and the remaining bounty once you are sitting comfortably in your parlor.\"", "{n}Horgus's face flushes a violent shade of purple, sputtering with wounded aristocratic pride before casting a terrified glance at the cavern shadows and furiously slapping a heavy purse into your palm.{/n} \"Five hundred gold pieces upfront?! You are an unprincipled brigand in crusader armor! ...Take it! Every coin I carried down here, you extortionist, but if a single mongrel or demon lays a finger on my velvet doublet, I'll have you thrown into the deepest dungeon in Mendev!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>: <i>\"A contract sealed in gold before the blade is drawn. Commendable pragmatism.\"</i>"));
		Port("IsekaiPrologueCaveBickeringMediation", "(Isekai Protagonist) [Diplomatic De-escalation Protocol] \"Cease your bickering, both of you. Master Gwerm, shouting at an injured crusader will not mend the ceiling or hasten our ascent. Anevia, save your breath for the climb. We survive together or we perish separately.\"", "{n}Horgus shuts his mouth with an indignant snap, smoothing his velvet lapels in embarrassed silence, while Anevia lets out a weary, appreciative chuckle.{/n} \"You heard the commander, Master Gwerm. Let's keep moving before more rocks drop on our skulls.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"A true commander brings immediate order to panic and discord.\"</i>"));
		Port("IsekaiPrologueDyraAppraisal", "(Isekai Protagonist) [Appraisal of the Otherworld] \"Let me look over those salvage bundles, Dyra. In my world, telling rare alchemical reagents from common scrap was second nature. There are preserved tinctures hidden under that tarpaulin.\"", "{n}Dyra blinks in astonishment, her hands sifting through the grime-caked satchels until she uncovers three perfectly preserved crystal phials.{/n} \"By the spirits... you spotted these through the ash and leather?! Most surface folk call all of this junk! Keep them, sharp-eyed traveler, and come back when you've got coin!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"A keen eye turns subterranean rubbish into liquid gold! Bottoms up!\"</i>"));
		Port("IsekaiPrologueHosillaInvalidation", "(Isekai Protagonist) [Psychological Disruption] \"This is the best a cowardly demon lord's servant can do, Hosilla? In the universe I hail from, petty cultists like you are the introductory warm-up encounter. Your dark god cannot shield you from what I bring.\"", "{n}Somewhere beyond the haze of the vision, Hosilla's voice cracks with fury, and for an instant you hear the dread beneath it.{/n} \"Blasphemous worm! Baphomet will strip the flesh from your bones!\" {n}The words cannot reach you here. The rage and the light are still waiting for your choice.{/n}", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Inheritor']</b></color>: <i>\"Righteous mockery breaks the cultist's resolve before steel even clashes!\"</i>"));
	}

	private static void Port(string name, string answerText, string replyText, string line = null)
	{
		Graft graft = Find(name);
		if (graft == null)
		{
			Main.IsekaiContext.Logger.LogError("IsekaiPrologueExpansions: '" + name + "' has no row in the table, skipping.");
			return;
		}
		if (graft.Beat == null || graft.Beat.HostList != GraftBeat.Normalize(graft.ListGuid))
		{
			Main.IsekaiContext.Logger.LogError("IsekaiPrologueExpansions: '" + name + "' has no beat on its list " + graft.ListGuid + ", skipping.");
			return;
		}
		Condition condition = Port62Rewards.Gate(graft.Gate);
		if (condition == null)
		{
			Main.IsekaiContext.Logger.LogWarning("IsekaiPrologueExpansions: gate " + graft.Gate + " not found, skipping " + name + ".");
			return;
		}
		BlueprintCueBase blueprintCueBase = (string.IsNullOrEmpty(graft.ContinueCueGuid) ? null : BlueprintTools.GetBlueprint<BlueprintCueBase>(graft.ContinueCueGuid));
		if (!string.IsNullOrEmpty(graft.ContinueCueGuid) && blueprintCueBase == null)
		{
			Main.IsekaiContext.Logger.LogWarning("IsekaiPrologueExpansions: continuation " + graft.ContinueCueGuid + " not found, skipping " + name + ".");
			return;
		}
		BlueprintUnit blueprintUnit = (string.IsNullOrEmpty(graft.ReplySpeaker) ? null : BlueprintTools.GetBlueprint<BlueprintUnit>(graft.ReplySpeaker));
		GraftedAnswer graftedAnswer = GraftedAnswer.Named(name).At(graft.Beat).Says(answerText)
			.ShownWhen(condition)
			.OnSelect(OnSelectActions(graft, line).ToArray())
			.Configure(delegate(BlueprintAnswer bp)
			{
				if (graft.AngelOnly)
				{
					bp.MythicRequirement = Mythic.AngelUnlocked;
				}
				if (graft.Shift != AlignmentShiftDirection.TrueNeutral)
				{
					bp.AlignmentShift = new AlignmentShift
					{
						Direction = graft.Shift,
						Value = 1,
						Description = new LocalizedString()
					};
				}
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
		if (blueprintCueBase != null)
		{
			graftedAnswer.ThenContinueTo(blueprintCueBase);
		}
		else
		{
			graftedAnswer.ThenBackToTheList();
		}
		BlueprintAnswer blueprintAnswer = graftedAnswer.Graft();
		if (blueprintAnswer != null && !string.IsNullOrEmpty(graft.ExclusiveWith))
		{
			BlueprintAnswer blueprint = BlueprintTools.GetBlueprint<BlueprintAnswer>(graft.ExclusiveWith);
			ExtensionMethods.HideEachOther(blueprintAnswer, blueprint);
		}
	}

	private static IEnumerable<GameAction> OnSelectActions(Graft graft, string sponsorLine)
	{
		if (graft.ExpCr > 0)
		{
			yield return new GainExp
			{
				Encounter = EncounterType.SkillCheck,
				CR = graft.ExpCr,
				Modifier = 1f
			};
		}
		if (!string.IsNullOrEmpty(graft.AchievementFlag))
		{
			BlueprintUnlockableFlag blueprint = BlueprintTools.GetBlueprint<BlueprintUnlockableFlag>(graft.AchievementFlag);
			if (blueprint != null)
			{
				yield return new IncrementFlagValue
				{
					m_Flag = blueprint.ToReference<BlueprintUnlockableFlagReference>(),
					Value = new IntConstant
					{
						Value = 1
					},
					UnlockIfNot = true
				};
			}
		}
		if (graft.HasReward)
		{
			yield return Port62Rewards.Reward(graft.ClaimKey, graft.Coins, graft.SixTwoSponsor, graft.Gold, graft.VanillaItems, graft.ModItems, graft.Buff, graft.BuffMinutes, graft.Feature, sponsorLine);
		}
	}

	internal static GameAction StorybookReward(string ported, string line, bool withCoins = true)
	{
		BookGraft bookGraft = FindBook(ported);
		if (bookGraft == null)
		{
			Main.IsekaiContext.Logger.LogError("IsekaiPrologueExpansions: '" + ported + "' has no book row, its reward is not paid.");
			return null;
		}
		return Port62Rewards.Reward(bookGraft.ClaimKey, withCoins ? bookGraft.Coins : 0, bookGraft.SixTwoSponsor, 0, null, bookGraft.ModItems, bookGraft.Buff, bookGraft.BuffMinutes, bookGraft.Feature, line, bookGraft.ExpCr);
	}
}
