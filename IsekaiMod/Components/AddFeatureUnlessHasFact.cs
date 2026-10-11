using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace IsekaiMod.Components;

[TypeId("9a4e2c7b18d34f0fa6b5d3e1c8724b09")]
[ComponentName("Add feature unless owner has fact")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[AllowMultipleComponents]
public class AddFeatureUnlessHasFact : UnitFactComponentDelegate, IOwnerGainLevelHandler, ISubscriber, IUnitSubscriber, IUnitGainFactHandler, IUnitLostFactHandler
{
	internal enum Change
	{
		None,
		Add,
		Remove
	}

	[SerializeField]
	public BlueprintUnitFactReference m_CheckedFact;

	[SerializeField]
	public BlueprintUnitFactReference m_Feature;

	internal static Change Decide(bool hasCheckedFact, bool hasFeature)
	{
		if (!hasCheckedFact && !hasFeature)
		{
			return Change.Add;
		}
		if (hasCheckedFact & hasFeature)
		{
			return Change.Remove;
		}
		return Change.None;
	}

	public override void OnActivate()
	{
		Apply();
	}

	public override void OnDeactivate()
	{
		BlueprintUnitFact blueprintUnitFact = m_Feature?.Get();
		UnitEntityData owner = base.Owner;
		if (blueprintUnitFact != null && !(owner == null))
		{
			EntityFact fact = owner.GetFact(blueprintUnitFact);
			if (fact != null)
			{
				owner.RemoveFact(fact);
			}
		}
	}

	public void HandleUnitGainLevel()
	{
		Apply();
	}

	public void HandleUnitGainFact(EntityFact fact)
	{
		if (fact != null && fact.Owner == base.Owner)
		{
			Apply();
		}
	}

	public void HandleUnitLostFact(EntityFact fact)
	{
		if (fact != null && fact.Owner == base.Owner)
		{
			Apply();
		}
	}

	private void Apply()
	{
		BlueprintUnitFact blueprintUnitFact = m_CheckedFact?.Get();
		BlueprintUnitFact blueprintUnitFact2 = m_Feature?.Get();
		UnitEntityData owner = base.Owner;
		if (blueprintUnitFact == null || blueprintUnitFact2 == null || owner == null)
		{
			return;
		}
		EntityFact entityFact = null;
		try
		{
			entityFact = base.Fact;
		}
		catch (NullReferenceException)
		{
		}
		if (entityFact != null && entityFact.Deactivating)
		{
			return;
		}
		switch (Decide(owner.HasFact(blueprintUnitFact), owner.HasFact(blueprintUnitFact2)))
		{
		case Change.Add:
			owner.AddFact(blueprintUnitFact2);
			break;
		case Change.Remove:
		{
			EntityFact fact = owner.GetFact(blueprintUnitFact2);
			if (fact != null)
			{
				owner.RemoveFact(fact);
			}
			break;
		}
		}
	}
}
