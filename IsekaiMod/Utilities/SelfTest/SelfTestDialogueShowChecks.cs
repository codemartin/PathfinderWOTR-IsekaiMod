using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.DialogSystem.Blueprints;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestDialogueShowChecks
{
	internal static SelfTestChecks.Outcome Run()
	{
		List<BlueprintAnswer> list = (from c in SelfTestProgressionChecks.ModBlueprints()
			select c.Blueprint).OfType<BlueprintAnswer>().OrderBy((BlueprintAnswer a) => a.name, StringComparer.Ordinal).ToList();
		if (list.Count == 0)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The mod created no answers, which cannot be right.");
		}
		List<string> list2 = new List<string>();
		int num = 0;
		foreach (BlueprintAnswer item in list)
		{
			ShowCheck showCheck = item.ShowCheck;
			if (showCheck == null)
			{
				list2.Add(item.name + ": ShowCheck is null; BlueprintAnswer.CanShow reads ShowCheck.Type and throws on it");
				continue;
			}
			if (item.HasShowCheck)
			{
				num++;
			}
			string text = DialogueShowCheckRules.Problem(item.name, showCheck.Type.ToString(), showCheck.DC);
			if (text != null)
			{
				list2.Add(text);
			}
		}
		return SelfTestChecks.Outcome.Of(list2.Count == 0, $"{list.Count} mod answers read; {num} carry a ShowCheck ({DialogueShowCheckRules.Allowed.Count} allowed); {list2.Count} problems.", list2);
	}
}
