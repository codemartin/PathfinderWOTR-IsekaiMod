using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.View;
using UnityEngine;

namespace IsekaiMod.Content.Guardians;

internal static class GuardianVisualScale
{
	private sealed class PolymorphViewCorrection
	{
		public float Factor;

		public Vector3 AuthoredDollRoomScale;
	}

	[HarmonyPatch(typeof(UnitEntityView), "CalculateSizeScale")]
	internal static class UnitEntityView_CalculateSizeScale_Patch
	{
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return PatchGuard.Resolves(typeof(UnitEntityView_CalculateSizeScale_Patch), typeof(UnitEntityView), "CalculateSizeScale");
		}

		[HarmonyPostfix]
		private static void Postfix(UnitEntityView __instance, ref float __result)
		{
			try
			{
				if (s_Factors.Count != 0)
				{
					BlueprintUnit blueprintUnit = __instance?.EntityData?.Blueprint;
					if (blueprintUnit != null && s_Factors.TryGetValue(blueprintUnit.AssetGuid, out var value))
					{
						__result *= value;
					}
				}
			}
			catch (Exception ex)
			{
				Main.Log("Guardian model: the visual scale correction failed: " + ex);
			}
		}
	}

	[HarmonyPatch(typeof(UnitEntityView), "OnDidAttachToData")]
	internal static class UnitEntityView_OnDidAttachToData_Patch
	{
		[HarmonyPrefix]
		private static void Prefix(UnitEntityView __instance)
		{
			try
			{
				if (s_PolymorphBuffs.Count != 0 && !(__instance == null) && __instance.DisableSizeScaling)
				{
					CorrectPolymorphView(__instance, OwedPolymorphFactor(__instance.EntityData));
				}
			}
			catch (Exception ex)
			{
				Main.Log("Guardian model: the polymorph view correction failed: " + ex);
			}
		}
	}

	[HarmonyPatch(typeof(UnitEntityView), "GetSizeScale", new Type[] { })]
	internal static class UnitEntityView_GetSizeScale_Patch
	{
		[HarmonyPostfix]
		private static void Postfix(UnitEntityView __instance, ref float __result)
		{
			if (!(__instance == null) && __instance.DisableSizeScaling && s_PolymorphViews.TryGetValue(__instance, out var value))
			{
				__result *= value.Factor;
			}
		}
	}

	internal const float MediumModelHeight = 1.8f;

	internal const float SizeStep = 0.66f;

	internal const float Tolerance = 0.25f;

	internal const float MinFactor = 0.2f;

	internal const float MaxFactor = 5f;

	private static readonly Dictionary<string, float> MeasuredModelHeight = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
	{
		{ "b50fa8864626ccc4cb7c2cd69575f5d2", 3.816f },
		{ "7836da2cd24be154fa7a10225c86c9f5", 2.891f },
		{ "88d43b5521651b043ad42d5febd03693", 1.721f },
		{ "6add925f7546e1947bbb58778fd736d8", 1.796f }
	};

	private static readonly Dictionary<BlueprintGuid, float> s_Factors = new Dictionary<BlueprintGuid, float>();

	private static readonly HashSet<BlueprintGuid> s_PolymorphBuffs = new HashSet<BlueprintGuid>();

	private static readonly ConditionalWeakTable<UnitEntityView, PolymorphViewCorrection> s_PolymorphViews = new ConditionalWeakTable<UnitEntityView, PolymorphViewCorrection>();

	internal static IReadOnlyDictionary<BlueprintGuid, float> Registered => s_Factors;

	internal static IEnumerable<BlueprintGuid> RegisteredPolymorphBuffs => s_PolymorphBuffs;

	internal static float? MeasuredHeight(string prefabAssetId)
	{
		if (string.IsNullOrEmpty(prefabAssetId))
		{
			return null;
		}
		if (!MeasuredModelHeight.TryGetValue(prefabAssetId, out var value))
		{
			return null;
		}
		return value;
	}

	internal static float NominalHeight(Size size)
	{
		float num = 1.8f;
		int num2 = (int)(size - 4);
		for (int i = 0; i < Math.Abs(num2); i++)
		{
			num = ((num2 < 0) ? (num * 0.66f) : (num / 0.66f));
		}
		return num;
	}

	internal static float Factor(float modelHeight, Size size)
	{
		if (modelHeight <= 0.01f || float.IsNaN(modelHeight) || float.IsInfinity(modelHeight))
		{
			return 1f;
		}
		float num = NominalHeight(size) / modelHeight;
		if (Math.Abs(1f - num) <= 0.25f)
		{
			return 1f;
		}
		if (num < 0.2f)
		{
			return 0.2f;
		}
		if (num > 5f)
		{
			return 5f;
		}
		return num;
	}

	internal static float FactorFor(BlueprintUnit unit)
	{
		if (unit == null)
		{
			return 1f;
		}
		return FactorForPrefab(unit.Prefab?.AssetId, unit.Size);
	}

	internal static float FactorForPrefab(string prefabAssetId, Size size)
	{
		if (string.IsNullOrEmpty(prefabAssetId))
		{
			return 1f;
		}
		if (!MeasuredModelHeight.TryGetValue(prefabAssetId, out var value))
		{
			return 1f;
		}
		return Factor(value, size);
	}

	internal static void Register(params BlueprintUnit[] units)
	{
		if (units == null)
		{
			return;
		}
		foreach (BlueprintUnit blueprintUnit in units)
		{
			if (blueprintUnit != null)
			{
				float num = FactorFor(blueprintUnit);
				if (!(Math.Abs(1f - num) < 0.001f))
				{
					s_Factors[blueprintUnit.AssetGuid] = num;
					Main.Log($"Guardian model: {blueprintUnit.name} is drawn at {num:0.###} so its Size.{blueprintUnit.Size} model matches its size.");
				}
			}
		}
	}

	internal static float RegisteredFactor(BlueprintGuid guid)
	{
		if (!s_Factors.TryGetValue(guid, out var value))
		{
			return 1f;
		}
		return value;
	}

	internal static void RegisterPolymorphBuff(BlueprintBuff buff)
	{
		Polymorph polymorph = buff?.GetComponent<Polymorph>();
		if (polymorph != null)
		{
			s_PolymorphBuffs.Add(buff.AssetGuid);
			Main.Log($"Guardian model: {buff.name} draws its Size.{polymorph.Size} polymorph model at " + $"{PolymorphFactor(polymorph, Gender.Male):0.###} for a male wearer and {PolymorphFactor(polymorph, Gender.Female):0.###} for a female one.");
		}
	}

	internal static bool IsRegisteredPolymorphBuff(BlueprintBuff buff)
	{
		if (buff != null)
		{
			return s_PolymorphBuffs.Contains(buff.AssetGuid);
		}
		return false;
	}

	internal static UnitViewLink PrefabFor(Polymorph poly, Gender gender)
	{
		if (poly == null)
		{
			return null;
		}
		UnitViewLink prefabFemale = poly.m_PrefabFemale;
		if (gender != Gender.Male && prefabFemale != null && !string.IsNullOrEmpty(prefabFemale.AssetId))
		{
			return prefabFemale;
		}
		return poly.m_Prefab;
	}

	internal static float PolymorphFactor(Polymorph poly, Gender gender)
	{
		if (poly == null)
		{
			return 1f;
		}
		return FactorForPrefab(PrefabFor(poly, gender)?.AssetId, poly.Size);
	}

	internal static float OwedPolymorphFactor(UnitEntityData unit)
	{
		if (unit == null || s_PolymorphBuffs.Count == 0)
		{
			return 1f;
		}
		BlueprintComponentAndRuntime<Polymorph> activePolymorph = unit.GetActivePolymorph();
		if (activePolymorph.Component == null)
		{
			return 1f;
		}
		if (!IsRegisteredPolymorphBuff(activePolymorph.Runtime?.Fact?.Blueprint as BlueprintBuff))
		{
			return 1f;
		}
		return PolymorphFactor(activePolymorph.Component, unit.Gender);
	}

	internal static void CorrectPolymorphView(UnitEntityView view, float factor)
	{
		if (!(view == null))
		{
			if (s_PolymorphViews.TryGetValue(view, out var value))
			{
				view.OverrideDollRoomScale = value.AuthoredDollRoomScale;
				s_PolymorphViews.Remove(view);
			}
			if (!(Math.Abs(1f - factor) < 0.001f))
			{
				Vector3 overrideDollRoomScale = view.OverrideDollRoomScale;
				s_PolymorphViews.Add(view, new PolymorphViewCorrection
				{
					Factor = factor,
					AuthoredDollRoomScale = overrideDollRoomScale
				});
				view.OverrideDollRoomScale = ((overrideDollRoomScale == Vector3.zero) ? Vector3.one : overrideDollRoomScale) * factor;
			}
		}
	}

	internal static float CorrectionOf(UnitEntityView view)
	{
		if (!(view != null) || !s_PolymorphViews.TryGetValue(view, out var value))
		{
			return 1f;
		}
		return value.Factor;
	}
}
