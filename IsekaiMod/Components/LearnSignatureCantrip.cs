using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using UnityEngine;

namespace IsekaiMod.Components;

[TypeId("5dabf78f311a46c6b3f7e2de92254207")]
[ComponentName("Teach a cantrip to the class spellbook")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[AllowMultipleComponents]
public class LearnSignatureCantrip : UnitFactComponentDelegate, IUnitReapplyFeaturesOnLevelUpHandler, ISubscriber, IUnitSubscriber
{
	public const int CantripLevel = 0;

	[SerializeField]
	public BlueprintCharacterClassReference m_CharacterClass;

	[SerializeField]
	public BlueprintAbilityReference m_Cantrip;

	public BlueprintCharacterClass CharacterClass => m_CharacterClass?.Get();

	public BlueprintAbility Cantrip => m_Cantrip?.Get();

	internal static bool ShouldTeach(bool hasBook, int maxSpellLevel, bool alreadyKnown)
	{
		if (!hasBook)
		{
			return false;
		}
		if (maxSpellLevel < 0)
		{
			return false;
		}
		return !alreadyKnown;
	}

	public override void OnActivate()
	{
		Teach();
	}

	public void HandleUnitReapplyFeaturesOnLevelUp()
	{
		Teach();
	}

	private void Teach()
	{
		UnitEntityData owner = base.Owner;
		BlueprintCharacterClass characterClass = CharacterClass;
		BlueprintAbility cantrip = Cantrip;
		if (!(owner == null) && characterClass != null && cantrip != null)
		{
			Spellbook spellbook = owner.GetSpellbook(characterClass);
			if (ShouldTeach(spellbook != null, spellbook?.MaxSpellLevel ?? (-1), spellbook?.IsKnownOnLevel(cantrip, 0) ?? false))
			{
				spellbook.AddKnown(0, cantrip);
			}
		}
	}
}
