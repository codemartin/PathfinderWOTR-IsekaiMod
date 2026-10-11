using System;
using System.Collections.Generic;
using System.Linq;
using Owlcat.Runtime.Core.Logging;

namespace IsekaiMod.Utilities.SelfTest;

internal sealed class SelfTestEngineLog : ILogSink
{
	internal sealed class Caught
	{
		internal string Context;

		internal string Channel;

		internal string Header = "";

		internal string Message = "";

		internal List<string> CatchFrames = new List<string>();

		internal List<string> ThrowFrames = new List<string>();

		internal string ClaimedBy;

		internal string Source;

		internal IEnumerable<string> AllFrames => CatchFrames.Concat(ThrowFrames);

		internal string ThrowSite
		{
			get
			{
				string text = EngineExceptionRules.ThrowSiteOf(ThrowFrames);
				if (text.Length <= 0)
				{
					return EngineExceptionRules.ThrowSiteOf(CatchFrames);
				}
				return text;
			}
		}

		internal string Signature => ThrowSite + "|" + (Message ?? "").Split('\n')[0].Trim();

		internal string Describe()
		{
			string text = (Message ?? "").Split('\n')[0].Trim();
			if (text.Length > 160)
			{
				text = text.Substring(0, 160) + "...";
			}
			string throwSite = ThrowSite;
			return "'" + text + "' thrown in " + ((throwSite.Length > 0) ? throwSite : "(no frame)");
		}
	}

	internal sealed class Capture : IDisposable
	{
		internal readonly List<Caught> Items = new List<Caught>();

		internal string Label { get; }

		internal Capture(string label)
		{
			Label = label;
		}

		public void Dispose()
		{
			lock (Gate)
			{
				OpenCaptures.Remove(this);
			}
		}
	}

	private const int MaxKept = 5000;

	internal const string Startup = "(startup)";

	internal const string BetweenChecks = "(between checks)";

	private static readonly object Gate = new object();

	private static readonly List<Capture> OpenCaptures = new List<Capture>();

	private static readonly List<Caught> Everything = new List<Caught>();

	private static readonly HashSet<string> MatchedKnown = new HashSet<string>(StringComparer.Ordinal);

	private static SelfTestEngineLog installed;

	private static int dropped;

	private LogInfo pendingHeader;

	private static HashSet<string> modNames;

	private static HashSet<string> modIds;

	private static int modCount = -1;

	internal static bool Installed => installed != null;

	internal static string Install()
	{
		try
		{
			if (installed != null)
			{
				return null;
			}
			SelfTestEngineLog logSink = new SelfTestEngineLog();
			Logger.Instance.AddLogger(logSink, populateWithExistingMessages: false);
			installed = logSink;
			return null;
		}
		catch (Exception ex)
		{
			return ex.GetType().Name + ": " + ex.Message;
		}
	}

	internal static Capture Open(string label)
	{
		Capture capture = new Capture(label);
		lock (Gate)
		{
			OpenCaptures.Add(capture);
			return capture;
		}
	}

	internal static List<Caught> All(out int droppedCount)
	{
		lock (Gate)
		{
			droppedCount = dropped;
			return new List<Caught>(Everything);
		}
	}

	internal static HashSet<string> KnownMatched()
	{
		lock (Gate)
		{
			return new HashSet<string>(MatchedKnown);
		}
	}

	internal static EngineExceptionRules.Known MatchKnown(Caught caught)
	{
		EngineExceptionRules.Known known = EngineExceptionRules.Match(caught.ThrowSite, caught.AllFrames);
		if (known != null)
		{
			lock (Gate)
			{
				MatchedKnown.Add(known.Id + "|" + known.ThrowSite);
			}
		}
		return known;
	}

	internal static List<string> Judge(IEnumerable<Caught> caught, out int unexplained, out int total)
	{
		List<string> list = new List<string>();
		unexplained = 0;
		total = 0;
		int num = 0;
		List<string> list2 = new List<string>();
		foreach (IGrouping<string, Caught> item in (from c in caught ?? Enumerable.Empty<Caught>()
			where c.ClaimedBy == null
			group c by c.Signature).OrderBy((IGrouping<string, Caught> g) => g.Key, StringComparer.Ordinal))
		{
			Caught caught2 = item.First();
			int num2 = item.Count();
			total += num2;
			if (!item.Any(IsOurs))
			{
				num += num2;
				if (list2.Count < 10)
				{
					list2.Add($"{caught2.Describe()} {num2} time(s)");
				}
				continue;
			}
			EngineExceptionRules.Known known = MatchKnown(caught2);
			foreach (Caught item2 in item.Skip(1))
			{
				MatchKnown(item2);
			}
			string text = EngineExceptionRules.FirstModFrame(caught2.AllFrames);
			string text2 = $"the engine caught {caught2.Describe()} {num2} time(s)" + ((text != null) ? (", on a stack through " + text) : "") + SourcesOf(item);
			if (known != null)
			{
				list.Add(EngineExceptionRules.KnownIssueLine(known, text2));
				continue;
			}
			unexplained++;
			list.Add(char.ToUpperInvariant(text2[0]) + text2.Substring(1) + ". Nothing escaped, so only GameLogFull.txt records it; the check provoked it and it is not allow-listed (EngineExceptionRules.KnownExceptions).");
		}
		if (num > 0)
		{
			list.AddRange(SelfTestReport.WrapList($"Not counted: {num} engine-caught exception(s) with no mod frame and no mod blueprint named" + " (the game's or another mod's, as Check-GameLog.ps1 reads them):", list2));
		}
		return list;
	}

