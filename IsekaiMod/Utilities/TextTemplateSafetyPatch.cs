using System;
using System.Collections.Generic;
using System.Diagnostics;
using HarmonyLib;
using IsekaiMod.Content.Constellations;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.TextTools;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

[HarmonyPatch]
internal static class TextTemplateSafetyPatch
{
	internal sealed class FailureLogRateLimiter
	{
		private readonly double m_IntervalSeconds;

		private readonly int m_MaxEntries;

		private readonly object m_Lock = new object();

		private double m_NextAllowedTime = double.NegativeInfinity;

		private int m_Logged;

		private int m_Suppressed;

		public FailureLogRateLimiter(double intervalSeconds, int maxEntries)
		{
			m_IntervalSeconds = intervalSeconds;
			m_MaxEntries = maxEntries;
		}

		public bool ShouldLog(double nowSeconds)
		{
			lock (m_Lock)
			{
				if (m_Logged >= m_MaxEntries || nowSeconds < m_NextAllowedTime)
				{
					m_Suppressed++;
					return false;
				}
				m_NextAllowedTime = nowSeconds + m_IntervalSeconds;
				m_Logged++;
				return true;
			}
		}

		public int TakeSuppressedCount()
		{
			lock (m_Lock)
			{
				int suppressed = m_Suppressed;
				m_Suppressed = 0;
				return suppressed;
			}
		}
	}

	private const string HumanRaceGuid = "0a5d473ead98b0646b94495af250fdc4";

	private static readonly Stopwatch Clock = Stopwatch.StartNew();

	private static readonly FailureLogRateLimiter FailureLog = new FailureLogRateLimiter(30.0, 20);

	internal static bool IsMainCharacterMissing(UnitReference mainCharacter)
	{
		try
		{
			return mainCharacter.Value == null;
		}
		catch
		{
			return true;
		}
	}

	internal static bool NoMainCharacter()
	{
		try
		{
			Player player = Game.Instance?.Player;
			return player == null || IsMainCharacterMissing(player.MainCharacter);
		}
		catch
		{
			return true;
		}
	}

	internal static string FallbackName()
	{
		return ConstellationText.Localized("TextTemplateFallback.Name", "Commander");
	}

	internal static string FallbackRace(bool capitalized)
	{
		string text = null;
		try
		{
			text = BlueprintTools.GetBlueprint<BlueprintRace>("0a5d473ead98b0646b94495af250fdc4")?.Name;
		}
		catch
		{
		}
		if (string.IsNullOrEmpty(text))
		{
			text = ConstellationText.Localized("TextTemplateFallback.Race", "Human");
		}
		if (!capitalized)
		{
			return text.ToLowerInvariant();
		}
		return text;
	}

	[HarmonyPatch(typeof(NameTemplate), "Generate")]
	[HarmonyPrefix]
	public static bool NameTemplate_Generate_Prefix(ref string __result)
	{
		if (NoMainCharacter())
		{
			__result = FallbackName();
			return false;
		}
		return true;
	}

	[HarmonyPatch(typeof(MaleFemaleTemplate), "Generate")]
	[HarmonyPrefix]
	public static bool MaleFemaleTemplate_Generate_Prefix(List<string> parameters, ref string __result)
	{
		if (NoMainCharacter())
		{
			__result = ((parameters != null && parameters.Count > 0) ? parameters[0] : "");
			return false;
		}
		return true;
	}

	[HarmonyPatch(typeof(RaceTemplate), "Generate")]
	[HarmonyPrefix]
	public static bool RaceTemplate_Generate_Prefix(bool capitalized, ref string __result)
	{
		if (NoMainCharacter())
		{
			__result = FallbackRace(capitalized);
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
			ReportTemplateFailure(text, __exception);
		}
		return null;
	}

	private static void ReportTemplateFailure(string text, Exception exception)
	{
		try
		{
			if (FailureLog.ShouldLog(Clock.Elapsed.TotalSeconds))
			{
				int num = FailureLog.TakeSuppressedCount();
				string arg = ((num > 0) ? $" ({num} further failure(s) not logged)" : "");
				Main.Log($"WARNING: TextTemplateEngine.Process failed on \"{text}\"{arg}: {exception}");
			}
		}
		catch
		{
		}
	}
}
