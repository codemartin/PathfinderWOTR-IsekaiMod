using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.AI.Blueprints;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.View;
using Pathfinding;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Arenas;

internal static class IsekaiNpcUnit
{
	internal class ServiceNpcGuard : IAreaHandler, ISubscriber, IGlobalSubscriber, IAreaPartHandler
	{
		public void OnAreaDidLoad()
		{
			foreach (UnitEntityData item in ServiceNpcsInArea())
			{
				Protect(item);
			}
		}

		public void OnAreaPartChanged(BlueprintAreaPart previous)
		{
			OnAreaDidLoad();
		}

		public void OnAreaBeginUnloading()
		{
		}
	}

	internal const string NeutralsFactionGuid = "d8de50cc80eb4dc409a983991e0b77ad";

	internal const string PassiveMonsterBrainGuid = "5718d805516c5ff48bf3e928b6c0e6fd";

	internal const string CiviliansFactionGuid = "5b8795a258cf0244c9ebae471e399d3a";

	internal static readonly string[] ServiceNpcNames = new string[6] { "PlanarColiseumHeraldUnit", "AstralCoinEnvoyUnit", "VoidMarketSmugglerUnit", "ConstellationOracleUnit", "LanternKingCameoUnit", "PlanarGatewayEmissaryUnit" };

