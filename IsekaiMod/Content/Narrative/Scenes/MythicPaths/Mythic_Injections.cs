using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.MythicPaths
{
	public static class Mythic_Injections
	{
		public static void Register()
		{
			RegisterAeon();
			RegisterAngel();
			RegisterAzata();
			RegisterDemon();
			RegisterDevil();
			RegisterDragon();
			RegisterLegend();
			RegisterLich();
			RegisterLocust();
			RegisterTrickster();
		}

		private static void RegisterAeon()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Mythic_Aeon_CosmicBalance", "6cacc7252c640d4448e67eae0b6c74e8", "MythicPaths", "Mythic Aeon: The Cosmic Mirror of Law");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Mythic_Aeon_Universal_Causality",
				Type = OptionType.Mythic,
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(Isekai Protagonist) \"The Monad perceives time as a straight path, but someone who crossed the boundary of realities understands that causal threads can be rewoven without breaking the tapestry of cosmic law.\"",
				NpcReplyText = "{n}The swirling cosmic glass of the Aeon mirror stabilizes, reflecting countless alternate timelines resolving into pristine mathematical symmetry.{/n} \"The anomaly perceives the deeper order. The law is not static; it is eternal harmony in motion.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.MetaLoop,
					SceneContext = "Aeon Mirror: Cosmic Reweaving",
					CosmicCoins = 160,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> murmurs through the ether: \"Law and temporal infinity converge. The scales of the universe align with your will.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterAngel()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Mythic_Angel_InheritorCouncil", "f24aa3c4dd617c045a0a36d36402fda4", "MythicPaths", "Mythic Angel: Audience with the Inheritor");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Mythic_Angel_Universal_DivineEqual",
				Type = OptionType.Mythic,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"My light did not originate from your realm, Iomedae, but it burns for the exact same purpose: to protect the innocent from demonic slaughter. We stand as allies in this crusade.\"",
				NpcReplyText = "{n}The radiant goddess regards you with a warm, approving nod of divine respect.{/n} \"Well spoken, Commander. The origin of the flame matters less than the darkness it dispels. May Heaven's host march proudly at your side!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor (Parallel Echo)",
					Category = ConstellationCategory.Quest,
					SceneContext = "Angel Council: Divine Alliance",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> echoes with triumphant radiance: \"A sacred compact sealed between worlds! The light of salvation shall not be quenched!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterAzata()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Mythic_Azata_ElysianRebellion", "cc75f56d477a8db459f9f62dd1b9c854", "MythicPaths", "Mythic Azata: Elysian Rebellion");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Mythic_Azata_Universal_FreedomSong",
				Type = OptionType.Mythic,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"Elysium taught us that joy and friendship are weapons far sharper than demonic cruelty! We will liberate the Worldwound not with grim martyrdom, but with flowers, songs, and unyielding freedom!\"",
				NpcReplyText = "{n}A chorus of whimsical sprites, treants, and havoc dragons erupts into joyous, roaring celebration around you.{/n} \"Sing the song! Paint the desolate wasteland in colors the demons have never dreamed of!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Quest,
					SceneContext = "Azata Climax: The Joy of Elysium",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> dances among falling starlight: \"Rejoice, dreamers! Let your laughter blow away the ashes of war!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDemon()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Mythic_Demon_SovereignWrath", "b9be5aeb6d3e8914c8fa253f0da33ba9", "MythicPaths", "Mythic Demon: Abyssal Sovereignty");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Mythic_Demon_Universal_Overlord",
				Type = OptionType.Mythic,
				Alignment = AlignmentShiftDirection.ChaoticEvil,
				PromptText = "(Isekai Protagonist) \"You thought I would be another disposable demon lord dancing on your marionette strings, Nocticula? I crossed the cosmos to conquer, not to serve. The Abyss bows to me now!\"",
				NpcReplyText = "{n}Nocticula's seductive facade vanishes, replaced by a calculating, wary glare as your abyssal power shakes the foundations of the House of Silken Shadows.{/n} \"Such fury... such monstrous ambition. You are far more dangerous than any beast the Worldwound has ever birthed.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lady in Shadow",
					Category = ConstellationCategory.Quest,
					SceneContext = "Demon Climax: Defying the Queen",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#9400D3><b>[The Constellation 'Our Lady in Shadow']</b></color> watches with dangerous intrigue: \"A predator that refuses the lure. Let us see who claims the crown of the Abyss!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDevil()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Mythic_Devil_InfernalContract", "5a137c2c6e3794d4bacc0d6c35af493f", "MythicPaths", "Mythic Devil: Hell's Grand Edict");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Mythic_Devil_Universal_FinePrint",
				Type = OptionType.Mythic,
				Alignment = AlignmentShiftDirection.LawfulEvil,
				PromptText = "(Isekai Protagonist) \"Every paragraph of your infernal contract has been counter-annotated. Hell provides the legions and the administration, but the sovereign authority over Drezen remains exclusively mine.\"",
				NpcReplyText = "{n}The emissary of Hell smirks, adjusting his iron spectacles in quiet admiration.{/n} \"Ruthless, thorough, and perfectly legally airtight. Archduke Mephistopheles will be thoroughly impressed. The pact is sealed.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Devil Pact: Sovereign Jurisprudence",
					CosmicCoins = 175,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> marks the parchment with burning crimson ink: \"Superb legal mastery. Power without order is chaos; power bound by contract is absolute.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDragon()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Mythic_Dragon_GoldenMercy", "661cbed36d5072845b2749bf6e37cfd2", "MythicPaths", "Mythic Gold Dragon: Hal's Wisdom of Mercy");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Mythic_Dragon_Universal_Redemption",
				Type = OptionType.Mythic,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) \"True strength is not measured by how easily you can obliterate an enemy, Hal, but by having the patience to offer redemption to those who thought they were beyond saving.\"",
				NpcReplyText = "{n}Hal's golden draconian eyes gleam with deep, paternal pride as his scales shimmer in the sunlight.{/n} \"You understand the golden heart, little brother. May your wings cast warmth across this scarred world!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Dawnflower",
					Category = ConstellationCategory.Quest,
					SceneContext = "Gold Dragon: Golden Mercy",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> smiles with boundless warmth: \"Forgiveness is the highest mantle of divine grace. Fly high, child of the sun!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterLegend()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Mythic_Legend_MortalDefiance", "1f3039d4a7379de489cb73bf1a19a3da", "MythicPaths", "Mythic Legend: Reclaiming Mortal Sovereignty");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Mythic_Legend_Universal_HumanGrit",
				Type = OptionType.Mythic,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"Keep your demonic wounds and celestial blessings! I conquered my way across a foreign world with my own two hands, my own companions, and my own mortal will. I am the Legend!\"",
				NpcReplyText = "{n}The lingering mythic resonance shatters into brilliant, harmless sparks around you, leaving only pure, untamed mortal prowess.{/n} \"The cosmic shackles fall away! You stand unburdened and unbroken, a living testament to mortal greatness!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Lucky Drunk",
					Category = ConstellationCategory.Quest,
					SceneContext = "Legend Climax: Pure Mortal Will",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars with thunderous laughter: \"YES! Throw the gods' toys back in their faces! A true hero makes their own damn path! Drinks on me!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterLich()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Mythic_Lich_SovereignTomb", "e84d6d88226d4fa4cbcaf66400fd6873", "MythicPaths", "Mythic Lich: The Darkest Ritual");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Mythic_Lich_Universal_ColdOrder",
				Type = OptionType.Mythic,
				Alignment = AlignmentShiftDirection.NeutralEvil,
				PromptText = "(Isekai Protagonist) \"The flesh was transient and full of doubt. In the cold stillness of the grave, there is only absolute discipline, eternal endurance, and unbroken obedience.\"",
				NpcReplyText = "{n}Zacharius bows his ancient, skeletal skull in profound submission as your phylactery pulses with freezing, supreme death essence.{/n} \"It is complete, Lord of the Dead! Rise, Sovereign of the Ziggurat!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Pallid Princess",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Lich Climax: Sovereign of Bones",
					CosmicCoins = 180,
					Lines = new List<string> { "<color=#8B0000><b>[The Constellation 'The Pallid Princess']</b></color> sighs with opulent indulgence: \"Eternal life stripped of mortal frailty. Feast upon eternity, my champion!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterLocust()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Mythic_Locust_SwarmReborn", "3d7ab2836228a484aa4a33420eb40d06", "MythicPaths", "Mythic Swarm: The All-Consuming Hunger");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Mythic_Locust_Universal_ConsumeAll",
				Type = OptionType.Mythic,
				Alignment = AlignmentShiftDirection.ChaoticEvil,
				PromptText = "(Isekai Protagonist) \"No gods. No demons. No crusade. There is only the swarm, and the infinite hunger that will swallow this entire reality whole.\"",
				NpcReplyText = "{n}Millions of mandibles chitter in horrifying unison as your body dissolves into an endless, ravenous cloud of abyssal locusts.{/n} \"WE ARE THE SWARM. WE ARE ETERNAL HUNGER.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.MetaLoop,
					SceneContext = "Swarm Reborn: The End of All Worlds",
					CosmicCoins = 200,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses with cosmic detachment: \"A reality devoured down to the void. Yet even the swarm is but an infinitesimal cycle within the boundless infinite.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterTrickster()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Mythic_Trickster_FourthWallShatter", "3b6f57fe60593774f8ee505e540b7d07", "MythicPaths", "Mythic Trickster: Council of Absurdity");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Mythic_Trickster_Universal_NarrativePrank",
				Type = OptionType.Mythic,
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Isekai Protagonist) \"The base game's dramatic tension is entirely overrated. Why fight a grueling war when you can simply edit the script and turn Deskari's scythe into a rubber chicken?\"",
				NpcReplyText = "{n}The Trickster Council members erupt into thunderous applause, throwing confetti and magical dice across the table.{/n} \"BRILLIANT! Give that mortal a promotion! Rewriting the cosmos for the sake of a punchline!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Quest,
					SceneContext = "Trickster Council: Fourth-Wall Prank",
					CosmicCoins = 200,
					Lines = new List<string> { "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> rolls on the celestial floor laughing: \"Hahahaha! You broke the script! The serious paladins are crying into their holy water! Perfection!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
