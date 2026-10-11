using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules;

namespace IsekaiMod.Utilities;

[HarmonyPatch]
internal static class PartyCheckTake20Patch
{
	internal const int Take20Edge = 9;

	[HarmonyPrepare]
	private static bool Prepare()
	{
		return PatchGuard.ResolvesAll(typeof(PartyCheckTake20Patch), (typeof(RulePartyStatCheck), "GetApplicableUnits"));
	}

	internal static List<int> Roller(IList<int> bonuses, IList<bool> holdsTake20)
	{
		List<int> list = Enumerable.Range(0, bonuses.Count).ToList();
		List<int> list2 = list.Where((int i) => holdsTake20[i]).ToList();
		if (list2.Count == 0)
		{
			return list;
		}
		int num = list2.Max((int i) => bonuses[i]);
		int num2 = bonuses.Max();
		if (num + 9 < num2)
		{
			return list;
		}
		return list2;
	}

	[HarmonyPatch(typeof(RulePartyStatCheck), "GetApplicableUnits")]
	[HarmonyPostfix]
	private static void GetApplicableUnits_Postfix(RulePartyStatCheck __instance, ref IEnumerable<UnitEntityData> __result)
	{
		try
		{
			if (__instance == null || __result == null || __instance.StatType != StatType.SkillPersuasion)
			{
				return;
			}
			BlueprintFeature feature = MastermindOverhaulFeatures.PersuasionTake20;
			if (feature == null)
			{
				return;
			}
			List<UnitEntityData> units = __result.Where((UnitEntityData u) => u != null && u.Descriptor.State.CanAct).ToList();
			if (units.Count < 2)
			{
				return;
			}
			List<int> bonuses = units.Select(delegate(UnitEntityData u)
			{
				ModifiableValue stat = u.Stats.GetStat(__instance.StatType);
				return (!(stat is ModifiableValueAttributeStat modifiableValueAttributeStat)) ? stat.ModifiedValue : modifiableValueAttributeStat.Bonus;
			}).ToList();
			List<bool> holdsTake = units.Select((UnitEntityData u) => u.Descriptor.Progression.Features.HasFact(feature)).ToList();
			List<int> list = Roller(bonuses, holdsTake);
			if (list.Count != units.Count)
			{
				__result = list.Select((int i) => units[i]).ToList();
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[PartyCheckTake20Patch] the party roller stands. {arg}");
		}
	}
}
