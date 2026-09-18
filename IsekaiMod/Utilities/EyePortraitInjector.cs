using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Blueprints;
using UnityEngine;

namespace IsekaiMod.Utilities
{
	[HarmonyPatch(typeof(PortraitData), "get_PetEyePortrait")]
	public static class EyePortraitInjector
	{
		public static Dictionary<PortraitData, Sprite> Replacements = new Dictionary<PortraitData, Sprite>();

		public static bool Prefix(PortraitData __instance, ref Sprite __result)
		{
			if (__instance != null && Replacements.TryGetValue(__instance, out var value) && value != null)
			{
				__result = value;
				return false;
			}
			return true;
		}
	}
}
