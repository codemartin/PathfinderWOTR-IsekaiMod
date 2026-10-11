using System;
using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;

internal class MastermindSpellList
{
	private const int SpellLevelCount = 11;

	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindSpellList", delegate(BlueprintSpellList bp)
		{
			bp.SpellsByLevel = new SpellLevelList[11];
			for (int i = 0; i < 11; i++)
			{
				bp.SpellsByLevel[i] = new SpellLevelList(i)
				{
					m_Spells = new List<BlueprintAbilityReference>()
				};
			}
		});
	}

	public static BlueprintSpellList Get()
	{
		return BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.IsekaiContext, "MastermindSpellList");
	}

	public static BlueprintSpellListReference GetReference()
	{
		return BlueprintTools.GetModBlueprintReference<BlueprintSpellListReference>(Main.IsekaiContext, "MastermindSpellList");
	}

	public static void PatchMastermindSpellList()
	{
		BlueprintSpellList blueprintSpellList = Get();
		SpellLevelList[] spellsByLevel = IsekaiProtagonistSpellList.Get().SpellsByLevel;
		blueprintSpellList.SpellsByLevel = Deepen((SpellLevelList[])spellsByLevel.Clone());
		blueprintSpellList.SpellsByLevel[10] = new SpellLevelList(10)
		{
			m_Spells = new List<BlueprintAbilityReference>
			{
				BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("483157d358afd1a498c2a4762f4057ba"),
				BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("a948e10ecf1fa674dbae5eaae7f25a7f"),
				BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("af92783492851f445abb2c01d346c376"),
				BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("1b76573f991543145897702b7edc4d7a"),
				BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("7d721be6d74f07f4d952ee8d6f8f44a0"),
				BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("24067ba8e0e69a14e83ff397826f6c6d"),
				BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("cc74245ba989480488925214dd925100")
			}
		};
	}

	private static SpellLevelList[] Deepen(SpellLevelList[] levels)
	{
		if (levels == null)
		{
			levels = new SpellLevelList[11];
		}
		if (levels.Length < 11)
		{
			Array.Resize(ref levels, 11);
		}
		for (int i = 0; i < levels.Length; i++)
		{
			SpellLevelList spellLevelList = levels[i];
			levels[i] = new SpellLevelList(i)
			{
				m_Spells = ((spellLevelList?.m_Spells == null) ? new List<BlueprintAbilityReference>() : new List<BlueprintAbilityReference>(spellLevelList.m_Spells))
			};
		}
		return levels;
	}
}
