using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

internal static class BonusFeatListRefresh
{
	private static readonly Dictionary<string, BlueprintFeatureReference[]> s_Snapshots = new Dictionary<string, BlueprintFeatureReference[]>();

	private static BlueprintFeatureSelection BasicFeats => FeatTools.Selections.BasicFeatSelection;

	internal static void Snapshot(string selectionName)
	{
		BlueprintFeatureReference[] array = BasicFeats?.m_AllFeatures;
		if (array != null)
		{
			s_Snapshots[selectionName] = (BlueprintFeatureReference[])array.Clone();
		}
	}

	internal static T[] Merge<T>(IEnumerable<T> own, IEnumerable<T> snapshot, IEnumerable<T> basicNow, Func<T, T, bool> same) where T : class
	{
		List<T> list = new List<T>();
		List<T> original = (snapshot ?? Enumerable.Empty<T>()).Where((T o) => o != null).ToList();
		List<T> list2 = new List<T>();
		List<T> list3 = new List<T>();
		bool flag = false;
		foreach (T option in own ?? Enumerable.Empty<T>())
		{
			if (option != null)
			{
				if (original.Any((T o) => same(o, option)))
				{
					flag = true;
				}
				else
				{
					(flag ? list3 : list2).Add(option);
				}
			}
		}
		List<T> now = (basicNow ?? Enumerable.Empty<T>()).Where((T o) => o != null).ToList();
		List<T> second = original.Where((T o) => now.Any((T n) => same(n, o))).ToList();
		List<T> second2 = now.Where((T n) => !original.Any((T o) => same(o, n))).ToList();
		foreach (T option2 in list2.Concat(second).Concat(second2).Concat(list3))
		{
			if (option2 != null && !list.Any((T existing) => same(existing, option2)))
			{
				list.Add(option2);
			}
		}
		return list.ToArray();
	}

	internal static void Refresh(BlueprintFeatureSelection selection)
	{
		if (selection != null && BasicFeats?.m_AllFeatures != null && s_Snapshots.TryGetValue(selection.name, out var value))
		{
			selection.m_Features = (BlueprintFeatureReference[])(selection.m_AllFeatures = Merge(selection.m_AllFeatures, value, BasicFeats.m_AllFeatures, (BlueprintFeatureReference a, BlueprintFeatureReference b) => a != null && b != null && a.Equals(b))).Clone();
			s_Snapshots[selection.name] = (BlueprintFeatureReference[])BasicFeats.m_AllFeatures.Clone();
		}
	}

	internal static void RefreshAll()
	{
		Refresh(BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection"));
		Refresh(BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SoloSovereignFeatSelection"));
	}
}
