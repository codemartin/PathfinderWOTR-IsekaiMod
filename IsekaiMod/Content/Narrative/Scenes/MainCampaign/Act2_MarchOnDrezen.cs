using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Narrative.Actions;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.MainCampaign
{
	public static class Act2_MarchOnDrezen
	{
		public static void Register()
		{
			RegisterQueenGalfreyWarCamp();
			RegisterLepersSmileVescavor();
			RegisterLostChapelRescue();
			RegisterRegillHellknightAlliance();
			RegisterDrezenCitadelProclamation();
		}

		private static void RegisterQueenGalfreyWarCamp()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act2_Galfrey_WarCampAudience", "e72da09ea23353949b8ab2b8d180eb98", "Act2", "Crusader War Camp: Commissioning by Queen Galfrey");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Galfrey_WarCamp_Universal_Logistics",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"Your Majesty, heroic valor is admirable, but an army marches on its stomach and its supply lines. Give me command of the vanguard, and I will ensure our logistics are as unyielding as our blades.\"",
				NpcReplyText = "{n}Queen Galfrey's tired eyes brighten with genuine respect.{/n} \"A commander who understands supply and discipline over reckless glory. Mendev has lacked such pragmatic leadership for far too long. Take the banner, Knight-Commander!\"",
				Rewards = new SceneRewards
				{
					Gold = 1500,
					CosmicCoins = 150
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "War Camp: Galfrey Vanguard Commission",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> nods solemnly: \"Logistics and morale are the twin wings of victory. Well spoken, Commander!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Galfrey_WarCamp_GodEmperor_Sovereignty",
				Type = OptionType.Subclass,
				RequiredProficiency = "GodEmperorProficiencies",
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(God Emperor) \"You seek to reclaim a fortress; I intend to reforge this continent into an empire where demons tremble to tread. Place the crusade under my standard, Queen of Mendev, and witness true dominion.\"",
				NpcReplyText = "{n}The royal guards reach for their pommels, but Galfrey raises an authoritative hand, gazing at you with quiet awe.{/n} \"Your presence is... overwhelming. If your strength matches your regal conviction, Drezen will fall before our banners within the week.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Subclass,
					SceneContext = "War Camp: Imperial Mandate",
					CosmicCoins = 160,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with supreme approval: \"Commanding a queen to yield authority while making her grateful for the privilege. Perfection.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterLepersSmileVescavor()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act2_LepersSmile_VescavorPheromone", "19bae6fd6767f424fafc3287d8ca4388", "Act2", "Leper's Smile: Countering the Vescavor swarm assault");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "LepersSmile_ThirdOption_Dispersal",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Otherworld Resonance) \"Do not sacrifice soldiers or burn innocent pack animals! I am saturating the canyon with an inverted acoustic harmonic pulse that disorients the Vescavors' hive antennae!\"",
				NpcReplyText = "{n}A high-frequency resonance hums across the limestone cliffs. The swirling clouds of flesh-eating insects screech in agony, colliding blindly with the stone before scattering into the wind.{/n}",
				Rewards = new SceneRewards
				{
					Gold = 1000,
					CosmicCoins = 180
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Quest,
					SceneContext = "Leper's Smile: Acoustic Swarm Dispersal",
					CosmicCoins = 180,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> rejoices: \"Harmonic vibration triumphing over ravenous chaos without spilling crusader blood! Splendid!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "LepersSmile_Slime_Gluttony",
				Type = OptionType.Subclass,
				RequiredProficiency = "DevourerProficiencies",
				Alignment = AlignmentShiftDirection.ChaoticNeutral,
				PromptText = "(Slime) \"A multi-million insectoid protein buffet? Stand back, everyone. Predator Arts: Infinite Stomach active! Let us see if this swarm can out-eat my digestion!\"",
				NpcReplyText = "{n}A vortex of voracious mana erupts around you, swallowing thousands of screaming Vescavors into a swirling digestive singularity until the skies are completely empty.{/n}",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Great Devourer",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Leper's Smile: Slime Swarm Feast",
					CosmicCoins = 200,
					Lines = new List<string> { "<color=#8B0000><b>[The Constellation 'The Rough Beast']</b></color> roars in subterranean delight: \"SWALLOW THE SWARM! DEVOUR THE RAVENOUS!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterLostChapelRescue()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act2_LostChapel_CompanionsRescue", "364877496cda83640a9663b45ed0fc10", "Act2", "Lost Chapel: Liberating crusader companions from ghoul slaughter");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "LostChapel_Universal_RallyCompanions",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) \"You took my companions and slaughtered my scouts while we slept. Did you truly believe a cliffside ambush would break the resolve of an Otherworlder? Pay for your insolence!\"",
				NpcReplyText = "{n}The rescued companions cheer as holy radiance washes down the desecrated altar steps, sending the ghoul priest shying away in terror.{/n}",
				Rewards = new SceneRewards
				{
					Gold = 1200,
					CosmicCoins = 150
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "Lost Chapel: Rallying the Faithful",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> proclaims: \"Never surrender your companions to the dark! Strike down the desecrators!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterRegillHellknightAlliance()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act2_Regill_HellknightAlliance", "5128a604b13cf554a846ab7c16054cbf", "Act2", "Gargoyle Canyon: Alliance with Paralictor Regill Derenge");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Regill_Alliance_Universal_Pragmatism",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				CustomAction = new ContextActionCompleteBounty
				{
					BountyId = "Asmodeus_Regill_Alliance"
				},
				PromptText = "(Isekai Protagonist) \"Your ruthlessness made my stomach turn, Paralictor. But your discipline kept your men alive while chaotic knights died in panic. The Fifth Crusade needs unyielding steel. Fall in behind me.\"",
				NpcReplyText = "{n}Regill straightens his cold iron collar, meeting your gaze with unblinking appraisal.{/n} \"A commander who sets emotion aside to recognize tactical necessity. The Order of the Godclaw accepts your command. We march on Drezen.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 150
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "Father of Creation",
					Category = ConstellationCategory.Companion,
					SceneContext = "Hellknight Canyon: Regill Alliance",
					CosmicCoins = 150,
					Lines = new List<string> { "<color=#D2691E><b>[The Constellation 'Father of Creation']</b></color> grunts in firm approval: \"Discipline and unyielding stone. A fine alliance for the trials ahead.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Regill_Alliance_Mastermind_Audit",
				Type = OptionType.Subclass,
				RequiredProficiency = "MastermindProficiencies",
				Alignment = AlignmentShiftDirection.LawfulEvil,
				CustomAction = new ContextActionCompleteBounty
				{
					BountyId = "Asmodeus_Regill_Alliance"
				},
				PromptText = "(Mastermind) \"Your triage ratio was 74% effective given terrain constraints, Paralictor. With my strategic logistics supporting your Godclaw phalanges, our casualty projections drop by half. An efficient proposition, wouldn't you agree?\"",
				NpcReplyText = "{n}Regill's eyebrow raises a fraction of a millimeter--the gnome's equivalent of profound shock.{/n} \"Accurate arithmetic. Rare among crusaders. You have earned our unwavering compliance, Commander.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Hellknight Canyon: Tactical Audit",
					CosmicCoins = 160,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with cold satisfaction: \"Out-calculating a Hellknight at his own philosophy. Impeccable.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterDrezenCitadelProclamation()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act2_Drezen_CitadelProclamation", "0c390d22c44f76c4aaa053ee987836d1", "Act2", "Drezen Citadel: Raising the Sword of Valor and reclaiming the bastion");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Drezen_Proclamation_Universal_TurnTheTide",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"For seventy years, humanity bled while demon lords laughed. Today, Drezen returns to the mortal world! Raise the Sword of Valor, and let every fiend from here to the Abyss know that the Fifth Crusade has arrived to end this war!\"",
				NpcReplyText = "{n}Thunderous roars erupt from thousands of crusaders along the battlements as the sacred banner flares with brilliant, unyielding starlight.{/n}",
				Rewards = new SceneRewards
				{
					Gold = 5000,
					CosmicCoins = 300
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "Drezen Citadel: Raising the Sword of Valor",
					CosmicCoins = 300,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> proclaims across the heavens: \"THE CRUSADE STANDS RESPLENDENT! DREZEN IS REBORN IN VALOR!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Drezen_Proclamation_ShadowMonarch_ClaimCitadel",
				Type = OptionType.Subclass,
				RequiredProficiency = "ShadowMonarchProficiencies",
				Alignment = AlignmentShiftDirection.NeutralEvil,
				PromptText = "(Shadow Monarch) \"Let the sun set on Drezen; its true defense rises from the Netherworld. Every fallen soldier who gave their life on these walls is welcomed into my eternal legion. Arise, and guard your home!\"",
				NpcReplyText = "{n}Shadows lengthen across the courtyard as silent, armored silhouettes raise phantom broadswords along the parapets, standing eternal watch.{/n}",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Pale Lady",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Drezen Citadel: Shadow Legion Proclamation",
					CosmicCoins = 300,
					Lines = new List<string> { "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> watches in silence: \"An army that does not sleep. The Worldwound has never seen such a fortress.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
