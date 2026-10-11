using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Utilities.SelfTest;

public static class SaveStateFingerprint
{
	public static string Line(string storage, string key, string value)
	{
		return Escape(storage) + "/" + Escape(key) + " = " + Escape(value);
	}

	public static List<string> Of(IDictionary<string, Dictionary<string, string>> storages)
	{
		List<string> list = new List<string>();
		if (storages == null)
		{
			return list;
		}
		foreach (KeyValuePair<string, Dictionary<string, string>> storage in storages)
		{
			if (storage.Key == null)
			{
				continue;
			}
			if (storage.Value == null)
			{
				list.Add(Line(storage.Key, "", ""));
				continue;
			}
			foreach (KeyValuePair<string, string> item in storage.Value)
			{
				list.Add(Line(storage.Key, item.Key ?? "", item.Value ?? ""));
			}
		}
		list.Sort(StringComparer.Ordinal);
		return list;
	}

	public static List<string> Differences(IEnumerable<string> before, IEnumerable<string> after)
	{
		Dictionary<string, string> dictionary = Values(before);
		Dictionary<string, string> dictionary2 = Values(after);
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> item in dictionary.OrderBy((KeyValuePair<string, string> e) => e.Key, StringComparer.Ordinal))
		{
			if (!dictionary2.TryGetValue(item.Key, out var value))
			{
				list.Add("lost after the reload: " + item.Key + " was '" + item.Value + "'");
			}
			else if (!string.Equals(value, item.Value, StringComparison.Ordinal))
			{
				list.Add("changed across the reload: " + item.Key + " was '" + item.Value + "', is now '" + value + "'");
			}
		}
		foreach (KeyValuePair<string, string> item2 in dictionary2.OrderBy((KeyValuePair<string, string> e) => e.Key, StringComparer.Ordinal))
		{
			if (!dictionary.ContainsKey(item2.Key))
			{
				list.Add("appeared after the reload: " + item2.Key + " is '" + item2.Value + "'");
			}
		}
		return list;
	}

	private static Dictionary<string, string> Values(IEnumerable<string> lines)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
		foreach (string item in lines ?? Enumerable.Empty<string>())
		{
			if (item != null)
			{
				int num = item.IndexOf(" = ", StringComparison.Ordinal);
				if (num < 0)
				{
					dictionary[item] = "";
				}
				else
				{
					dictionary[item.Substring(0, num)] = item.Substring(num + 3);
				}
			}
		}
		return dictionary;
	}

	public static string Escape(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return "";
		}
		return text.Replace("\\", "\\\\").Replace("/", "\\s").Replace("=", "\\e")
			.Replace("\r", " ")
			.Replace("\n", " ");
	}
}
