using System;
using System.Collections.Generic;

namespace IsekaiMod.Utilities;

internal static class DialogueGuardLogic
{
	internal sealed class AnswerPlan<TVm>
	{
		public readonly List<TVm> Answers = new List<TVm>();

		public readonly List<string> Skipped = new List<string>();

		public bool UsedFallback;

		public bool Empty => Answers.Count == 0;
	}

	internal sealed class OncePerKeyLog
	{
		private readonly int m_MaxKeys;

		private readonly object m_Lock = new object();

		private readonly HashSet<string> m_Seen = new HashSet<string>(StringComparer.Ordinal);

		private int m_Suppressed;

		public int SuppressedCount
		{
			get
			{
				lock (m_Lock)
				{
					return m_Suppressed;
				}
			}
		}

		public OncePerKeyLog(int maxKeys)
		{
			m_MaxKeys = maxKeys;
		}

		public bool ShouldLog(string key)
		{
			lock (m_Lock)
			{
				key = key ?? string.Empty;
				if (m_Seen.Contains(key) || m_Seen.Count >= m_MaxKeys)
				{
					m_Suppressed++;
					return false;
				}
				m_Seen.Add(key);
				return true;
			}
		}
	}

	internal static AnswerPlan<TVm> BuildAnswers<TSrc, TVm>(IEnumerable<TSrc> sources, Func<TSrc, string> describe, Func<TSrc, int, TVm> build, Func<TVm> fallback)
	{
		AnswerPlan<TVm> answerPlan = new AnswerPlan<TVm>();
		if (sources != null && build != null)
		{
			int num = 0;
			foreach (TSrc source in sources)
			{
				if (source == null)
				{
					continue;
				}
				try
				{
					TVm val = build(source, num);
					if (val != null)
					{
						answerPlan.Answers.Add(val);
						num++;
					}
				}
				catch (Exception exception)
				{
					answerPlan.Skipped.Add(Describe(describe, source, exception));
				}
			}
		}
		if (answerPlan.Answers.Count == 0 && fallback != null)
		{
			try
			{
				TVm val2 = fallback();
				if (val2 != null)
				{
					answerPlan.Answers.Add(val2);
					answerPlan.UsedFallback = true;
				}
			}
			catch (Exception exception2)
			{
				answerPlan.Skipped.Add("the fallback Exit answer (" + ShortReason(exception2) + ")");
			}
		}
		return answerPlan;
	}

	private static string Describe<TSrc>(Func<TSrc, string> describe, TSrc source, Exception exception)
	{
		string text;
		try
		{
			text = describe?.Invoke(source);
		}
		catch
		{
			text = null;
		}
		return (string.IsNullOrEmpty(text) ? "<unnamed answer>" : text) + " (" + ShortReason(exception) + ")";
	}

	private static string ShortReason(Exception exception)
	{
		if (exception == null)
		{
			return "no exception";
		}
		Exception ex = exception;
		while (ex.InnerException != null)
		{
			ex = ex.InnerException;
		}
		return ex.GetType().Name + ": " + ex.Message;
	}

	internal static bool NeedsAnswerRepair(bool threw, int survivingAnswers)
	{
		if (threw)
		{
			return survivingAnswers <= 0;
		}
		return false;
	}

	internal static bool NeedsDialogStop(bool threw, bool dialogRunning, int controllerAnswers)
	{
		if (threw & dialogRunning)
		{
			return controllerAnswers <= 0;
		}
		return false;
	}

	internal static bool NeedsCueRepair(bool threw, bool cuePresent)
	{
		if (threw)
		{
			return !cuePresent;
		}
		return false;
	}

	internal static string FailureLine(string dialog, string cue, int keptAnswers, bool usedFallback, IEnumerable<string> skipped, Exception exception)
	{
		string text = "Dialogue guard: " + (string.IsNullOrEmpty(dialog) ? "<unknown dialog>" : dialog) + " / cue " + (string.IsNullOrEmpty(cue) ? "<unknown cue>" : cue) + " threw while the window was being built; the conversation was kept usable with " + keptAnswers + " answer(s)" + (usedFallback ? " (the vanilla Exit answer only)" : "") + ".";
		if (skipped != null)
		{
			List<string> list = new List<string>(skipped);
			if (list.Count > 0)
			{
				text = text + " Skipped: " + string.Join("; ", list.ToArray()) + ".";
			}
		}
		return text + " " + ((exception == null) ? "<no exception>" : exception.ToString());
	}
}
