using System;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;

namespace IsekaiMod.Components;

[ClassInfoBox("If Owner summons, it summons additional units")]
[TypeId("d104b3fcf9094fae838ad3793767348c")]
public class ExtraSummonCount : UnitFactComponentDelegate, ISubscriber, IGlobalSubscriber, ICalculateSummonUnitsCount
{
	public int Count = 2;

	public void HandleCalculateSummonUnitsCount(MechanicsContext mechanicsContext, ContextDiceValue contextDiceValue, ref int count)
	{
		if (!(mechanicsContext.MaybeCaster != base.Owner))
		{
			BlueprintAbility blueprintAbility = mechanicsContext?.SourceAbility;
			if (blueprintAbility != null && AppliesToAbilityType((int)blueprintAbility.Type))
			{
				count += CalculateExtra(Count, (base.Owner?.Progression?.CharacterLevel).GetValueOrDefault(), GetMaxStat());
			}
		}
	}

	internal static bool AppliesToAbilityType(int sourceAbilityType)
	{
		return sourceAbilityType == 0;
	}

	internal static int CalculateExtra(int baseCount, int characterLevel, int maxStatBonus)
	{
		int val = baseCount + ((characterLevel >= 15) ? 1 : 0);
		return Math.Max(1, Math.Min(val, maxStatBonus));
	}

	public int GetMaxStat()
	{
		int valueOrDefault = (base.Owner?.Stats?.GetAttribute(StatType.Intelligence)?.Bonus).GetValueOrDefault();
		int valueOrDefault2 = (base.Owner?.Stats?.GetAttribute(StatType.Wisdom)?.Bonus).GetValueOrDefault();
		int valueOrDefault3 = (base.Owner?.Stats?.GetAttribute(StatType.Charisma)?.Bonus).GetValueOrDefault();
		return Math.Max(valueOrDefault, Math.Max(valueOrDefault2, valueOrDefault3));
	}
}
