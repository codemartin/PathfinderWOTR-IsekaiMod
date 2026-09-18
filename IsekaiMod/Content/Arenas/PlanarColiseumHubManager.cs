using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.View;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace IsekaiMod.Content.Arenas
{
	public static class PlanarColiseumHubManager
	{
		private class ColiseumHubSpawnHandler : IAreaHandler, ISubscriber, IGlobalSubscriber
		{
			public void OnAreaDidLoad()
			{
				try
				{
					string text = Game.Instance?.CurrentlyLoadedArea?.name ?? "";
					if (!(Game.Instance?.CurrentlyLoadedArea?.AssetGuid.ToString() ?? "").Equals("a3f0d5be00238c649a42edb97e22b044", StringComparison.OrdinalIgnoreCase) && text.IndexOf("AreshkaArena", StringComparison.OrdinalIgnoreCase) < 0)
					{
						return;
					}
					IEnumerable<UnitEntityData> enumerable = Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>();
					BlueprintFaction neutrals;
					if (enumerable != null && enumerable.Any((UnitEntityData u) => u.Blueprint?.name == "AstralCoinEnvoyUnit"))
					{
						// The units survive in the save but their interaction does not, so it is attached again on every load.
						foreach (UnitEntityData existing in enumerable.ToList())
						{
							switch (existing.Blueprint?.name)
							{
								case "PlanarColiseumHeraldUnit": HubUnitFactory.AttachDialog(existing, PlanarColiseumEntrance.ColiseumHeraldDialog); break;
								case "AstralCoinEnvoyUnit": HubUnitFactory.AttachDialog(existing, AstralCoinEnvoyDialog); break;
								case "VoidMarketSmugglerUnit": HubUnitFactory.AttachDialog(existing, VoidMarketSmugglerDialog); break;
								case "ConstellationOracleUnit": HubUnitFactory.AttachDialog(existing, ConstellationOracleDialog); break;
							}
						}
					}
					else
					{
						UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
						if (!(unitEntityData == null) && Game.Instance?.LoadedAreaState?.MainState != null)
						{
							Vector3 position = unitEntityData.Position;
							Vector3 orientationDirection = unitEntityData.OrientationDirection;
							Vector3 vector = Vector3.Cross(Vector3.up, orientationDirection);
							neutrals = BlueprintTools.GetBlueprint<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad");
							SpawnHubUnit(PlanarColiseumEntrance.ColiseumHeraldUnit, PlanarColiseumEntrance.ColiseumHeraldDialog, position + orientationDirection * 6f);
							SpawnHubUnit(AstralCoinEnvoyUnit, AstralCoinEnvoyDialog, position + orientationDirection * 4f - vector * 8f);
							SpawnHubUnit(VoidMarketSmugglerUnit, VoidMarketSmugglerDialog, position + orientationDirection * 4f + vector * 8f);
							SpawnHubUnit(ConstellationOracleUnit, ConstellationOracleDialog, position + orientationDirection * 10f - vector * 6f);
							Main.IsekaiContext.Logger.Log("[PlanarColiseumHubManager] Successfully populated Planar Coliseum Hub with in-world NPCs.");
						}
					}
					UnitEntityData SpawnHubUnit(BlueprintUnit bp, BlueprintDialog dialog, Vector3 pos)
					{
						if (bp == null)
						{
							return null;
						}
						if (NavMesh.SamplePosition(pos, out var hit, 15f, -1))
						{
							pos = hit.position;
						}
						UnitEntityData unitEntityData2 = Game.Instance.EntityCreator.SpawnUnit(bp, pos, Quaternion.identity, Game.Instance.LoadedAreaState.MainState);
						if (unitEntityData2 != null)
						{
							if (neutrals != null)
							{
								unitEntityData2.SwitchFactions(neutrals);
							}
							if (dialog != null)
							{
								AttachDialog(unitEntityData2, dialog);
							}
						}
						return unitEntityData2;
					}
				}
				catch (Exception ex)
				{
					Main.IsekaiContext.Logger.LogError("Error in PlanarColiseumHubManager.OnAreaDidLoad: " + ex);
				}
			}

			public void OnAreaBeginUnloading()
			{
			}
		}

		public const string ArenaAreaGuid = "a3f0d5be00238c649a42edb97e22b044";

		private const string MonadicDevaStandardGuid = "7bcffd91514f489469a6fb6dea0a50d2";

		private const string NeutralsFactionGuid = "d8de50cc80eb4dc409a983991e0b77ad";

		public static BlueprintUnit AstralCoinEnvoyUnit;

		public static BlueprintUnit VoidMarketSmugglerUnit;

		public static BlueprintUnit ConstellationOracleUnit;

		public static BlueprintDialog AstralCoinEnvoyDialog;

		public static BlueprintDialog VoidMarketSmugglerDialog;

		public static BlueprintDialog ConstellationOracleDialog;

		public static string ColiseumReturnEnterPointGuid
		{
			get
			{
				if (!string.IsNullOrEmpty(TimelineManager.Data.LastArea))
				{
					return TimelineManager.Data.LastArea;
				}
				return "cdaba43118a82ff429b803ae846b35b0";
			}
			set
			{
				TimelineManager.Data.LastArea = value;
				TimelineManager.Save();
			}
		}

		public static void Init()
		{
			CreateHubBlueprints();
			EventBus.Subscribe(new ColiseumHubSpawnHandler());
		}

		private static void CreateHubBlueprints()
		{
			BlueprintUnit baseDeva = BlueprintTools.GetBlueprint<BlueprintUnit>("7bcffd91514f489469a6fb6dea0a50d2");
			AstralCoinEnvoyUnit = HubUnitFactory.Create(baseDeva, "AstralCoinEnvoyUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "The Astral Coin Envoy");
				if (baseDeva != null)
				{
					bp.m_Portrait = baseDeva.m_Portrait;
					bp.Prefab = baseDeva.Prefab;
					bp.Visual = baseDeva.Visual;
					bp.Gender = Gender.Female;
					bp.Size = baseDeva.Size;
					bp.Alignment = Alignment.TrueNeutral;
				}
				bp.AddComponent(delegate(AddSharedVendor c)
				{
					c.m_m_Table = CosmicRelicsVendorTable.Table.ToReference<BlueprintSharedVendorTableReference>();
				});
				bp.AddComponent<AddImmortality>();
			});
			DialogSpeaker envoySpeaker = new DialogSpeaker
			{
				m_Blueprint = AstralCoinEnvoyUnit.ToReference<BlueprintUnitReference>(),
				MoveCamera = false
			};
			AstralCoinEnvoyDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "AstralCoinEnvoyDialog", delegate(BlueprintDialog bp)
			{
				bp.Type = DialogType.Common;
				bp.Conditions = new ConditionsChecker
				{
					Conditions = Array.Empty<Condition>()
				};
				BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("Envoy_CueIntro", delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, "{n}An ethereal custodian surrounded by constellations of drifting starlight welcomes you with serene reverence.{/n}\n\n\"Greetings, champion of the mortal spheres. I oversee the celestial relic exchange. Defeating trials across the cosmos awards Cosmic Coins, which may be bartered here for supreme artifacts and eternal knowledge.\"");
					c.Speaker = envoySpeaker;
				});
				BlueprintAnswer ansTrade = TTCoreExtensions.CreateAnswer("Ans_Envoy_Trade", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "Browse the Cosmic Relics exchange (Trade Cosmic Coins).");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Envoy_Trade", delegate(BlueprintCue ct)
					{
						ct.SetText(Main.IsekaiContext, "\"Behold the treasures of the cosmos.\"");
						ct.Speaker = envoySpeaker;
						ct.OnStop = Helpers.CreateActionList(new ActionStartTradeWithEnvoy());
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansLore = TTCoreExtensions.CreateAnswer("Ans_Envoy_Lore", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "How do I earn more Cosmic Coins?");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Envoy_Lore", delegate(BlueprintCue cl)
					{
						cl.SetText(Main.IsekaiContext, "\"Cosmic Coins are awarded for conquering tournament cups in this coliseum, defeating rift bosses, progressing through New Game+ cycles, achieving milestones, and issuing commercial crusade decrees.\"");
						cl.Speaker = envoySpeaker;
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_Envoy_Leave", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "I will return when I possess more coins.");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Envoy_Leave", delegate(BlueprintCue cl)
					{
						cl.SetText(Main.IsekaiContext, "\"May the stars guide your path, otherworlder.\"");
						cl.Speaker = envoySpeaker;
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswersList bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "Envoy_AnswersList", delegate(BlueprintAnswersList al)
				{
					al.Answers = new List<BlueprintAnswerBaseReference>
					{
						ansTrade.ToReference<BlueprintAnswerBaseReference>(),
						ansLore.ToReference<BlueprintAnswerBaseReference>(),
						ansLeave.ToReference<BlueprintAnswerBaseReference>()
					};
				});
				blueprintCue.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
				bp.FirstCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { blueprintCue.ToReference<BlueprintCueBaseReference>() }
				};
			});
			VoidMarketSmugglerUnit = HubUnitFactory.Create(baseDeva, "VoidMarketSmugglerUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "The Void Market Smuggler");
				if (baseDeva != null)
				{
					bp.m_Portrait = baseDeva.m_Portrait;
					bp.Prefab = baseDeva.Prefab;
					bp.Visual = baseDeva.Visual;
					bp.Gender = Gender.Male;
					bp.Size = baseDeva.Size;
					bp.Alignment = Alignment.ChaoticNeutral;
				}
				bp.AddComponent(delegate(AddSharedVendor c)
				{
					c.m_m_Table = OtherworldLuxuryVendorTable.Table.ToReference<BlueprintSharedVendorTableReference>();
				});
				bp.AddComponent<AddImmortality>();
			});
			DialogSpeaker smugglerSpeaker = new DialogSpeaker
			{
				m_Blueprint = VoidMarketSmugglerUnit.ToReference<BlueprintUnitReference>(),
				MoveCamera = false
			};
			VoidMarketSmugglerDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "VoidMarketSmugglerDialog", delegate(BlueprintDialog bp)
			{
				bp.Type = DialogType.Common;
				bp.Conditions = new ConditionsChecker
				{
					Conditions = Array.Empty<Condition>()
				};
				BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("Smuggler_CueIntro", delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, "{n}A sly merchant in starlight-woven silk leans against a planar strongbox, grinning with sharp amusement.{/n}\n\n\"Hehehe... Look at what the rift dragged in. In need of weapons, rings, or reagents that mortal blacksmiths can only dream of? For enough gold, even the gods would part with their teeth. What can I get you?\"");
					c.Speaker = smugglerSpeaker;
				});
				BlueprintAnswer ansTrade = TTCoreExtensions.CreateAnswer("Ans_Smuggler_Trade", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "Show me your luxury contraband (Trade Gold).");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Smuggler_Trade", delegate(BlueprintCue ct)
					{
						ct.SetText(Main.IsekaiContext, "\"Gold talks, otherworlder. Take a look at this lot.\"");
						ct.Speaker = smugglerSpeaker;
						ct.OnStop = Helpers.CreateActionList(new ActionStartTradeWithSmuggler());
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansLore = TTCoreExtensions.CreateAnswer("Ans_Smuggler_Lore", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "How did you acquire +5 and +6 enchantments in the Worldwound?");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Smuggler_Lore", delegate(BlueprintCue cl)
					{
						cl.SetText(Main.IsekaiContext, "\"Who said anything about the Worldwound? I siphon supplies straight from the Astral Plane and dead empires across the Great Beyond. The crusaders fight in mud; we deal in cosmic supremacy.\"");
						cl.Speaker = smugglerSpeaker;
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_Smuggler_Leave", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "I have no need of your wares right now.");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Smuggler_Leave", delegate(BlueprintCue cl)
					{
						cl.SetText(Main.IsekaiContext, "\"Come back when your bags are heavier. I never close up shop.\"");
						cl.Speaker = smugglerSpeaker;
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswersList bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "Smuggler_AnswersList", delegate(BlueprintAnswersList al)
				{
					al.Answers = new List<BlueprintAnswerBaseReference>
					{
						ansTrade.ToReference<BlueprintAnswerBaseReference>(),
						ansLore.ToReference<BlueprintAnswerBaseReference>(),
						ansLeave.ToReference<BlueprintAnswerBaseReference>()
					};
				});
				blueprintCue.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
				bp.FirstCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { blueprintCue.ToReference<BlueprintCueBaseReference>() }
				};
			});
			ConstellationOracleUnit = HubUnitFactory.Create(baseDeva, "ConstellationOracleUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "The High Oracle of the Constellations");
				if (baseDeva != null)
				{
					bp.m_Portrait = baseDeva.m_Portrait;
					bp.Prefab = baseDeva.Prefab;
					bp.Visual = baseDeva.Visual;
					bp.Gender = Gender.Female;
					bp.Size = baseDeva.Size;
					bp.Alignment = Alignment.TrueNeutral;
				}
				bp.AddComponent<AddImmortality>();
			});
			DialogSpeaker oracleSpeaker = new DialogSpeaker
			{
				m_Blueprint = ConstellationOracleUnit.ToReference<BlueprintUnitReference>(),
				MoveCamera = false
			};
			ConstellationOracleDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "ConstellationOracleDialog", delegate(BlueprintDialog bp)
			{
				bp.Type = DialogType.Common;
				bp.Conditions = new ConditionsChecker
				{
					Conditions = Array.Empty<Condition>()
				};
				BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("Oracle_CueIntro", delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, "{n}A blindfolded seer clad in celestial stoles meditates before an astral scrying basin reflecting galaxies.{/n}\n\n\"The constellations observe all who tread beyond fate. Speak, otherworlder: what communion do you seek with the celestial observers?\"");
					c.Speaker = oracleSpeaker;
				});
				BlueprintAnswer ansStore = TTCoreExtensions.CreateAnswer("Ans_Oracle_Store", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "Commune with the constellations for spiritual blessings and patron ascensions.");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Oracle_Store", delegate(BlueprintCue cs)
					{
						cs.SetText(Main.IsekaiContext, "\"Let the divine resonance wash over your soul.\"");
						cs.Speaker = oracleSpeaker;
						cs.OnStop = Helpers.CreateActionList(new ActionOpenCosmicStoreFromColiseum());
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansLore = TTCoreExtensions.CreateAnswer("Ans_Oracle_Lore", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "Which constellations watch over the coliseum?");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Oracle_Lore", delegate(BlueprintCue cl)
					{
						cl.SetText(Main.IsekaiContext, "\"Desna's song, Gorum's fury, Iomedae's righteousness, Calistria's venom, Nethys's duality, Asmodeus's order, the Lantern King's mischief, and Yog-Sothoth's timeless gate. When you fight, they all bear witness.\"");
						cl.Speaker = oracleSpeaker;
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_Oracle_Leave", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "I will meditate on your words.");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Oracle_Leave", delegate(BlueprintCue cl)
					{
						cl.SetText(Main.IsekaiContext, "\"The cosmos remains with you, always.\"");
						cl.Speaker = oracleSpeaker;
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswersList bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "Oracle_AnswersList", delegate(BlueprintAnswersList al)
				{
					al.Answers = new List<BlueprintAnswerBaseReference>
					{
						ansStore.ToReference<BlueprintAnswerBaseReference>(),
						ansLore.ToReference<BlueprintAnswerBaseReference>(),
						ansLeave.ToReference<BlueprintAnswerBaseReference>()
					};
				});
				blueprintCue.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
				bp.FirstCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { blueprintCue.ToReference<BlueprintCueBaseReference>() }
				};
			});
		}

		public static void AttachDialog(UnitEntityData unit, BlueprintDialog dialog)
		{
			HubUnitFactory.AttachDialog(unit, dialog);
		}
	}
}
