using System;
using System.Collections.Generic;
using UnityEngine;

namespace IsekaiMod.Content.Arenas;

internal static class NpcLayout
{
	internal struct Keepout(Vector3 point, float clearance)
	{
		public Vector3 Point = point;

		public float Clearance = clearance;
	}

	internal const float MinSpacing = 3f;

	internal const float OverlapDistance = 2f;

	internal const float SlotSpacing = 4f;

	internal const float GalleryDepth = 4f;

	internal const float FightDepth = 18f;

	internal const float CombatantClearance = 6f;

	internal const float MaxSnapDistance = 3f;

	internal const float PartyClearance = 6f;

	internal const float ContestantSpacing = 3f;

	internal static readonly string[] ColiseumSlotOrder = new string[5] { "AstralCoinEnvoyUnit", "VoidMarketSmugglerUnit", "PlanarColiseumHeraldUnit", "ConstellationOracleUnit", "LanternKingCameoUnit" };

	internal static readonly float[] ReachFractions = new float[4] { 1f, 0.7f, 0.45f, 0.25f };

	internal static bool IsClear(Vector3 point, IList<Keepout> keepouts)
	{
		if (keepouts == null)
		{
			return true;
		}
		for (int i = 0; i < keepouts.Count; i++)
		{
			if (GroundDistance(point, keepouts[i].Point) < keepouts[i].Clearance)
			{
				return false;
			}
		}
		return true;
	}

	internal static bool IsReachableGround(bool hasNode, float snapOffset, uint nodeArea, uint? requiredArea)
	{
		if (!hasNode)
		{
			return false;
		}
		if (snapOffset > 3f)
		{
			return false;
		}
		if (requiredArea.HasValue)
		{
			return nodeArea == requiredArea.Value;
		}
		return true;
	}

	internal static Vector3 ResolveClear(Vector3 preferred, Vector3 forward, IList<Keepout> keepouts, float step, Func<Vector3, Vector3?> snap)
	{
		Vector3? vector = null;
		foreach (Vector3 item in Candidates(preferred, forward, step))
		{
			Vector3? vector2 = ((snap != null) ? snap(item) : new Vector3?(item));
			if (vector2.HasValue)
			{
				if (!vector.HasValue)
				{
					vector = vector2;
				}
				if (IsClear(vector2.Value, keepouts))
				{
					return vector2.Value;
				}
			}
		}
		return vector ?? preferred;
	}

	internal static Vector3 Flatten(Vector3 forward)
	{
		Vector3 vector = new Vector3(forward.x, 0f, forward.z);
		float num = (float)Math.Sqrt(vector.x * vector.x + vector.z * vector.z);
		if (num < 0.001f)
		{
			return new Vector3(0f, 0f, 1f);
		}
		return new Vector3(vector.x / num, 0f, vector.z / num);
	}

	internal static Vector3 RightOf(Vector3 forward)
	{
		Vector3 vector = Flatten(forward);
		return new Vector3(vector.z, 0f, 0f - vector.x);
	}

	internal static Vector3 InFrame(Vector3 anchor, Vector3 forward, float right, float ahead)
	{
		Vector3 forward2 = Flatten(forward);
		Vector3 vector = RightOf(forward2);
		return new Vector3(anchor.x + vector.x * right + forward2.x * ahead, anchor.y, anchor.z + vector.z * right + forward2.z * ahead);
	}

	internal static int ColiseumSlotIndex(string unitName)
	{
		if (!string.IsNullOrEmpty(unitName))
		{
			return Array.IndexOf(ColiseumSlotOrder, unitName);
		}
		return -1;
	}

	internal static Vector3 ColiseumSlot(string unitName, Vector3 anchor, Vector3 forward)
	{
		int num = ColiseumSlotIndex(unitName);
		if (num < 0)
		{
			num = ColiseumSlotOrder.Length;
		}
		float num2 = (float)(ColiseumSlotOrder.Length - 1) / 2f;
		return InFrame(anchor, forward, ((float)num - num2) * 4f, 4f);
	}

