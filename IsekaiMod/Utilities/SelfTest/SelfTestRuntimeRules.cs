using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace IsekaiMod.Utilities.SelfTest;

public static class SelfTestRuntimeRules
{
	public const int MaxSaneDc = 80;

	public const int MinSaneDc = 0;

	public const int MaxSaneCasterLevel = 60;

	public const int MinSaneCasterLevel = 1;

	private static readonly Regex OfferPrice = new Regex("([0-9][0-9,]*)\\s*Coins", RegexOptions.CultureInvariant);

	public static string DcProblem(int dc)
	{
		if (dc < 0)
		{
			return $"DC {dc} is negative";
		}
		if (dc > 80)
		{
			return $"DC {dc} is above the sane maximum of {80}";
		}
		return null;
	}

	public static string CasterLevelProblem(int casterLevel)
	{
		if (casterLevel < 1)
		{
			return $"caster level {casterLevel} is below {1}";
		}
		if (casterLevel > 60)
		{
			return $"caster level {casterLevel} is above the sane maximum of {60}";
		}
		return null;
	}

	public static List<int> LabelledCoinAmounts(string answerText)
	{
		List<int> list = new List<int>();
		if (string.IsNullOrEmpty(answerText))
		{
			return list;
		}
		foreach (Match item in OfferPrice.Matches(answerText))
		{
			if (int.TryParse(item.Groups[1].Value.Replace(",", ""), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
			{
				list.Add(result);
			}
		}
		return list;
	}

	public static int? LabelledCoins(string answerText)
	{
		List<int> list = LabelledCoinAmounts(answerText);
		if (list.Count != 0)
		{
			return list[0];
		}
		return null;
	}

	public static List<string> PriceProblems(string answerName, string labelText, int? actionCost, int? conditionCost)
	{
		List<string> list = new List<string>();
		List<int> list2 = LabelledCoinAmounts(labelText);
		if (actionCost.HasValue && actionCost.Value > 0)
		{
			if (list2.Count == 0)
			{
				list.Add($"{answerName}: the action spends {actionCost.Value} coins but the label advertises no price.");
			}
			else if (!list2.Contains(actionCost.Value))
			{
				list.Add(string.Format("{0}: the label says {1} coins but the action spends {2}.", answerName, string.Join(" / ", list2), actionCost.Value));
			}
			if (!conditionCost.HasValue)
			{
				list.Add($"{answerName}: the action spends {actionCost.Value} coins but the answer has no affordability condition, so it is offered as selectable at any balance.");
			}
			else if (conditionCost.Value != actionCost.Value)
			{
				list.Add($"{answerName}: the action spends {actionCost.Value} coins but the affordability condition checks {conditionCost.Value}.");
			}
		}
		else if (conditionCost.HasValue && conditionCost.Value > 0)
		{
			list.Add($"{answerName}: the affordability condition checks {conditionCost.Value} coins but nothing in the answer spends any.");
		}
		return list;
	}

	public static List<string> UnreachableObjectives(IEnumerable<string> listed, IDictionary<string, List<string>> next, IDictionary<string, List<string>> addendums)
	{
		HashSet<string> source = new HashSet<string>(listed ?? Enumerable.Empty<string>(), StringComparer.Ordinal);
		HashSet<string> opened = new HashSet<string>(StringComparer.Ordinal);
		IDictionary<string, List<string>>[] array = new IDictionary<string, List<string>>[2] { next, addendums };
		foreach (IDictionary<string, List<string>> dictionary in array)
		{
			if (dictionary == null)
			{
				continue;
			}
			foreach (KeyValuePair<string, List<string>> item in dictionary)
			{
				foreach (string item2 in item.Value ?? new List<string>())
				{
					if (item2 != null && item2 != item.Key)
					{
						opened.Add(item2);
					}
				}
			}
		}
		HashSet<string> reachable = new HashSet<string>(source.Where((string o) => !opened.Contains(o)), StringComparer.Ordinal);
		Queue<string> queue = new Queue<string>(reachable);
		while (queue.Count > 0)
		{
			string key = queue.Dequeue();
			array = new IDictionary<string, List<string>>[2] { next, addendums };
			foreach (IDictionary<string, List<string>> dictionary2 in array)
			{
				if (dictionary2 == null || !dictionary2.TryGetValue(key, out var value))
				{
					continue;
				}
				foreach (string item3 in value ?? new List<string>())
				{
					if (item3 != null && reachable.Add(item3))
					{
						queue.Enqueue(item3);
					}
				}
			}
		}
		return source.Where((string o) => !reachable.Contains(o)).OrderBy((string o) => o, StringComparer.Ordinal).ToList();
	}

	public static string StackedBonusProblem(string what, string stat, int statBefore, int statAfter, int abilityModifierBefore, int abilityModifierAfter, int promised, string bonusType)
	{
		int num = abilityModifierAfter - abilityModifierBefore;
		int num2 = statAfter - statBefore - num;
		if (num2 == promised)
		{
			return null;
		}
		return $"{what} added {num2} {stat} next to another {bonusType} bonus (a rise of {statAfter - statBefore}, {num} of it from the ability modifier); its text promises +{promised} {bonusType} that stacks.";
	}

	public static HashSet<string> EndingObjectivesReached(IEnumerable<KeyValuePair<string, string>> answerEndings, Func<string, string> objectiveForEnding, List<string> problems)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		foreach (KeyValuePair<string, string> item in answerEndings ?? Enumerable.Empty<KeyValuePair<string, string>>())
		{
			string text = ((string.IsNullOrEmpty(item.Value) || objectiveForEnding == null) ? null : objectiveForEnding(item.Value));
			if (text == null)
			{
				problems?.Add(item.Key + " triggers the ending '" + (item.Value ?? "(null)") + "', which no ending objective records.");
			}
			else
			{
				hashSet.Add(text);
			}
		}
		return hashSet;
	}

	public static bool IsEmptyNode(bool hasText, bool hasAnswers, bool hasContinue, bool hasOnShowActions)
	{
		if (!hasText && !hasAnswers && !hasContinue)
		{
			return !hasOnShowActions;
		}
		return false;
	}

	public static bool AnswerMayResolveNoReply(bool requireValidCue, IEnumerable<bool> namedCuesCarryConditions)
	{
		if (requireValidCue || namedCuesCarryConditions == null)
		{
			return false;
		}
		List<bool> list = namedCuesCarryConditions.ToList();
		if (list.Count > 0)
		{
			return list.All((bool conditional) => conditional);
		}
		return false;
	}

	public static bool AnswerNoReplyIsModsToFix(bool modOwnsAnswer, bool modOwnsAnyReplyCue, bool requireValidCue, IEnumerable<bool> namedCuesCarryConditions)
	{
		if (!AnswerMayResolveNoReply(requireValidCue, namedCuesCarryConditions))
		{
			return false;
		}
		return modOwnsAnswer | modOwnsAnyReplyCue;
	}

	public static bool CueCanRefuse(bool showOnce, bool hasConditions, bool namesSpeaker)
	{
		return showOnce | hasConditions | namesSpeaker;
	}

	public static bool CueSelectionCanRefuseEntirely(IEnumerable<bool> cuesCanRefuse)
	{
		if (cuesCanRefuse == null)
		{
			return false;
		}
		List<bool> list = cuesCanRefuse.ToList();
		if (list.Count > 0)
		{
			return list.All((bool canRefuse) => canRefuse);
		}
		return false;
	}

	public static bool ContinueHidesAnswers(bool hasAnswers, bool hasContinue, bool continueCanRefuseEntirely)
	{
		if (hasAnswers & hasContinue)
		{
			return !continueCanRefuseEntirely;
		}
		return false;
	}

	public static bool ComplementaryPairAlwaysOffers(IList<KeyValuePair<string, bool>> soleConditions)
	{
		if (soleConditions == null)
		{
			return false;
		}
		for (int i = 0; i < soleConditions.Count; i++)
		{
			if (soleConditions[i].Key == null)
			{
				continue;
			}
			for (int j = i + 1; j < soleConditions.Count; j++)
			{
				if (soleConditions[j].Key == soleConditions[i].Key && soleConditions[j].Value != soleConditions[i].Value)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool CycleNodeCanStrand(bool onCycle, bool hasContinue, IEnumerable<bool> answerIsAlwaysOffered)
	{
		if ((!onCycle | hasContinue) || answerIsAlwaysOffered == null)
		{
			return false;
		}
		List<bool> list = answerIsAlwaysOffered.ToList();
		if (list.Count > 0)
		{
			return !list.Any((bool always) => always);
		}
		return false;
	}
}
