using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages;

internal static class AlternateTraitExclusion
{
	internal static readonly (string Race, string Selection, string[] Heritages)[] Selections = new(string, string, string[])[4]
	{
		("c4faf439f0e70bd40b5e36ee80d06be7", "fd6e1f53589049cbbbc6a8e058d83b74", new string[2] { "IsekaiTitanDwarfHeritage", "IsekaiDragonLordHeritage" }),
		("25a5878d125338244896ebd3238226c8", "5482f879dcfd40f9a3168fdb48bc938c", new string[4] { "IsekaiDarkElfHeritage", "IsekaiHighElfHeritage", "IsekaiWoodElfHeritage", "IsekaiDragonLordHeritage" }),
		("ef35a22c9a27da345a4528f0d5889157", "584d8b50817b49b2bb7aab3d6add8d3a", new string[2] { "GnomeOfTheFirstWorldHeritage", "IsekaiSprigganHeritage" }),
		("b0c3ef2729c498f47970bb50fa1acd30", "b3bebe76e6c64e2ca11585f9e3e2554a", new string[1] { "IsekaiFateweaverHeritage" })
	};

	public static void Patch()
	{
		(string, string, string[])[] selections = Selections;
		for (int i = 0; i < selections.Length; i++)
		{
			(string, string, string[]) tuple = selections[i];
			BlueprintRace blueprint = BlueprintTools.GetBlueprint<BlueprintRace>(tuple.Item1);
			BlueprintFeatureSelection blueprint2 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(tuple.Item2);
			if (blueprint == null || blueprint2 == null)
			{
				continue;
			}
			BlueprintFeature blueprintFeature = FindAbilityModifiers(blueprint);
			if (blueprintFeature == null)
			{
				continue;
			}
			List<BlueprintFeature> list = ReplacingTraits(blueprint2, blueprintFeature);
			if (list.Count == 0)
			{
				continue;
			}
			string[] item = tuple.Item3;
			foreach (string name in item)
			{
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, name);
				if (modBlueprint != null)
				{
					Exclude(modBlueprint, list);
				}
			}
		}
	}

	internal static BlueprintFeature FindAbilityModifiers(BlueprintRace race)
	{
		return race.GetComponents<AddFeatureOnApply>().Select((Func<AddFeatureOnApply, BlueprintScriptableObject>)((AddFeatureOnApply c) => c.Feature)).Concat(((IEnumerable<BlueprintFeatureBase>)race.Features).Select((Func<BlueprintFeatureBase, BlueprintScriptableObject>)((BlueprintFeatureBase f) => f)))
			.OfType<BlueprintFeature>()
			.FirstOrDefault((BlueprintFeature f) => f.name != null && f.name.EndsWith("AbilityModifiers"));
	}

	internal static List<BlueprintFeature> ReplacingTraits(BlueprintFeatureSelection selection, BlueprintFeature abilityModifiers)
	{
		return (from f in (from r in (selection.m_AllFeatures ?? new BlueprintFeatureReference[0]).Concat(selection.m_Features ?? new BlueprintFeatureReference[0])
				select r?.Get() into f
				where f != null
				select f).Distinct()
			where f.GetComponents<RemoveFeatureOnApply>().Any((RemoveFeatureOnApply c) => c.m_Feature != null && ((BlueprintReferenceBase)c.m_Feature).deserializedGuid == abilityModifiers.AssetGuid)
			select f).ToList();
	}

	internal static void Exclude(BlueprintFeature heritage, IEnumerable<BlueprintFeature> traits)
	{
		foreach (BlueprintFeature trait in traits)
		{
			if (!heritage.GetComponents<PrerequisiteNoFeature>().Any((PrerequisiteNoFeature p) => p.m_Feature != null && ((BlueprintReferenceBase)p.m_Feature).deserializedGuid == trait.AssetGuid))
			{
				heritage.AddComponent(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = trait.ToReference<BlueprintFeatureReference>();
				});
			}
		}
	}
}
