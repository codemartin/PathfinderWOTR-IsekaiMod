using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using UnityEngine;

namespace IsekaiMod.Components;

[TypeId("52d6f253abb84207b1244f6718af2247")]
[ComponentName("Add progression on apply (removed with the fact)")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[AllowMultipleComponents]
public class AddProgressionOnApply : UnitFactComponentDelegate
{
	[SerializeField]
	public BlueprintFeatureReference m_Feature;

	public BlueprintFeature Feature => m_Feature?.Get();

	public override void OnActivate()
	{
		try
		{
			UnitDescriptor unitDescriptor = base.Owner?.Descriptor;
			BlueprintFeature feature = Feature;
			if (unitDescriptor != null && feature != null && unitDescriptor.Progression.Features.AddFeature(feature) != null && feature is BlueprintProgression blueprintProgression)
			{
				LevelUpState levelUpState = Game.Instance?.LevelUpController?.State ?? new LevelUpState(unitDescriptor, LevelUpState.CharBuildMode.LevelUp, isPregen: false);
				if (blueprintProgression.ExclusiveProgression != null && unitDescriptor.Progression.GetClassData(blueprintProgression.ExclusiveProgression) != null)
				{
					levelUpState.SelectedClass = blueprintProgression.ExclusiveProgression;
				}
				LevelUpHelper.UpdateProgression(levelUpState, unitDescriptor, blueprintProgression);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error applying AddProgressionOnApply: " + ex);
		}
	}

	public override void OnDeactivate()
	{
		if (!base.IsReapplying)
		{
			Remove();
		}
	}

	public void Remove()
	{
		try
		{
			UnitDescriptor unitDescriptor = base.Owner?.Descriptor;
			BlueprintFeature feature = Feature;
			if (unitDescriptor == null || feature == null)
			{
				return;
			}
			foreach (BlueprintFeatureBase item in GrantedFeatures(feature))
			{
				if (item is BlueprintFeatureSelection selection)
				{
					RemoveSelection(unitDescriptor, selection);
				}
				BlueprintUnitFact blueprintUnitFact = item;
				if (blueprintUnitFact != null)
				{
					unitDescriptor.RemoveFact(blueprintUnitFact);
				}
			}
			unitDescriptor.RemoveFact(feature);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error removing AddProgressionOnApply: " + ex);
		}
	}

	private static IEnumerable<BlueprintFeatureBase> GrantedFeatures(BlueprintFeature feature)
	{
		if (!(feature is BlueprintProgression { LevelEntries: not null, LevelEntries: var levelEntries }))
		{
			yield break;
		}
		foreach (LevelEntry levelEntry in levelEntries)
		{
			if (levelEntry?.Features == null)
			{
				continue;
			}
			foreach (BlueprintFeatureBase feature2 in levelEntry.Features)
			{
				if (feature2 != null)
				{
					yield return feature2;
				}
			}
		}
	}

	private static void RemoveSelection(UnitDescriptor owner, BlueprintFeatureSelection selection)
	{
		if (!owner.Progression.Selections.TryGetValue(selection, out var value) || value == null)
		{
			return;
		}
		foreach (List<BlueprintFeature> item in value.SelectionsByLevel.Values.ToList())
		{
			foreach (BlueprintFeature item2 in item.ToList())
			{
				if (item2 != null)
				{
					owner.RemoveFact(item2);
				}
			}
		}
		owner.Progression.Selections.Remove(selection);
	}
}
