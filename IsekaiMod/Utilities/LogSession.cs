using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace IsekaiMod.Utilities;

public static class LogSession
{
	public const string Marker = "[IsekaiMod] Log session ";

	private static readonly Regex Pattern = new Regex("\\[IsekaiMod\\] Log session ([0-9a-f]{32})\\b", RegexOptions.CultureInvariant);

	public static string Id { get; private set; }

	public static string Format(string id, DateTime started)
	{
		return "[IsekaiMod] Log session " + id + " started " + started.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
	}

	public static string IdIn(string line)
	{
		if (string.IsNullOrEmpty(line))
		{
			return null;
		}
		Match match = Pattern.Match(line);
		if (!match.Success)
		{
			return null;
		}
		return match.Groups[1].Value;
	}

	public static string NewId()
	{
		return Guid.NewGuid().ToString("N");
	}

	internal static void Write()
	{
		if (Id != null)
		{
			return;
		}
		try
		{
			Id = NewId();
			Debug.Log(Format(Id, DateTime.Now));
		}
		catch
		{
		}
	}
}
