using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace IsekaiMod.Components;

[TypeId("288c517508fe40218eadd53639189e7b")]
[ComponentName("Add feature once the owner ascends")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
public class AddFeatureOnLegendAscension : UnitFactComponentDelegate, IOwnerGainLevelHandler, ISubscriber, IUnitSubscriber, IUnitGainFactHandler
{
	[SerializeField]
	public BlueprintFeatureReference m_Feature;

	[SerializeField]
	public BlueprintUnitFactReference m_RequiredFact;

	[SerializeField]
	public BlueprintCharacterClassReference m_RequiredClass;

	public int RequiredCharacterLevel = 20;

	internal static bool ShouldGrant(bool alreadyHasFeature, bool hasRequiredFact, int requiredClassLevel, int characterLevel, int requiredCharacterLevel)
	{
		if (alreadyHasFeature)
		{
			return false;
		}
		if (!hasRequiredFact)
		{
			return false;
		}
		if (requiredClassLevel < 1)
		{
			return false;
		}
		return characterLevel >= requiredCharacterLevel;
	}

	public override void OnActivate()
	{
		Apply();
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

	private void Apply()
	{
		BlueprintFeature blueprintFeature = m_Feature?.Get();
		BlueprintUnitFact blueprintUnitFact = m_RequiredFact?.Get();
		BlueprintCharacterClass blueprintCharacterClass = m_RequiredClass?.Get();
		UnitEntityData owner = base.Owner;
		if (blueprintFeature != null && blueprintUnitFact != null && blueprintCharacterClass != null && !(owner == null))
		{
			UnitProgressionData progression = owner.Progression;
			if (progression != null && ShouldGrant(owner.HasFact(blueprintFeature), owner.HasFact(blueprintUnitFact), progression.GetClassLevel(blueprintCharacterClass), progression.CharacterLevel, RequiredCharacterLevel))
			{
				owner.AddFact(blueprintFeature);
			}
		}
	}
}
