using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Narrative.Actions;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Narrative.Scenes.MainCampaign
{
	public static class Act1_KenabresLiberation
	{
		public static void Register()
		{
			RegisterDefendersHeartWarCouncil();
			RegisterMinaghoGrayGarrisonClimax();
			RegisterHulrunMarketSquare();
			RegisterHorgusNegotiation();
			RegisterKaylessaAmbush();
			RegisterWardstoneAwakening();
			RegisterDesnaTempleCommunion();
		}

		private static void RegisterDefendersHeartWarCouncil()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act1_DefendersHeart_WarCouncil", "5be1d70239344a4e8c483b7ba94b019d", "Act1", "Defender's Heart: Liberation Council of Kenabres");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DH_WarCouncil_Universal_Logistics",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"Frontal charges against the Gray Garrison play directly into Minagho's defensive lines. We need coordinated pincer strikes: diversionary skirmishes at the gate while infiltration squads breach the upper terrace.\"",
				NpcReplyText = "{n}Irabeth straightens up from the tactical map, her battered face visibly softening with genuine relief.{/n} \"A sound tactical assessment. Most volunteers shout for reckless vengeance, but precision coordination is what will actually keep our soldiers alive. Anevia, adjust our scout routes to mirror this plan.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor (Parallel Echo)",
					Category = ConstellationCategory.Quest,
					SceneContext = "Defender's Heart: Strategic Coordination",
					CosmicCoins = 75,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> nods with solemn approval: \"Valorous courage tempered by disciplined reason. A commander who values the lives of their vanguard is worthy of leading the crusade.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DH_WarCouncil_ThirdOption_Leylines",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Otherworld Knowledge) \"The demons are drawing power from the corrupted Wardstone network. Instead of merely slaughtering the garrison guards, we can destabilize their abyssal resonance from the subterranean catacombs first.\"",
				NpcReplyText = "{n}Irabeth blinks, trading a startled glance with Anevia.{/n} \"Subterranean resonance? That explains the strange vibrations our sentries reported near the old aqueducts. If you can sever their planar conduit, we won't just breach their gates, we will shatter their entire defensive perimeter!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The All-Seeing Eye",
					Category = ConstellationCategory.Quest,
					SceneContext = "Defender's Heart: Leyline Insight",
					CosmicCoins = 100,
					Lines = new List<string> { "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> flares with arcane brilliance: \"Insight piercing the veil of mortal siegecraft! Unravel the leylines, and the stone crumbles of its own weight!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DH_WarCouncil_Slime_Corrosion",
				Type = OptionType.Subclass,
				RequiredProficiency = "DevourerProficiencies",
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Slime) \"Send me in first. The Garrison's reinforced iron gates and alchemical barricades will dissolve quite easily under my digestive secretions.\"",
				NpcReplyText = "{n}Irabeth stares at you, momentarily speechless as she watches the fluid glint along your fingers.{/n} \"You intend to... eat their fortifications? By the gods, if it spares our sappers from baleful brimstone fire, you have full clearance. Just ensure nothing innocent is swallowed alongside the rubble.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Defender's Heart: Slime Infiltration",
					CosmicCoins = 120,
					Lines = new List<string> { "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles heartily: \"Splendid! Why waste days with battering rams when you can simply dissolve the gates into nutrient broth? Delicious ingenuity!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DH_WarCouncil_Shadow_Vanguard",
				Type = OptionType.Subclass,
				RequiredProficiency = "ShadowMonarchProficiencies",
				Alignment = AlignmentShiftDirection.NeutralEvil,
				PromptText = "(Shadow Monarch) \"Keep your living soldiers in reserve. The fallen dead of Kenabres will rise from the Netherworld to lead the vanguard. No crusader blood needs to be spilled at the breach.\"",
				NpcReplyText = "{n}Irabeth grips the hilt of her blade, her jaw clenching as shadows lengthen across the council table.{/n} \"Shadow legions... It chills my heart to think of using the dead, but if it protects our living crusaders from Minagho's blades, I cannot refuse the tactical advantage. Lead your shades, Commander.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Lady of Graves",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Defender's Heart: Shadow Sovereign Call",
					CosmicCoins = 120,
					Lines = new List<string> { "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> watches with cold neutrality: \"The boundary between life and the shadow plane blurs, yet the balance holds so long as no soul is severed from its ultimate judgment.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DH_WarCouncil_GodEmperor_Aegis",
				Type = OptionType.Subclass,
				RequiredProficiency = "GodEmperorProficiencies",
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(God Emperor) \"Order, discipline, and absolute authority will reclaim this city. I will personally sanctify the advance with an imperial aegis that no demonic blasphemy can penetrate.\"",
				NpcReplyText = "{n}Irabeth stands at attention, instinctively straightening her back under the weight of your presence.{/n} \"Your conviction is formidable. It rallies the men faster than any sermon from the pulpit. We march under your command!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "Master of the First Vault",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Defender's Heart: Imperial Order",
					CosmicCoins = 120,
					Lines = new List<string> { "<color=#DAA520><b>[The Constellation 'Master of the First Vault']</b></color> hums with satisfaction: \"Law brings stability, and stability ensures victory. Build the foundation of order upon these bloodstained cobblestones.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DH_WarCouncil_Hero_Promise",
				Type = OptionType.Subclass,
				RequiredProficiency = "HeroProficiencies",
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Hero) \"Not one more citizen of Kenabres falls today. We fight not for glory or territory, but so the people trapped in the ruins can see tomorrow's sunrise. Trust in us!\"",
				NpcReplyText = "{n}The tension in the cellar breaks as a warm wave of confidence washes over the gathered fighters. Even Anevia smiles faintly.{/n} \"You make it sound so simple... yet somehow, I believe you. Let us give the demons a lesson they will never forget!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Dawnflower",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Defender's Heart: Heroic Resolve",
					CosmicCoins = 120,
					Lines = new List<string> { "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> radiates golden warmth: \"Hope is the brightest torch in the darkest night. May your blade guide the innocent to safety!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DH_WarCouncil_MetaLoop_Foreknowledge",
				Type = OptionType.MetaLoop,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Meta Loop) \"Minagho has posted brimstone alchemists on the upper battlements and planted ghoul ambushers in the lower wine cellars. I already know every trap she set.\"",
				NpcReplyText = "{n}Irabeth stares at you in absolute astonishment, reviewing her scout reports with trembling fingers.{/n} \"How... how could you possibly know the layout of the upper battlements? Our scouts haven't even returned from that sector! But if your information is accurate, we can neutralize their ambushes before a single trap is sprung!\"",
				Variations = new List<CycleVariation>
				{
					new CycleVariation
					{
						MinCycle = 2,
						MaxCycle = int.MaxValue,
						VariantText = "(Meta Loop) \"Minagho has posted brimstone alchemists on the upper battlements and planted ghoul ambushers in the lower wine cellars. I have seen this siege play out before, and this time we will not bleed for her surprises.\"",
						VariantReplyText = "{n}Irabeth looks at you with profound reverence, recognizing a terrifying depth of foreknowledge in your gaze.{/n} \"It is as if you walk through time itself. We will execute your counter-ambush without hesitation!\""
					}
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.MetaLoop,
					SceneContext = "Defender's Heart: Temporal Echoes",
					CosmicCoins = 200,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> whispers through the fabric of time: \"The cycle ripples. What has been surveyed in a previous timeline becomes the inescapable trap for your foes in this one.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DH_WarCouncil_Universal_HulrunRamienUnity",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) [Prelate & High Priest Reconciled] \"Irabeth, Prelate Hulrun and High Priest Ramien have set aside their feud. Their combined inquisitors and Desnan scouts are securing the outer districts together as one united front.\"",
				NpcReplyText = "{n}Irabeth lets out a long, incredulous breath, shaking her head with overwhelming relief.{/n} \"Hulrun and Ramien... cooperating without drawing steel? Commander, that alone is a greater miracle than any sword we possess. With the inquisitors and the Desnans watching each other's backs instead of each other's throats, our rear guard is completely secure. We can strike the Gray Garrison with everything we have!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Quest,
					SceneContext = "Defender's Heart: Hulrun and Ramien Holy Unity",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> sings softly: \"When suspicion yields to starlight, hope blooms even in the ruins of Kenabres.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "DH_WarCouncil_Cayden_TavernDefense",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				CustomAction = new ContextActionCompleteBounty
				{
					BountyId = "Cayden_DefendersHeart_Tavern"
				},
				PromptText = "(Isekai Protagonist) [Tavern Defense Rally] \"The recruits held the line, the cellar barrels didn't catch fire, and the demons were sent packing back to the ruins. Let's raise a flagon to Defender's Heart before we march on the garrison!\"",
				NpcReplyText = "{n}Irabeth's grim expression cracks into a rare, weary smile, raising a wooden tankard toward the assembled crusaders.{/n} \"To Kenabres, and to the recruits who stood their ground when the walls shook. One drink, Commander... and then we finish this.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Quest,
					SceneContext = "Defender's Heart: Tavern Defense Rally",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars with laughter: \"A toast to the defenders! May your blades be sharp and your ale never run dry!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterMinaghoGrayGarrisonClimax()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act1_GrayGarrison_MinaghoClimax", "5acd8001d9f7d2443bd57fb1291a03e4", "Act1", "Gray Garrison: Minagho Showdown");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Minagho_First_Universal_Defiance",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				NextCueGuid = "3bd9a4263d8064b49a9d1eec365807b9",
				CustomAction = new ContextActionCompleteBounty
				{
					BountyId = "Gorum_Minagho_Defeat"
				},
				ChainedReplies = new List<NarrativeCueDefinition>
				{
					new NarrativeCueDefinition
					{
						SpeakerGuid = "e778129f817a5fa4286e64b061df84a5",
						Text = "{n}Irabeth straightens her bruised shoulders, drawing courage from your defiance.{/n} \"The Commander is right, Minagho. Your reign of terror ends tonight!\"",
						MoveCamera = true
					},
					new NarrativeCueDefinition
					{
						SpeakerGuid = "397b090721c41044ea3220445300e1b8",
						Text = "{n}Camellia tilts her head, giving a quiet, amused smirk behind her buckler.{/n} \"Such bold words... let us see if your blade can back them up.\"",
						MoveCamera = true
					},
					new NarrativeCueDefinition
					{
						SpeakerGuid = "54be53f0b35bf3c4592a97ae335fe765",
						Text = "{n}Seelah grips her longsword with renewed conviction, nodding firmly at your resolve.{/n} \"Well said! Kenabres will not bend to you again, demon!\"",
						MoveCamera = true
					}
				},
				PromptText = "(Isekai Protagonist) \"So you are the infamous commander behind Kenabres's slaughter. You look remarkably complacent for someone whose army is about to be driven out of the garrison.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Lucky Drunk",
					Category = ConstellationCategory.Quest,
					SceneContext = "Gray Garrison: Minagho Defiance",
					CosmicCoins = 80,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises his mug high: \"That's the spirit! Never let a demon lord think they own the room while you still have breath in your lungs!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Minagho_First_Mastermind_Outplayed",
				Type = OptionType.Subclass,
				RequiredProficiency = "MastermindProficiencies",
				Alignment = AlignmentShiftDirection.LawfulEvil,
				PromptText = "(Mastermind) \"Your tactics were predictable from the start. You overextended your vanguard, left your flanks exposed to our scouts, and relied on brute terror. You've already lost, Minagho.\"",
				NpcReplyText = "{n}Minagho flinches, a momentary crack showing in her haughty demeanor as she scans the courtyard behind you.{/n} \"You dare lecture me on strategy?! You know nothing of the Grand Plan! Kill them! Kill them all!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Gray Garrison: Mastermind Dissection",
					CosmicCoins = 130,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with icy amusement: \"Dissecting the chaotic flaws of an abyssal commander before the first blow is struck. A delightful display of superior intellect.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Minagho_First_MartialGod_Duel",
				Type = OptionType.Subclass,
				RequiredProficiency = "MartialGodProficiencies",
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Martial God) \"Draw whatever dark blades you possess, Minagho. Your little garrison army won't save you from a warrior whose speed and strikes outpace the wind itself.\"",
				NpcReplyText = "{n}Minagho's demonic horns flare with angry crimson flame, her voice shrill with outrage.{/n} \"Another swaggering mortal boasting of swordplay! I will tear that boast from your throat!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Gray Garrison: Martial Challenge",
					CosmicCoins = 130,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with battle fury: \"Issue the challenge right to her face! Let the garrison echo with the clash of steel!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterHulrunMarketSquare()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act1_Hulrun_MarketSquare", "e27807b731f3b1a4eb19c1a04fdfcf53", "Act1", "Kenabres Market Square: Confrontation with Prelate Hulrun");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Hulrun_MarketSquare_Universal_Amnesia",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				NextCueGuid = "ba9c82193a32275408973a8aebdb3a6d",
				CustomActionList = ActionFlow.DoSingle(delegate(StartEtude c)
				{
					c.Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>("d6c6161d2cf0ac44786f9df67fca5ce9");
					c.Evaluate = false;
				}),
				PromptText = "(Isekai Protagonist) \"Other than the blinding lights of an out-of-control vehicle and waking up on these cobblestones, my memories before the festival are completely hazy...\"",
				NpcReplyText = "{n}Hulrun narrows his cold eyes, scrutinizing your face with fierce intensity.{/n} \"'Blinding lights'? Either you are delirious from blood loss, or you're mocking an inquisitor of the crusade. Lady Terendelev vouched for you, so I will let it pass for now. Step aside.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Quest,
					SceneContext = "Market Square: Hulrun Interrogation",
					CosmicCoins = 75,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> snickers: \"Old Hulrun looks like he hasn't had a proper pint in sixty years. Stand your ground!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Hulrun_MarketSquare_ThirdOption_ExposeMimic",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.LawfulGood,
				CustomAction = new ContextActionExposeMimicAndReconcile(),
				PromptText = "(Otherworld Clarity) \"Hold your blades! Hulrun, Ramien isn't your enemy; the inquisitor who whispered treason in your ear was a demonic mimic sent to divide Kenabres from within. Look upon this consecrated starlight and see the true deceiver!\"",
				NpcReplyText = "{n}Hulrun freezes as radiant mana exposes the abyssal residue on his fallen adjutant's medallion. Ramien steps forward, offering his hand in solemn unity.{/n} \"By the heavens... we were dancing on demon strings. Prelate, let us unite our faithful to save Kenabres!\"",
				Rewards = new SceneRewards
				{
					Gold = 500,
					CosmicCoins = 200
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Quest,
					SceneContext = "Market Square: Hulrun & Ramien Reconciliation",
					CosmicCoins = 200,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> sings with radiant joy: \"A cycle of suspicion shattered by truth! Kenabres stands united under the stars!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Hulrun_MarketSquare_Slime_Hunger",
				Type = OptionType.Subclass,
				RequiredProficiency = "DevourerProficiencies",
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Slime) \"Easy with the polearm, sir! I just woke up starving after that terrible carriage accident. I came to the square looking for festival skewers and pastries, not an inquisition.\"",
				NpcReplyText = "{n}Hulrun glares at you with stern suspicion.{/n} \"Starving? Looking for festival treats? You look completely hale thanks to Lady Terendelev, yet you speak like an insatiable wanderer. Keep your hands where the guards can see them.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Great Devourer",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Market Square: Slime Appetite",
					CosmicCoins = 80,
					Lines = new List<string> { "<color=#8B0000><b>[The Constellation 'The Rough Beast']</b></color> grunts in dark amusement: \"EAT... CONSUME... THE SKEWERS OF WAR...\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Hulrun_MarketSquare_ShadowMonarch_Chill",
				Type = OptionType.Subclass,
				RequiredProficiency = "ShadowMonarchProficiencies",
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(Shadow Monarch) \"My pulse is steady and my memory is clearing, inquisitor. The only shadows following me are the ones your festival banners cast on the cobblestones. There is no need for threats.\"",
				NpcReplyText = "{n}The shadows beneath Hulrun's boots seem to stillness as you meet his gaze, causing the veteran inquisitor to pause.{/n} \"You speak quietly, yet there is an unnatural coldness in your eyes. If Terendelev had not tended to you with her own hands, I would have you detained for questioning. Step aside.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Pale Lady",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Market Square: Shadow Presence",
					CosmicCoins = 80,
					Lines = new List<string> { "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> murmurs: \"An inquisitor blind to the genuine planar boundaries of shadow and abyss.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterHorgusNegotiation()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act1_Horgus_EscortNegotiation", "12e42316950f8c9498afb8b0fb2baaae", "Act1", "Underground Caverns: Escort fee negotiation with Horgus Gwerm");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Horgus_Negotiation_Universal_HazardPay",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				SpeakerGuid = "c02e641bf8cf0984fb49604afa224563",
				ExcludeVanillaAnswerGuid = "3ab564082485b034a9d0a7b550e1a3e2",
				CueOnStopActionList = ActionFlow.DoSingle(delegate(UnlockFlag c)
				{
					c.m_flag = BlueprintTools.GetBlueprintReference<BlueprintUnlockableFlagReference>("ddfedbdeab95ed941b6968b06162c921");
					c.flagValue = 2000;
				}),
				PromptText = "(Isekai Protagonist) \"Two thousand gold to escort a high nobleman through monster-infested caverns? That barely covers hazard pay, Sir Gwerm, but considering your daughter's safety is on the line, we have an agreement.\"",
				NpcReplyText = "{n}Horgus scowls, his mustache twitching with indignation as he glances at Camellia.{/n} \"Hazard pay?! Such impudence from a stranger! But fine... seeing as we are trapped under a collapsed city, two thousand gold it is. Just keep those subterranean beasts away from us!\"",
				Rewards = new SceneRewards
				{
					Gold = 2000,
					CosmicCoins = 100
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Quest,
					SceneContext = "Caverns: Gwerm Hazard Pay",
					CosmicCoins = 100,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs loudly: \"Nothing like extracting two thousand gold from an arrogant nobleman before breakfast! Well negotiated!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterKaylessaAmbush()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act1_Kaylessa_TranAmbush", "b8e680587a76f064fac9a01034c02391", "Act1", "Kenabres Ruins: Tran's betrayal ambush in Kaylessa's hunt");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Kaylessa_Ambush_Universal_SeeThrough",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				NextCueGuid = "a8cc736feec11024eb6a5d3dbcb69f5c",
				PromptText = "(Isekai Protagonist) [Attack] \"A convincing disguise, but your monologue gave away your stance. Draw your steel and let us see if your blade matches your deceit.\"",
				NpcReplyText = "{n}The treacherous elf snarls in shock, his false smile twisting into an ugly grimace as cultist blades flash in the dark.{/n} \"Curse you! Kill them! Leave none of them alive!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 100
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Our Lord in Iron",
					Category = ConstellationCategory.Quest,
					SceneContext = "Kenabres Ruins: Unmasking Tran",
					CosmicCoins = 100,
					Lines = new List<string> { "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows: \"Words mean nothing! Let the blood on the cobblestones prove who stands supreme!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterWardstoneAwakening()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act1_Wardstone_MythicAwakening", "de1e8e44fc1139c40bb04209e5e8b75d", "Act1", "Gray Garrison: Awakening dormant mythic resonance at the Wardstone");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Wardstone_ThirdOption_HarmonizeMana",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Otherworld Resonance) \"Do not destroy the stone or surrender to corruption! I am channeling my dimensional mana to purify the celestial core and anchor it permanently to our vanguard!\"",
				NpcReplyText = "{n}Blinding golden starlight erupts through the fractures of the Wardstone, blasting the corrupted tendrils into vapor and settling into your chest like a burning celestial star.{/n}",
				Rewards = new SceneRewards
				{
					Gold = 1000,
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.Quest,
					SceneContext = "Gray Garrison: Wardstone Mana Harmonization",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> proclaims with divine power: \"The starlight of the Wardstone endures! Arise, herald of the Fifth Crusade!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDesnaTempleCommunion()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act1_Desna_ArankaCommunion", "259e4a8e097ab2c4c977e78ffeea4853", "Act1", "Temple of Desna: Communion with the priests of the Song of the Spheres");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Desna_Communion_Universal_StarlightSong",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"The Song of the Spheres sounds like the melodies I remember from another life across the stars. Play on, disciples of Desna; your music brings hope to this burning city.\"",
				NpcReplyText = "{n}Aranka smiles warmly, a silver butterfly fluttering about her lute.{/n} \"You understand the music of dreams, Commander. Desna's starlight shall light your road through the darkest shadows of Kenabres.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 120
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Quest,
					SceneContext = "Temple of Desna: Starlight Melody",
					CosmicCoins = 120,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles warmly: \"Follow your dreams across the night sky, traveler. Hope is never extinguished.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
