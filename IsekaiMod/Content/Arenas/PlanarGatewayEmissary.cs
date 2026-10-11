using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.View;
using Pathfinding;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Arenas;

public static class PlanarGatewayEmissary
{
	private class GatewayEmissarySpawnHandler : IAreaHandler, ISubscriber, IGlobalSubscriber, IAreaPartHandler
	{
		public void OnAreaPartChanged(BlueprintAreaPart previous)
		{
			OnAreaDidLoad();
		}

		public void OnAreaDidLoad()
		{
			try
			{
				BlueprintArea blueprintArea = Game.Instance?.CurrentlyLoadedArea;
				string text = blueprintArea?.name ?? "";
				int valueOrDefault = (Game.Instance?.Player?.Chapter).GetValueOrDefault();
				List<UnitEntityData> list = Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>()?.ToList();
				List<UnitEntityData> list2 = list?.Where((UnitEntityData u) => u != null && !u.Destroyed && u.Blueprint?.name == "PlanarGatewayEmissaryUnit").ToList() ?? new List<UnitEntityData>();
				bool flag = IsEmissaryArea(text, valueOrDefault) && IsPresent(text, valueOrDefault, Main.IsekaiContext.AddedContent?.EnableDimensionalArena ?? true, AnySiteOfferedHere());
				int num = EmissariesToKeep(list2.Count, flag);
				for (int num2 = num; num2 < list2.Count; num2++)
				{
					list2[num2].MarkForDestroy();
				}
				if (!flag || !IsAreaOwnPart(Game.Instance?.CurrentlyLoadedAreaPart, blueprintArea))
				{
					return;
				}
				UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
				if (unitEntityData == null || Game.Instance?.LoadedAreaState?.MainState == null)
				{
					return;
				}
				if (num > 0)
				{
					UnitEntityData emissary = list2[0];
					if (!IsReachableFromParty(emissary))
					{
						Vector3 position = ChooseSpot(text, valueOrDefault, unitEntityData, (list ?? new List<UnitEntityData>()).Where((UnitEntityData u) => u != emissary), out var facing);
						emissary.View?.StopMoving();
						emissary.Commands?.InterruptMove();
						emissary.Position = position;
						emissary.Orientation = Quaternion.LookRotation(facing).eulerAngles.y;
						Main.IsekaiContext.Logger.Log("[GatewayEmissary] Moved the emissary in " + text + " onto ground the party can reach.");
					}
				}
				else
				{
					Vector3 position2 = ChooseSpot(text, valueOrDefault, unitEntityData, list, out var facing2);
					IsekaiNpcUnit.Protect(Game.Instance.EntityCreator.SpawnUnit(PlanarGatewayEmissaryUnit, position2, Quaternion.LookRotation(facing2), Game.Instance.LoadedAreaState.MainState));
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

	public static BlueprintUnit PlanarGatewayEmissaryUnit;

	public static BlueprintDialog GatewayEmissaryDialog;

	internal const string EmissaryUnitName = "PlanarGatewayEmissaryUnit";

	internal const float AnchorDistance = 6f;

	internal const string DrezenCitadelAnchorGuid = "d17fefb30cff7c141a31465b38ed56d7";

	public static void Add()
	{
		CreateEmissaryBlueprints();
		EventBus.Subscribe(new GatewayEmissarySpawnHandler());
	}

	private static void CreateEmissaryBlueprints()
	{
		BlueprintUnit baseDeva = BlueprintTools.GetBlueprint<BlueprintUnit>("7bcffd91514f489469a6fb6dea0a50d2");
		PlanarGatewayEmissaryUnit = Helpers.CreateBlueprint(Main.IsekaiContext, "PlanarGatewayEmissaryUnit", delegate(BlueprintUnit bp)
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
			IsekaiNpcUnit.MakeServiceNpc(bp);
			bp.AddComponent(delegate(DialogOnClick c)
			{
				c.m_Dialog = BlueprintTools.GetModBlueprintReference<BlueprintDialogReference>(Main.IsekaiContext, "PlanarGatewayEmissaryDialog");
				c.Conditions = ActionFlow.EmptyCondition();
				c.NoDialogActions = ActionFlow.DoNothing();
			});
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
				c.SetText(Main.IsekaiContext, "{n}A shimmering rift of astral starlight coalesces into the form of a masked celestial emissary, their robes billowing in planar currents.{/n}\n\n\"Greetings, traveler. I keep the ways between worlds: to the Planar Coliseum, where heroes across timelines test their mettle before the observing constellations, and to the breaches and anomalies that tear open across your crusade. Where shall I open the threshold for you?\"");
				c.Speaker = speaker;
			});
			BlueprintAnswer ansEnter = TTCoreExtensions.CreateAnswer("Ans_Gateway_Enter", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Open the planar rift to the Planar Coliseum.");
				a.AddShowCondition<ConditionPlanarColiseumEnabled>();
				ColiseumGates.AddDefendersHeartSiegeGate(a);
				ColiseumGates.AddSceneStateGate(a);
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
			BlueprintCue cueLore = null;
			BlueprintAnswer ansLore = TTCoreExtensions.CreateAnswer("Ans_Gateway_Lore", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "What awaits me in the Planar Coliseum?");
				cueLore = TTCoreExtensions.CreateCue("Cue_Gateway_Lore", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"An interdimensional gladiatorial hub outside linear time, with a stage for every chapter of your crusade. You will find the Herald of the Planar Coliseum, who registers champions for the tournament cups, the Astral Coin Envoy, who trades relics for Cosmic Coins, a smuggler dealing in planar contraband, and the High Oracle of the Constellations.\"");
					cl.Speaker = speaker;
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { cueLore.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_Gateway_Leave", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Not at this time.");
				BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Gateway_Leave", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"As you will, champion. The rift endures whenever you are ready.\"");
					cl.Speaker = speaker;
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswer ansRift = TTCoreExtensions.CreateAnswer("Ans_Gateway_RiftSite", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Follow the rift tremor.");
				a.AddShowCondition<ConditionEmissaryOffersRift>();
				ColiseumGates.AddDefendersHeartSiegeGate(a);
				ColiseumGates.AddSceneStateGate(a);
				BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Gateway_RiftSealed", delegate(BlueprintCue cs)
				{
					cs.SetText(Main.IsekaiContext, "\"The Arbiter's breach will not open for you yet. The loop still holds threads you have left unresolved; the constellations will name them.\"");
					cs.Speaker = speaker;
					cs.Conditions = new ConditionsChecker
					{
						Conditions = new Condition[1]
						{
							new ConditionArbiterBreachSealed()
						}
					};
					cs.OnStop = Helpers.CreateActionList(new ActionReportArbiterRequirements());
				});
				BlueprintCue bp4 = TTCoreExtensions.CreateCue("Cue_Gateway_RiftSite", delegate(BlueprintCue cr)
				{
					cr.SetText(Main.IsekaiContext, "\"The breach answers your resonance. Step through, and settle what waits on the other side.\"");
					cr.Speaker = speaker;
					cr.OnStop = Helpers.CreateActionList(new ActionEnterBossSite
					{
						Kind = BossSiteKind.Rift
					});
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference>
					{
						bp3.ToReference<BlueprintCueBaseReference>(),
						bp4.ToReference<BlueprintCueBaseReference>()
					},
					Strategy = Strategy.First
				};
			});
			List<BlueprintAnswer> anomalyAnswers = new List<BlueprintAnswer>
			{
				CreateAnomalyAnswer(1, "Ans_Gateway_Anomaly1", "Cue_Gateway_Anomaly1", "Open the way to the Barren Anomaly: The Outer World Stray.", speaker),
				CreateAnomalyAnswer(2, "Ans_Gateway_Anomaly2", "Cue_Gateway_Anomaly2", "Open the way to the Barren Anomaly: Chronos Exile.", speaker),
				CreateAnomalyAnswer(3, "Ans_Gateway_Anomaly3", "Cue_Gateway_Anomaly3", "Open the way to the Barren Anomaly: The Transdimensional Behemoth.", speaker),
				CreateAnomalyAnswer(4, "Ans_Gateway_Anomaly4", "Cue_Gateway_Anomaly4", "Open the way to the Barren Anomaly: Abyssal Void Vavakia.", speaker),
				CreateAnomalyAnswer(5, "Ans_Gateway_Anomaly5", "Cue_Gateway_Anomaly5", "Open the way to the Barren Anomaly: Cosmic Arbiter of Ruin.", speaker)
			};
			BlueprintAnswersList bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "GatewayEmissary_AnswersList", delegate(BlueprintAnswersList al)
			{
				al.Answers = new List<BlueprintAnswerBaseReference>
				{
					ansEnter.ToReference<BlueprintAnswerBaseReference>(),
					ansRift.ToReference<BlueprintAnswerBaseReference>()
				};
				al.Answers.AddRange(anomalyAnswers.Select((BlueprintAnswer x) => x.ToReference<BlueprintAnswerBaseReference>()));
				al.Answers.Add(ansLore.ToReference<BlueprintAnswerBaseReference>());
				al.Answers.Add(ansLeave.ToReference<BlueprintAnswerBaseReference>());
			});
			blueprintCue.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			cueLore.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			bp.FirstCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference> { blueprintCue.ToReference<BlueprintCueBaseReference>() }
			};
		});
	}

	private static BlueprintAnswer CreateAnomalyAnswer(int act, string answerName, string cueName, string text, DialogSpeaker speaker)
	{
		return TTCoreExtensions.CreateAnswer(answerName, delegate(BlueprintAnswer a)
		{
			a.SetText(Main.IsekaiContext, text);
			a.AddShowCondition(delegate(ConditionEmissaryOffersAnomaly c)
			{
				c.Act = act;
			});
			ColiseumGates.AddDefendersHeartSiegeGate(a);
			ColiseumGates.AddSceneStateGate(a);
			BlueprintCue bp = TTCoreExtensions.CreateCue(cueName, delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, "\"The anomaly stirs beyond the veil. Go, and mind its teeth.\"");
				c.Speaker = speaker;
				c.OnStop = Helpers.CreateActionList(new ActionEnterBossSite
				{
					Kind = BossSiteKind.Anomaly,
					Act = act
				});
			});
			a.NextCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference> { bp.ToReference<BlueprintCueBaseReference>() }
			};
		});
	}

	internal static bool IsEmissaryArea(string areaName, int chapter)
	{
		if (IsEmissaryHubArea(areaName))
		{
			return true;
		}
		if (chapter >= 6)
		{
			return string.Equals(areaName, "ThresholdOutdoor", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	internal static bool OffersRift(string areaName, int chapter, bool riftOffered)
	{
		if (riftOffered)
		{
			return IsEmissaryArea(areaName, chapter);
		}
		return false;
	}

	internal static bool OffersAnomaly(string areaName, int chapter, bool anomalyOffered, bool siteBuilt)
	{
		if (anomalyOffered)
		{
			return IsEmissaryArea(areaName, chapter);
		}
		return false;
	}

	internal static int EmissariesToKeep(int standing, bool present)
	{
		if (standing <= 0 || !present)
		{
			return 0;
		}
		return 1;
	}

	internal static bool IsPresent(string areaName, int chapter, bool arenaEnabled, bool anySiteOffered)
	{
		if (!IsEmissaryArea(areaName, chapter))
		{
			return false;
		}
		return arenaEnabled | anySiteOffered;
	}

	internal static bool CurrentlyOffersRift()
	{
		Game instance = Game.Instance;
		return OffersRift(instance?.CurrentlyLoadedArea?.name, (instance?.Player?.Chapter).GetValueOrDefault(), BossSites.CurrentRiftOffered());
	}

	internal static bool CurrentlyOffersAnomaly(int act)
	{
		Game instance = Game.Instance;
		return OffersAnomaly(instance?.CurrentlyLoadedArea?.name, (instance?.Player?.Chapter).GetValueOrDefault(), BossSites.CurrentAnomalyOffered(act), BossSites.SiteEnterPointFor(BossSiteKind.Anomaly, act) != null);
	}

	internal static bool AnySiteOfferedHere()
	{
		if (CurrentlyOffersRift())
		{
			return true;
		}
		for (int i = 1; i <= 5; i++)
		{
			if (CurrentlyOffersAnomaly(i))
			{
				return true;
			}
		}
		return false;
	}

	internal static bool IsEmissaryHubArea(string areaName)
	{
		return ActionEnterColiseum.IsHubArea(areaName);
	}

	internal static bool IsAreaOwnPart(BlueprintAreaPart loadedPart, BlueprintArea area)
	{
		if (area != null)
		{
			if (loadedPart != null)
			{
				return loadedPart == area;
			}
			return true;
		}
		return false;
	}

	internal static string AnchorEnterPointGuid(string areaName, int chapter)
	{
		if (!IsEmissaryArea(areaName, chapter))
		{
			return null;
		}
		if (string.Equals(areaName, "DrezenCapital", StringComparison.OrdinalIgnoreCase))
		{
			return "d17fefb30cff7c141a31465b38ed56d7";
		}
		return ActionEnterColiseum.ResolveReturnEnterPoint(areaName, chapter);
	}

	internal static bool NeedsMove(bool hasNode, float snapOffset, uint emissaryArea, uint? partyArea)
	{
		return !NpcLayout.IsReachableGround(hasNode, snapOffset, emissaryArea, partyArea);
	}

	internal static bool IsReachableFromParty(UnitEntityData unit)
	{
		if (unit == null)
		{
			return false;
		}
		try
		{
			if (AstarPath.active == null)
			{
				return true;
			}
			NNInfo nearestNode = ObstacleAnalyzer.GetNearestNode(unit.Position);
			bool flag = nearestNode.node != null;
			return !NeedsMove(flag, flag ? NpcLayout.GroundDistance(nearestNode.position, unit.Position) : 0f, flag ? nearestNode.node.Area : 0u, IsekaiNpcUnit.PartyGroundArea());
		}
		catch
		{
			return true;
		}
	}

	private static Vector3 ChooseSpot(string areaName, int chapter, UnitEntityData player, IEnumerable<UnitEntityData> others, out Vector3 facing)
	{
		uint? partyArea = IsekaiNpcUnit.PartyGroundArea();
		Func<Vector3, Vector3?> func = (Vector3 p) => IsekaiNpcUnit.SnapToReachableGround(p, partyArea);
		List<Vector3> taken = (from u in others ?? Enumerable.Empty<UnitEntityData>()
			where u != null && !u.Destroyed && u.IsInGame
			select u.Position).Concat(IsekaiNpcUnit.PartyPositions()).ToList();
		try
		{
			string text = AnchorEnterPointGuid(areaName, chapter);
			BlueprintAreaEnterPoint blueprintAreaEnterPoint = (string.IsNullOrEmpty(text) ? null : BlueprintTools.GetBlueprint<BlueprintAreaEnterPoint>(text));
			AreaEnterPoint areaEnterPoint = ((blueprintAreaEnterPoint != null) ? AreaEnterPoint.FindAreaEnterPointOnScene(blueprintAreaEnterPoint) : null);
			if (areaEnterPoint != null)
			{
				Vector3 vector = NpcLayout.Flatten(areaEnterPoint.transform.forward);
				Vector3 vector2 = NpcLayout.Resolve(areaEnterPoint.transform.position + vector * 6f, vector, taken, 3f, func);
				if (func(vector2).HasValue)
				{
					facing = -vector;
					return vector2;
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[GatewayEmissary] Could not read the town's entrance: " + ex);
		}
		Vector3 vector3 = NpcLayout.Flatten(player.OrientationDirection);
		facing = -vector3;
		return NpcLayout.Resolve(player.Position + vector3 * 6f, vector3, taken, 3f, func);
	}
}
