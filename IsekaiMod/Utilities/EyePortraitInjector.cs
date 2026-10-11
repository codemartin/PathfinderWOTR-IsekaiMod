using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Blueprints;
using UnityEngine;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(PortraitData), "get_PetEyePortrait")]
public static class EyePortraitInjector
{
	public static Dictionary<PortraitData, Sprite> Replacements = new Dictionary<PortraitData, Sprite>();

	private static readonly HashSet<PortraitData> CropFallback = new HashSet<PortraitData>();

	private static readonly Dictionary<PortraitData, Sprite> Crops = new Dictionary<PortraitData, Sprite>();

	internal const float PetEyeWidth = 176f;

	internal const float PetEyeHeight = 24f;

	internal const float EyeLine = 0.6f;

	[HarmonyPrepare]
	private static bool Prepare()
	{
		return PatchGuard.Resolves(typeof(EyePortraitInjector), typeof(PortraitData), "get_PetEyePortrait");
	}

	internal static void RegisterCropFallback(PortraitData portrait)
	{
		if (portrait == null || Replacements.ContainsKey(portrait))
		{
			return;
		}
		lock (CropFallback)
		{
			CropFallback.Add(portrait);
		}
	}

	internal static bool HasCropFallback(PortraitData portrait)
	{
		if (portrait == null)
		{
			return false;
		}
		lock (CropFallback)
		{
			return CropFallback.Contains(portrait);
		}
	}

	internal static Rect EyeBand(Rect source)
	{
		float num = Math.Max(0f, source.width);
		float num2 = Math.Min(Math.Max(0f, source.height), num * 24f / 176f);
		float val = source.y + source.height * 0.6f - num2 / 2f;
		val = Math.Max(source.y, Math.Min(val, source.y + source.height - num2));
		return new Rect(source.x, val, num, num2);
	}

	public static bool Prefix(PortraitData __instance, ref Sprite __result)
	{
		if (__instance != null && Replacements.TryGetValue(__instance, out __result))
		{
			return false;
		}
		return true;
	}

	public static void Postfix(PortraitData __instance, ref Sprite __result)
	{
		if (__result != null || !HasCropFallback(__instance))
		{
			return;
		}
		try
		{
			__result = CropFor(__instance);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext?.Logger?.LogError("[PetEye] Could not cut a pet eye-strip: " + ex.Message);
		}
	}

	private static Sprite CropFor(PortraitData portrait)
	{
		lock (Crops)
		{
			if (Crops.TryGetValue(portrait, out var value) && value != null)
			{
				return value;
			}
		}
		Sprite smallPortrait = portrait.SmallPortrait;
		if (smallPortrait == null || smallPortrait.texture == null)
		{
			return null;
		}
		Rect source;
		try
		{
			source = smallPortrait.textureRect;
		}
		catch (Exception)
		{
			source = smallPortrait.rect;
		}
		Sprite sprite = Sprite.Create(smallPortrait.texture, EyeBand(source), new Vector2(0.5f, 0.5f), smallPortrait.pixelsPerUnit);
		lock (Crops)
		{
			Crops[portrait] = sprite;
			return sprite;
		}
	}
}
