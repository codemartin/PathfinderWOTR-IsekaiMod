using System;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using UnityEngine;

namespace IsekaiMod.Components;

[TypeId("6b1c7e0d5f0c4a3e9d2a8f41c3b7e952")]
[ComponentName("Update progression on apply")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[AllowMultipleComponents]
public class UpdateProgressionOnApply : UnitFactComponentDelegate
{
	[SerializeField]
	public BlueprintFeatureReference m_Feature;

	public BlueprintFeature Feature => m_Feature?.Get();

	public override void OnActivate()
	{
		try
		{
			UnitDescriptor unitDescriptor = base.Owner?.Descriptor;
			if (Feature is BlueprintProgression blueprintProgression && unitDescriptor != null && unitDescriptor.GetFeature(blueprintProgression) != null)
			{
				LevelUpHelper.UpdateProgression(Game.Instance?.LevelUpController?.State ?? new LevelUpState(unitDescriptor, LevelUpState.CharBuildMode.LevelUp, isPregen: false), unitDescriptor, blueprintProgression);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error applying UpdateProgressionOnApply: " + ex);
		}
	}
}
