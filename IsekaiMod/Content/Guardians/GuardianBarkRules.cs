using System;
using System.Collections.Generic;
using System.Text;

namespace IsekaiMod.Content.Guardians;

internal static class GuardianBarkRules
{
	internal enum BarkCategory
	{
		Click,
		CombatStart,
		CombatEnd,
		Rest
	}

	internal const float SecondsBetweenAnyBarks = 4f;

	private static readonly Dictionary<string, float> s_LastBark = new Dictionary<string, float>(StringComparer.Ordinal);

	internal const string MasterToken = "{master}";

	internal const string MasterGenderTokenStart = "{master_mf|";

	internal static float CooldownSeconds(BarkCategory category)
	{
		return category switch
		{
			BarkCategory.Click => 20f, 
			BarkCategory.CombatStart => 60f, 
			BarkCategory.CombatEnd => 60f, 
			BarkCategory.Rest => 30f, 
			_ => 4f, 
		};
	}

	internal static float Chance(BarkCategory category)
	{
		if (category != BarkCategory.CombatStart && category != BarkCategory.CombatEnd)
		{
			return 1f;
		}
		return 0.5f;
	}

	internal static bool PassesChance(BarkCategory category, float roll)
	{
		return roll < Chance(category);
	}

	internal static bool MayBark(bool conscious, bool hasMaster, bool inParty, bool isInGame, bool restingInHiddenScene, bool dialogOrCutscene)
	{
		if ((!conscious || !hasMaster || !inParty) | dialogOrCutscene)
		{
			return false;
		}
		return isInGame | restingInHiddenScene;
	}

	internal static bool TryConsumeCooldown(string unitId, BarkCategory category, float now)
	{
		return TryConsumeCooldown(s_LastBark, unitId, category, now);
	}

	internal static bool TryConsumeCooldown(IDictionary<string, float> clock, string unitId, BarkCategory category, float now)
	{
		if (clock == null || string.IsNullOrEmpty(unitId))
		{
			return false;
		}
		string key = unitId + "|any";
		string key2 = unitId + "|" + category;
		if (clock.TryGetValue(key, out var value) && now - value < 4f)
		{
			return false;
		}
		if (clock.TryGetValue(key2, out var value2) && now - value2 < CooldownSeconds(category))
		{
			return false;
		}
		clock[key] = now;
		clock[key2] = now;
		return true;
	}

	internal static string ApplyTokens(string text, string masterName, bool masterIsFemale)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		string value = (string.IsNullOrEmpty(masterName) ? "Master" : masterName);
		StringBuilder stringBuilder = new StringBuilder(text.Length + 16);
		int num = 0;
		while (num < text.Length)
		{
			if (string.CompareOrdinal(text, num, "{master_mf|", 0, "{master_mf|".Length) == 0)
			{
				int num2 = text.IndexOf('}', num);
				if (num2 > num)
				{
					string[] array = text.Substring(num + "{master_mf|".Length, num2 - num - "{master_mf|".Length).Split('|');
					if (array.Length == 2)
					{
						stringBuilder.Append(masterIsFemale ? array[1] : array[0]);
						num = num2 + 1;
						continue;
					}
				}
			}
			if (string.CompareOrdinal(text, num, "{master}", 0, "{master}".Length) == 0)
			{
				stringBuilder.Append(value);
				num += "{master}".Length;
			}
			else
			{
				stringBuilder.Append(text[num]);
				num++;
			}
		}
		return stringBuilder.ToString();
	}
}
