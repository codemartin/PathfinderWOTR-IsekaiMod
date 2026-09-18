using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Dialogue;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.SpecialInteractions
{
	public static class ArtifactInteractions
	{
		public static void Register()
		{
			RegisterRadianceAwakening();
			RegisterFinneanAwakening();
			RegisterNenioNamelessRuinsRiddle();
		}

		private static void RegisterRadianceAwakening()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Artifact_Radiance_AwakeningAndTransmute", "d7669703a6d923d4e8b34bc56ec16c31", "SpecialInteractions", "Shield Maze: Awakening and shaping the holy blade Radiance");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Radiance_Awaken_Handwraps",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.LawfulGood,
				NextCueGuid = "6603e3274d42438faa38af673024a832",
				CustomAction = new ContextActionAwakenRadiance
				{
					InitialForm = RadianceForm.Handwraps
				},
				PromptText = "(Isekai Protagonist) [Shape Radiance: Sacred Handwraps] \"This holy steel need not remain a sword. Dissolve into sacred ki wraps around my fists!\"",
				NpcReplyText = "{n}The dormant blade dissolves into liquid celestial starlight, wrapping tightly around your knuckles in blazing golden cloth.{/n}",
				Rewards = new SceneRewards
				{
					CosmicCoins = 100
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Quest,
					SceneContext = "Radiance: Sacred Handwraps",
					CosmicCoins = 100,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars: \"FISTS OF HOLY STEEL! SMITE THE ABYSS WITH YOUR BARE HANDS!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Radiance_Awaken_Longbow",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.LawfulGood,
				NextCueGuid = "6603e3274d42438faa38af673024a832",
				CustomAction = new ContextActionAwakenRadiance
				{
					InitialForm = RadianceForm.Longbow
				},
				PromptText = "(Isekai Protagonist) [Shape Radiance: Composite Longbow] \"Melt the blade into a composite bow of radiant starlight! Let demons fear the heavens from afar!\"",
				NpcReplyText = "{n}The cold iron flexes and stretches, forming a graceful recurve longbow strung with pure celestial light.{/n}",
				Rewards = new SceneRewards
				{
					CosmicCoins = 100
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.Quest,
					SceneContext = "Radiance: Starlight Longbow",
					CosmicCoins = 100,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> smiles: \"Rain celestial retribution upon the demonic hordes from the clouds!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Radiance_Awaken_Greatsword",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.LawfulGood,
				NextCueGuid = "6603e3274d42438faa38af673024a832",
				CustomAction = new ContextActionAwakenRadiance
				{
					InitialForm = RadianceForm.Greatsword
				},
				PromptText = "(Isekai Protagonist) [Shape Radiance: Heavy Greatsword] \"Expand the sacred steel into a colossal two-handed greatsword! Cleave through demon hordes!\"",
				NpcReplyText = "{n}The blade expands with deafening resonance, heavy crossguards ringing as a massive greatsword hums in your hands.{/n}",
				Rewards = new SceneRewards
				{
					CosmicCoins = 100
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Quest,
					SceneContext = "Radiance: Colossal Greatsword",
					CosmicCoins = 100,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> laughs in battle thunder: \"NOW THAT IS A PROPER TWO-HANDED CLEAVER! BREAK THEIR BONES!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterFinneanAwakening()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Artifact_Finnean_DimensionalAwakening", "615ef80243cfc184ba42286395880b0e", "SpecialInteractions", "Ancient Crypt: Awakening Finnean the Talking Weapon");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Finnean_Awaken_Universal_TalkingWeaponTrope",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"A sentient talking weapon trapped in a forgotten tomb? Every legendary Otherworld hero needs a witty, shape-shifting blade companion! Join my retinue, Finnean, and let us make history!\"",
				NpcReplyText = "{n}The spectral blade glows with warm, vibrant amber luminescence, trembling in sheer delight.{/n} \"A legendary hero's partner?! Haha! Splendid! I've been waiting decades for someone with an eye for quality! Point me at a demon, partner!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 150
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Quest,
					SceneContext = "Ancient Crypt: Finnean Retinue Partner",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs heartily: \"A talking sword who likes a good scrap! Keep him polished and keep him swinging!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "IsekaiDialogueFinnean",
				Type = OptionType.Universal,
				ShowOnce = true,
				SpeakerGuid = "ea8034769ab7d584e97b5227cbc03296",
				PromptText = "(Isekai Protagonist) [Appraisal of the Sentient Blade] \"A sentient, self-aware soul woven into an ever-shifting polymorphic armament? In the chronicles of my previous world, an ego weapon of your caliber is an enchanted legendary relic. Can you alter your structure into other exotic martial forms?\"",
				NpcReplyText = "{n}The blade's crystalline surface pulses with iridescent starlight, sending a pleasant, tingling warmth through your fingers and palm.{/n} \"Exotic forms? Ha! You have no idea, partner! In life I was a seasoned adventurer, but bound inside this steel, I can feel every contour of my essence! Longswords, composite bows, curved scythes, heavy flails... you name the shape, and I will mold my soul into whatever edge keeps us alive out there!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 400,
					Buff = IsekaiFinnean.FinneanAwakenedEdgeBuff
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Hermit",
					Category = ConstellationCategory.Quest,
					SceneContext = "Ancient Crypt: Sentient Polymorphic Blade",
					CosmicCoins = 400,
					Lines = new List<string> { "[The Hermit] whispers: \"A soul sealed within an ever-shifting blade. Fascinating metamorphic soul mechanics.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterNenioNamelessRuinsRiddle()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Artifact_Nenio_NamelessRuinsRiddle", "9b38499627b7eda4f86d959f3f222834", "SpecialInteractions", "Nameless Ruins: Nenio's Kitsune statue cosmological riddle");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Nenio_Riddle_Universal_MultiverseAnswer",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"Nenio, your riddle assumes a closed cosmological system. In quantum multiversal physics, the question resolves itself: the observer determines the reality of the statue. Rotate the four dials simultaneously to bypass the paradox!\"",
				NpcReplyText = "{n}Nenio's tail stands on end, her glasses sliding down her snout as her quills furiously scribble across her parchment.{/n} \"BRILLIANT! A quantum multiversal resolution! The statistical probability was merely 0.003%, yet it unlocks the mechanism completely!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 200
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.Quest,
					SceneContext = "Nameless Ruins: Quantum Multiverse Riddle",
					CosmicCoins = 200,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> whispers through the stone: \"The observer shatters the labyrinth with a single thought. Proceed, scholar.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