	internal static List<Vector3> WithParty(IEnumerable<Vector3> taken)
	{
		List<Vector3> list = taken?.ToList() ?? new List<Vector3>();
		try
		{
			List<UnitEntityData> list2 = Game.Instance?.Player?.PartyAndPets;
			if (list2 != null)
			{
				foreach (UnitEntityData item in list2)
				{
					if (item != null && !item.Destroyed && item.IsInGame)
					{
						list.Add(item.Position);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[IsekaiNpcUnit] Could not read the party's positions: " + ex);
		}
		return list;
	}

	internal static void MakeSpawnable(BlueprintUnit bp)
	{
		if (bp == null)
		{
			return;
		}
		if (bp.Skills == null)
		{
			bp.Skills = new BlueprintUnit.UnitSkills();
		}
		if (bp.m_Faction == null || ((BlueprintReferenceBase)bp.m_Faction).deserializedGuid == BlueprintGuid.Empty)
		{
			BlueprintFaction blueprintFaction = Lookup<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad");
			if (blueprintFaction != null)
			{
				bp.m_Faction = blueprintFaction.ToReference<BlueprintFactionReference>();
			}
		}
		if (bp.FactionOverrides == null)
		{
			bp.FactionOverrides = new FactionOverrides
			{
				m_AttackFactionsToAdd = new BlueprintFactionReference[0],
				m_AttackFactionsToRemove = new BlueprintFactionReference[0]
			};
		}
		if (bp.m_AddFacts == null)
		{
			bp.m_AddFacts = new BlueprintUnitFactReference[0];
		}
		if (bp.m_AdditionalTemplates == null)
		{
			bp.m_AdditionalTemplates = new BlueprintUnitTemplateReference[0];
		}
		if (bp.m_StartingInventory == null)
		{
			bp.m_StartingInventory = new BlueprintItemReference[0];
		}
		if (bp.m_Brain == null || ((BlueprintReferenceBase)bp.m_Brain).deserializedGuid == BlueprintGuid.Empty)
		{
			BlueprintBrain blueprintBrain = Lookup<BlueprintBrain>("5718d805516c5ff48bf3e928b6c0e6fd");
			if (blueprintBrain != null)
			{
				bp.m_Brain = blueprintBrain.ToReference<BlueprintBrainReference>();
			}
		}
	}

	internal static bool IsServiceNpc(string unitBlueprintName)
	{
		if (!string.IsNullOrEmpty(unitBlueprintName))
		{
			return Array.IndexOf(ServiceNpcNames, unitBlueprintName) >= 0;
		}
		return false;
	}

	internal static void MakeServiceNpc(BlueprintUnit bp)
	{
		if (bp == null)
		{
			return;
		}
		BlueprintFaction blueprintFaction = Lookup<BlueprintFaction>("5b8795a258cf0244c9ebae471e399d3a");
		if (blueprintFaction != null)
		{
			bp.m_Faction = blueprintFaction.ToReference<BlueprintFactionReference>();
		}
		MakeSpawnable(bp);
		if (bp.FactionOverrides != null)
		{
			bp.FactionOverrides.m_AttackFactionsToAdd = new BlueprintFactionReference[0];
		}
		if (bp.GetComponent<AddImmortality>() == null)
		{
			bp.AddComponent<AddImmortality>();
		}
		if (bp.GetComponent<CompleteDamageImmunity>() == null)
		{
			bp.AddComponent<CompleteDamageImmunity>();
		}
		if (!bp.GetComponents<AddMechanicsFeature>().Any((AddMechanicsFeature c) => c.m_Feature == AddMechanicsFeature.MechanicsFeatureType.IsIgnoredByCombat))
		{
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.IsIgnoredByCombat;
			});
		}
	}

	internal static void Protect(UnitEntityData unit)
	{
		if (unit == null || unit.Destroyed || !IsServiceNpc(unit.Blueprint?.name))
		{
			return;
		}
		try
		{
			BlueprintFaction blueprintFaction = Lookup<BlueprintFaction>("5b8795a258cf0244c9ebae471e399d3a");
			if (blueprintFaction != null && (unit.Faction != blueprintFaction || unit.AttackFactions.Any()))
			{
				unit.SwitchFactions(blueprintFaction, resetAttackFactions: true);
			}
			if (!unit.Descriptor.State.IsConscious)
			{
				unit.Descriptor.ResurrectAndFullRestore();
			}
			if (unit.IsInCombat)
			{
				unit.CombatState?.LeaveCombat();
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[ServiceNpc] Could not protect " + unit.Blueprint?.name + ": " + ex);
		}
	}

	internal static List<string> BlueprintProtectionProblems(BlueprintUnit bp, bool checkFaction)
	{
		List<string> list = new List<string>();
		if (bp == null)
		{
			list.Add("The blueprint does not exist.");
			return list;
		}
		if (bp.GetComponent<AddImmortality>() == null)
		{
			list.Add(bp.name + " has no AddImmortality.");
		}
		if (bp.GetComponent<CompleteDamageImmunity>() == null)
		{
			list.Add(bp.name + " has no CompleteDamageImmunity.");
		}
		if (!bp.GetComponents<AddMechanicsFeature>().Any((AddMechanicsFeature c) => c.m_Feature == AddMechanicsFeature.MechanicsFeatureType.IsIgnoredByCombat))
		{
			list.Add(bp.name + " is not IsIgnoredByCombat.");
		}
		if (bp.FactionOverrides?.m_AttackFactionsToAdd != null && bp.FactionOverrides.m_AttackFactionsToAdd.Length != 0)
		{
			list.Add(bp.name + " adds factions to attack.");
		}
		if (checkFaction)
		{
			if (bp.m_Faction == null || ((BlueprintReferenceBase)bp.m_Faction).deserializedGuid != BlueprintGuid.Parse("5b8795a258cf0244c9ebae471e399d3a"))
			{
				list.Add(bp.name + " is not in Factions/Civilians.");
			}
			BlueprintFaction faction = bp.Faction;
			if (faction == null || !faction.Peaceful)
			{
				list.Add(bp.name + "'s faction is not Peaceful.");
			}
			if (faction != null && faction.AttackFactions.Any())
			{
				list.Add(bp.name + "'s faction attacks other factions.");
			}
		}
		return list;
	}

	internal static List<string> ProtectionProblems(UnitEntityData unit)
	{
		List<string> list = new List<string>();
		if (unit == null)
		{
			return list;
		}
		string text = unit.Blueprint?.name ?? "?";
		BlueprintFaction faction = unit.Faction;
		if (faction == null || !faction.Peaceful)
		{
			list.Add(text + " is in faction '" + (faction?.name ?? "none") + "', which is not Peaceful.");
		}
		if (unit.AttackFactions.Any())
		{
			list.Add(text + " attacks " + string.Join(", ", unit.AttackFactions.Select((BlueprintFaction f) => f.name)) + ".");
		}
		if (!unit.Descriptor.State.Features.Immortality)
		{
			list.Add(text + " is not immortal.");
		}
		if (!unit.Descriptor.State.Features.IsIgnoredByCombat)
		{
			list.Add(text + " is not ignored by combat.");
		}
		if (!unit.Descriptor.State.IsConscious)
		{
			list.Add(text + " is down.");
		}
		return list;
	}

	internal static List<UnitEntityData> ServiceNpcsInArea()
	{
		try
		{
			return Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>().Where((UnitEntityData u) => u != null && !u.Destroyed && IsServiceNpc(u.Blueprint?.name)).ToList() ?? new List<UnitEntityData>();
		}
		catch
		{
			return new List<UnitEntityData>();
		}
	}

	internal static List<Vector3> ServiceNpcPositions()
	{
		return (from u in ServiceNpcsInArea()
			select u.Position).ToList();
	}

	internal static Vector3? SnapToGround(Vector3 point)
	{
		try
		{
			if (AstarPath.active == null)
			{
				return point;
			}
			NNInfo nearestNode = ObstacleAnalyzer.GetNearestNode(point);
			if (nearestNode.node == null)
			{
				return null;
			}
			if (NpcLayout.GroundDistance(nearestNode.position, point) > 3f)
			{
				return null;
			}
			return nearestNode.position;
		}
		catch
		{
			return point;
		}
	}

	internal static Vector3 KeepClearOfServiceNpcs(Vector3 point, Vector3 forward)
	{
		return NpcLayout.Resolve(point, forward, ServiceNpcPositions(), 6f, SnapToGround);
	}

	internal static Func<Vector3, Vector3?> ReachableGroundSnap(Vector3 partyPoint)
	{
		uint partyArea;
		try
		{
			if (AstarPath.active == null)
			{
				return SnapToGround;
			}
			NNInfo nearestNode = ObstacleAnalyzer.GetNearestNode(partyPoint);
			if (nearestNode.node == null)
			{
				return SnapToGround;
			}
			partyArea = nearestNode.node.Area;
		}
		catch
		{
			return SnapToGround;
		}
		return delegate(Vector3 point)
		{
			try
			{
				NNInfo nearestNode2 = ObstacleAnalyzer.GetNearestNode(point);
				bool flag = nearestNode2.node != null;
				if (!NpcLayout.IsReachableGround(flag, NpcLayout.GroundDistance(nearestNode2.position, point), flag ? nearestNode2.node.Area : 0u, partyArea))
				{
					return (Vector3?)null;
				}
				return nearestNode2.position;
			}
			catch
			{
				return (Vector3?)null;
			}
		};
	}

	internal static Vector3 PlaceWithinReach(Vector3 preferred, Vector3 forward, Vector3 partyPoint)
	{
		List<Vector3> list = ServiceNpcPositions();
		try
		{
			List<UnitEntityData> list2 = Game.Instance?.Player?.PartyAndPets;
			if (list2 != null)
			{
				list.AddRange(from u in list2
					where u != null && u.IsInGame
					select u.Position);
			}
		}
		catch
		{
		}
		Vector3? vector = NpcLayout.ResolveWithinReach(preferred, partyPoint, forward, list, 6f, ReachableGroundSnap(partyPoint));
		if (vector.HasValue)
		{
			return vector.Value;
		}
		try
		{
			if (AstarPath.active != null)
			{
				FreePlaceSelector.PlaceSpawnPlaces(1, 1f, partyPoint);
				return FreePlaceSelector.GetRelaxedPosition(0, projectOnGround: true);
			}
		}
		catch
		{
		}
		return preferred;
	}

	internal static List<Vector3> PartyPositions()
	{
		try
		{
			List<UnitEntityData> list = Game.Instance?.Player?.PartyAndPets;
			if (list == null)
			{
				return new List<Vector3>();
			}
			return (from u in list
				where u != null && !u.Destroyed && u.IsInGame
				select u.Position).ToList();
		}
		catch
		{
			return new List<Vector3>();
		}
	}

	internal static uint? GroundAreaAt(Vector3 point)
	{
		try
		{
			if (AstarPath.active == null)
			{
				return null;
			}
			NNInfo nearestNode = ObstacleAnalyzer.GetNearestNode(point);
			if (nearestNode.node == null || NpcLayout.GroundDistance(nearestNode.position, point) > 3f)
			{
				return null;
			}
			return nearestNode.node.Area;
		}
		catch
		{
			return null;
		}
	}

	internal static uint? PartyGroundArea()
	{
		UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
		if (!(unitEntityData == null))
		{
			return GroundAreaAt(unitEntityData.Position);
		}
		return null;
	}

	internal static Vector3? SnapToReachableGround(Vector3 point, uint? area)
	{
		try
		{
			if (AstarPath.active == null)
			{
				return point;
			}
			NNInfo nearestNode = ObstacleAnalyzer.GetNearestNode(point);
			bool flag = nearestNode.node != null;
			if (!NpcLayout.IsReachableGround(flag, flag ? NpcLayout.GroundDistance(nearestNode.position, point) : 0f, flag ? nearestNode.node.Area : 0u, area))
			{
				return null;
			}
			return nearestNode.position;
		}
		catch
		{
			return point;
		}
	}

	internal static Vector3 PlaceCombatant(Vector3 point, Vector3 forward, IEnumerable<Vector3> placedContestants, IEnumerable<Vector3> partyPositions = null)
	{
		List<NpcLayout.Keepout> list = new List<NpcLayout.Keepout>();
		foreach (Vector3 item in ServiceNpcPositions())
		{
			list.Add(new NpcLayout.Keepout(item, 6f));
		}
		foreach (Vector3 item2 in partyPositions ?? PartyPositions())
		{
			list.Add(new NpcLayout.Keepout(item2, 6f));
		}
		foreach (Vector3 item3 in placedContestants ?? Enumerable.Empty<Vector3>())
		{
			list.Add(new NpcLayout.Keepout(item3, 3f));
		}
		uint? area = PartyGroundArea();
		return NpcLayout.ResolveClear(point, forward, list, 3f, (Vector3 p) => SnapToReachableGround(p, area));
	}

	private static T Lookup<T>(string guid) where T : SimpleBlueprint
	{
		try
		{
			return BlueprintTools.GetBlueprint<T>(guid);
		}
		catch
		{
			return null;
		}
	}
}
