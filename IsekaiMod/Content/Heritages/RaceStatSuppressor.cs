using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages;

internal static class RaceStatSuppressor
{
	internal const string DwarfRace = "c4faf439f0e70bd40b5e36ee80d06be7";

	internal const string GnomeRace = "ef35a22c9a27da345a4528f0d5889157";

	internal const string HalflingRace = "b0c3ef2729c498f47970bb50fa1acd30";

	internal static readonly Dictionary<string, string[]> Heritages = new Dictionary<string, string[]>
	{
		{
			"c4faf439f0e70bd40b5e36ee80d06be7",
			new string[2] { "IsekaiTitanDwarfHeritage", "IsekaiDragonLordHeritage" }
		},
		{
			"ef35a22c9a27da345a4528f0d5889157",
			new string[2] { "GnomeOfTheFirstWorldHeritage", "IsekaiSprigganHeritage" }
		},
		{
			"b0c3ef2729c498f47970bb50fa1acd30",
			new string[1] { "IsekaiFateweaverHeritage" }
		}
	};

	public static void Patch()
	{
		foreach (KeyValuePair<string, string[]> heritage in Heritages)
		{
			BlueprintRace blueprint = BlueprintTools.GetBlueprint<BlueprintRace>(heritage.Key);
			if (blueprint != null)
			{
				BlueprintUnitFactReference[] heritages = heritage.Value.Select((string name) => BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, name)).ToArray();
				Suppress(blueprint, heritages);
			}
		}
	}

	internal static void Suppress(BlueprintRace race, BlueprintUnitFactReference[] heritages)
	{
		List<BlueprintScriptableObject> list = new List<BlueprintScriptableObject>();
		list.Add(race);
		list.AddRange((from f in race.GetComponents<AddFeatureOnApply>().Select((Func<AddFeatureOnApply, BlueprintScriptableObject>)((AddFeatureOnApply c) => c.Feature)).Concat(((IEnumerable<BlueprintFeatureBase>)race.Features).Select((Func<BlueprintFeatureBase, BlueprintScriptableObject>)((BlueprintFeatureBase f) => f)))
			where f != null && f.name != null && f.name.EndsWith("AbilityModifiers")
			select f).Distinct());
		foreach (BlueprintScriptableObject item in list)
		{
			foreach (AddStatBonusIfHasFactTTT component in item.GetComponents<AddStatBonusIfHasFactTTT>())
			{
				BlueprintUnitFactReference[] blocked = component.m_BlockedFacts ?? new BlueprintUnitFactReference[0];
				BlueprintUnitFactReference[] array = heritages.Where((BlueprintUnitFactReference h) => !blocked.Any((BlueprintUnitFactReference b) => h.Equals(b))).ToArray();
				if (array.Length != 0)
				{
					component.m_BlockedFacts = blocked.Concat(array).ToArray();
				}
			}
			if (item.GetComponents<AddStatBonusIfNotHasFact>().Any((AddStatBonusIfNotHasFact c) => c.m_CheckedFacts != null && c.m_CheckedFacts.Any((BlueprintUnitFactReference f) => heritages.Any((BlueprintUnitFactReference h) => h.Equals(f)))))
			{
				continue;
			}
			List<AddStatBonus> list2 = item.GetComponents<AddStatBonus>().ToList();
			List<AddStatBonusIfHasFact> list3 = (from c in item.GetComponents<AddStatBonusIfHasFact>()
				where c.InvertCondition
				select c).ToList();
			if (list2.Count == 0 && list3.Count == 0)
			{
				continue;
			}
			HashSet<BlueprintComponent> removed = new HashSet<BlueprintComponent>(list2.Cast<BlueprintComponent>().Concat(list3));
			item.ComponentsArray = item.ComponentsArray.Where((BlueprintComponent c) => !removed.Contains(c)).ToArray();
			foreach (AddStatBonus bonus in list2)
			{
				item.AddComponent(delegate(AddStatBonusIfNotHasFact c)
				{
					c.Descriptor = bonus.Descriptor;
					c.Stat = bonus.Stat;
					c.Value = bonus.Value;
					c.m_CheckedFacts = heritages.ToArray();
				});
			}
			foreach (AddStatBonusIfHasFact bonus2 in list3)
			{
				item.AddComponent(delegate(AddStatBonusIfNotHasFact c)
				{
					c.Descriptor = bonus2.Descriptor;
					c.Stat = bonus2.Stat;
					c.Value = bonus2.Value;
					c.m_CheckedFacts = (bonus2.m_CheckedFacts ?? new BlueprintUnitFactReference[0]).Concat(heritages).ToArray();
				});
			}
		}
	}
}
