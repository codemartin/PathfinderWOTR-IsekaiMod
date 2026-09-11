using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal static class IsekaiEpilogueSlides
	{
		private const string CueSequencePlayerFinalChoiceGuid = "a3096e5b145badb448827a7336d86d02";

		public static void Add()
		{
			CreateMastermindEpilogueSlides();
			CreateMartialGodEpilogueSlides();
			CreateGeneralEpilogueSlides();
			HookIntoEpilogueSequence();
		}

		private static void CreateMastermindEpilogueSlides()
		{
			TTCoreExtensions.CreateCue("IsekaiEndingGrandArchitectSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}In the centuries following the Fifth Crusade, historians would marvel at how smoothly the broken realm rebuilt. Famines were mysteriously averted by anonymous grain shipments; border skirmishes between Cheliax and Mendev evaporated before blood was spilled, dissolved by subtle economic pressures and forged alliances whose origins remained enigmatic. Behind the veil of mortal politics, you orchestrated a grand renaissance of statecraft and enlightenment. You never wore a crown or sought a statue in your honor; your true monument was a living, flourishing continent that never knew it was guided by the gentle hand of an unseen architect.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "IsekaiEndingGrandArchitect";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingPuppetParliamentSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}The mortal kings, high priests, and crusader generals thought they had retained their sovereignty. Yet every decision of state, every royal decree, and every economic pact was drafted in an unassuming office in Drezen. You installed compliant figureheads upon every influential throne across northern Avistan, binding kings and archbishops into an invisible puppet parliament. When crises arose, your quiet directives mobilized armies and redistributed treasuries without the common folk ever suspecting that the Fifth Crusade had replaced chaotic demon lords with an omniscient shadow government.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "IsekaiEndingPuppetParliament";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingChessboardApocalypseSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}To your detached intellect, Golarion was never a home to cherish: it was merely a sandbox simulation of mortal psychology and planar physics. You orchestrated sweeping wars, manufactured diplomatic catastrophes, and collapsed continental banking cartels purely to test your strategic models. When entire nations fell into engineered ruin, you felt neither guilt nor malice, only the cold satisfaction of an experiment yielding novel data. Across the multiverse, deities and fiends watched in creeping dread as a mortal mind turned reality itself into a merciless game of speed chess.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "IsekaiEndingChessboardApocalypse";
				});
			});
		}

		private static void CreateMartialGodEpilogueSlides()
		{
			TTCoreExtensions.CreateCue("IsekaiEndingInfiniteHorizonsSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}With a single transcendent strike of pure sovereign Ki, you severed the Worldwound away from reality, cutting cleanly through the causal loop holding Golarion hostage. The ancient anchors of Yog-Sothoth shattered into cosmic dust, opening the unwritten future. Transcending mortal form, you stepped out into the boundless astral slipstream, ascending as the Sovereign God of the Martial Dao. Across infinite dimensions, wanderers and martial artists speak of the Peerless Sovereign whose divine Ki shatters destiny itself.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "IsekaiEndingInfiniteHorizons";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingBoundaryHermitSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Forsaking mortal dominion and celestial adoration, you stepped into a quiet, forgotten pocket dimension on the edge of the Astral Plane. There, beneath timeless skies, you sat in meditative stillness, condensing your internal spirit and refining the ultimate strike of supreme enlightenment. Decades passed in silent harmony with the Dao of Ki. Yet just as your consciousness touched the apex of ultimate perfection, the subtle chime of Yog-Sothoth's celestial hourglass echoed through the void, and time gently looped back to the beginning...{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "IsekaiEndingBoundaryHermit";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingSeveredHorizonSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Your boundless Ki detonated beyond all mortal and divine limiters, cleaving through the dimensional tapestry of reality with catastrophic force. The Worldwound was destroyed, but the strike cut too deep: the foundational threads anchoring Golarion to existence snapped like dry silk. Continents dissolved into shimmering iridescence and fading cosmic static. In the silent, empty void that remained, you stood alone, the undisputed sovereign of an empty cosmos, bearing witness to the terrible consequence of unrestrained power.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "IsekaiEndingSeveredHorizon";
				});
			});
		}

		private static void CreateGeneralEpilogueSlides()
		{
			TTCoreExtensions.CreateCue("IsekaiEndingUnwrittenDawnSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}The causal circle of Yog-Sothoth has shattered forever. The endless replay of the Fifth Crusade has ended, and for the first time in ten thousand cycles, Golarion steps into an unwritten dawn. A permanent interdimensional gateway stands open between Kenabres and Earth, bridging magic and technology in an unprecedented cosmic renaissance.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "IsekaiEndingUnwrittenDawn";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingAbyssalSingularitySlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Your bottomless appetite plunged into the deepest chasms of the Abyss, devouring demonic lords and tearing away entire planar layers. As your ravenous hunger began to disturb the slumber of Azathoth at the center of infinity, a panicked Yog-Sothoth activated the emergency temporal failsafe, instantly resetting reality back to the start of the crusade to preserve the multiverse from total annihilation.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "IsekaiEndingAbyssalSingularity";
				});
			});
		}

		private static void HookIntoEpilogueSequence()
		{
			BlueprintCueSequence blueprint = BlueprintTools.GetBlueprint<BlueprintCueSequence>("a3096e5b145badb448827a7336d86d02");
			if (blueprint == null || blueprint.Cues == null)
			{
				return;
			}
			string[] array = new string[8] { "IsekaiEndingGrandArchitectSlide", "IsekaiEndingPuppetParliamentSlide", "IsekaiEndingChessboardApocalypseSlide", "IsekaiEndingInfiniteHorizonsSlide", "IsekaiEndingBoundaryHermitSlide", "IsekaiEndingSeveredHorizonSlide", "IsekaiEndingUnwrittenDawnSlide", "IsekaiEndingAbyssalSingularitySlide" };
			foreach (string name in array)
			{
				BlueprintCue modBlueprint = BlueprintTools.GetModBlueprint<BlueprintCue>(Main.IsekaiContext, name);
				if (modBlueprint != null)
				{
					blueprint.Cues.Add(modBlueprint.ToReference<BlueprintCueBaseReference>());
				}
			}
		}
	}
}
