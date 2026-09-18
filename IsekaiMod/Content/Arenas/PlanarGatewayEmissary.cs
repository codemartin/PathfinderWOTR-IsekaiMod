using System;
using System.Collections.Generic;
using System.Linq;
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
	public static class PlanarGatewayEmissary
	{
		private class GatewayEmissarySpawnHandler : IAreaHandler, ISubscriber, IGlobalSubscriber
		{
			public void OnAreaDidLoad()
			{
				try
				{
					string text = Game.Instance?.CurrentlyLoadedArea?.name ?? "";
					if (text.IndexOf("DefendersHeart", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("WarCamp", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("Drezen", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("MidnightIsles", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("DLC3", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("HubShip", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("Gundrun", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("Festival", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("DLC6", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("Axis", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("DLC1", StringComparison.OrdinalIgnoreCase) < 0)
					{
						return;
					}
					IEnumerable<UnitEntityData> enumerable = Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>();
					if (enumerable != null && enumerable.Any((UnitEntityData u) => u.Blueprint?.name == "PlanarGatewayEmissaryUnit"))
					{
						// The unit survives in the save but its interaction does not, so it is attached again on every load.
						foreach (UnitEntityData existing in enumerable.Where((UnitEntityData u) => u.Blueprint?.name == "PlanarGatewayEmissaryUnit").ToList())
						{
							HubUnitFactory.AttachDialog(existing, GatewayEmissaryDialog);
						}
						return;
					}
					UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
					if (unitEntityData == null || Game.Instance?.LoadedAreaState?.MainState == null)
					{
						return;
					}
					Vector3 vector = unitEntityData.Position + unitEntityData.OrientationDirection * 6f;
					if (NavMesh.SamplePosition(vector, out var hit, 15f, -1))
					{
						vector = hit.position;
					}
					UnitEntityData unitEntityData2 = Game.Instance.EntityCreator.SpawnUnit(PlanarGatewayEmissaryUnit, vector, Quaternion.identity, Game.Instance.LoadedAreaState.MainState);
					if (unitEntityData2 != null)
					{
						BlueprintFaction blueprint = BlueprintTools.GetBlueprint<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad");
						if (blueprint != null)
						{
							unitEntityData2.SwitchFactions(blueprint);
						}
						AttachDialog(unitEntityData2, GatewayEmissaryDialog);
					}
				}
				catch (Exception ex)
				{
					Main.IsekaiContext.Logger.LogError("Error spawning PlanarGatewayEmissary: " + ex);
				}
			}

			public void OnAreaBeginUnloading()
			{
			}
		}

		private const string MonadicDevaStandardGuid = "7bcffd91514f489469a6fb6dea0a50d2";

		private const string NeutralsFactionGuid = "d8de50cc80eb4dc409a983991e0b77ad";

		public static BlueprintUnit PlanarGatewayEmissaryUnit;

		public static BlueprintDialog GatewayEmissaryDialog;

		public static void Add()
		{
			CreateEmissaryBlueprints();
			EventBus.Subscribe(new GatewayEmissarySpawnHandler());
		}

		private static void CreateEmissaryBlueprints()
		{
			BlueprintUnit baseDeva = BlueprintTools.GetBlueprint<BlueprintUnit>("7bcffd91514f489469a6fb6dea0a50d2");
			PlanarGatewayEmissaryUnit = HubUnitFactory.Create(baseDeva, "PlanarGatewayEmissaryUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "Otherworldly Emissary");
				if (baseDeva != null)
				{
					bp.m_Portrait = baseDeva.m_Portrait;
					bp.Prefab = baseDeva.Prefab;
					bp.Visual = baseDeva.Visual;
					bp.Gender = baseDeva.Gender;
					bp.Size = baseDeva.Size;
					bp.Alignment = Alignment.TrueNeutral;
				}
				bp.AddComponent<AddImmortality>();
			});
			DialogSpeaker speaker = new DialogSpeaker
			{
				m_Blueprint = PlanarGatewayEmissaryUnit.ToReference<BlueprintUnitReference>(),
				MoveCamera = false
			};
			GatewayEmissaryDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "PlanarGatewayEmissaryDialog", delegate(BlueprintDialog bp)
			{
				bp.Type = DialogType.Common;
				bp.Conditions = new ConditionsChecker
				{
					Conditions = Array.Empty<Condition>()
				};
				BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("GatewayEmissary_CueIntro", delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, "{n}A shimmering rift of astral starlight coalesces into the form of a masked celestial emissary, their robes billowing in planar currents.{/n}\n\n\"Greetings, awakened otherworlder. Beyond the mortal coil lies the Planar Coliseum, where heroes across timelines test their mettle before the observing constellations. Shall I open the threshold for you?\"");
					c.Speaker = speaker;
				});
				BlueprintAnswer ansEnter = TTCoreExtensions.CreateAnswer("Ans_Gateway_Enter", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "Open the planar rift to the Planar Coliseum.");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Gateway_Enter", delegate(BlueprintCue ce)
					{
						ce.SetText(Main.IsekaiContext, "\"Step through the threshold, champion. May the constellations favor your courage.\"");
						ce.Speaker = speaker;
						ce.OnStop = Helpers.CreateActionList(new ActionEnterColiseum());
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansLore = TTCoreExtensions.CreateAnswer("Ans_Gateway_Lore", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "What awaits me in the Planar Coliseum?");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Gateway_Lore", delegate(BlueprintCue cl)
					{
						cl.SetText(Main.IsekaiContext, "\"An interdimensional gladiatorial hub outside linear time. You will find the Coliseum Master to organize tournament cups, an Astral Envoy trading relics for Cosmic Coins, a smuggler dealing planar contraband, and the High Oracle of the Constellations.\"");
						cl.Speaker = speaker;
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_Gateway_Leave", delegate(BlueprintAnswer a)
				{
					a.SetText(Main.IsekaiContext, "Not at this time.");
					BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Gateway_Leave", delegate(BlueprintCue cl)
					{
						cl.SetText(Main.IsekaiContext, "\"As you will, otherworlder. The rift endures whenever you are ready.\"");
						cl.Speaker = speaker;
					});
					a.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
					};
				});
				BlueprintAnswersList bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "GatewayEmissary_AnswersList", delegate(BlueprintAnswersList al)
				{
					al.Answers = new List<BlueprintAnswerBaseReference>
					{
						ansEnter.ToReference<BlueprintAnswerBaseReference>(),
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