	internal static string SourcesOf(IEnumerable<Caught> group)
	{
		List<string> list = (from c in @group ?? Enumerable.Empty<Caught>()
			select c.Source into s
			where !string.IsNullOrEmpty(s)
			select s).Distinct(StringComparer.Ordinal).OrderBy((string s) => s, StringComparer.Ordinal).ToList();
		if (list.Count == 0)
		{
			return "";
		}
		List<string> list2 = list.Take(5).ToList();
		return ", in " + string.Join("; ", list2) + ((list.Count > list2.Count) ? $" and {list.Count - list2.Count} more" : "");
	}

	internal static bool IsOurs(Caught caught)
	{
		if (caught == null)
		{
			return false;
		}
		HashSet<string> names;
		HashSet<string> ids;
		lock (Gate)
		{
			List<SelfTestRecorder.CreatedBlueprint> list = SelfTestRecorder.CreatedBlueprints();
			if (modNames == null || list.Count != modCount)
			{
				modNames = new HashSet<string>(from c in list
					where c.Name != null
					select c.Name, StringComparer.Ordinal);
				modIds = new HashSet<string>(list.Select((SelfTestRecorder.CreatedBlueprint c) => c.Guid.ToString().Replace("-", "").ToLowerInvariant()), StringComparer.Ordinal);
				modCount = list.Count;
			}
			names = modNames;
			ids = modIds;
		}
		return EngineExceptionRules.IsModException(caught.AllFrames, caught.Header + " " + caught.Message + " " + caught.Source, names, ids);
	}

	public void Log(LogInfo logInfo)
	{
		try
		{
			if (logInfo == null)
			{
				return;
			}
			if (logInfo.Severity < LogSeverity.Error)
			{
				pendingHeader = null;
				return;
			}
			List<string> list = FramesOf(logInfo);
			if (!EngineExceptionRules.IsExceptionBody(logInfo.IsException, list))
			{
				pendingHeader = ((logInfo.Callstack != null && logInfo.Callstack.Count > 0 && logInfo.Channel != LogChannel.Unity) ? logInfo : null);
				return;
			}
			Caught caught = new Caught
			{
				Channel = (logInfo.Channel?.Name ?? ""),
				Message = (logInfo.Message ?? ""),
				ThrowFrames = list,
				Source = SelfTestComponentContext.Current()
			};
			LogInfo logInfo2 = pendingHeader;
			pendingHeader = null;
			if (logInfo2 != null && logInfo.TimeStamp - logInfo2.TimeStamp < 0.25)
			{
				caught.Header = logInfo2.Message ?? "";
				caught.CatchFrames = FramesOf(logInfo2);
			}
			lock (Gate)
			{
				caught.Context = ((OpenCaptures.Count > 0) ? OpenCaptures[0].Label : (IsekaiSelfTest.RunnerStarted ? ("(between checks) after " + IsekaiSelfTest.CurrentCheck) : "(startup)"));
				if (Everything.Count < 5000)
				{
					Everything.Add(caught);
				}
				else
				{
					dropped++;
				}
				foreach (Capture openCapture in OpenCaptures)
				{
					openCapture.Items.Add(caught);
				}
			}
		}
		catch
		{
		}
	}

	public void Destroy()
	{
	}

	private static List<string> FramesOf(LogInfo info)
	{
		List<string> list = new List<string>();
		if (info?.Callstack == null)
		{
			return list;
		}
		foreach (LogStackFrame item in info.Callstack)
		{
			string text = null;
			try
			{
				text = item?.GetFormattedMethodName();
			}
			catch
			{
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				continue;
			}
			string[] array = text.Split('\n');
			foreach (string text2 in array)
			{
				if (!string.IsNullOrWhiteSpace(text2))
				{
					list.Add(text2.TrimEnd());
				}
			}
		}
		return list;
	}
}
