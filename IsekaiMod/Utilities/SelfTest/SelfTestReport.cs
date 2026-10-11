using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IsekaiMod.Utilities.SelfTest;

public sealed class SelfTestReport
{
	public sealed class ReferenceProblem
	{
		public string Blueprint { get; }

		public string Path { get; }

		public string Problem { get; }

		public ReferenceProblem(string blueprint, string path, string problem)
		{
			Blueprint = blueprint ?? "";
			Path = path ?? "";
			Problem = problem ?? "";
		}
	}

	public const string Marker = "[IsekaiSelfTest]";

	public const string KnownIssuePrefix = "KNOWN ISSUE ";

	public const int MaxDetailsPerCheck = 200;

	public const int MaxLineLength = 600;

	public const int WrapWidth = 540;

	private static readonly Regex IdCharacters = new Regex("[^a-z0-9-]+", RegexOptions.CultureInvariant);

	private static readonly Regex IndexPart = new Regex("\\[\\d+\\]", RegexOptions.CultureInvariant);

	private readonly Action<string> sink;

	private readonly object gate = new object();

	private int passed;

	private int failed;

	private bool begun;

	private bool ended;

	public int Passed
	{
		get
		{
			lock (gate)
			{
				return passed;
			}
		}
	}

	public int Failed
	{
		get
		{
			lock (gate)
			{
				return failed;
			}
		}
	}

	public bool Ended
	{
		get
		{
			lock (gate)
			{
				return ended;
			}
		}
	}

	public static string FieldPattern(string path)
	{
		return IndexPart.Replace(path ?? "", "[]");
	}

	public static string Positions(string path)
	{
		return string.Concat(from Match m in IndexPart.Matches(path ?? "")
			select m.Value);
	}

	public static List<string> GroupReferenceProblems(IEnumerable<ReferenceProblem> problems)
	{
		List<string> list = new List<string>();
		foreach (IGrouping<string, ReferenceProblem> item in (from p in problems ?? Enumerable.Empty<ReferenceProblem>()
			where p != null
			group p by p.Blueprint + " " + FieldPattern(p.Path)).OrderBy((IGrouping<string, ReferenceProblem> g) => g.Key, StringComparer.Ordinal))
		{
			List<string> list2 = new List<string>();
			foreach (IGrouping<string, ReferenceProblem> item2 in from p in item
				group p by p.Problem)
			{
				List<string> list3 = (from p in item2
					select Positions(p.Path) into p
					where p.Length > 0
					select p).ToList();
				if (list3.Count == 0)
				{
					list2.Add(item2.Key);
					continue;
				}
				list2.Add(item2.Key + " at " + list3[0]);
				list2.AddRange(list3.Skip(1));
			}
			list.AddRange(WrapList($"{item.Key} ({item.Count()}):", list2));
		}
		return list;
	}

	public static List<string> WrapList(string head, IEnumerable<string> items)
	{
		head = head ?? "";
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder(head);
		bool flag = true;
		foreach (string item in (items ?? Enumerable.Empty<string>()).Where((string i) => !string.IsNullOrEmpty(i)))
		{
			if (!flag && stringBuilder.Length + 2 + item.Length > 540)
			{
				list.Add(stringBuilder.ToString());
				stringBuilder = new StringBuilder(head.TrimEnd(':') + " (continued):");
				flag = true;
			}
			stringBuilder.Append(flag ? " " : ", ").Append(item);
			flag = false;
		}
		list.Add(stringBuilder.ToString());
		return list;
	}

	public SelfTestReport(Action<string> sink)
	{
		this.sink = sink ?? throw new ArgumentNullException("sink");
	}

	public static string Clean(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder(text.Length);
		foreach (char c in text)
		{
			stringBuilder.Append(char.IsControl(c) ? ' ' : c);
		}
		string text2 = stringBuilder.ToString().Trim();
		if (text2.Length > 600)
		{
			return text2.Substring(0, 600) + "...";
		}
		return text2;
	}

	public static string CleanId(string id)
	{
		string text = IdCharacters.Replace((id ?? "").Trim().ToLowerInvariant().Replace(' ', '-'), "").Trim('-');
		if (text.Length != 0)
		{
			return text;
		}
		return "unnamed";
	}

	public static string FormatBegin(string details)
	{
		string text = Clean(details);
		if (text.Length != 0)
		{
			return "[IsekaiSelfTest] BEGIN " + text;
		}
		return "[IsekaiSelfTest] BEGIN";
	}

	public static string FormatInfo(string text)
	{
		return "[IsekaiSelfTest] INFO " + Clean(text);
	}

	public static string FormatResult(string id, bool success, string summary)
	{
		return "[IsekaiSelfTest]" + (success ? " PASS " : " FAIL ") + CleanId(id) + ": " + Clean(summary);
	}

	public static string FormatDetail(string detail)
	{
		return "[IsekaiSelfTest]   - " + Clean(detail);
	}

	public static string FormatEnd(int passedCount, int failedCount)
	{
		return "[IsekaiSelfTest] END passed=" + passedCount + " failed=" + failedCount;
	}

	public static List<string> FormatResultLines(string id, bool success, string summary, IEnumerable<string> details)
	{
		List<string> list = new List<string> { FormatResult(id, success, summary) };
		List<string> list2 = (details ?? Enumerable.Empty<string>()).Where((string d) => !string.IsNullOrWhiteSpace(d)).ToList();
		foreach (string item in list2.Take(200))
		{
			list.Add(FormatDetail(item));
		}
		if (list2.Count > 200)
		{
			list.Add(FormatDetail("... and " + (list2.Count - 200) + " more"));
		}
		return list;
	}

	public void Begin(string details)
	{
		lock (gate)
		{
			if (!begun && !ended)
			{
				begun = true;
				Write(FormatBegin(details));
			}
		}
	}

	public void Info(string text)
	{
		lock (gate)
		{
			if (!ended)
			{
				Write(FormatInfo(text));
			}
		}
	}

	public bool Result(string id, bool success, string summary, IEnumerable<string> details = null)
	{
		lock (gate)
		{
			if (ended)
			{
				return false;
			}
			if (success)
			{
				passed++;
			}
			else
			{
				failed++;
			}
			foreach (string item in FormatResultLines(id, success, summary, details))
			{
				Write(item);
			}
			return true;
		}
	}

	public bool End()
	{
		lock (gate)
		{
			if (ended)
			{
				return false;
			}
			ended = true;
			Write(FormatEnd(passed, failed));
			return true;
		}
	}

	private void Write(string line)
	{
		try
		{
			sink(line);
		}
		catch
		{
		}
	}
}
