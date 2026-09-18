using System.Collections.Generic;
using System.Linq;
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
			CreateOverlordEpilogueSlides();
			CreateSlimeEpilogueSlides();
			CreateHeroEpilogueSlides();
			CreateGodEmperorEpilogueSlides();
			CreateShadowMonarchEpilogueSlides();
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
					c.EndingId = "GrandArchitect";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingPuppetParliamentSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}The mortal kings, high priests, and crusader generals thought they had retained their sovereignty. Yet every decision of state, every royal decree, and every economic pact was drafted in an unassuming office in Drezen. You installed compliant figureheads upon every influential throne across northern Avistan, binding kings and archbishops into an invisible puppet parliament. When crises arose, your quiet directives mobilized armies and redistributed treasuries without the common folk ever suspecting that the Fifth Crusade had replaced chaotic demon lords with an omniscient shadow government.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "PuppetParliament";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingChessboardApocalypseSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}To your detached intellect, Golarion was never a home to cherish: it was merely a sandbox simulation of mortal psychology and planar physics. You orchestrated sweeping wars, manufactured diplomatic catastrophes, and collapsed continental banking cartels purely to test your strategic models. When entire nations fell into engineered ruin, you felt neither guilt nor malice, only the cold satisfaction of an experiment yielding novel data. Across the multiverse, deities and fiends watched in creeping dread as a mortal mind turned reality itself into a merciless game of speed chess.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "ChessboardApocalypse";
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
					c.EndingId = "InfiniteHorizons";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingBoundaryHermitSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Forsaking mortal dominion and celestial adoration, you stepped into a quiet, forgotten pocket dimension on the edge of the Astral Plane. There, beneath timeless skies, you sat in meditative stillness, condensing your internal spirit and refining the ultimate strike of supreme enlightenment. Decades passed in silent harmony with the Dao of Ki. Yet just as your consciousness touched the apex of ultimate perfection, the subtle chime of Yog-Sothoth's celestial hourglass echoed through the void, and time gently looped back to the beginning...{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "BoundaryHermit";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingSeveredHorizonSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Your boundless Ki detonated beyond all mortal and divine limiters, cleaving through the dimensional tapestry of reality with catastrophic force. The Worldwound was destroyed, but the strike cut too deep: the foundational threads anchoring Golarion to existence snapped like dry silk. Continents dissolved into shimmering iridescence and fading cosmic static. In the silent, empty void that remained, you stood alone, the undisputed sovereign of an empty cosmos, bearing witness to the terrible consequence of unrestrained power.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "SeveredHorizon";
				});
			});
		}

		private static void CreateOverlordEpilogueSlides()
		{
			TTCoreExtensions.CreateCue("IsekaiEndingPaxSepulchrumSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Beneath the sovereign authority of the Great Tomb, northern Avistan entered an era of eerie, unassailable peace. Legions of deathless bone laborers tirelessly tilled the soil, dug silver mines, and rebuilt the ruined cities of Sarkoris without demanding food or coin. The mortal citizenry lived in unprecedented safety and prosperity, their lives protected by an impartial, immortal monarch whose gaze saw through all mortal deception. Historians would debate whether the Supreme One was a savior or a tyrant, but none could deny that under the banner of the Great Tomb, the land knew neither hunger nor fear.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "PaxSepulchrum";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingIronCitadelSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Turning your back on the chaotic kingdoms of the Inner Sea, you raised monumental obsidian ramparts around the borders of Sarkoris, sealing the realm into an impregnable necropolis. Within the Iron Citadel, ancient secrets of necromancy, planar magic, and deathless engineering were studied in eternal silence. Kings and inquisitors sent scouts to peer across the border, but none returned; the silence of the Iron Citadel was absolute, an eternal monument to a sovereign who chose solitude over the fickle gratitude of mortals.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "IronCitadel";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingNecroTyrantSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Convinced that the fragility of mortal emotion was the root cause of every planar catastrophe, you unleashed an all-consuming necrotic harvest across Avistan. Millions of living souls were gently severed from their frail vessels, inducted into an eternal, perfectly disciplined legion of undeath. The squabbling of kings and the cries of the starving ceased forever. Northern Avistan became a silent, immaculate paradise of bone and iron, ruled by an eternal Necro-Tyrant whose empire would outlive the stars.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "NecroTyrant";
				});
			});
		}

		private static void CreateSlimeEpilogueSlides()
		{
			TTCoreExtensions.CreateCue("IsekaiEndingTempestFederationSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}By metabolizing the chaotic abyssal rift into pure, fertile life-mana, you transformed the scarred wasteland of Sarkoris into the blossoming Tempest Multiverse Federation. Where demonic corruption once festered, lush primeval forests and crystal hot springs flourished. Mongrels, humans, reformed fiends, and fey lived side by side in harmony, drawn together by your boundless benevolence, delectable culinary festivals, and radiant hospitality. The Federation became a beacon of joy and cultural renaissance celebrated across every plane in the Great Beyond.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "TempestFederation";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingPrimevalMarshSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Sated with adventure and planar glory, you allowed nature to reclaim the shattered scars of the Worldwound, dissolving the ruins into a peaceful primeval marsh. Beneath the ancient roots of a revitalized World Tree, you curled into a tranquil, slumbering sphere of living jelly, watching over the wild beasts and wandering spirits of Golarion. Whenever dark shadows stirred in the borderlands, the great slime god would briefly stir, absorb the threat, and quietly return to an eternal, contented nap.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "PrimevalMarsh";
				});
			});
		}

		private static void CreateHeroEpilogueSlides()
		{
			TTCoreExtensions.CreateCue("IsekaiEndingEternalHopeSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Refusing the solitary glory of godhood or the burden of crowns, you channeled the collective hopes, prayers, and sacrifices of every crusader who ever bled for Kenabres into a radiant celestial nova. Ascending as the Sovereign Beacon of Eternal Hope, your starlight became a perpetual compass across the multiverse for those lost in darkness. Wanderers in distress, soldiers facing impossible odds, and children weeping in the night would look to the skies and see your golden dawn, knowing they were never truly alone.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "EternalHope";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingRetiredLegendSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}When the crusade was won, you quietly hung up your legendary weapons, declined every title Queen Galfrey offered, and opened a cozy, warm-lit tavern and training hall in the heart of rebuilt Kenabres. Crusaders, adventurers, and travelers from distant lands flocked to your counter, eager to hear tales of the Otherworld and taste exotic spiced dishes no tavern in Avistan had ever served. Surrounded by beloved companions and the laughter of friends, you lived out a fulfilled, peaceful mortal life, proving that the greatest adventure of all is finding a place to call home.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "RetiredLegend";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingCorruptedChampionSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}The horrific cost of victory broke something fundamental within your spirit. Having witnessed the slaughter of so many comrades, you vowed that peace would be maintained at any cost. Your crusade for justice hardened into an iron-fisted, despotic regime that executed dissenters and crushed independent thought before rebellion could take root. You became the very monster you once swore to destroy, an immortal protector whose suffocating embrace terrorized the very people you sought to save.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "CorruptedChampion";
				});
			});
		}

		private static void CreateGodEmperorEpilogueSlides()
		{
			TTCoreExtensions.CreateCue("IsekaiEndingPaxSolarisSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Ascending the Golden Throne of Sarkoris, you proclaimed the Pax Solaris, an age of solar renaissance and technological enlightenment. Massive solar relays and radiant aqueducts crisscrossed the reclaimed plains, powering automated foundries and crystal streetlamps. The Solar Commonwealth integrated magic and advanced science, establishing an egalitarian golden age where disease was eradicated and mortal potential reached heights unseen since ancient Azlant. The God Emperor's light shone unclouded across Avistan.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "PaxSolaris";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingSarkorianImperiumSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Uniting the broken remnants of Mendev, Sarkoris, and Numeria beneath an unyielding golden banner, you forged the Sarkorian Imperium. Governed by strict imperial codices and defended by mechanized legionaries, the Imperium tolerated neither demonic heresy nor noble corruption. Neighboring kingdoms bowed before your economic and military supremacy, trading their sovereignty for absolute security. Order had come to northern Avistan, etched in bronze and iron by the God Emperor's decree.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "SarkorianImperium";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingDualThroneSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Your supreme will refused to be bounded by planar frontiers. Marching your solar legions into the depths of the Midnight Isles, you subjugated both the mortal kingdoms of Avistan and the chaotic fiefdoms of the Abyss under the undisputed authority of the Dual Throne. Demons knelt beside crusader paladins, bound by unbreakable imperial oaths. From your seat between light and shadow, you established a terrifying, cosmic equilibrium that bridged heaven and hell under one absolute monarch.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "DualThrone";
				});
			});
		}

		private static void CreateShadowMonarchEpilogueSlides()
		{
			TTCoreExtensions.CreateCue("IsekaiEndingSilentAegisSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Rather than ruling from a sunlit palace, you commanded the Legion of Shadows to melt into the twilight of northern Avistan. Invisible and eternal, your shadow soldiers kept vigil over every lonely road, darkened forest, and humble village. Bandits and cultists vanished without a trace into the night, while honest folk slept peacefully under stars guarded by an unseen sovereign. Darkness was no longer an omen of evil, but a protective shield wielded by the Monarch of Shadows.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "SilentAegis";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingObsidianBastionSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Where the Worldwound once tore reality apart, you erected the Obsidian Bastion, a towering fortress of black crystal that pierced the sky. Seated upon the obsidian throne with millions of loyal shadow soldiers standing at attention across the planar horizon, you became the immortal warden of the threshold. Neither abyssal lords nor conquering archons dared approach the rift, knowing that beyond the obsidian gates lay the absolute dominion of the Shadow Monarch.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "ObsidianBastion";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingNetherworldCataclysmSlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Embracing the cold, boundless void of the Netherworld, you dissolved the boundary between life and shadow, plunging northern Avistan into an eternal, starless eclipse. Flesh and bone withered away as mortals, beasts, and demons were remade into pure, loyal shadows. Across the silent, frozen continent, no breath was drawn, no cry was uttered. In the absolute quiet of the shadow realm, you reigned supreme over an empire of eternal stillness.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "NetherworldCataclysm";
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
					c.EndingId = "UnwrittenDawn";
				});
			});
			TTCoreExtensions.CreateCue("IsekaiEndingAbyssalSingularitySlide", delegate(BlueprintCue bp)
			{
				bp.SetText(Main.IsekaiContext, "{n}Your bottomless appetite plunged into the deepest chasms of the Abyss, devouring demonic lords and tearing away entire planar layers. As your ravenous hunger began to disturb the slumber of Azathoth at the center of infinity, a panicked Yog-Sothoth activated the emergency temporal failsafe, instantly resetting reality back to the start of the crusade to preserve the multiverse from total annihilation.{/n}");
				bp.Conditions = ActionFlow.IfSingle(delegate(ConditionCheckIsekaiEnding c)
				{
					c.EndingId = "AbyssalSingularity";
				});
			});
		}

		private static void HookIntoEpilogueSequence()
		{
			BlueprintCueSequence blueprint = BlueprintTools.GetBlueprint<BlueprintCueSequence>("a3096e5b145badb448827a7336d86d02");
			if (blueprint == null)
			{
				return;
			}
			BlueprintCueSequence blueprintCueSequence = blueprint;
			if (blueprintCueSequence.Cues == null)
			{
				blueprintCueSequence.Cues = new List<BlueprintCueBaseReference>();
			}
			string[] array = new string[22]
			{
				"IsekaiEndingGrandArchitectSlide", "IsekaiEndingPuppetParliamentSlide", "IsekaiEndingChessboardApocalypseSlide", "IsekaiEndingInfiniteHorizonsSlide", "IsekaiEndingBoundaryHermitSlide", "IsekaiEndingSeveredHorizonSlide", "IsekaiEndingPaxSepulchrumSlide", "IsekaiEndingIronCitadelSlide", "IsekaiEndingNecroTyrantSlide", "IsekaiEndingTempestFederationSlide",
				"IsekaiEndingPrimevalMarshSlide", "IsekaiEndingEternalHopeSlide", "IsekaiEndingRetiredLegendSlide", "IsekaiEndingCorruptedChampionSlide", "IsekaiEndingPaxSolarisSlide", "IsekaiEndingSarkorianImperiumSlide", "IsekaiEndingDualThroneSlide", "IsekaiEndingSilentAegisSlide", "IsekaiEndingObsidianBastionSlide", "IsekaiEndingNetherworldCataclysmSlide",
				"IsekaiEndingUnwrittenDawnSlide", "IsekaiEndingAbyssalSingularitySlide"
			};
			foreach (string name in array)
			{
				BlueprintCue cue = BlueprintTools.GetModBlueprint<BlueprintCue>(Main.IsekaiContext, name);
				if (cue != null)
				{
					BlueprintCueBaseReference item = cue.ToReference<BlueprintCueBaseReference>();
					if (!blueprint.Cues.Any((BlueprintCueBaseReference c) => c != null && ((BlueprintReferenceBase)c).deserializedGuid == cue.AssetGuid))
					{
						blueprint.Cues.Add(item);
					}
				}
			}
		}
	}
}
