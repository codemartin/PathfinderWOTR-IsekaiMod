using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.DLC;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestDlcChecks
{
	internal static SelfTestChecks.Outcome KnownSpells()
	{
		List<BlueprintScriptableObject> list = (from c in SelfTestProgressionChecks.ModBlueprints()
			select c.Blueprint).OfType<BlueprintScriptableObject>().ToList();
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>(list.Select((BlueprintScriptableObject b) => b.AssetGuid));
		HashSet<BlueprintScriptableObject> hashSet2 = new HashSet<BlueprintScriptableObject>(list);
		foreach (BlueprintScriptableObject host in AddKnownSpellUnlessDlcRestricted.Hosts)
		{
			hashSet2.Add(host);
		}
		List<string> list2 = new List<string>();
		int num = 0;
		int num2 = 0;
		foreach (BlueprintScriptableObject item2 in hashSet2.OrderBy((BlueprintScriptableObject h) => h.name, StringComparer.Ordinal))
		{
			BlueprintComponent[] componentsArray = item2.ComponentsArray;
			if (componentsArray == null)
			{
				continue;
			}
			foreach (AddKnownSpell item3 in componentsArray.OfType<AddKnownSpell>())
			{
				BlueprintGuid item = item3.m_CharacterClass?.Guid ?? BlueprintGuid.Empty;
				if (!hashSet.Contains(item))
				{
					continue;
				}
				BlueprintAbility spell = item3.Spell;
				if (spell == null)
				{
					continue;
				}
				if (item3 is AddKnownSpellUnlessDlcRestricted)
				{
					num2++;
					continue;
				}
				num++;
				if (spell.GetComponent<DlcCondition>() != null)
				{
					list2.Add($"{item2.name} ({item2.AssetGuid}): a plain AddKnownSpell grants {spell.name} ({spell.AssetGuid}), which carries a DlcCondition, to {item3.m_CharacterClass.Get()?.name}");
				}
			}
		}
		return SelfTestChecks.Outcome.Of(list2.Count == 0, $"{hashSet2.Count} blueprints read; {num} plain known-spell grants to mod classes, {num2} DLC-gated; {list2.Count} DLC spells granted without a gate.", list2);
	}
}
