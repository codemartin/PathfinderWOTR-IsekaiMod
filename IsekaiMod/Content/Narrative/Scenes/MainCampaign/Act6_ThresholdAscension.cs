using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Narrative.Actions;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.MainCampaign
{
	public static class Act6_ThresholdAscension
	{
		public static void Register()
		{
			RegisterThresholdFinalConfrontation();
			RegisterPharasmaAstralJudgment();
		}

		private static void RegisterThresholdFinalConfrontation()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act6_Threshold_FinalShowdown", "bb3528754c7e741438acef95ec3b6430", "Act6", "Threshold: Final confrontation with Areelu Vorlesh at the Worldwound rift");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Threshold_Universal_ShatterScript",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				CustomAction = new ContextActionCompleteBounty
				{
					BountyId = "Yog_Threshold_LoopBreaker"
				},
				PromptText = "(Isekai Protagonist) \"We stand at the precipice of reality, Areelu. You thought your grief could justify bleeding an entire world for a century. But this story doesn't end with your sacrifice or mine; it ends with the wound sealed and our souls choosing their own destiny!\"",
				NpcReplyText = "{n}Areelu's eyes wide with profound revelation, a gentle, sorrowful smile gracing her lips as her abyssal stitching begins to dissolve into pure starlight.{/n} \"You truly are... someone who walked beyond the edge of the world. Show me, child of another universe... show me how this story ends.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 1500
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "Threshold: Shattering the Tragic Script",
					CosmicCoins = 1500,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> thunders through the celestial firmament: \"THE INK FLOWS BACK INTO THE PEN. THE FINAL CHAPTER IS WRITTEN BY YOUR OWN HAND.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Threshold_MetaLoop_CloseTheLoop",
				Type = OptionType.MetaLoop,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				CustomAction = new ContextActionCompleteBounty
				{
					BountyId = "Yog_Threshold_LoopBreaker"
				},
				PromptText = "(Close the Causal Loop) \"I remember every death, every reset, every bitter tears shed across a hundred cycles, Areelu. The tape ends here. The projector shuts down. Together, we step into the Unwritten Dawn!\"",
				NpcReplyText = "{n}The dimensional rift behind Vorlesh shatters into a billion glittering crystal mirrors, each reflecting a past timeline dissolving into golden dawn.{/n} \"The causal cycle... is closed. We are free.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 2500
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.MetaLoop,
					SceneContext = "Threshold: Closing the Causal Loop into Unwritten Dawn",
					CosmicCoins = 2500,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> whispers in absolute stillness: \"The loop is broken. The tape has run out. Welcome, sovereign traveler, to the true horizon.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterPharasmaAstralJudgment()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Act6_Pharasma_AstralJudgment", "d33f9c38d05c4d04c99b2d6b76b7d962", "Act6", "Pharasma's Spire: Standing before the Lady of Graves for Astral Judgment");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Pharasma_Astral_Universal_OtherworldSoul",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"Lady of Graves, your scroll chronicles every soul born under Golarion's sky. But I crossed from beyond the Great Beyond. My destiny belongs neither to the Boneyard nor the Abyss. My soul is unwritten.\"",
				NpcReplyText = "{n}Pharasma gazes down from her throne of white marble, her ageless eyes reflecting infinities of stars not recorded in her ledger.{/n} \"A traveler whose thread was spun in distant heavens. You healed this fractured realm, and your fate cannot be weighed upon mortal scales. Walk freely between worlds, Otherworlder.\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 2000
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Pale Lady",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "Pharasma's Spire: The Unwritten Soul",
					CosmicCoins = 2000,
					Lines = new List<string> { "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with solemn finality: \"No ledger can contain a soul that chose its own horizon. Depart in peace.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
