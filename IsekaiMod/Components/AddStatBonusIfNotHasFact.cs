using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using UnityEngine;

namespace IsekaiMod.Components
{
	[TypeId("ef3cfeb920ad4483a7ab34d00f006bf4")]
	[ComponentName("Add stat bonus if owner does not have any Facts")]
	[AllowedOn(typeof(BlueprintBuff), false)]
	[AllowMultipleComponents]
	public class AddStatBonusIfNotHasFact : UnitBuffComponentDelegate, IUnitGainFactHandler, ISubscriber, IUnitSubscriber, IUnitLostFactHandler
	{
		public ModifierDescriptor Descriptor;

		public StatType Stat;

		public ContextValue Value;

		[SerializeField]
		public BlueprintUnitFactReference[] m_CheckedFacts;

		public ReferenceArrayProxy<BlueprintUnitFact, BlueprintUnitFactReference> CheckedFacts => m_CheckedFacts;

		public override void OnTurnOn()
		{
			Update();
		}

		public override void OnTurnOff()
		{
			Cancel();
		}

		public bool ShouldApplyBonus()
		{
			foreach (BlueprintUnitFact checkedFact in CheckedFacts)
			{
				if (base.Owner.HasFact(checkedFact))
				{
					return false;
				}
			}
			return true;
		}

		public void Update()
		{
			if (ShouldApplyBonus())
			{
				int value = Value.Calculate(base.Context);
				(base.Owner?.Stats?.GetStat(Stat))?.AddModifierUnique(value, base.Runtime, Descriptor);
			}
			else
			{
				Cancel();
			}
		}

		public void Cancel()
		{
			(base.Owner?.Stats?.GetStat(Stat))?.RemoveModifiersFrom(base.Runtime);
		}

		public void HandleUnitGainFact(EntityFact fact)
		{
			if (fact.Owner == base.Owner && fact.Blueprint is BlueprintUnitFact bp && CheckedFacts.HasReference(bp))
			{
				Update();
			}
		}

		public void HandleUnitLostFact(EntityFact fact)
		{
			if (fact.Owner == base.Owner && fact.Blueprint is BlueprintUnitFact bp && CheckedFacts.HasReference(bp))
			{
				Update();
			}
		}
	}
}
