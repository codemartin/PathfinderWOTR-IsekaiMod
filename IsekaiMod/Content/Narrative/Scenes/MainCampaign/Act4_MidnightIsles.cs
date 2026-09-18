using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Narrative.Actions;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.MainCampaign
{
	public static class Act4_MidnightIsles
	{
		public static void Register()
		{
			RegisterNexusArrival();
			RegisterShamiraHaremAudience();
			RegisterNocticulaPalaceSummit();
			RegisterBaphometColyphyrMine();
		}

		private static void RegisterNexusArrival()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act4_Nexus_ArrivalOperations", "91097a9b90537a34fbe379d5aa4d9cbe", "Act4", "The Nexus: Establishing the abyssal base camp in the Midnight Isles");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Nexus_Arrival_Universal_BaseOperations",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"The Abyss may be infinite chaos, but this cave has defensible choke points and a crystal warp gate. Secure the perimeters, unpack our portable rations, and establish our staging ground. We conquer Alushinyrra from here.\"",
				NpcReplyText = "{n}The surviving crusaders and companion retinue cheer with renewed spirit, rapidly pitching warding banners around the crystal hearth.{/n}",
				Rewards = new SceneRewards
				{
					Gold = 5000,
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "The Nexus: Staging Base Camp",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Setting up a cozy base camp right in the belly of the demon realm! Pass the flagon, let's explore this city!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterShamiraHaremAudience()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act4_Shamira_HaremAudience", "d138954fd7cdb2d4e90bb28cbd76235e", "Act4", "Harem of Ardent Dreams: Audience with the Ardent Dream Shamira");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Shamira_Audience_Mastermind_SubtleRebellion",
				Type = OptionType.Subclass,
				RequiredProficiency = "MastermindProficiencies",
				Alignment = AlignmentShiftDirection.LawfulEvil,
				PromptText = "(Mastermind) \"You play the loyal courtier to Lady Nocticula while secretly hoarding Nahyndrian crystal dust to fuel your own ascension. A standard court betrayal trope, Shamira. Shall we negotiate an arrangement where your ambitions serve my agenda?\"",
				NpcReplyText = "{n}Shamira's smoky eyes widen in predatory shock, her seductive fan freezing mid-flick.{/n} \"How delightfully dangerous you are, mortal. You see right through the silk veils of my harem... Let us talk of mutually beneficial arrangements.\"",
				Rewards = new SceneRewards
				{
					Gold = 8000,
					CosmicCoins = 300
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Harem of Ardent Dreams: Mastermind Leverage",
					CosmicCoins = 300,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> purrs with exquisite delight: \"Turning an archfiend's closest advisor into an unwitting asset in three sentences. True statecraft.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterNocticulaPalaceSummit()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act4_Nocticula_PalaceSummit", "75678bafb5e33854baaee17ee6eaf69c", "Act4", "House of Silken Shadows: Royal audience with Lady Nocticula");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Nocticula_Palace_Overlord_PeerToPeer",
				Type = OptionType.Subclass,
				RequiredProficiency = "OverlordProficiencies",
				Alignment = AlignmentShiftDirection.ChaoticEvil,
				CustomAction = new ContextActionCompleteBounty
				{
					BountyId = "Nocticula_Palace_Peer"
				},
				PromptText = "(Overlord) \"You sit upon a throne carved from slain demon lords, Nocticula, yet you dream of shedding your abyssal filth for celestial starlight. Do not pretend we are master and petitioner. We speak as sovereign rulers of our own destinies.\"",
				NpcReplyText = "{n}Nocticula reclines upon her velvet throne, her silken lips curving into a slow, intoxicating smile that could shatter mortal sanity.{/n} \"An Overlord from across the multiverse who sees the longing hidden behind my shadows. How intoxicating... I accept you as my guest and peer, Commander.\"",
				Rewards = new SceneRewards
				{
					Gold = 15000,
					CosmicCoins = 400
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Lady of Shadows",
					Category = ConstellationCategory.Subclass,
					SceneContext = "House of Silken Shadows: Overlord Peer Audience",
					CosmicCoins = 400,
					Lines = new List<string> { "<color=#9400D3><b>[The Constellation 'Our Lady in Shadow']</b></color> murmurs through the perfume of night: \"Finally, an Otherworlder who understands the burden of transcending one's own nature.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Nocticula_Palace_Universal_PeerToPeer",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				CustomAction = new ContextActionCompleteBounty
				{
					BountyId = "Nocticula_Palace_Peer"
				},
				PromptText = "(Isekai Protagonist) [Sovereign Peer] \"Lady Nocticula, I have crossed planes, broken Deskari's horde, and carved my own legend across the Worldwound. I come before you not as a mortal petitioner groveling for scraps, but as a sovereign peer to negotiate the future of the Midnight Isles.\"",
				NpcReplyText = "{n}Nocticula reclines languidly upon her throne of midnight silk, her eyes glowing with intoxicating amusement.{/n} \"An Otherworlder with the audacity to address me as an equal. How deliciously refreshing... Very well. Let us speak as sovereign peers.\"",
				Rewards = new SceneRewards
				{
					Gold = 15000,
					CosmicCoins = 400
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Lady of Shadows",
					Category = ConstellationCategory.Quest,
					SceneContext = "House of Silken Shadows: Sovereign Peer Audience",
					CosmicCoins = 400,
					Lines = new List<string> { "<color=#9400D3><b>[The Constellation 'Our Lady in Shadow']</b></color> purrs with dangerous amusement: \"Courage and audacity. You may yet survive my court, darling.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterBaphometColyphyrMine()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act4_Baphomet_ColyphyrConfrontation", "9ab9ad8f6e11d67499b48fd595e50972", "Act4", "Colyphyr Mine: Lord of the Labyrinth Baphomet's ambush");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Baphomet_Colyphyr_MartialGod_Strike",
				Type = OptionType.Subclass,
				RequiredProficiency = "MartialGodProficiencies",
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Martial God) \"A demon lord descended in person? Magnificent! Your labyrinths mean nothing to a blade that splits dimensions! Ready your glaive, goat-lord; let us see if your horns can withstand a martial god's zenith strike!\"",
				NpcReplyText = "{n}Baphomet barks with thunderous, discordant rage, his flaming glaive cleaving the mine air as demonic runes flare across his obsidian horns.{/n} \"Insolent wretch! I shall grind your bones into the crystal dust of Colyphyr!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 500
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Colyphyr Mine: Challenging Baphomet",
					CosmicCoins = 500,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars so loud the cavern shakes: \"A DEMON LORD ON THE ANVIL! HAMMER HIM INTO ASH!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
