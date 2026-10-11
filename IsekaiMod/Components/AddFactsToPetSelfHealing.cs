using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using UnityEngine;

namespace IsekaiMod.Components;

[TypeId("2f4b81c0d5e74e3a9c6b0af7128d53e9")]
[ComponentName("Add facts to pet (rescanning)")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[AllowMultipleComponents]
public class AddFactsToPetSelfHealing : UnitFactComponentDelegate, ISubscriber, IGlobalSubscriber, IAreaHandler
{
	[SerializeField]
	public BlueprintUnitFactReference[] m_Facts = new BlueprintUnitFactReference[0];

	[SerializeField]
	public PetType m_PetType;

	[SerializeField]
	public BlueprintUnitReference m_PetBlueprint;

	public override void OnActivate()
	{
		Apply();
	}

	public override void OnRecalculate()
	{
		Apply();
	}

	public override void OnApplyPostLoadFixes()
	{
		if (base.Fact != null && base.Fact.Active)
		{
			Apply();
		}
	}

	public override void OnDeactivate()
	{
		if (!base.IsReapplying)
		{
			Remove();
		}
	}

	public void OnAreaBeginUnloading()
	{
	}

	public void OnAreaDidLoad()
	{
		if (base.Fact != null && base.Fact.Active)
		{
			Apply();
		}
	}

	private bool Matches(UnitEntityData pet)
	{
		if (pet == null)
		{
			return false;
		}
		if (pet.Get<UnitPartPet>()?.Type != m_PetType)
		{
			return false;
		}
		BlueprintUnit blueprintUnit = m_PetBlueprint?.Get();
		if (blueprintUnit == null)
		{
			return true;
		}
		if (pet.Blueprint != blueprintUnit)
		{
			return pet.OriginalBlueprint == blueprintUnit;
		}
		return true;
	}

	public void Apply()
	{
		try
		{
			UnitEntityData owner = base.Owner;
			if (owner == null)
			{
				return;
			}
			foreach (EntityPartRef<UnitEntityData, UnitPartPet> pet in owner.Pets)
			{
				UnitEntityData entity = pet.Entity;
				if (!Matches(entity))
				{
					continue;
				}
				BlueprintUnitFactReference[] facts = m_Facts;
				for (int i = 0; i < facts.Length; i++)
				{
					BlueprintUnitFact blueprintUnitFact = facts[i]?.Get();
					if (blueprintUnitFact != null && !entity.Descriptor.HasFact(blueprintUnitFact))
					{
						entity.Descriptor.AddFact(blueprintUnitFact);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error applying AddFactsToPetSelfHealing: " + ex);
		}
	}

	public void Remove()
	{
		try
		{
			UnitEntityData owner = base.Owner;
			if (owner == null)
			{
				return;
			}
			foreach (EntityPartRef<UnitEntityData, UnitPartPet> pet in owner.Pets)
			{
				UnitEntityData entity = pet.Entity;
				if (!Matches(entity))
				{
					continue;
				}
				BlueprintUnitFactReference[] facts = m_Facts;
				for (int i = 0; i < facts.Length; i++)
				{
					BlueprintUnitFact blueprintUnitFact = facts[i]?.Get();
					if (blueprintUnitFact != null)
					{
						entity.Descriptor.RemoveFact(blueprintUnitFact);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error removing AddFactsToPetSelfHealing: " + ex);
		}
	}
}
