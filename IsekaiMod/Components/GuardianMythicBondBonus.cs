using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components;

[TypeId("08b5fdcca4274d65b8b7aa7c6873e2f4")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
public class GuardianMythicBondBonus : UnitFactComponentDelegate, ILevelUpCompleteUIHandler, ISubscriber, IGlobalSubscriber, IAreaLoadingStagesHandler
{
	public ModifierDescriptor Descriptor = ModifierDescriptor.Mythic;

	public int MaxBonus = 5;

	internal static readonly StatType[] BondStats = new StatType[5]
	{
		StatType.AC,
		StatType.AdditionalAttackBonus,
		StatType.SaveFortitude,
		StatType.SaveReflex,
		StatType.SaveWill
	};

	internal static int BonusFor(int masterMythicLevel, int max)
	{
		if (masterMythicLevel <= 0 || max <= 0)
		{
			return 0;
		}
		return Math.Min(max, masterMythicLevel / 2);
	}

	public override void OnTurnOn()
	{
		Apply();
	}

	public override void OnTurnOff()
	{
		Remove();
	}

	public void HandleLevelUpComplete(UnitEntityData unit, bool isChargen)
	{
		UnitEntityData owner = base.Owner;
		if (!(unit == null) && !(owner == null) && (unit == owner || unit == owner.Master))
		{
			Apply();
		}
	}

	public void OnAreaScenesLoaded()
	{
	}

	public void OnAreaLoadingComplete()
	{
		Apply();
	}

	private void Apply()
	{
		UnitEntityData owner = base.Owner;
		if (owner == null || base.Fact == null || !base.Fact.IsActive)
		{
			return;
		}
		Remove();
		int num = BonusFor((owner.Master?.Progression?.MythicLevel).GetValueOrDefault(), MaxBonus);
		if (num > 0)
		{
			StatType[] bondStats = BondStats;
			foreach (StatType type in bondStats)
			{
				owner.Stats.GetStat(type)?.AddModifierUnique(num, base.Runtime, Descriptor);
			}
		}
	}

	private void Remove()
	{
		UnitEntityData owner = base.Owner;
		if (owner?.Stats != null)
		{
			StatType[] bondStats = BondStats;
			foreach (StatType type in bondStats)
			{
				owner.Stats.GetStat(type)?.RemoveModifiersFrom(base.Runtime);
			}
		}
	}
}
