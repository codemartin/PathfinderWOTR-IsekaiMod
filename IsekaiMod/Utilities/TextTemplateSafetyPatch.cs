using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker;
using Kingmaker.TextTools;

namespace IsekaiMod.Utilities
{
	[HarmonyPatch]
	internal static class TextTemplateSafetyPatch
	{
		[HarmonyPatch(typeof(NameTemplate), "Generate")]
		[HarmonyPrefix]
		public static bool NameTemplate_Generate_Prefix(ref string __result)
		{
			try
			{
				Game instance = Game.Instance;
				if (instance == null)
				{
					goto IL_0021;
				}
				Player player = instance.Player;
				if (player == null)
				{
					goto IL_0021;
				}
				_ = player.MainCharacter;
				if (false)
				{
					goto IL_0021;
				}
				goto end_IL_0000;
				IL_0021:
				__result = "Commander";
				return false;
				end_IL_0000:;
			}
			catch
			{
				__result = "Commander";
				return false;
			}
			return true;
		}

		[HarmonyPatch(typeof(MaleFemaleTemplate), "Generate")]
		[HarmonyPrefix]
		public static bool MaleFemaleTemplate_Generate_Prefix(List<string> parameters, ref string __result)
		{
			try
			{
				Game instance = Game.Instance;
				if (instance == null)
				{
					goto IL_0021;
				}
				Player player = instance.Player;
				if (player == null)
				{
					goto IL_0021;
				}
				_ = player.MainCharacter;
				if (false)
				{
					goto IL_0021;
				}
				goto end_IL_0000;
				IL_0021:
				__result = ((parameters != null && parameters.Count > 0) ? parameters[0] : "");
				return false;
				end_IL_0000:;
			}
			catch
			{
				__result = ((parameters != null && parameters.Count > 0) ? parameters[0] : "");
				return false;
			}
			return true;
		}

		[HarmonyPatch(typeof(RaceTemplate), "Generate")]
		[HarmonyPrefix]
		public static bool RaceTemplate_Generate_Prefix(ref string __result)
		{
			try
			{
				Game instance = Game.Instance;
				if (instance == null)
				{
					goto IL_0021;
				}
				Player player = instance.Player;
				if (player == null)
				{
					goto IL_0021;
				}
				_ = player.MainCharacter;
				if (false)
				{
					goto IL_0021;
				}
				goto end_IL_0000;
				IL_0021:
				__result = "Human";
				return false;
				end_IL_0000:;
			}
			catch
			{
				__result = "Human";
				return false;
			}
			return true;
		}

		[HarmonyPatch(typeof(TextTemplateEngine), "Process")]
		[HarmonyFinalizer]
		public static Exception Process_Finalizer(Exception __exception, string text, ref string __result)
		{
			if (__exception != null)
			{
				__result = text ?? string.Empty;
				return null;
			}
			return null;
		}
	}
}
