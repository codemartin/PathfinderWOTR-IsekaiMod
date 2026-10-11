using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;

namespace IsekaiMod.Content.Guardians;

internal static class GuardianPetLevel
{
	[HarmonyPatch(typeof(AddPet), "TryLevelUpPet")]
	internal static class AddPet_TryLevelUpPet_Patch
	{
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return PatchGuard.Resolves(typeof(AddPet_TryLevelUpPet_Patch), typeof(AddPet), "TryLevelUpPet");
		}

		[HarmonyPrefix]
		private static void Prefix(AddPet __instance, out bool __state)
		{
			__state = false;
			try
			{
				if (IsGuardian(__instance))
				{
					__state = ((EntityFactComponentDelegate<UnitEntityData, AddPetData>)__instance).Data?.AutoLevelup ?? false;
				}
			}
			catch (Exception ex)
			{
				Main.Log("Guardian level: prefix failed: " + ex);
			}
		}

		[HarmonyPostfix]
		[HarmonyPriority(0)]
		private static void Postfix(AddPet __instance, bool __state)
		{
			try
			{
				LevelUp(__instance, __state);
			}
			catch (Exception ex)
			{
				Main.Log("Guardian level: could not level " + (__instance?.OwnerBlueprint?.name ?? "a guardian") + ": " + ex);
			}
		}
	}

	internal const int MaxGuardianLevel = 20;

	internal const string TabletopTweaksPatchType = "TabletopTweaks.Base.Bugfixes.Features.AnimalCompanions+LevelAdjustments+AddPet_TryLevelUpPet_Patch";

	internal const string TabletopTweaksAssembly = "TabletopTweaks-Base";

	private static readonly HashSet<BlueprintGuid> s_GuardianFeatures = new HashSet<BlueprintGuid>();

	internal static void Register(BlueprintFeature feature)
	{
		if (feature != null)
		{
			s_GuardianFeatures.Add(feature.AssetGuid);
		}
	}

	internal static bool IsRegistered(BlueprintFeature feature)
	{
		if (feature != null)
		{
			return s_GuardianFeatures.Contains(feature.AssetGuid);
		}
		return false;
	}

	internal static bool IsGuardian(AddPet component)
	{
		if (component == null)
		{
			return false;
		}
		BlueprintScriptableObject ownerBlueprint = component.OwnerBlueprint;
		if (ownerBlueprint != null)
		{
			return s_GuardianFeatures.Contains(ownerBlueprint.AssetGuid);
		}
		return false;
	}

	internal static int TargetLevel(int masterLevel, int rankCount)
	{
		if (masterLevel <= 0 || rankCount <= 0)
		{
			return 0;
		}
		return Math.Min(masterLevel, Math.Min(20, rankCount));
	}

	internal static int LevelsToAdd(int currentPetLevel, int masterLevel, int rankCount)
	{
		return Math.Max(0, TargetLevel(masterLevel, rankCount) - Math.Max(0, currentPetLevel));
	}

	internal static int EngineLevel(int masterLevel, int rankCount, int[] table)
	{
		return Math.Min(masterLevel, TableLevel(rankCount, table));
	}

	internal static int TabletopTweaksLevel(int masterLevel, int rankCount, int[] table)
	{
		return Math.Max(EngineLevel(masterLevel, rankCount, table), TableLevel(rankCount, table));
	}

	private static int TableLevel(int rankCount, int[] table)
	{
		if (table == null || table.Length == 0)
		{
			return 0;
		}
		int num = Math.Max(0, Math.Min(Math.Min(20, rankCount), table.Length - 1));
		return table[num];
	}

	internal static void LevelUp(AddPet component, bool autoLevelup)
	{
		if (!IsGuardian(component))
		{
			return;
		}
		UnitEntityData unitEntityData = ((EntityFactComponentDelegate<UnitEntityData, AddPetData>)component).Data?.SpawnedPetRef.Value;
		UnitEntityData owner = ((EntityFactComponentDelegate<UnitEntityData, AddPetData>)component).Owner;
		BlueprintFeature levelRank = component.LevelRank;
		if (unitEntityData == null || owner == null || levelRank == null)
		{
			return;
		}
		int rankCount = owner.GetFact(levelRank)?.GetRank() ?? 0;
		int valueOrDefault = (owner.Descriptor?.Progression?.CharacterLevel).GetValueOrDefault();
		int num = LevelsToAdd((unitEntityData.Descriptor?.Progression?.CharacterLevel).GetValueOrDefault(), valueOrDefault, rankCount);
		if (num <= 0)
		{
			return;
		}
		int level = TargetLevel(valueOrDefault, rankCount);
		UnitPartCompanion unitPartCompanion = owner.Get<UnitPartCompanion>();
		if (unitPartCompanion != null && unitPartCompanion.State != CompanionState.None && unitPartCompanion.State != CompanionState.ExCompanion && !autoLevelup && component.Type == PetType.AnimalCompanion)
		{
			BlueprintStatProgression blueprintStatProgression = BlueprintRoot.Instance?.Progression?.XPTable;
			if (blueprintStatProgression != null)
			{
				unitEntityData.Progression.AdvanceExperienceTo(blueprintStatProgression.GetBonus(level), log: false, allowForPet: true);
			}
			return;
		}
		AddClassLevels addClassLevels = unitEntityData.Master?.Blueprint?.GetComponent<AddClassLevelsToPets>()?.LevelsProgression;
		if (addClassLevels != null)
		{
			AddClassLevels.LevelUp(addClassLevels, unitEntityData.Descriptor, num);
			return;
		}
		BlueprintComponentAndRuntime<AddClassLevels> blueprintComponentAndRuntime = unitEntityData.Facts.List.Select((EntityFact f) => f.GetComponentWithRuntime<AddClassLevels>()).FirstOrDefault((BlueprintComponentAndRuntime<AddClassLevels> c) => c.Component != null);
		if (blueprintComponentAndRuntime.Component != null)
		{
			blueprintComponentAndRuntime.Component.LevelUp(blueprintComponentAndRuntime.Runtime, unitEntityData.Descriptor, num);
		}
	}

	internal static void ApplyTabletopTweaksGate(Harmony harmony)
	{
		try
		{
			if (harmony == null)
			{
				return;
			}
			Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.GetName().Name == "TabletopTweaks-Base");
			if (!(assembly == null))
			{
				Type type = assembly.GetType("TabletopTweaks.Base.Bugfixes.Features.AnimalCompanions+LevelAdjustments+AddPet_TryLevelUpPet_Patch", throwOnError: false);
				MethodInfo methodInfo = ((type == null) ? null : AccessTools.Method(type, "Postfix"));
				if (methodInfo == null)
				{
					Main.Log("Guardian level: TabletopTweaks-Base is loaded but its AddPet.TryLevelUpPet patch was not found; guardians rely on the level clamp alone.");
				}
				else
				{
					GateTabletopTweaksPostfix(harmony, methodInfo);
				}
			}
		}
		catch (Exception ex)
		{
			Main.Log("Guardian level: could not gate TabletopTweaks-Base's pet level patch: " + ex);
		}
	}

	internal static void GateTabletopTweaksPostfix(Harmony harmony, MethodInfo postfix)
	{
		harmony.Patch(postfix, new HarmonyMethod(typeof(GuardianPetLevel), "SkipTabletopTweaksForGuardians"));
	}

	internal static bool ShouldRunTabletopTweaksPostfix(AddPet component)
	{
		return !IsGuardian(component);
	}

	private static bool SkipTabletopTweaksForGuardians(AddPet __0)
	{
		try
		{
			return ShouldRunTabletopTweaksPostfix(__0);
		}
		catch (Exception ex)
		{
			Main.Log("Guardian level: TabletopTweaks gate failed: " + ex);
			return true;
		}
	}
}
