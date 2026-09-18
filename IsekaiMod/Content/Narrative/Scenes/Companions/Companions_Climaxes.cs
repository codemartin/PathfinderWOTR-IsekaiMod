using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.Companions
{
	public static class Companions_Climaxes
	{
		public static void Register()
		{
			RegisterDaeranOther();
			RegisterEmberSermon();
			RegisterSosielTreverArena();
			RegisterRegillTribunal();
			RegisterArueshalaeDream();
			RegisterSavamelechCavern();
			RegisterWoljifShadow();
			RegisterSeelahJeweler();
			RegisterGreyborAmbush();
			RegisterNenioFox();
		}

		private static void RegisterDaeranOther()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Daeran_TheOther", "bd13cf0cc399bf147ae85c745b330f90", "Companions", "Trap for the Other: Daeran vs. Inquisitor Liotr");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Daeran_Other_Universal_Defense",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"Liotr, lower your weapon. Daeran was a traumatized child who survived by hosting an alien nightmare he never asked for. I will not allow you to execute him for surviving.\"",
				NpcReplyText = "{n}Daeran lets out a faint, shaking breath, his trademark smirk completely absent as he stares at your back.{/n} \"Commander... you truly are a fool. But for once... I am profoundly grateful for your foolishness.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Lucky Drunk",
					Category = ConstellationCategory.Quest,
					SceneContext = "The Other: Daeran Defense",
					CosmicCoins = 120,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> nods firmly: \"Standing between an inquisitor and a broken kid. That's what heroes do! Pour that boy a stiff drink!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Daeran_Other_ThirdOption_Severance",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Otherworld Technique) \"I will anchor the entity into an isolated dimensional pocket. We can sever the parasitic tether without harming Daeran's mind or soul.\"",
				NpcReplyText = "{n}Liotr gasps as ethereal ley-chains hum with otherworld luminescence, locking the terrifying void-presence away from Daeran's consciousness.{/n} \"By the Inheritor... the presence has receded! His soul is unblemished! What manner of celestial art is this?!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.MetaLoop,
					SceneContext = "The Other: Dimensional Severance",
					CosmicCoins = 180,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> reverberates softly: \"The threshold is closed. What once devoured from the dark is folded away into the infinite corners of space.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterEmberSermon()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Ember_Sermon", "f88c50427545bb74696abb63ade9f5cf", "Companions", "Alushinyrra: Ember's Sermon to the Demons");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Ember_Sermon_Universal_Support",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) \"Listen to her words! Even in the heart of the Abyss, despair is a choice. You don't have to live solely for cruelty and slaughter.\"",
				NpcReplyText = "{n}The gathered demons mutter and shift uncomfortably. Several succubi avert their gaze, tears of forgotten shame cutting through their cosmetic rouge.{/n} \"The mortal speaks... why does it hurt to hear it? Why do we remember the sun?\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Quest,
					SceneContext = "Alushinyrra: Ember's Sermon",
					CosmicCoins = 140,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> weeps tears of starlight: \"Pure compassion blossoming in the darkest gutter of the multiverse. Let the song guide their weeping souls home.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterSosielTreverArena()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Sosiel_TreverArena", "47e0721c470ff9e42b509ac257ea373f", "Companions", "Battlebliss Arena: Sosiel's Reunion with Trever");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Sosiel_Trever_Universal_Shield",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) \"Trever, look at your brother! You survived the horrors of the Worldwound and the arena. You are not a mindless beast of the pit; you are a knight of Shelyn. Come home!\"",
				NpcReplyText = "{n}Trever's bloodshot eyes widen as his grip on his heavy falchion loosens, the weapon clattering to the blood-caked arena sand.{/n} \"Sosiel...? Is it... truly you? The rose... has not withered?\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Eternal Rose",
					Category = ConstellationCategory.Quest,
					SceneContext = "Battlebliss: Brothers Reunited",
					CosmicCoins = 130,
					Lines = new List<string> { "<color=#FF69B4><b>[The Constellation 'The Eternal Rose']</b></color> sings with radiant triumph: \"Love transcends the butcher's arena! The lost brother is returned to the light of the Rose!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Sosiel_Trever_MartialGod_Salute",
				Type = OptionType.Subclass,
				RequiredProficiency = "MartialGodProficiencies",
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Martial God) \"A warrior's soul does not break so easily, Trever. Pick up your blade not for the amusement of demon spectators, but to stand shoulder to shoulder with the Commander of the crusade.\"",
				NpcReplyText = "{n}Trever straightens his battered posture, recognizing the peerless martial authority in your stance. A solemn, dignified resolve returns to his weathered face.{/n} \"To fight for an honorable commander once more... I would ask for nothing greater. Lead, Commander.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Battlebliss: Martial Redemption",
					CosmicCoins = 140,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> clashes colossal iron gauntlets: \"A warrior reclaiming his honor from the dirt! That is how true soldiers answer the call of battle!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterRegillTribunal()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Regill_FinalTribunal", "e8574d8240b93384986d44a597c7cb94", "Companions", "Godclaw Camp: Regill's Final Hellknight Tribunal");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Regill_Tribunal_Universal_Loyalty",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(Isekai Protagonist) \"Regill staged this trial not to betray me, but to bind the Hellknight orders irrevocably to my command before the final march. I see your strategy, Paralictor, and I accept the verdict.\"",
				NpcReplyText = "{n}Regill's expression remains completely stoic, but a subtle nod of profound, unreserved respect indicates his total satisfaction.{/n} \"A commander who sees through the theater of authority to its strategic necessity. The Order of the Godclaw has no further questions. Our legions are yours, Commander.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Fivefold Order",
					Category = ConstellationCategory.Quest,
					SceneContext = "Godclaw Camp: The Trial Passed",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#708090><b>[The Constellation 'The Fivefold Order']</b></color> resonates with unyielding discipline: \"Discipline tested and proven in the crucible of truth. The five orders march as one behind your standard!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Regill_Tribunal_GodEmperor_Decree",
				Type = OptionType.Subclass,
				RequiredProficiency = "GodEmperorProficiencies",
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(God Emperor) \"Your loyalty was never in doubt, Regill. But understand this: Hellknights and crusaders alike bow to a single supreme throne now. The Bleaching ends when I decree it.\"",
				NpcReplyText = "{n}Regill salutes crisply, slamming his fist against his breastplate.{/n} \"Understood, Sovereign. Command, and we shall enforce.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "Master of the First Vault",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Godclaw Camp: Imperial Supremacy",
					CosmicCoins = 160,
					Lines = new List<string> { "<color=#DAA520><b>[The Constellation 'Master of the First Vault']</b></color> seals the edict: \"Order perfected through undisputed sovereignty. The law finds its purest expression.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterArueshalaeDream()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Arueshalae_Dream", "461a1c3acbb9c4041b892cac5f18ade2", "Companions", "Dream Realm: Arueshalae's Sanctuary of the Mind");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Arue_Dream_Universal_Hope",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"Every dream of green grass and peaceful skies is a victory against the Abyss, Arueshalae. You are not defined by what you were made to be, but by what you choose to protect.\"",
				NpcReplyText = "{n}Arueshalae's eyes fill with wonder and luminous warmth as dream petals swirl softly around her wings.{/n} \"What I choose to protect... thank you, my friend. With you beside me, I feel like I can finally believe in tomorrow.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Quest,
					SceneContext = "Dream Realm: Starlit Sanctuary",
					CosmicCoins = 125,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles across the cosmos: \"A heart daring to dream in the darkest cage. May the butterfly carry her beyond fear!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterSavamelechCavern()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Savamelech_Cavern", "47f5d5cda15666d4187175ba72b36ca6", "Companions", "Underground Cavern: Savamelech Confrontation");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Savamelech_Universal_SeverPoison",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) \"You fed these people poison and called it a blessing, Savamelech. Your reign of petty subterranean tyranny ends here and now!\"",
				NpcReplyText = "{n}Savamelech hisses furiously, his scaled coils whipping against the cavern walls in agitation.{/n} \"Insolent wretch! The blood of the Abyss flows through their veins! They belong to me body and soul!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor (Parallel Echo)",
					Category = ConstellationCategory.Quest,
					SceneContext = "Cavern: Savamelech Reckoning",
					CosmicCoins = 110,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> blazes with celestial radiance: \"Cut down the serpent and purge the corruption from this sacred soil!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterWoljifShadow()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Woljif_ShadowCrossroads", "66383078723475648955aed24d5adda3", "Companions", "Alushinyrra: Woljif's Shadow Crossroads");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Woljif_Universal_TrueSelf",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"You don't need a demon grandfather's approval or an infernal shadow to be extraordinary, Woljif. You're already our chief thief and loyal companion. That's more than enough.\"",
				NpcReplyText = "{n}Woljif wipes a sleeve across his eyes, trying and failing to hold back a wide, genuine grin.{/n} \"Chief thief, huh? Yeah... yeah, you're right, boss! Who needs some creepy old shadow demon when I've got the greatest crew in the multiverse!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Mischievous Friend",
					Category = ConstellationCategory.Quest,
					SceneContext = "Alushinyrra: Woljif's Choice",
					CosmicCoins = 115,
					Lines = new List<string> { "<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> giggles warmly: \"A rogue choosing friends over terrifying dark magic! Good choice, kiddo!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Woljif_ShadowMonarch_TrueShadow",
				Type = OptionType.Subclass,
				RequiredProficiency = "ShadowMonarchProficiencies",
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Shadow Monarch) \"That demon entity was merely a parasitic mimic. True shadow belongs to the Netherworld, Woljif. If you ever seek mastery over darkness, you need only ask your Commander.\"",
				NpcReplyText = "{n}Woljif stares at the dark, serene aura radiating from your silhouette with sheer, unadulterated awe.{/n} \"Whoa... boss... you make shadows look like royal cloaks instead of curses! I'm definitely sticking with you!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Lady of Graves",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Alushinyrra: The True Shadow Master",
					CosmicCoins = 135,
					Lines = new List<string> { "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> hums quietly: \"Guidance given to a stray soul wandering along the boundary of light and dark.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterSeelahJeweler()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Seelah_JewelerClimax", "77d195b42c726794992d138394e42750", "Companions", "Weight of My Sword: Seelah's Resolution");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Seelah_Jeweler_Universal_Faith",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"Friendship isn't about never failing each other, Seelah; it is about standing back up and walking the path together. Your past doesn't invalidate your honor.\"",
				NpcReplyText = "{n}Seelah smiles through her tears, sheathing her sword with renewed purpose.{/n} \"Thanks, Commander. Sometimes a paladin needs to hear that even saints started out as sinners. Let's head back!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor (Parallel Echo)",
					Category = ConstellationCategory.Quest,
					SceneContext = "Weight of My Sword: True Honor",
					CosmicCoins = 110,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> radiates warmth: \"Valorous loyalty that heals the wounded spirit. A friendship forged in righteousness!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterGreyborAmbush()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Greybor_Ambush", "f0f0d199b1c25e142920587878d88cda", "Companions", "Price of Loyalty: Horzalah Ambush Climax");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Greybor_Ambush_Universal_Professional",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"The Assassins Guild thought they could buy you out, Greybor, but they forgot one crucial detail: they hired amateurs to kill professionals.\"",
				NpcReplyText = "{n}Greybor's weathered face twitches into a grim, deeply satisfied grin as he tests the edge of his battleaxe.{/n} \"Well put, Commander. Let us show these guild cowards what an actual contract looks like when executed properly.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Quest,
					SceneContext = "Price of Loyalty: Contract Executed",
					CosmicCoins = 120,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with battle lust: \"Steel speaks louder than assassin coin! Split them from helm to greave!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterNenioFox()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Companion_Nenio_FoxReveal", "db068bf5b388cce4c9f828d389ca537d", "Companions", "The Enigma: Nenio's Identity Awakening");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Nenio_Fox_Universal_Identity",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Isekai Protagonist) \"You are not an empty vessel for Areshkagal's riddles, Nenio. You are our wonderfully eccentric, insatiably curious scholar. Your encyclopedia is your own creation!\"",
				NpcReplyText = "{n}Nenio blinks her golden kitsune eyes, vigorously scribbling into her travel notes.{/n} \"Hypothesis confirmed! The subject asserts my individual scientific ego against external nihilistic interference! Conclusion: I shall retain my personal designation as Nenio!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Silence Between the Stars",
					Category = ConstellationCategory.Quest,
					SceneContext = "The Enigma: The Scholar's Awakening",
					CosmicCoins = 140,
					Lines = new List<string> { "<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color> shines brightly: \"Curiosity triumphs over the void! A name written by one's own quill can never be erased.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "IsekaiDialogueNenioStatueAnswer1",
				Type = OptionType.Universal,
				ShowOnce = true,
				NextCueGuid = "1701cb6cba55ed04cac7908e072563ac",
				PromptText = "(Isekai Protagonist) \"I am the bone of my sword.\"",
				NpcReplyText = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}",
				ChainedReplies = new List<NarrativeCueDefinition>
				{
					new NarrativeCueDefinition
					{
						Text = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}"
					},
					new NarrativeCueDefinition
					{
						SpeakerGuid = "1b893f7cf2b150e4f8bc2b3c389ba71d",
						Text = "{n}Nenio's tail stands straight up as her quill flies furiously across the parchment.{/n} \"'Bone of my sword'?! An anatomical impossibility! Steel has no skeletal osteocytes! Are your weapons calcified biological organisms?! BOY! Write this down!\"",
						MoveCamera = true
					}
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "IsekaiDialogueNenioStatueAnswer2",
				Type = OptionType.Universal,
				ShowOnce = true,
				NextCueGuid = "1701cb6cba55ed04cac7908e072563ac",
				PromptText = "(Isekai Protagonist) \"I am the hope of the universe.\"",
				NpcReplyText = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}",
				ChainedReplies = new List<NarrativeCueDefinition>
				{
					new NarrativeCueDefinition
					{
						Text = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}"
					},
					new NarrativeCueDefinition
					{
						SpeakerGuid = "1b893f7cf2b150e4f8bc2b3c389ba71d",
						Text = "{n}Nenio frantically flips through three reference folios at once.{/n} \"'Hope of the universe'?! A wildly unquantifiable metaphysical claim! Yet the statue accepts it as empirical truth! Fascinating!\"",
						MoveCamera = true
					}
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "IsekaiDialogueNenioStatueAnswer3",
				Type = OptionType.Universal,
				ShowOnce = true,
				NextCueGuid = "1701cb6cba55ed04cac7908e072563ac",
				PromptText = "(Isekai Protagonist) \"I am just a guy who's a hero for fun.\"",
				NpcReplyText = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}",
				ChainedReplies = new List<NarrativeCueDefinition>
				{
					new NarrativeCueDefinition
					{
						Text = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}"
					},
					new NarrativeCueDefinition
					{
						SpeakerGuid = "1b893f7cf2b150e4f8bc2b3c389ba71d",
						Text = "{n}Nenio blinks in genuine scientific bewilderment, tapping her chin with her inkwell.{/n} \"A hobbyist savior? Altruism pursued as casual recreation without institutional tenure? A statistical anomaly of the highest order!\"",
						MoveCamera = true
					}
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "IsekaiDialogueNenioStatueAnswer4",
				Type = OptionType.Universal,
				ShowOnce = true,
				NextCueGuid = "1701cb6cba55ed04cac7908e072563ac",
				PromptText = "(Isekai Protagonist) \"I am Atomic.\"",
				NpcReplyText = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}",
				ChainedReplies = new List<NarrativeCueDefinition>
				{
					new NarrativeCueDefinition
					{
						Text = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}"
					},
					new NarrativeCueDefinition
					{
						SpeakerGuid = "1b893f7cf2b150e4f8bc2b3c389ba71d",
						Text = "{n}Nenio drops her spectacles in sheer academic euphoria.{/n} \"'ATOMIC'?! The indivisible building block of matter?! The elemental substratum of all physical planes?! What terrifying Otherworld lexicon is this?!\"",
						MoveCamera = true
					}
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "IsekaiDialogueNenioStatueAnswer5",
				Type = OptionType.Universal,
				ShowOnce = true,
				NextCueGuid = "1701cb6cba55ed04cac7908e072563ac",
				PromptText = "(Isekai Protagonist) \"I am a God.\"",
				NpcReplyText = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}",
				ChainedReplies = new List<NarrativeCueDefinition>
				{
					new NarrativeCueDefinition
					{
						Text = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}"
					},
					new NarrativeCueDefinition
					{
						SpeakerGuid = "1b893f7cf2b150e4f8bc2b3c389ba71d",
						Text = "{n}Nenio sniffs the air with scholarly skepticism, adjusting her notebook.{/n} \"A divinity claim without an established celestial portfolio or documented clerical domains. Statistically arrogant, yet functionally intimidating!\"",
						MoveCamera = true
					}
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "IsekaiDialogueNenioStatueAnswer6",
				Type = OptionType.Universal,
				ShowOnce = true,
				NextCueGuid = "1701cb6cba55ed04cac7908e072563ac",
				PromptText = "(Isekai Protagonist) \"I am a wanderer cast across the rift between worlds, forging my own fate.\"",
				NpcReplyText = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}",
				ChainedReplies = new List<NarrativeCueDefinition>
				{
					new NarrativeCueDefinition
					{
						Text = "{n}The emptiness is bewildered and confused, but cannot deny what you say to be truth.{/n}"
					},
					new NarrativeCueDefinition
					{
						SpeakerGuid = "1b893f7cf2b150e4f8bc2b3c389ba71d",
						Text = "{n}Nenio nods vigorously, ink spattering across her whiskers.{/n} \"A trans-dimensional itinerant! A self-determining planar anomaly! This fits our ongoing research parameters with remarkable precision!\"",
						MoveCamera = true
					}
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
