using System;
using System.Collections.Generic;
using HarmonyLib;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Constellations;

internal static class CosmicWishRespec
{
	internal sealed class Snapshot
	{
		internal int Charges;

		internal bool HadResource;

		internal bool HadAbility;

		internal readonly Dictionary<string, int> BoostRanks = new Dictionary<string, int>(StringComparer.Ordinal);
	}

	internal enum BoostRepair
	{
		None,
		ReAdd,
		SetRank
	}

	[HarmonyPatch(typeof(UnitHelper), "RespecOnCommit")]
	internal static class UnitHelper_RespecOnCommit_Patch
	{
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return PatchGuard.Resolves(typeof(UnitHelper_RespecOnCommit_Patch), typeof(UnitHelper), "RespecOnCommit");
		}

		[HarmonyPrefix]
		internal static void Prefix(UnitEntityData targetUnit)
		{
			try
			{
				if ((object)targetUnit != null && targetUnit.UniqueId != null)
				{
					Taken[targetUnit.UniqueId] = Take(targetUnit);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error recording Cosmic Wish charges before a respec: " + ex);
			}
		}

		[HarmonyPostfix]
		internal static void Postfix(UnitEntityData targetUnit)
		{
			try
			{
				if ((object)targetUnit != null && targetUnit.UniqueId != null && Taken.TryGetValue(targetUnit.UniqueId, out var value))
				{
					Taken.Remove(targetUnit.UniqueId);
					Apply(targetUnit, value);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error restoring Cosmic Wish charges after a respec: " + ex);
			}
		}
	}

	internal static readonly string[] BoostFeatureNames = new string[6] { "WishStrengthAbilityFeature", "WishDexterityAbilityFeature", "WishConstitutionAbilityFeature", "WishIntelligenceAbilityFeature", "WishWisdomAbilityFeature", "WishCharismaAbilityFeature" };

	private static readonly Dictionary<string, Snapshot> Taken = new Dictionary<string, Snapshot>(StringComparer.Ordinal);

	internal static int ChargesToRestore(int before, int after)
	{
		if (before <= 0 || after >= before)
		{
			return 0;
		}
		return before - Math.Max(0, after);
	}

	internal static int RankToRestore(int before, int after)
	{
		if (before <= 1 || after >= before)
		{
			return 0;
		}
		return before;
	}

	internal static BoostRepair DecideBoostRepair(int before, int after)
	{
		if (before <= 0)
		{
			return BoostRepair.None;
		}
		if (after <= 0)
		{
			return BoostRepair.ReAdd;
		}
		if (RankToRestore(before, after) <= 0)
		{
			return BoostRepair.None;
		}
		return BoostRepair.SetRank;
	}

	private static BlueprintAbilityResource WishResource()
	{
		return BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(Main.IsekaiContext, "CosmicWishResource");
	}

	private static BlueprintFeature BoostFeature(string name)
	{
		return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, name);
	}

	private static BlueprintAbility WishAbility()
	{
		return BlueprintTools.GetModBlueprint<BlueprintAbility>(Main.IsekaiContext, "ModularCosmicWishAbility");
	}

	internal static Snapshot Take(UnitEntityData unit)
	{
		Snapshot snapshot = new Snapshot();
		if (unit == null)
		{
			return snapshot;
		}
		BlueprintAbilityResource blueprintAbilityResource = WishResource();
		if (blueprintAbilityResource != null && unit.Descriptor.Resources.ContainsResource(blueprintAbilityResource))
		{
			snapshot.HadResource = true;
			snapshot.Charges = unit.Descriptor.Resources.GetResourceAmount(blueprintAbilityResource);
		}
		BlueprintAbility blueprintAbility = WishAbility();
		snapshot.HadAbility = blueprintAbility != null && unit.Descriptor.HasFact(blueprintAbility);
		string[] boostFeatureNames = BoostFeatureNames;
		foreach (string text in boostFeatureNames)
		{
			BlueprintFeature blueprintFeature = BoostFeature(text);
			if (blueprintFeature != null)
			{
				int rank = unit.Descriptor.Progression.Features.GetRank(blueprintFeature);
				if (rank > 0)
				{
					snapshot.BoostRanks[text] = rank;
				}
			}
		}
		return snapshot;
	}

	internal static void Apply(UnitEntityData unit, Snapshot snapshot)
	{
		if (unit == null || snapshot == null)
		{
			return;
		}
		BlueprintAbility blueprintAbility = WishAbility();
		if (snapshot.HadAbility && blueprintAbility != null && !unit.Descriptor.HasFact(blueprintAbility))
		{
			unit.Descriptor.AddFact(blueprintAbility);
		}
		BlueprintAbilityResource blueprintAbilityResource = WishResource();
		if (snapshot.HadResource && blueprintAbilityResource != null && !unit.Descriptor.Resources.ContainsResource(blueprintAbilityResource))
		{
			unit.Descriptor.Resources.Add(blueprintAbilityResource, restoreAmount: false);
		}
		if (blueprintAbilityResource != null && unit.Descriptor.Resources.ContainsResource(blueprintAbilityResource))
		{
			int num = ChargesToRestore(snapshot.Charges, unit.Descriptor.Resources.GetResourceAmount(blueprintAbilityResource));
			if (num > 0)
			{
				unit.Descriptor.Resources.Restore(blueprintAbilityResource, num);
			}
		}
		string[] boostFeatureNames = BoostFeatureNames;
		foreach (string text in boostFeatureNames)
		{
			if (!snapshot.BoostRanks.TryGetValue(text, out var value))
			{
				continue;
			}
			BlueprintFeature blueprintFeature = BoostFeature(text);
			if (blueprintFeature == null)
			{
				continue;
			}
			FeatureCollection features = unit.Descriptor.Progression.Features;
			Feature fact = features.GetFact(blueprintFeature);
			switch (DecideBoostRepair(value, fact?.GetRank() ?? 0))
			{
			case BoostRepair.ReAdd:
				fact = features.AddFeature(blueprintFeature);
				if (fact != null && value > 1)
				{
					fact.SetRank(value);
				}
				break;
			case BoostRepair.SetRank:
				fact.SetRank(value);
				break;
			}
		}
	}
}
