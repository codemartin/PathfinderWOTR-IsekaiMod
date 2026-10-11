using System;
using System.Collections.Generic;

namespace IsekaiMod.Utilities.SelfTest;

public static class DialogueShowCheckRules
{
	public const int MinSaneDc = 5;

	public const int MaxSaneDc = 60;

	public static readonly IReadOnlyDictionary<string, string> Allowed = new Dictionary<string, string>(StringComparer.Ordinal);

	public static string Problem(string answerName, string statType, int dc)
	{
		if (string.IsNullOrEmpty(statType) || !(statType != "Unknown"))
		{
			if (dc != 0)
			{
				return $"{answerName}: ShowCheck has no stat (Unknown) but DC {dc}; the check never rolls, so the DC is dead data";
			}
			return null;
		}
		if (answerName == null || !Allowed.ContainsKey(answerName))
		{
			return $"{answerName}: ShowCheck {statType} DC {dc} rolls a party check while the answer list is built, before the player " + "chooses, and a failure hides the answer for the rest of the save. Add it to DialogueShowCheckRules.Allowed with the reason, or gate the answer with a ShowCondition instead";
		}
		if (dc < 5 || dc > 60)
		{
			return $"{answerName}: ShowCheck {statType} DC {dc} is outside {5}-{60}";
		}
		return null;
	}
}
