using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.DLC;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Components;

[TypeId("ea76f15a790e4c2a997564a15c9b0a3e")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[AllowMultipleComponents]
public class AddKnownSpellUnlessDlcRestricted : AddKnownSpell, ILevelUpCompleteUIHandler, ISubscriber, IGlobalSubscriber
{
	private static readonly HashSet<BlueprintScriptableObject> s_Hosts = new HashSet<BlueprintScriptableObject>();

	internal static IEnumerable<BlueprintScriptableObject> Hosts => s_Hosts;

	public override void OnActivate()
	{
		if (Grants(base.Spell))
		{
			base.OnActivate();
		}
	}

	void ILevelUpCompleteUIHandler.HandleLevelUpComplete(UnitEntityData unit, bool isChargen)
	{
		if (Grants(base.Spell))
		{
			HandleLevelUpComplete(unit, isChargen);
		}
	}

	internal static bool Grants(BlueprintAbility spell)
	{
		if (spell != null)
		{
			return !spell.IsDlcRestricted();
		}
		return false;
	}

	internal static bool NeedsGate(BlueprintAbility spell)
	{
		if (spell != null)
		{
			return spell.GetComponent<DlcCondition>() != null;
		}
		return false;
	}

	internal static void AddTo(BlueprintScriptableObject host, BlueprintCharacterClassReference characterClass, int spellLevel, BlueprintAbilityReference spell)
	{
		if (host == null || spell == null)
		{
			return;
		}
		s_Hosts.Add(host);
		if (NeedsGate(spell.Get()))
		{
			host.AddComponent(delegate(AddKnownSpellUnlessDlcRestricted c)
			{
				((AddKnownSpell)c).m_CharacterClass = characterClass;
				((AddKnownSpell)c).m_Spell = spell;
				c.SpellLevel = spellLevel;
			});
		}
		else
		{
			host.AddComponent(delegate(AddKnownSpell c)
			{
				c.m_CharacterClass = characterClass;
				c.m_Spell = spell;
				c.SpellLevel = spellLevel;
			});
		}
	}
}
