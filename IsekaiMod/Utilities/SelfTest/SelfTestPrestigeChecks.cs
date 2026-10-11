using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestPrestigeChecks
{
	private const int IsekaiLevels = 10;

	internal static SelfTestChecks.Outcome PrestigeLevels()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		Stopwatch stopwatch = Stopwatch.StartNew();
		int num = CheckTables(list);
		int num2 = CheckClaims(list);
		int num3 = 0;
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		if (blueprintCharacterClass == null)
		{
			list.Add("The Isekai Protagonist class does not exist, so no prestige class could be levelled.");
		}
		else
		{
			foreach (string item in PrestigeLevelPlan.ClassNames.Where((string n) => n != "OtherworldRetinueClass"))
			{
				num3++;
				Build(blueprintCharacterClass, item, null, null, list, list3);
			}
			string[][] sovereignArchetypes = PrestigeLevelPlan.SovereignArchetypes;
			foreach (string[] array in sovereignArchetypes)
			{
				num3++;
				Build(blueprintCharacterClass, "TranscendentSovereignClass", array[1], array[0], list, list3);
			}
		}
		stopwatch.Stop();
		string summary = $"{PrestigeLevelPlan.ClassNames.Length} prestige classes: {num} level entries compared with the plan, " + $"{num2} promised components found, {num3} characters levelled Isekai {10} + prestige {10} " + $"in {stopwatch.Elapsed.TotalSeconds:0} s; {list.Count} problems.";
		list2.AddRange(list);
		List<string> list4 = list3.Distinct().ToList();
		if (list4.Count > 0)
		{
			list2.Add($"{list4.Count} generic selections could not be filled by a scripted build and are noted, not failed:");
			list2.AddRange(list4);
		}
		return SelfTestChecks.Outcome.Of(list.Count == 0, summary, list2);
	}

	private static BlueprintCharacterClass ClassNamed(string name)
	{
		return BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, name);
	}

	private static HashSet<string> Names(IEnumerable<BlueprintFeatureBase> features)
	{
		return new HashSet<string>(from f in features
			where f != null
			select f.name, StringComparer.Ordinal);
	}

	private static int CheckTables(List<string> problems)
	{
		int num = 0;
		string[] classNames = PrestigeLevelPlan.ClassNames;
		foreach (string text in classNames)
		{
			BlueprintProgression blueprintProgression = ClassNamed(text)?.Progression;
			if (blueprintProgression == null)
			{
				problems.Add(text + ": the class or its progression does not exist.");
				continue;
			}
			int level;
			for (level = 1; level <= 10; level++)
			{
				num++;
				HashSet<string> hashSet = Names(blueprintProgression.LevelEntries.Where((LevelEntry e) => e.Level == level).SelectMany((LevelEntry e) => e.Features));
				HashSet<string> hashSet2 = new HashSet<string>(PrestigeLevelPlan.AllAt(text, level), StringComparer.Ordinal);
				if (!hashSet.SetEquals(hashSet2))
				{
					problems.Add(string.Format("{0} level {1}: grants [{2}], ", text, level, string.Join(", ", hashSet.OrderBy((string n) => n))) + "the plan says [" + string.Join(", ", hashSet2.OrderBy((string n) => n)) + "].");
				}
			}
		}
		string[][] sovereignArchetypes = PrestigeLevelPlan.SovereignArchetypes;
		foreach (string[] array in sovereignArchetypes)
		{
			BlueprintArchetype modBlueprint = BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, array[0]);
			if (modBlueprint == null)
			{
				problems.Add(array[0] + ": the archetype does not exist.");
				continue;
			}
			HashSet<string> hashSet3 = Names(modBlueprint.AddFeatures.SelectMany((LevelEntry e) => e.Features));
			HashSet<string> hashSet4 = Names(modBlueprint.RemoveFeatures.SelectMany((LevelEntry e) => e.Features));
			classNames = new string[2]
			{
				array[2],
				array[3]
			};
			foreach (string text2 in classNames)
			{
				if (!hashSet3.Contains(text2))
				{
					problems.Add(array[0] + ": does not add '" + text2 + "'.");
				}
			}
			if (!hashSet4.Contains("SovereignCrownFeature"))
			{
				problems.Add(array[0] + ": does not replace 'SovereignCrownFeature' with its apex.");
			}
		}
		return num;
	}

	private static int CheckClaims(List<string> problems)
	{
		int num = 0;
		foreach (KeyValuePair<string, string[]> item in PrestigeLevelPlan.NewFeatureClaims())
		{
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, item.Key);
			if (modBlueprint == null)
			{
				problems.Add(item.Key + ": was not built.");
				continue;
			}
			HashSet<string> hashSet = new HashSet<string>(from c in modBlueprint.ComponentsArray
				where c != null
				select c.GetType().Name, StringComparer.Ordinal);
			string[] value = item.Value;
			foreach (string text in value)
			{
				num++;
				if (!hashSet.Contains(text))
				{
					problems.Add(item.Key + ": its description promises what a " + text + " does, and it carries none.");
				}
			}
			int num3 = PrestigeLevelPlan.MasteryRanks(item.Key);
			if (modBlueprint.Ranks != num3)
			{
				problems.Add($"{item.Key}: has {modBlueprint.Ranks} ranks, it is granted {num3} times.");
			}
		}
		return num;
	}

	private static void Build(BlueprintCharacterClass isekai, string className, string isekaiArchetypeName, string prestigeArchetypeName, List<string> problems, List<string> notes)
	{
		BlueprintCharacterClass blueprintCharacterClass = ClassNamed(className);
		string text = ((prestigeArchetypeName == null) ? className : (className + " + " + prestigeArchetypeName));
		if (blueprintCharacterClass == null)
		{
			problems.Add(text + ": the class does not exist.");
			return;
		}
		BlueprintArchetype blueprintArchetype = ((isekaiArchetypeName == null) ? null : BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, isekaiArchetypeName));
		BlueprintArchetype blueprintArchetype2 = ((prestigeArchetypeName == null) ? null : BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, prestigeArchetypeName));
		if (isekaiArchetypeName != null && (blueprintArchetype == null || blueprintArchetype2 == null))
		{
			problems.Add(text + ": '" + isekaiArchetypeName + "' or '" + prestigeArchetypeName + "' does not exist.");
			return;
		}
		string problem;
		using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
		if (selfTestUnit == null)
		{
			problems.Add(text + ": no blank unit could be created (" + problem + ").");
			return;
		}
		int num = 0;
		for (int i = 1; i <= 10; i++)
		{
			num++;
			if (!SelfTestProgressionChecks.TakeLevel(selfTestUnit.Unit, isekai, (i == 1) ? blueprintArchetype : null, num, text, problems, null, notes))
			{
				return;
			}
		}
		for (int j = 1; j <= 10; j++)
		{
			num++;
			if (!SelfTestProgressionChecks.TakeLevel(selfTestUnit.Unit, blueprintCharacterClass, (j == 1) ? blueprintArchetype2 : null, num, text, problems, null, notes))
			{
				return;
			}
		}
		Verify(selfTestUnit.Unit, blueprintCharacterClass, className, prestigeArchetypeName, text, problems);
	}

	private static void Verify(UnitEntityData unit, BlueprintCharacterClass prestige, string className, string archetypeName, string label, List<string> problems)
	{
		int num = 0;
		try
		{
			num = unit.Descriptor.Progression.GetClassLevel(prestige);
		}
		catch
		{
		}
		if (num != 10)
		{
			problems.Add($"{label}: has {num} levels in the prestige class, expected {10}.");
		}
		List<string> list = new List<string>();
		for (int i = 1; i <= 10; i++)
		{
			list.AddRange(PrestigeLevelPlan.IdentityAt(className, i));
		}
		if (archetypeName != null)
		{
			string[] array = PrestigeLevelPlan.SovereignArchetypes.First((string[] r) => r[0] == archetypeName);
			list.Remove("TranscendentDomainFeature");
			list.Remove("SovereignCrownFeature");
			list.Add(array[2]);
			list.Add(array[3]);
		}
		foreach (string item in list.Distinct())
		{
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, item);
			bool flag = false;
			try
			{
				flag = modBlueprint != null && unit.Descriptor.HasFact(modBlueprint);
			}
			catch
			{
			}
			if (!flag)
			{
				problems.Add(label + ": finished without '" + item + "', which the plan promises.");
			}
			if (flag)
			{
				int num2 = PrestigeLevelPlan.MasteryRanks(item);
				int num3 = 0;
				try
				{
					num3 = unit.Descriptor.Progression.Features.GetRank(modBlueprint);
				}
				catch
				{
				}
				if (num2 > 1 && num3 != num2)
				{
					problems.Add($"{label}: '{item}' is at rank {num3}, expected {num2}.");
				}
			}
		}
		int num4 = 0;
		try
		{
			num4 = unit.Descriptor.Spellbooks.Count();
		}
		catch
		{
		}
		int num5 = SelfTestProgressionChecks.HighestCasterLevel(unit);
		int num6 = 20;
		if (num4 != 1 || num5 != num6)
		{
			problems.Add($"{label}: ends with {num4} spellbook(s) at best caster level {num5} " + $"[{SelfTestProgressionChecks.DescribeSpellbooks(unit)}]; full advancement is one book at caster level {num6}.");
		}
	}
}
