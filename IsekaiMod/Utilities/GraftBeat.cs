using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Utilities;

internal sealed class GraftBeat
{
	public string Name { get; private set; }

	public string HostList { get; private set; }

	public IReadOnlyList<string> PresentingCues { get; private set; }

	public bool Hub { get; private set; }

	public string Why { get; private set; }

	private GraftBeat()
	{
	}

	internal static GraftBeat At(string name, string hostList, string why, params string[] presentingCues)
	{
		if (presentingCues == null || presentingCues.Length == 0)
		{
			throw new ArgumentException("A beat names at least one presenting cue; use Hub for a topic hub.", "presentingCues");
		}
		return new GraftBeat
		{
			Name = name,
			HostList = Normalize(hostList),
			PresentingCues = presentingCues.Select(Normalize).ToArray(),
			Why = why
		};
	}

	internal static GraftBeat HubOf(string name, string hostList, string why)
	{
		return new GraftBeat
		{
			Name = name,
			HostList = Normalize(hostList),
			PresentingCues = new string[0],
			Hub = true,
			Why = why
		};
	}

	internal static string Normalize(string id)
	{
		return (id ?? string.Empty).Replace("-", string.Empty).Trim().ToLowerInvariant();
	}

	public override string ToString()
	{
		return Name;
	}
}
