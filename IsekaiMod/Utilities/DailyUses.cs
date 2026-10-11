using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

internal static class DailyUses
{
	public static BlueprintAbilityResource.Amount Amount(int baseValue, StatType bonusStat = StatType.Unknown)
	{
		return new BlueprintAbilityResource.Amount
		{
			BaseValue = baseValue,
			IncreasedByLevel = false,
			IncreasedByStat = (bonusStat != StatType.Unknown),
			ResourceBonusStat = bonusStat
		};
	}

	public static void Limit(BlueprintAbility ability, BlueprintAbilityResource resource)
	{
		ability.AddComponent(delegate(AbilityResourceLogic c)
		{
			c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
			c.m_IsSpendResource = true;
			c.Amount = 1;
		});
	}

	public static void Grant(BlueprintFeature feature, BlueprintAbilityResource resource)
	{
		feature.AddComponent(delegate(AddAbilityResources c)
		{
			c.m_Resource = resource.ToReference<BlueprintAbilityResourceReference>();
			c.RestoreAmount = true;
		});
	}
}