	internal static Vector3 FightPoint(Vector3 anchor, Vector3 forward, float x, float z)
	{
		return InFrame(anchor, forward, x, 18f + z);
	}

	internal static float GroundDistance(Vector3 a, Vector3 b)
	{
		float num = a.x - b.x;
		float num2 = a.z - b.z;
		return (float)Math.Sqrt(num * num + num2 * num2);
	}

	internal static bool IsClear(Vector3 point, IList<Vector3> taken, float minDistance)
	{
		if (taken == null)
		{
			return true;
		}
		for (int i = 0; i < taken.Count; i++)
		{
			if (GroundDistance(point, taken[i]) < minDistance)
			{
				return false;
			}
		}
		return true;
	}

	internal static List<Vector3> Candidates(Vector3 preferred, Vector3 forward, float step)
	{
		Vector3 vector = Flatten(forward);
		Vector3 vector2 = RightOf(vector);
		List<Vector3> list = new List<Vector3> { preferred };
		for (int i = 1; i <= 4; i++)
		{
			float num = (float)i * step;
			list.Add(preferred + vector2 * num);
			list.Add(preferred - vector2 * num);
			list.Add(preferred + vector * num);
			list.Add(preferred - vector * num);
			list.Add(preferred + (vector2 + vector) * num);
			list.Add(preferred + (vector - vector2) * num);
			list.Add(preferred + (vector2 - vector) * num);
			list.Add(preferred - (vector2 + vector) * num);
		}
		return list;
	}

	internal static Vector3 Resolve(Vector3 preferred, Vector3 forward, IList<Vector3> taken, float minDistance, Func<Vector3, Vector3?> snap)
	{
		Vector3? vector = null;
		foreach (Vector3 item in Candidates(preferred, forward, minDistance))
		{
			Vector3? vector2 = ((snap != null) ? snap(item) : new Vector3?(item));
			if (vector2.HasValue)
			{
				if (!vector.HasValue)
				{
					vector = vector2;
				}
				if (IsClear(vector2.Value, taken, minDistance))
				{
					return vector2.Value;
				}
			}
		}
		return vector ?? preferred;
	}

	internal static Vector3? ResolveWithinReach(Vector3 preferred, Vector3 party, Vector3 forward, IList<Vector3> taken, float minDistance, Func<Vector3, Vector3?> snap)
	{
		if (snap == null)
		{
			return null;
		}
		Vector3? result = null;
		float[] reachFractions = ReachFractions;
		foreach (float num in reachFractions)
		{
			foreach (Vector3 item in Candidates(new Vector3(party.x + (preferred.x - party.x) * num, preferred.y, party.z + (preferred.z - party.z) * num), forward, minDistance))
			{
				Vector3? vector = snap(item);
				if (vector.HasValue)
				{
					if (IsClear(vector.Value, taken, minDistance))
					{
						return vector.Value;
					}
					if (!result.HasValue)
					{
						result = vector;
					}
				}
			}
		}
		return result;
	}

	internal static List<Vector3> ResolveAll(IList<Vector3> preferred, Vector3 forward, IEnumerable<Vector3> taken, float minDistance, Func<Vector3, Vector3?> snap)
	{
		List<Vector3> list = new List<Vector3>(taken ?? new Vector3[0]);
		List<Vector3> list2 = new List<Vector3>();
		foreach (Vector3 item2 in preferred)
		{
			Vector3 item = Resolve(item2, forward, list, minDistance, snap);
			list2.Add(item);
			list.Add(item);
		}
		return list2;
	}

	internal static List<string> TooClose(IList<string> names, IList<Vector3> points, float minDistance)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < points.Count; i++)
		{
			for (int j = i + 1; j < points.Count; j++)
			{
				float num = GroundDistance(points[i], points[j]);
				if (num < minDistance)
				{
					list.Add($"{names[i]} and {names[j]} are {num:0.0} m apart (at least {minDistance:0.0} m required).");
				}
			}
		}
		return list;
	}
}
