using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.MainCampaign
{
	public static class Act3_DrezenReign
	{
		public static void Register()
		{
			RegisterAreeluLabLoopTruth();
			RegisterIvorySanctumJerribeth();
			RegisterMidnightFaneGalfrey();
			RegisterDrezenThroneReign();
		}

		private static void RegisterAreeluLabLoopTruth()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act3_AreeluLab_LoopTruth", "cd9c9facc3a8ded4e9683cde8958295e", "Act3", "Areelu's Laboratory: Discovering the rift between dimensions and the causal experiment");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "AreeluLab_Universal_SummonerDeconstruct",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"So that's how it is. You tore open the boundary between dimensions to stitch another soul into your lost child's vessel. You thought you were pulling puppet strings, Vorlesh, but an Otherworlder is no one's instrument.\"",
				NpcReplyText = "{n}The phantom projection of Areelu Vorlesh tilts her feathered head, a strange mixture of sorrow and profound fascination dancing in her abyssal eyes.{/n} \"You speak of other stars as if you remembered them. How magnificent... the experiment has produced an anomaly beyond even my calculations.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "Areelu Laboratory: Dimensional Rift Truth",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> whispers through the fractures of space: \"She breached the veil with crude mortal stitching. You are the paradox she cannot control.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "AreeluLab_MetaLoop_Foreknowledge",
				Type = OptionType.MetaLoop,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Cycle Insight) \"I already know every note in this laboratory, Areelu. The Nahyndrian crystal formulas, the Stitch's key, the rift in the abyss. How many times must we dance this cycle before the true loop shatters?\"",
				NpcReplyText = "{n}The projection's wings flare erratically as temporal feedback ripples through the laboratory illusion.{/n} \"'How many times'? What knowledge is this? Who taught you to speak in temporal circles?!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.MetaLoop,
					SceneContext = "Areelu Laboratory: Causal Foreknowledge",
					CosmicCoins = 350,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> resonates with cosmic power: \"The architect of the Worldwound trembles before the reader who has already turned the final page.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterIvorySanctumJerribeth()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act3_IvorySanctum_JerribethConfrontation", "f6a5baf5df275a4418614524a3932e97", "Act3", "Ivory Sanctum: Unmasking Jerribeth and Xanthir Vang");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "IvorySanctum_Universal_DismantleScheme",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"You traded your humanity for insectoid swarms and demonic flattery. What a pathetic downward spiral! The Fifth Crusade dismantles your laboratory today!\"",
				NpcReplyText = "{n}Jerribeth sneers with serpentine malice, her silk robes parting to reveal chittering abyssal claws.{/n} \"Insolent insect! You will feed Xanthir's broods before the sun rises over Drezen!\"",
				Rewards = new SceneRewards
				{
					Gold = 4000,
					CosmicCoins = 200
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Quest,
					SceneContext = "Ivory Sanctum: Defying Jerribeth",
					CosmicCoins = 200,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars: \"Crush the insects beneath your boots! Cleanse the sanctum in steel and flame!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterMidnightFaneGalfrey()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act3_MidnightFane_GalfreyDescent", "313722ee2b8089b45acb904d97125b81", "Act3", "Midnight Fane: Descent into the Abyss and Galfrey's command crisis");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "MidnightFane_Universal_AbyssDescent",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"Strip my title if you must, Your Majesty, but you cannot strip away what I have accomplished. The Abyss holds no terror for someone who crossed the cosmic void to reach Golarion. I will tear their rift shut from the other side!\"",
				NpcReplyText = "{n}Galfrey looks away, tears of profound guilt and political burden in her eyes, unable to meet your fearless gaze.{/n} \"May Iomedae watch over you, Commander... or whatever destiny now names you.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 300
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "Midnight Fane: Descent into the Abyss",
					CosmicCoins = 300,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> watches with heavy celestial sorrow: \"Kings and queens are bound by mortal crowns. But your destiny soars beyond all borders.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDrezenThroneReign()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act3_Drezen_ThroneRoomCouncil", "ba079958882c46941b09720e3e8e4b18", "Act3", "Drezen Citadel: Ruling from the Throne Room and shaping crusade policy");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DrezenThrone_GodEmperor_ImperialDecree",
				Type = OptionType.Subclass,
				RequiredProficiency = "GodEmperorProficiencies",
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(God Emperor) \"Let it be proclaimed across northern Avistan: Drezen is no mere outpost of Mendev, but the capital of an eternal sovereign empire. Send word to Cheliax and Numeria; any nation that honors our trade pacts shall prosper, and any that opposes us shall kneel.\"",
				NpcReplyText = "{n}The council scribes frantically inscribe your decrees onto parchment embossed with your imperial seal, bowing deeply as the chamber rings with majesty.{/n}",
				Rewards = new SceneRewards
				{
					Gold = 10000,
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Drezen Throne: Imperial Mandate",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> raises a goblet of ruby wine: \"Statecraft executed with absolute grandeur. Avistan will sing of your empire for ten thousand years.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
