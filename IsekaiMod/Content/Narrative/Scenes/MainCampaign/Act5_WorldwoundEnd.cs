using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Narrative.Actions;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.MainCampaign
{
	public static class Act5_WorldwoundEnd
	{
		public static void Register()
		{
			RegisterIzTriadCrisis();
			RegisterDeskariIzDefeat();
			RegisterDivineSummitGoddesses();
		}

		private static void RegisterIzTriadCrisis()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act5_Iz_TriadThreeWayCrisis", "c6d5321c350bf7c4fa364e34c0ee30a4", "Act5", "City of Iz: The Three-Way Crisis (Queen Galfrey, Irabeth, and Sword of Valor)");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Iz_ThirdOption_MultiFrontDefense",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.LawfulGood,
				CustomAction = new ContextActionIzTriadMultiFrontDefense(),
				PromptText = "(Omniscient Multi-Front Commander) \"A false dilemma crafted by Deskari's swarms! I reject your fatalistic sacrifice! Vanguard squad, secure Irabeth at the camp; retinue specialists, reinforce the Sword of Valor; I personally take Queen Galfrey's flank! All three shall stand triumphant today!\"",
				NpcReplyText = "{n}Dimensional warp gates blaze across the ruins of Iz, instantly deploying reinforced crusader contingents to all three sectors. Queen Galfrey gasps in awe as the demon ambush is pulverized on every front simultaneously.{/n} \"By the Inheritor... you saved Irabeth, preserved our sacred banner, and broke the siege in a single decisive strike!\"",
				Rewards = new SceneRewards
				{
					Gold = 25000,
					CosmicCoins = 600
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.Quest,
					SceneContext = "City of Iz: Golden Triumph Multi-Front Defense",
					CosmicCoins = 600,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> proclaims with celestial triumph: \"THE GOLDEN TRIUMPH OF IZ! NO COMRADE LEFT BEHIND, NO BANNER LOST TO THE FLOCK!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Iz_Universal_GalfreyIrabethReunion",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) [Triumph of the Three] \"Look around us, Galfrey, Irabeth. The Sword of Valor still flies above our heads, not a single crusader was abandoned to the swarm, and we broke Deskari's fatal trap without sacrificing anyone.\"",
				NpcReplyText = "{n}Irabeth's eyes shine with unshed tears as she leans against the battlements, her hand clasped firmly in Galfrey's.{/n} \"I came here expecting to die in the mud, Commander. You refused to accept that death. You gave us back our queen, our sacred banner, and our future.\" {n}Galfrey bows her head in profound, humble reverence.{/n} \"History will record this day as the Golden Triumph of Iz. In a hundred years of crusade, no leader has accomplished what you achieved today.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 600
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.Quest,
					SceneContext = "City of Iz: Galfrey and Irabeth Golden Reunion",
					CosmicCoins = 600,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> proclaims with celestial triumph: \"THE GOLDEN TRIUMPH OF IZ! SUNG ACROSS THE HEAVENS FOR GENERATIONS TO COME!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDeskariIzDefeat()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act5_Deskari_IzConfrontation", "7d877541fed521c489e6b33845647ad3", "Act5", "City of Iz: Deskari Lord of the Locust Host broken in the dust");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Deskari_Iz_Universal_CastDown",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"You spent a hundred years boasting of your hunger, Deskari. But today, your locust scythe is broken, your swarms are scattered, and you bleed upon the very mortal earth you sought to consume. Crawl back into your abyss and await your final extinction at Threshold!\"",
				NpcReplyText = "{n}Deskari shrieks with earth-shattering fury, his chitinous wings splintered and vomiting black ichor into the chasm.{/n} \"CURSE YOU, OTHERWORLD INTRUDER! AT THRESHOLD, I SHALL CONSUME YOUR SOUL TO THE LAST SPARK!\"",
				Rewards = new SceneRewards
				{
					Gold = 30000,
					CosmicCoins = 750
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "City of Iz: Deskari Broken in the Dust",
					CosmicCoins = 750,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with apocalyptic joy: \"BROKEN UPON THE STONES! THE LOCUST LORD BLEEDS BEFORE MORTAL STEEL!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDivineSummitGoddesses()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act5_DivineSummit_GoddessesMeeting", "ec740ac6a039d8c4b83a076cee6c1ff0", "Act5", "The Divine Summit: Confrontation between the Inheritor Iomedae and Lady Nocticula");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DivineSummit_Universal_SovereignAutonomy",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"Goddess of the Crusade, Lady of the Midnight Isles--hear me both. I was not summoned to Golarion to be a pawn on your divine chessboard. This power is mine, forged by my choices and the blood of my companions. I will close the Worldwound on my own terms.\"",
				NpcReplyText = "{n}A stunned silence falls across the celestial and shadowy assembly. Iomedae lowers her radiant sword with solemn respect, while Nocticula laughs with breathtaking delight.{/n} \"A mortal who stands before divinity and claims their own crown. How magnificent...\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 1000
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "Divine Summit: Asserting Cosmic Autonomy",
					CosmicCoins = 1000,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> echoes through all dimensions: \"The pawn steps off the board and demands the dice. The cycle trembles at your sovereign will.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
