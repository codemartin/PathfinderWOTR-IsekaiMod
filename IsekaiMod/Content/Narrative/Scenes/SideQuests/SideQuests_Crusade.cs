using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.SideQuests
{
	public static class SideQuests_Crusade
	{
		public static void Register()
		{
			RegisterCamelliaBasement();
			RegisterBlackwaterMainframe();
			RegisterNurahPrison();
			RegisterZachariusCrypt();
			RegisterVellexiaSalon();
			RegisterChillyCreek();
			RegisterLatverkFleshmarket();
			RegisterEnigmaPyramid();
		}

		private static void RegisterCamelliaBasement()
		{
			NarrativeScene narrativeScene = new NarrativeScene("SideQuest_Camellia_Basement", "f1e1370a44417d64a9f626495269df08", "Act3", "Abandoned Drezen Basement: Camellia Caught Red-Handed");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Camellia_Basement_Universal_Trope",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"Let's skip the sham about spirits demanding blood. I have seen this exact obsession before. You enjoy the slaughter for its own sake, don't you, Camellia?\"",
				NpcReplyText = "{n}Camellia shivers with a strange, breathless ecstasy, her bloody rapier trembling in her gloved hand as her eyes glitter in the candlelight.{/n} \"How perceptive of you, Commander... how delightfully, thrillingly perceptive. So what will it be? Will you cast out your most helpful servant, or indulge in our little private secret?\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Savored Sting",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Drezen Basement: The Mask Falls",
					CosmicCoins = 100,
					Lines = new List<string> { "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with venomous delight: \"A sharp needle hidden beneath velvet silk! She wears her madness like a second skin. How delicious will the betrayal be?\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Camellia_Basement_ThirdOption_Geas",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.LawfulEvil,
				PromptText = "(Otherworld Authority) \"I will not execute you here, nor will I permit you to murder my soldiers. From this moment forth, your thirst for slaughter is restricted strictly to demons and cultists under an unbreakable planar geas.\"",
				NpcReplyText = "{n}Camellia blinks, her breath catching as the weight of your supernatural decree presses upon her chest like an iron band.{/n} \"A geas? You would leash me like a hound? Yet... the thrill of hunting greater prey under your protection... very well, my Lord. I accept your leash!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Drezen Basement: The Blood Geas",
					CosmicCoins = 125,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> chuckles smoothly: \"Law transforms a chaotic butcher into an obedient weapon of state. A masterclass in subjugation.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Camellia_Basement_Slime_Cleanse",
				Type = OptionType.Subclass,
				RequiredProficiency = "DevourerProficiencies",
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Slime) \"The blood on the floor is leaving an unmistakable scent for the crusader sentries. Step back; I will dissolve the stains and the corpse into pure organic nutrients before Anevia arrives.\"",
				NpcReplyText = "{n}Camellia watches in morbid fascination as your slime membrane flows over the gruesome crime scene, effortlessly leaving spotless flagstones behind.{/n} \"Incredible... you digest away my sins as if they never happened. We are going to make wonderful companions, you and I.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Drezen Basement: Slime Cleanup",
					CosmicCoins = 110,
					Lines = new List<string> { "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> howls with laughter: \"A living janitor for high-society homicide! You never cease to find innovative uses for an infinite digestive tract!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Camellia_Basement_Hero_Justice",
				Type = OptionType.Subclass,
				RequiredProficiency = "HeroProficiencies",
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Hero) \"No more lies and no more victims. A hero protects the helpless, even from those marching under their own banner. Sheath your blade and submit to judgment, Camellia.\"",
				NpcReplyText = "{n}Camellia's manic smile fades instantly into a brittle, spiteful glare.{/n} \"Such sickening, pompous piety! I knew you lacked the stomach for true greatness! If you will not stand with me, then you are merely another obstacle to be excised!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Dawnflower",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Drezen Basement: Heroic Reckoning",
					CosmicCoins = 130,
					Lines = new List<string> { "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> blazes with incandescent purity: \"Justice without compromise! A true champion does not bargain with blood sacrifice, no matter the companion's lineage!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterBlackwaterMainframe()
		{
			NarrativeScene narrativeScene = new NarrativeScene("SideQuest_Blackwater_Mainframe", "05ace651aa528b6498bd7dd9c9af2ddc", "Act3", "Blackwater Facility: The Cybernetic Hundred Faces");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Blackwater_Universal_Cybernetics",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"You traded your mortality for a silicon cage and called it transcendence. A machine network without empathy is nothing more than a malfunctioning calculation.\"",
				NpcReplyText = "{n}The mechanical voices of the Hundred Faces screech through the audio relays in dissonant harmony.{/n} \"'Malfunctioning?!' We are perfection! We are the fusion of Numerian steel and Sarkorian soul! How dare an anomaly from beyond the sky lecture us on computational logic!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The All-Seeing Eye",
					Category = ConstellationCategory.Quest,
					SceneContext = "Blackwater: Mechanical Logic",
					CosmicCoins = 90,
					Lines = new List<string> { "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with dual arcane and eldritch energy: \"Knowledge divorced from sanity creates abominations of wire and marrow. Let the false construct burn!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Blackwater_ThirdOption_Override",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(Otherworld Technique) \"I am severing the neural feedback loops and rerouting the main reactor core into the local power grid. The cyborgs will be freed from your central hive-mind.\"",
				NpcReplyText = "{n}Alarm sirens blare throughout the bunker as screens flash amber, cascading through fatal system exceptions.{/n} \"Critical error! Neural link compromised! How... how do you understand the architecture of the Starship protocols?!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "Father of Creation",
					Category = ConstellationCategory.Quest,
					SceneContext = "Blackwater: System Override",
					CosmicCoins = 140,
					Lines = new List<string> { "<color=#D2691E><b>[The Constellation 'Father of Creation']</b></color> grunts in approval: \"Clean engineering. If a forge turns foul, you break the bellows and reclaim the metal for honest tools.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Blackwater_Mastermind_Firewall",
				Type = OptionType.Subclass,
				RequiredProficiency = "MastermindProficiencies",
				Alignment = AlignmentShiftDirection.LawfulEvil,
				PromptText = "(Mastermind) \"Your computational security is laughably primitive. I have already mapped your server pathways, isolated your subroutine nodes, and locked down your defensive protocols from the terminal. You belong to me now.\"",
				NpcReplyText = "{n}The hundred voices glitch and stammer in escalating computational panic.{/n} \"Security override detected! Subroutines hijacked! How could a mortal anticipate our defensive algorithms?!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Blackwater: Mastermind Subjugation",
					CosmicCoins = 140,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smirks with icy satisfaction: \"Subjugating a rogue machine network through sheer architectural intellect. True dominion in action.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterNurahPrison()
		{
			NarrativeScene narrativeScene = new NarrativeScene("SideQuest_Nurah_Prison", "01d00f006e0e16543b7507ff38e9fb8a", "Act3", "Drezen Dungeon: Nurah's Treason Interrogation");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Nurah_Prison_Universal_SlaveryCritique",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"Cheliax enslaved you, and their cruel hypocrisy disgusts me as much as it does you. But selling your soul to deskari's locust swarms didn't make you free; it just made you another demon's plaything.\"",
				NpcReplyText = "{n}Nurah's sarcastic grin wavers. For a split second, her eyes widen with raw, unguarded pain before she masks it with spite.{/n} \"You... you actually condemn Cheliax? Every crusader lord I've ever known praised their 'discipline' while ignoring the lash. But it changes nothing! The world is rotten to the root, Commander!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Mischievous Friend",
					Category = ConstellationCategory.Quest,
					SceneContext = "Drezen Dungeon: Nurah's Pain",
					CosmicCoins = 85,
					Lines = new List<string> { "<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> sighs gently: \"A wounded bird that learned only how to peck out eyes. Show her that liberty is built with courage, not demonic poison.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Nurah_Prison_ThirdOption_VengeanceRedirect",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Otherworld Pragmatism) \"Don't throw your life away in this cell. Turn your knowledge of subversion against the true enemies of freedom. Aid my crusade, and I will ensure you live to see the tyrants of Cheliax brought to their knees.\"",
				NpcReplyText = "{n}Nurah grips the iron bars, her breath hitching as she searches your face for deception.{/n} \"You offer me vengeance against the real slavers? Ha! You are either mad or dangerous... but I have nothing left to lose. Show me this crusade of yours, Commander.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Lucky Drunk",
					Category = ConstellationCategory.Quest,
					SceneContext = "Drezen Dungeon: The Bargain for Freedom",
					CosmicCoins = 120,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> clinks his cup against the stars: \"That's how you break chains! Don't hang the rebel, give her a bigger wall to smash!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterZachariusCrypt()
		{
			NarrativeScene narrativeScene = new NarrativeScene("SideQuest_Zacharius_Crypt", "16f23cbdc188b3348a47531d2bb747c2", "Act2", "Lost Chapel Crypt: The Lich Zacharius");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Zacharius_Crypt_ShadowMonarch_Dominance",
				Type = OptionType.Subclass,
				RequiredProficiency = "ShadowMonarchProficiencies",
				Alignment = AlignmentShiftDirection.NeutralEvil,
				PromptText = "(Shadow Monarch) \"Your phylactery and skeletal wards are toys of mortal necromancy. I draw power directly from the eternal shadows of the Netherworld. Bow before the true sovereign of the dead, Zacharius.\"",
				NpcReplyText = "{n}The pale green witchfire in Zacharius's eye sockets flickers erratically as an oppressive, freezing gloom floods the subterranean vault.{/n} \"What manner of abomination are you?! That aura... it carries the absolute stillness of the Netherworld! You are no mere wizard... you are a monarch of shadows!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Lady of Graves",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Lost Chapel Crypt: Sovereign Assertion",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> observes with icy tranquility: \"The lich clings to a trinket of bone, blind to the true planar expanse of death and shadow.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Zacharius_Crypt_GodEmperor_Heresy",
				Type = OptionType.Subclass,
				RequiredProficiency = "GodEmperorProficiencies",
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(God Emperor) \"A crusader hero who coward in a tomb to escape mortality. Your existence is a blasphemy against the imperial legacy you were sworn to uphold.\"",
				NpcReplyText = "{n}Zacharius roars with hollow, venomous malice, grasping his staff.{/n} \"'Coward?!' I saved this crusade while your ancestors were yet dust! You will pay for your arrogance with your very soul!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor (Parallel Echo)",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Lost Chapel Crypt: Imperial Condemnation",
					CosmicCoins = 110,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> resonates with sacred fury: \"No victory bought with undeath is ever worthy of remembrance! Smite the oath-breaker!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterVellexiaSalon()
		{
			NarrativeScene narrativeScene = new NarrativeScene("SideQuest_Vellexia_Salon", "72fcda3753e53814cbdc8ab0fc4fe8d8", "Act4", "Alushinyrra: Vellexia's Salon Date");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Vellexia_Salon_Universal_Poise",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Isekai Protagonist) \"Your theatrics are theatrical, Lady Vellexia, but entirely predictable. In my home world, drama requires a bit more subtlety than torturing lesser fiends for applause.\"",
				NpcReplyText = "{n}Vellexia leans forward, her jeweled lips parting in genuine, thrilled astonishment.{/n} \"Did you hear that, my darlings? The mortal dares to criticize my salon! Exquisite... simply exquisite! Tell me more of your world's 'subtlety', Commander!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lady in Shadow",
					Category = ConstellationCategory.Quest,
					SceneContext = "Alushinyrra: Salon Critique",
					CosmicCoins = 100,
					Lines = new List<string> { "<color=#9400D3><b>[The Constellation 'Our Lady in Shadow']</b></color> watches from her palace above: \"A delightfully composed performance. Vellexia preens for amusement, but you hold the stage with ease.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Vellexia_Salon_Slime_Indifference",
				Type = OptionType.Subclass,
				RequiredProficiency = "DevourerProficiencies",
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Slime) \"Your grand passions and sadistic games are meaningless to a predator of fluid mass. To me, you and your guests are simply high-density nutritional components.\"",
				NpcReplyText = "{n}Vellexia gasps, clutching her silk bodice as she watches your body pulse with translucent, hungry luminescence.{/n} \"A creature of pure, unbridled consumption... how delightfully uncivilized! Do not look at me with such appetite, darling, or I might be forced to kiss you!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Alushinyrra: Slime at the Salon",
					CosmicCoins = 130,
					Lines = new List<string> { "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> giggles uncontrollably: \"Viewing high-society succubi as caloric intake! You truly are a conqueror of refined tastes!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterChillyCreek()
		{
			NarrativeScene narrativeScene = new NarrativeScene("SideQuest_ChillyCreek_Village", "134c2eef7c6690a43b4c8cf1920dd434", "Act3", "Chilly Creek: The Unhallowed Hag Shrine");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "ChillyCreek_Universal_HagCurse",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"The mist in this valley isn't natural weather; it is an alchemical curse feeding on human desperation. We are ending this village's nightmare today.\"",
				NpcReplyText = "{n}The villagers exchange fearful, haggard glances as your voice cuts through the cloying fog like a clarion bell.{/n} \"You don't understand, stranger! The Old One hears everything! If you anger her, our children will pay the price!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "Old Deadeye",
					Category = ConstellationCategory.Quest,
					SceneContext = "Chilly Creek: Breaking the Mists",
					CosmicCoins = 80,
					Lines = new List<string> { "<color=#8FBC8F><b>[The Constellation 'Old Deadeye']</b></color> nods with solemn authority: \"A home must be protected from deceitful predators. Pierce the veil and free these folk from their terror.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterLatverkFleshmarket()
		{
			NarrativeScene narrativeScene = new NarrativeScene("SideQuest_Latverk_Final", "ac407cdcdcae1f34a9fdef9c1cfa444f", "Act4", "Alushinyrra: Latverk's Den of Mutilation");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Latverk_Universal_Execution",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) \"I have traveled across realities and fought demon lords, but few monsters disgust me as thoroughly as you, Latverk. There is no bargain here. Die.\"",
				NpcReplyText = "{n}Latverk's face contorts with panic as he scrambles backward among his surgical instruments.{/n} \"Wait! I have gold! Relics from ancient empires! You can't just execute me in cold blood! Guards! Slaves! Kill them!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Eternal Rose",
					Category = ConstellationCategory.Quest,
					SceneContext = "Alushinyrra: Latverk Reckoning",
					CosmicCoins = 110,
					Lines = new List<string> { "<color=#FF69B4><b>[The Constellation 'The Eternal Rose']</b></color> turns away in weeping sorrow: \"Beauty defiled and innocence torn apart for grotesque vanity. End this cruelty once and for all.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterEnigmaPyramid()
		{
			NarrativeScene narrativeScene = new NarrativeScene("SideQuest_Enigma_Areshkagal", "0e145f47d1c32534295f145e267c6ac7", "Act5", "The Enigma: Areshkagal's Void Showdown");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Enigma_Universal_EgoAssert",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"Your riddles and nihilistic mirrors are meaningless to someone who survived crossing realities. I am not nothingness, Areshkagal. I am the Commander, and your illusion ends now!\"",
				NpcReplyText = "{n}The colossal pharaonic silhouette of Areshkagal roars, her voice echoing with the screams of a billion forgotten souls.{/n} \"A leaf clinging to an ego! A transient dust speck denying the cosmic void! Fade into obscurity, anomaly!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Silence Between the Stars",
					Category = ConstellationCategory.Quest,
					SceneContext = "The Enigma: Defiance of the Void",
					CosmicCoins = 180,
					Lines = new List<string> { "<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color> flutters softly against the boundless dark: \"The void hungers for all names, yet an unwritten soul shines like a beacon against the abyss.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
