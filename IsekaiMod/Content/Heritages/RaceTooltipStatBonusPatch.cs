using System;
using System.Collections.Generic;
using HarmonyLib;
using IsekaiMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.LevelClassScores.AbilityScores;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UnitLogic.Mechanics;

namespace IsekaiMod.Content.Heritages;

[HarmonyPatch(typeof(TooltipTemplateLevelUpRace), "Prepare")]
internal static class RaceTooltipStatBonusPatch
{
	[HarmonyPostfix]
	private static void Postfix(BlueprintRace ___m_Race, List<UIStatBonus> ___m_StatBonusList)
	{
		try
		{
			if (___m_Race == null || ___m_StatBonusList == null)
			{
				return;
			}
			foreach (UIStatBonus item in RacialBonuses(___m_Race.GetComponents<AddStatBonusIfNotHasFact>()))
			{
				___m_StatBonusList.Add(item);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[RaceTooltipStatBonusPatch] Could not list a race's ability scores: " + ex);
		}
	}

	internal static List<UIStatBonus> RacialBonuses(IEnumerable<AddStatBonusIfNotHasFact> components)
	{
		List<UIStatBonus> list = new List<UIStatBonus>();
		if (components == null)
		{
			return list;
		}
		foreach (AddStatBonusIfNotHasFact component in components)
		{
			if (component != null && component.Descriptor == ModifierDescriptor.Racial && component.Value != null && component.Value.ValueType == ContextValueType.Simple)
			{
				list.Add(new UIStatBonus
				{
					StatType = component.Stat,
					Descriptor = component.Descriptor,
					Value = component.Value.Value
				});
			}
		}
		return list;
	}
}
