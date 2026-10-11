using System;
using System.Collections.Generic;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(LevelUpHelper), "UpdateProgression")]
internal static class LegacyProgressionBackfill
{
	private static readonly HashSet<BlueprintGuid> s_WalkedProgressions = new HashSet<BlueprintGuid>();

	internal static void Register(BlueprintProgression progression)
	{
		if (progression != null)
		{
			s_WalkedProgressions.Add(progression.AssetGuid);
		}
	}

	internal static bool IsRegistered(BlueprintProgression progression)
	{
		if (progression != null)
		{
			return s_WalkedProgressions.Contains(progression.AssetGuid);
		}
		return false;
	}

	internal static bool ShouldBackfill(bool blueprintGivesPreviousLevels, bool walkedByLegacy, int isekaiClassLevel, int previousLevel, int newLevel)
	{
		if (blueprintGivesPreviousLevels)
		{
			return false;
		}
		if (!walkedByLegacy)
		{
			return false;
		}
		if (isekaiClassLevel <= 0)
		{
			return false;
		}
		return newLevel - previousLevel > 1;
	}

	[HarmonyPrefix]
	[HarmonyBefore(new string[] { "0ToyBox0" })]
	private static bool Prefix(LevelUpState state, UnitDescriptor unit, BlueprintProgression progression)
	{
		ProgressionData progressionData;
		int level;
		int num2;
		try
		{
			if (state == null || unit?.Progression == null || progression == null)
			{
				return true;
			}
			if (progression.GiveFeaturesForPreviousLevels || !IsRegistered(progression))
			{
				return true;
			}
			BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
			int num = ((blueprintCharacterClass != null) ? unit.Progression.GetClassLevel(blueprintCharacterClass) : 0);
			if (num <= 0)
			{
				return true;
			}
			progressionData = unit.Progression.SureProgressionData(progression);
			level = progressionData.Level;
			num2 = progression.CalcLevel(unit);
			if (!ShouldBackfill(blueprintGivesPreviousLevels: false, walkedByLegacy: true, num, level, num2))
			{
				return true;
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[LegacyProgressionBackfill] decision failed for {progression?.AssetGuid} {progression?.name}; using the game's own rule. {arg}");
			return true;
		}
		progressionData.Level = num2;
		if (progression.ExclusiveProgression == null || state.SelectedClass == progression.ExclusiveProgression)
		{
			for (int i = level + 1; i <= num2; i++)
			{
				LevelEntry levelEntry = progressionData.GetLevelEntry(i);
				LevelUpHelper.AddFeaturesFromProgression(state, unit, levelEntry.Features, progression, i);
			}
		}
		return false;
	}
}
