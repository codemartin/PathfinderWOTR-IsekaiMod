using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.Romance
{
	public static class Romance_Scenes
	{
		public static void Register()
		{
			RegisterArueshalaeRomance();
			RegisterDaeranRomance();
			RegisterWenduagRomance();
			RegisterLannRomance();
			RegisterSosielRomance();
			RegisterGalfreyRomance();
			RegisterCamelliaRomance();
			RegisterUlbrigRomance();
		}

		private static void RegisterArueshalaeRomance()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Romance_Arueshalae_ExileSoul", "03ebad9587cbea0438d901a0f8df44f1", "Romance", "Arueshalae Romance: Shared existential isolation of two exiles");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Romance_Arueshalae_Universal_IntruderInReality",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"Arueshalae... have you ever felt like you're an intruder in a world you weren't born into? Sometimes I look at the stars and dread that one morning I'll wake up back where I came from, and all of this, including us, will just vanish.\"",
				NpcReplyText = "{n}Arueshalae looks at you with wide, luminous eyes filled with profound understanding. She gently takes your hand, her fingers trembling slightly.{/n} \"Every single day of my life, my love. For centuries I was a creature of the Abyss, a monster woven from sin and agony. Desna touched my soul and tore me from the only existence I ever knew, casting me into a realm of light and guilt where I never felt I belonged. I used to wake up every night terrified that Elysium was an illusion and that I would awaken back in the blood-soaked mud of the Rasping Rifts.\"\n\n{n}She draws your hand to her chest, where you can feel the steady, warm beat of her heart.{/n} \"If you are from beyond the stars, then you and I are both exiles who found each other in the dark. If that distant sky ever calls you back... promise me you won't let go without taking me with you. Because wherever you are, that is the only world I ever want to belong to.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Companion,
					SceneContext = "Romance: Arueshalae Starlight Promise",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles with celestial tears: \"Two wandering souls finding home in each other across the cosmic expanse. Hold each other tight.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDaeranRomance()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Romance_Daeran_MasksAndVulnerability", "6aed9713cabd64d45baf24b0d193964d", "Romance", "Daeran Romance: Dropping the cynical aristocrat mask");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Romance_Daeran_Universal_NoAudience",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Isekai Protagonist) \"You don't have to perform for me, Daeran. No sarcastic barbs, no lavish theatrical scandals. I came from an entire world of people wearing artificial masks just to survive the day. When you're with me, you can just exist.\"",
				NpcReplyText = "{n}Daeran freezes, his wine goblet halting halfway to his lips. For a fleeting instant, his sardonic smirk completely evaporates, revealing the exhausted, terrified youth beneath the grand facade.{/n} \"Cruel of you, Commander. Taking away a man's shields and leaving him utterly defenseless before your gaze... But fine. No audience. Just you and me in the quiet of midnight.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Companion,
					SceneContext = "Romance: Daeran Quiet Midnight",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> nods quietly: \"Every tavern joker has a heavy heart when the lanterns burn down. Treat him well, kid.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterWenduagRomance()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Romance_Wenduag_EqualAlliance", "866b5be09c0c0f24facfd183b1905dbe", "Romance", "Wenduag Romance: Equal partnership over brutal subservience");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Romance_Wenduag_Universal_NoMasterAndSlave",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralEvil,
				PromptText = "(Isekai Protagonist) \"Wenduag, stop measuring our bond by masters and slaves. In my old life, power wasn't about bowing to the strongest beast in the cave; it was about two predators standing back to back against the universe. I don't want your submission. I want you at my side.\"",
				NpcReplyText = "{n}Wenduag's breath hitches, her spider limbs trembling against the stone. She presses her forehead against your chest, her claws digging gently into your tunic.{/n} \"Back to back... Two predators hunting the gods themselves. I would tear the stars from the sky if they dared try to take you away from me, my love.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Companion,
					SceneContext = "Romance: Wenduag Back to Back",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts in primal satisfaction: \"TWO WOLVES HUNTING TOGETHER! A COVENANT OF STEEL AND BLOOD!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterLannRomance()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Romance_Lann_ShortLivesAndForever", "104b14cde71103c4799361e7d6313f4b", "Romance", "Lann Romance: Living thirty years with the intensity of an eternity");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Romance_Lann_Universal_ThirtyYearsEternity",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"You keep worrying about your thirty-year lifespan, Lann. But where I come from, people waste decades waiting for life to begin. If we only have thirty years together under these stars, then every single day will outshine a thousand years of boring eternity.\"",
				NpcReplyText = "{n}Lann rubs the back of his neck, his face turning an endearing shade of crimson behind his scales. He pulls you into an awkward, fiercely protective embrace.{/n} \"Well, damn. You really know how to make a mongrel feel like the center of the cosmos. Thirty years with you... I'll take that deal in a heartbeat.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.Companion,
					SceneContext = "Romance: Lann Thirty Year Covenant",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> smiles warmly: \"True devotion is measured not in centuries, but in the unyielding fire of each shared dawn.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterSosielRomance()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Romance_Sosiel_PaintingTheOtherworld", "f8951566c8174d3fa34856d6c1d582b6", "Romance", "Sosiel Romance: Painting the skyline of another world");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Romance_Sosiel_Universal_SkylinesOfHome",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) \"Sosiel, your paintings capture the beauty of Golarion, but someday I want to show you the skylines of my old home--towers of glass and electric lights stretching into the clouds. Until then, you are my home in this realm.\"",
				NpcReplyText = "{n}Sosiel's eyes shine with profound artistic reverence. He gently clasps both your hands in his paint-flecked fingers.{/n} \"Towers of glass touching the stars... I would cross every plane in existence just to see that sky with you, Commander. As long as you are with me, my brush will never know despair.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Dawnflower",
					Category = ConstellationCategory.Companion,
					SceneContext = "Romance: Sosiel Glass Skylines",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> shines with gentle blessing: \"May your love paint hope upon every canvas of reality.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterGalfreyRomance()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Romance_Galfrey_SovereignAndMortal", "703c804457a49d74ca0e68966321b11b", "Romance", "Queen Galfrey Romance: Shedding the crown of Mendev for true love");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Romance_Galfrey_Universal_BeyondTheCrown",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"Galfrey... for a hundred years you have been the Queen of Mendev, the living symbol of an endless holy war. But to me, you are not a marble statue on a pedestal. You are Galfrey--a woman I love, who deserves peace and joy in her own right.\"",
				NpcReplyText = "{n}A soft tear slips past Galfrey's lashes. She removes her golden circlet, setting it upon the pavilion table, and steps into your arms with a shuddering sigh of profound release.{/n} \"For a century, no one saw the woman behind the crown. Only you... my strange, wondrous Otherworlder. With you, I am finally free to just love.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 500
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.Companion,
					SceneContext = "Romance: Galfrey Setting Aside the Crown",
					CosmicCoins = 500,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> watches with quiet maternal blessing: \"A century of duty rewarded by true affection. Walk in peace, my daughter.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterCamelliaRomance()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Romance_Camellia_DarkUnderstanding", "d5bccc3abc7a9ba4ebbf712c19df3fa0", "Romance", "Camellia Romance: Mutual understanding without moral hypocrisy");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Romance_Camellia_Universal_NoHypocrisy",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticEvil,
				PromptText = "(Isekai Protagonist) \"I'm not going to preach virtue to you, Camellia. Where I come from, humanity's darkest impulses are dissected under fluorescent lights. You are lethal, monstrous, and utterly captivating. And you're mine.\"",
				NpcReplyText = "{n}Camellia shivers with delicious, decadent ecstasy, pressing her lips against your neck with intoxicating fervor.{/n} \"No sermons... no righteous tears... You understand me better than anyone ever has! We shall paint this dying world in crimson together!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 300
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Companion,
					SceneContext = "Romance: Camellia Dark Understanding",
					CosmicCoins = 300,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with icy refinement: \"An alliance based on mutual acceptance of depravity. Truly, a match forged in midnight.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterUlbrigRomance()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Romance_Ulbrig_AncientWingsAndStarTraveler", "41bb06e3822f43218b8c9ca6dc47057b", "Romance", "Ulbrig Romance: The griffon chief and the star traveler");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Romance_Ulbrig_Universal_AncientWings",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) \"Ulbrig, you woke up a century after your civilization fell; I woke up a universe away from where I was born. Two ghosts from extinct realities who found warmth under the same sky.\"",
				NpcReplyText = "{n}Ulbrig folds his massive feathered wings around you in a fierce, protective embrace that blocks out the cold mountain winds.{/n} \"Then we make our own clan, traveler! With your strange star-wisdom and my claws, no ghost shall ever take what is ours!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 300
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Dawnflower",
					Category = ConstellationCategory.Companion,
					SceneContext = "Romance: Ulbrig Ancient Wings",
					CosmicCoins = 300,
					Lines = new List<string> { "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> smiles with radiant warmth: \"May the warmth of your hearth outlive every winter of the Worldwound.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
