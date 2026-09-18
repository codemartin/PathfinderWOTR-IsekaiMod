using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.Companions
{
	public static class Companion_RetinueCampBanter
	{
		public static void Register()
		{
			RegisterSeelahCampBanter();
			RegisterCamelliaCampBanter();
			RegisterLannCampBanter();
			RegisterWenduagCampBanter();
			RegisterWoljifCampBanter();
			RegisterDaeranCampBanter();
			RegisterEmberCampBanter();
			RegisterNenioCampBanter();
			RegisterSosielCampBanter();
			RegisterRegillCampBanter();
		}

		private static void RegisterSeelahCampBanter()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Seelah_CampHub", "417fa384f3250634bb71859fbc913453", "Companions", "Crusader Camp: Sharing Otherworld festivals and street food with Seelah");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Seelah_Camp_Universal_StreetFood",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) [Otherworld Festivals & Street Food] \"Seelah, back in my world, we had midsummer festivals where folks dressed in full armor just to eat greasy fried street snacks, drink icy cider, and watch fireworks light up the night sky.\"",
				NpcReplyText = "{n}Seelah's eyes light up with unvarnished joy, a wide, radiant grin spreading across her face as she leans forward with hearty laughter.{/n} \"Fried street snacks and sky-fire?! Now that sounds like my kind of holiday! Here I thought your realm was all metal carriages and solemn glass towers! Promise me this, Commander: once we kick the demons back into the Abyss and reclaim Drezen, you're teaching the citadel cooks how to make those fried treats!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Seelah Street Food",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Fried food and festival fireworks! Cayden approves this doctrine with flying colors!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterCamelliaCampBanter()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Camellia_CampHub", "9497b573bde03724c8e08090509a1774", "Companions", "Crusader Camp: Inquiring into Lady Camellia's peculiar shamanic talisman");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Camellia_Camp_Universal_CursedRelics",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) [Otherworlder's Appraisal: Cursed Relics] \"You keep caressing that necklace, Lady Camellia. In my world, antique jewelry that radiates spiritual static usually comes with an exorcism hotline.\"",
				NpcReplyText = "{n}Camellia's aristocratic smile freezes for a fraction of a second, her fingers lingering over the polished stone with delicate, practiced elegance.{/n} \"How remarkably fanciful. An 'exorcism hotline'? My spirits require no such vulgar meddling, stranger. They are refined, ancient, and quite... particular. Do mind your own curiosities, lest you uncover matters far beyond your delicate sensibilities.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 150
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Camellia Shamanic Appraisal",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> purrs: \"She guards her little secrets with exquisite aristocratic venom. Delightful.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterLannCampBanter()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Lann_CampHub", "66385ad77fa743e4bb1234078dbd804c", "Companions", "Crusader Camp: Appreciating Lann's deadpan mongrel humor");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Lann_Camp_Universal_DeadpanComedy",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) [Deadpan Appreciation] \"Lann, your dry humor would kill at an Otherworld comedy cellar. We call that 'deadpan self-deprecating satire'--people pay good money for sets like yours.\"",
				NpcReplyText = "{n}Lann blinks in mock astonishment, carefully checking both hands before letting out a dry, raspy chuckle.{/n} \"People pay money for someone complaining about their awful lifespan and scaly skin? Truly, your world is a paradise of questionable tastes. Maybe I picked the wrong profession--Chief Executive Stand-Up Mongrel has a nice ring to it.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Lann Deadpan Stand-Up",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs heartily: \"The mongrel's comedic timing is second to none! Five stars!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Lann_Camp_TwinAvatar_Doppelganger",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) [Twin Avatar] \"Lann, having a twin avatar means I can be in two places at once. Useful for scouting and crusade logistics.\"",
				NpcReplyText = "{n}Lann blinks, rubs his mismatched eyes, and lets out a long, incredulous whistle.{/n} \"Two Commanders?! Oh, the council meetings are going to be legendary! You can send your twin to listen to the diplomats whine about supply wagons while the real you sneaks out back with us to shoot demons! Just... do me a favor. If one of you turns evil and puts on an evil goatee, warn me ahead of time so I know which one to shoot at!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Lann Twin Avatar Doppelganger",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"An evil goatee! The classic multidimensional trope! Cayden approves!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Lann_Camp_Universal_WenduagTruce",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) [Mongrel Coexistence] \"Lann, how are you and Wenduag managing in the same vanguard? Most uplanders assumed you two would have settled your feud with daggers by now.\"",
				NpcReplyText = "{n}Lann chuckles softly, testing the tension on his bowstring with a wry grin.{/n} \"To be honest, Chief, I sleep with one eye open and an arrow under my bedroll. But Wenduag knows you hold the leash. In the caves, we fought over scraps of dead rats; up here, you've got us liberating cities and breaking demon sieges. Even a ruthless predator like her recognizes that staying in your vanguard is the smartest move in northern Avistan. Just... don't seat us together at banquet dinners, alright?\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Lann on Wenduag Truce",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Keeping two rival hunters from tearing each other's throats out! That's true party leadership!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterWenduagCampBanter()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Wenduag_CampHub", "ced27e744d2dded40bbb5adf17816dbb", "Companions", "Crusader Camp: Discussing predator tactics and nocturnal archery with Wenduag");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Wenduag_Camp_Universal_PredatorTactics",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralEvil,
				PromptText = "(Isekai Protagonist) [Predator Doctrine] \"Wenduag, in my world, snipers don't shoot from open battlements. They conceal their heat signatures, calculate wind trajectories, and drop high-value targets from a mile away. You have the instincts of an elite special forces sniper.\"",
				NpcReplyText = "{n}Wenduag's eyes gleam with lethal fascination, running a curved claw along the wood of her shortbow.{/n} \"Striking from a mile away without the prey ever sensing the wind? Teach me those calculations, master. Every demon officer in the Worldwound shall fall before my arrow.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 200
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Wenduag Sniper Doctrine",
					CosmicCoins = 200,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts in grim satisfaction: \"SILENT KILLERS SHARPENING THEIR ARROWS. THE HUNT BEGINS!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Wenduag_Camp_Universal_LannTruce",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralEvil,
				PromptText = "(Isekai Protagonist) [Mongrel Truce] \"Wenduag, you haven't put an arrow through Lann's neck yet. How does it feel marching beside your old rival beneath surface skies?\"",
				NpcReplyText = "{n}Wenduag curls a claw slowly along her cheek, her yellow eyes gleaming with cold appraisal.{/n} \"Lann is a sentimental fool, master. But under your command, his bow hits demon throats instead of crying about mongrel curses. You gave us both an army and prey worthy of our lineage. So long as he draws his bowstring and stays out of my line of fire, I have no reason to slaughter your archers. But should he ever hesitate... I will finish his miserable life before you can blink.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Lady of Shadows",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Wenduag on Lann Truce",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#9400D3><b>[The Constellation 'Our Lady in Shadow']</b></color> purrs softly: \"Predators respecting the stronger hand at the helm. Delicious obedience.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterWoljifCampBanter()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Woljif_CampHub", "e41585da330233143b34ef64d7d62d69", "Companions", "Crusader Camp: Comparing street urchin hustles and corporate scams with Woljif");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Woljif_Camp_Universal_StreetHustles",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Isekai Protagonist) [Street Economics] \"Woljif, picking pockets in tavern alleys is amateur league. Where I come from, master rogues wear three-piece suits and run 'pyramid schemes' that fleece entire kingdoms without touching a single blade.\"",
				NpcReplyText = "{n}Woljif's jaw drops, his tail twitching excitedly as he scrambles closer with eyes wide as saucers.{/n} \"Wait, wait! You can steal entire kingdoms without getting your hands dirty?! Chief, you HAVE to teach me this 'pyramid' magic! We're gonna be filthy rich!\"",
				Rewards = new SceneRewards
				{
					Gold = 1000,
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Woljif Pyramid Schemes",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Do not corrupt the tiefling youth with corporate capitalism! It is too lethal!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Woljif_Camp_Universal_ModernBanking",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Isekai Protagonist) [Fractional Reserve Banking] \"Woljif, why steal pouches of coins from merchants when you can start a merchant bank? You take deposits, lend out ten times what you hold in the vault, and earn interest on money that doesn't even exist.\"",
				NpcReplyText = "{n}Woljif stares at you with absolute awe, his hands gripping his daggers tightly before slowly dropping them in disbelief.{/n} \"Lend... lend money that DOESN'T EXIST?! And people PAY you for it?! Chief, that ain't thievery... that's the darkest, sweetest sorcery I ever heard of! We gotta draft a charter before the Thieflings find out!\"",
				Rewards = new SceneRewards
				{
					Gold = 1500,
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Woljif Modern Banking",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with cold amusement: \"Fractional reserve debt mechanics. Truly, mortal greed achieves diabolical heights without even needing Hell's counsel.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDaeranCampBanter()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Daeran_CampHub", "4d978cbd2aa780d46874255282039f3f", "Companions", "Crusader Camp: Trading salon gossip and reality television tropes with Count Daeran");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Daeran_Camp_Universal_RealityTV",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Isekai Protagonist) [High Society Satire] \"Daeran, your scandalous escapades would make you the crown jewel of an Otherworld reality TV show. We have entire broadcasting networks dedicated to rich nobles throwing champagne in each other's faces.\"",
				NpcReplyText = "{n}Daeran throws his head back and laughs with genuine, sparkling delight, waving his crystal glass with exquisite flourish.{/n} \"Broadcasting my daily debaucheries to millions of eager spectators?! Oh, Commander, your world possesses terrifying genius! I should demand royalties immediately!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Daeran Reality TV",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> snickers: \"A reality show following Count Daeran around would top the charts across seven planes!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Daeran_Camp_TwinAvatar_Narcissism",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Isekai Protagonist) [Twin Avatar] \"Having an exact duplicate body comes in handy, Daeran. Double the productivity, double the presence.\"",
				NpcReplyText = "{n}Daeran swirls his wine glass with an arch, decadent smirk, his eyes dancing with mischief.{/n} \"Productivity? How dreadfully mundane! With two bodies, my dear Commander, you could attend two separate balls at once, double your romantic scandals, or simply spend all afternoon admiring your own perfection in an oversized mirror! The narcissistic possibilities are virtually limitless!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Savored Sting",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Daeran Twin Avatar Narcissism",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with wicked delight: \"Two bodies for double the seduction and double the mischief. Now that is divine inspiration, darling!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterEmberCampBanter()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Ember_CampHub", "f2a35965e9bc601449498bd022b04d9d", "Companions", "Crusader Camp: Listening to Ember's sermon of empathy and redemption");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Ember_Camp_Universal_GentleWarmth",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) [Universal Kindness] \"Ember, people in this world call you mad because their hearts are calcified by fear and war. But in my world, we learned that compassion is the only fire that can truly burn away darkness. Keep preaching, little one. I will protect you.\"",
				NpcReplyText = "{n}Ember smiles with innocent, luminous joy, her scarred fingers gently touching the hem of your cloak.{/n} \"You see it too, don't you? People aren't bad; they're just cold and frightened. When the campfire burns warm, nobody needs to hurt each other anymore.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 300
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Ember Gentle Kindness",
					CosmicCoins = 300,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> whispers with starry tenderness: \"A fragile candle lighting the deepest abyss. Bless you for guarding her flame.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Ember_Camp_Summoner_EvolvedBeast",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) [Summoner Evolution] \"Careful around my summoned beast, Ember. Its planar evolutions make it quite intimidating.\"",
				NpcReplyText = "{n}Ember gently extends a slender, scarred hand toward the terrifying planar creature, offering it a piece of dried crust with a gentle, disarming smile.{/n} \"It's not scary at all. It just looks fierce because the cold world was mean to it. Don't worry, friend. You don't have to hurt anyone unless you're protecting someone you love. You're safe here by the fire.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 300
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Ember Summoner Evolved Beast",
					CosmicCoins = 300,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> whispers with starry tenderness: \"A child comforting a monster born of planar storms. Even the fiercest beast bows to pure love.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Ember_Camp_Universal_LostHomeComfort",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) [Comfort Food and Warm Hearth] \"Ember, you've spent too many years sleeping on cold Kenabres flagstones. In my home world, we had warm fleece blankets, hot cocoa, and soup that didn't taste like swamp moss. Come sit by the stove; you never have to shiver in the rain again.\"",
				NpcReplyText = "{n}Ember's eyes fill with gentle tears as she clutches the warm bowl in both hands, sniffing the savory broth.{/n} \"It smells like... like a home that doesn't have broken walls. Thank you. Even when the cold wind blows from the wound, this fire feels like the whole world is giving everyone a hug.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 300
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Ember Lost Home Comfort",
					CosmicCoins = 300,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> whispers softly: \"A warm hearth in a broken world. May peace cradle this child's spirit tonight.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterNenioCampBanter()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Nenio_CampHub", "9b38499627b7eda4f86d959f3f222834", "Companions", "Crusader Camp: Assisting Nenio with her encyclopedia of Otherworld science");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Nenio_Camp_Universal_EncyclopediaScience",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) [Empirical Method] \"Nenio, if you want your encyclopedia to survive the centuries, you need the scientific method: empirical hypothesis, control groups, and reproducible peer review.\"",
				NpcReplyText = "{n}Nenio's tail wags with violent, ecstatic enthusiasm as ink spatters wildly across her notebook.{/n} \"FASCINATING! 'Reproducible peer review'! I must immediately establish a control group of crusaders and observe how long they survive without eating onions!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Nenio Scientific Method",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> watches with quiet cosmic amusement: \"The scholar records the grains of sand on the cosmic shore. Let her write.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Nenio_Camp_TwinAvatar_Ontology",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) [Twin Avatar] \"Nenio, my twin avatar shares my consciousness and sensory inputs through planar entanglement.\"",
				NpcReplyText = "{n}Nenio gasps, dropping her quill and thrusting a magnifying glass inches from your nose, then immediately sprinting to examine your twin.{/n} \"SENSORY ENTANGLEMENT?! INCREDIBLE! If I pinch the Commander on the left arm, does the twin on the right flinch?! Quick, Commander A, drink this concentrated squid ink so I can observe whether Commander B's tongue turns purple! For science!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Nenio Twin Avatar Sensory Entanglement",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> watches with quiet cosmic amusement: \"Two eyes looking through one aperture. The fox grasps the edge of non-Euclidean duality.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Nenio_Camp_Universal_PhysicsInquisition",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) [Thermodynamics and Entropy] \"Nenio, have you considered that magic might simply be an external force acting upon standard laws of thermodynamics? Energy cannot be created or destroyed; it merely changes state across planar barriers.\"",
				NpcReplyText = "{n}Nenio's ears twitch frantically as she violently scribbles across three blank pages, muttering formulas at deafening speed.{/n} \"THERMODYNAMIC CONSERVATION ACROSS PLANAR MEMBRANES! BOY! If energy is conserved, then fireball spells are merely localized thermodynamic phase transitions! I must calculate the exact entropy loss of a dretche's vaporization!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Nenio Thermodynamics Inquisition",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> watches with quiet cosmic amusement: \"The fox probes the fabric of entropy. Let the equations spiral into eternity.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterSosielCampBanter()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Sosiel_CampHub", "129b55b8b5d50974f84f7c607d894fd0", "Companions", "Crusader Camp: Discussing artistic expression and watercolor painting with Sosiel");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Sosiel_Camp_Universal_WatercolorArt",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) [Impressionism & Perspective] \"Sosiel, your portraits capture divine grace, but have you ever tried 'impressionism'? Blurring the harsh edges to let the warm sunlight and raw emotion take over the canvas?\"",
				NpcReplyText = "{n}Sosiel steps back from his easel, his eyes widening in artistic epiphany as he studies his mixing palette.{/n} \"Blurring the edges to let the emotional truth emerge... Commander, that is profound! Shelyn herself must have guided your hand to this realization!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Dawnflower",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Sosiel Impressionism",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> smiles: \"Let your art bring warmth to the frozen corners of the Worldwound.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterRegillCampBanter()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Regill_CampHub", "2366a8db6481070439fee222c0c52e45", "Companions", "Crusader Camp: Reviewing supply chain logistics and drill formations with Regill");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Regill_Camp_Universal_SupplyLogistics",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(Isekai Protagonist) [Modern Military Supply Chains] \"Regill, standard crusader baggage trains lose 18% of rations to spoilage. If we implement modular palletized containers and cold-storage seals, we can extend our march radius by fifty leagues.\"",
				NpcReplyText = "{n}Regill sets down his tactical map, fixing you with a stare of intense, cold calculation.{/n} \"A quantifiable efficiency gain of eighteen percent. Remarkable. Implement this container protocol immediately across all fifth crusade units, Commander. Victory is forged in precision.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Father of Creation",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Regill Logistics Optimization",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#D2691E><b>[The Constellation 'Father of Creation']</b></color> nods in deep approval: \"Order, efficiency, and iron discipline. An army that heeds supply shall conquer any foe.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Regill_Camp_TwinAvatar_TacticalRedundancy",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(Isekai Protagonist) [Twin Avatar] \"Regill, having a twin avatar eliminates single-point-of-failure risks in our command structure.\"",
				NpcReplyText = "{n}Regill nods once with rigid, uncompromising approval, his gauntleted hand resting firmly upon his hooked hammer.{/n} \"Tactical redundancy. If an abyssal assassin strikes down one Commander on the front line, the secondary avatar maintains operational continuity and counterattacks immediately. An acceptable protocol. Ensure both avatars are equipped with identical heraldry and standard gear to prevent enemy scouts from determining the primary unit.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Father of Creation",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Regill Twin Avatar Tactical Redundancy",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#D2691E><b>[The Constellation 'Father of Creation']</b></color> nods in deep approval: \"A backup anvil for when the hammer strikes true. Iron discipline and sound strategy.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Regill_Camp_Universal_ModernArtillery",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(Isekai Protagonist) [Combined Arms and Artillery] \"Regill, line infantries and mounted cavaliers are vulnerable to massed demon charges. What we need is concentrated artillery barrages and trench-line interlocking fields of fire before the front ranks ever clash.\"",
				NpcReplyText = "{n}Regill's stern expression tightens into a nod of profound military respect.{/n} \"Interlocking fields of fire combined with preliminary siege barrages to shatter demon shock formations before engagement. Ruthless. Efficient. A doctrine worthy of the Hellknights. I will draft drilling manual amendments for the Godclaw contingent tonight.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Companion,
					SceneContext = "Crusader Camp: Regill Combined Arms Artillery",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with bloodthirsty approval: \"MASS CANNONS AND IRON BARRAGES! SHATTER THEIR LINES INTO RED MUD!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
