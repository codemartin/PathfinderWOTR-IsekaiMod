using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;

namespace IsekaiMod.Components;

[ComponentName("Predicates/Target HP below a level-scaling threshold")]
[AllowedOn(typeof(BlueprintAbility), false)]
[TypeId("d00562b11dcb4257a3c03de0341b978a")]
public class AbilityTargetHPBelowThresholdScaling : BlueprintComponent, IAbilityTargetRestriction
{
	public int BaseValue = 50;

	public int PerLevel = 10;

	public int PercentOfMaximum;

	internal static int Threshold(int baseValue, int perLevel, int casterLevel)
	{
		return baseValue + perLevel * Math.Max(0, casterLevel);
	}

	internal static bool Passes(int baseValue, int perLevel, int percentOfMaximum, int casterLevel, int currentHitPoints, int maximumHitPoints)
	{
		if (currentHitPoints < Threshold(baseValue, perLevel, casterLevel))
		{
			return true;
		}
		if (percentOfMaximum <= 0)
		{
			return false;
		}
		return currentHitPoints * 100 / Math.Max(1, maximumHitPoints) <= percentOfMaximum;
	}

	public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper target)
	{
		UnitEntityData unitEntityData = target?.Unit;
		if (unitEntityData == null || caster == null)
		{
			return false;
		}
		if (unitEntityData.State.IsFinallyDead)
		{
			return false;
		}
		int valueOrDefault = (caster.Descriptor?.Progression?.CharacterLevel).GetValueOrDefault();
		return Passes(BaseValue, PerLevel, PercentOfMaximum, valueOrDefault, unitEntityData.HPLeft, unitEntityData.MaxHP);
	}

	public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
	{
		return BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetHPCondition;
	}
}
