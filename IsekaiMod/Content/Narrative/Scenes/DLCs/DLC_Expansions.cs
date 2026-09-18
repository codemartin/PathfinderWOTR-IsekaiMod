using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.DLCs
{
	public static class DLC_Expansions
	{
		public static void Register()
		{
			RegisterDLC1InevitableExcess();
			RegisterDLC3MidnightIsles();
			RegisterDLC4LastSarkorians();
			RegisterDLC5ThroughTheAshes();
			RegisterDLC6DanceOfMasks();
		}

		private static void RegisterDLC1InevitableExcess()
		{
			NarrativeScene narrativeScene = new NarrativeScene("DLC1_Valmallos_ParadoxDebate", "b686f80a62f24ca08899e558b96c7a1b", "DLC1", "DLC 1 Inevitable Excess: Valmallos and the Cosmic Axiomatic Paradox");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DLC1_Valmallos_Universal_Anomaly",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"You spent eons simulating causal anomalies, Valmallos. But my very presence violates your axiomatic equations because I originate outside your cosmological lattice. I am the free variable your simulation cannot resolve.\"",
				NpcReplyText = "{n}The colossal inevitable's clockwork dials spin with frantic acceleration, sparks of axiomatic light dancing across his brass plates.{/n} \"An extraneous vector... an origin outside the Great Beyond. Fascinating. Axiomatic law must adapt to encompass the transcendent traveler.\"",
				Rewards = new SceneRewards
				{
					Gold = 50000,
					CosmicCoins = 1000
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.Quest,
					SceneContext = "DLC 1: Valmallos Axiomatic Debate",
					CosmicCoins = 1000,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> whispers with infinite cosmic irony: \"The machine seeks to measure the sky with a ruler. Show him the boundless void.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDLC3MidnightIsles()
		{
			NarrativeScene narrativeScene = new NarrativeScene("DLC3_Helmsman_CursedVoyage", "aac49e0c7220417c89bde055dd698ebb", "DLC3", "DLC 3 Midnight Isles: Boarding the cursed ship and charting the abyssal archipelago");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DLC3_Helmsman_Universal_Voyage",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"An uncharted archipelago of nightmare islands, ancient treasure, and an undead crew? This sounds like the ultimate pirate anime arc! Unfurl the sails, Helmsman, we sail for glory and booty!\"",
				NpcReplyText = "{n}The spectral helmsman's skeletal jaw drops slightly, his hollow eye sockets flickering with green ethereal fire.{/n} \"A mad captain for a cursed voyage. Very well... steer toward the whirlpool, and let the depths test your mettle!\"",
				Rewards = new SceneRewards
				{
					Gold = 10000,
					CosmicCoins = 500
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Quest,
					SceneContext = "DLC 3: Sailing the Cursed Isles",
					CosmicCoins = 500,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a tankard high: \"A sea voyage with an undead crew! Now that is what I call a proper adventurer's holiday!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDLC4LastSarkorians()
		{
			NarrativeScene narrativeScene = new NarrativeScene("DLC4_Ulbrig_SarkorianHeritage", "57929455cd9546d28fc159656bf41eda", "DLC4", "DLC 4 The Last Sarkorians: Defending Gundrun and honoring Sarkorian memory");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DLC4_Ulbrig_Universal_HonorHeritage",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) \"Ulbrig, your home was stolen by abyssal corruption, but your spirit carries the soul of Sarkoris alive into the present day. We will ensure Gundrun stands proud, honoring your ancestors with every victory!\"",
				NpcReplyText = "{n}Ulbrig clashes his feathered bracers together, his griffon eyes shining with fierce, proud tears.{/n} \"Spoken like a true chief of the clans! By the spirits of old Sarkoris, we shall hunt these demons to the last feather!\"",
				Rewards = new SceneRewards
				{
					Gold = 8000,
					CosmicCoins = 400
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Dawnflower",
					Category = ConstellationCategory.Quest,
					SceneContext = "DLC 4: Honoring Sarkorian Clan Heritage",
					CosmicCoins = 400,
					Lines = new List<string> { "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> radiates warmth: \"Preserving the sacred memories of a shattered land. May the sun never set on Sarkoris!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDLC5ThroughTheAshes()
		{
			NarrativeScene narrativeScene = new NarrativeScene("DLC5_Rekarth_SubterraneanSurvival", "0cbb2e81134942d2aac2cf6312072c80", "DLC5", "DLC 5 Through the Ashes: Supporting the refugee survivors of Kenabres");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DLC5_Survivors_Universal_Solidarity",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"You survived with broken spoons, scavenged ropes, and sheer willpower while the heavens burned. You are the real heart of Kenabres. Here, take these rations and clean water; no survivor under my watch goes hungry.\"",
				NpcReplyText = "{n}The weary refugees weep openly as warm food and healing supplies are distributed among the children and wounded.{/n} \"Thank you... thank you, Commander! May the gods preserve you!\"",
				Rewards = new SceneRewards
				{
					Gold = 5000,
					CosmicCoins = 350
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Quest,
					SceneContext = "DLC 5: Refugee Solidarity",
					CosmicCoins = 350,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles softly: \"Compassion given to the humble and vulnerable shines brighter than any crown.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDLC6DanceOfMasks()
		{
			NarrativeScene narrativeScene = new NarrativeScene("DLC6_Festival_ArenaChampion", "a9f9c6440ea049c8aab718a18eab74cb", "DLC6", "DLC 6 A Dance of Masks: Kenabres Festival Arena Champion Challenge");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DLC6_Arena_Universal_TournamentArc",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"A grand festival martial tournament with cheering crowds, festive banners, and mythical challengers? Finally, my dream festival tournament arc has arrived! Announce my entrance, Master of Ceremonies!\"",
				NpcReplyText = "{n}The Arena Master bellows with showmanship, sweeping his feathered hat low as the crowd roars with deafening applause.{/n} \"LADIES AND GENTLEMEN OF KENABRES! WITNESS THE CHAMPION OF THE STARS HIMSELF ENTER THE SANDS!\"",
				Rewards = new SceneRewards
				{
					Gold = 20000,
					CosmicCoins = 600
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Quest,
					SceneContext = "DLC 6: Festival Tournament Arc",
					CosmicCoins = 600,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with festival thunder: \"LET THE ARENA ECHO WITH STEEL AND CHEERS! A GLORIOUS TOURNAMENT ARC!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
