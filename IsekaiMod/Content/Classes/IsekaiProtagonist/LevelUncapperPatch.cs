using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

internal static class LevelUncapperPatch
{
	internal enum XpTableKind
	{
		None,
		Vanilla,
		Legend
	}

	internal enum XpTableChoice
	{
		Unchanged,
		Legend,
		VanillaThenLegend
	}

	private sealed class KindBox
	{
		internal XpTableKind Kind;
	}

	private sealed class RecordKeeper : IAreaHandler, ISubscriber, IGlobalSubscriber
	{
		public void OnAreaDidLoad()
		{
			try
			{
				FlushRecords();
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[LevelUncapper] Could not record the characters' experience tables: " + ex);
			}
		}

		public void OnAreaBeginUnloading()
		{
		}
	}

	[HarmonyPatch(typeof(SaveManager), "PrepareSave")]
	private static class SaveManager_PrepareSave_Patch
	{
		[HarmonyPrefix]
		public static void Prefix()
		{
			try
			{
				FlushRecords();
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[LevelUncapper] Could not record the characters' experience tables before a save: " + ex);
			}
		}
	}

	[HarmonyPatch(typeof(UnitHelper), "RespecOnCommit")]
	private static class UnitHelper_RespecOnCommit_Patch
	{
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return PatchGuard.Resolves(typeof(UnitHelper_RespecOnCommit_Patch), typeof(UnitHelper), "RespecOnCommit");
		}

		[HarmonyPrefix]
		public static void Prefix(UnitEntityData targetUnit)
		{
			try
			{
				PinBeforeRespec(targetUnit);
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[LevelUncapper] Could not record the experience table before a respec: " + ex);
			}
		}
	}

	[HarmonyPatch(typeof(LevelUpUnit), "RunAction")]
	private static class LevelUpUnit_RunAction_Patch
	{
		public static bool Prefix(LevelUpUnit __instance)
		{
			try
			{
				return ApplyScriptedLevel(__instance?.Unit?.GetValue(), __instance.TargetLevel.GetValue(), log: true);
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[LevelUncapper] Could not price a scripted level grant: " + ex);
				return true;
			}
		}
	}

	[HarmonyPatch(typeof(AdvanceUnitLevel), "RunAction")]
	private static class AdvanceUnitLevel_RunAction_Patch
	{
		public static bool Prefix(AdvanceUnitLevel __instance)
		{
			try
			{
				return ApplyScriptedLevel(__instance?.Unit?.GetValue(), __instance.Level.GetValue(), log: false);
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[LevelUncapper] Could not price a scripted level grant: " + ex);
				return true;
			}
		}
	}

	[HarmonyPatch(typeof(UnitProgressionData), "get_MaxCharacterLevel")]
	private static class UnitProgressionData_MaxCharacterLevel_Patch
	{
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return PatchGuard.Resolves(typeof(UnitProgressionData_MaxCharacterLevel_Patch), typeof(UnitProgressionData), "get_MaxCharacterLevel");
		}

		public static void Postfix(UnitProgressionData __instance, ref int __result)
		{
			if (!Main.IsekaiContext.AddedContent.EnableLevelUncapping)
			{
				return;
			}
			UnitDescriptor unitDescriptor = __instance?.Owner;
			if (unitDescriptor != null)
			{
				bool isIsekai = unitDescriptor.Progression?.GetClassData(IsekaiProtagonistClass.Get()) != null;
				if (ShouldRaiseCharacterLevelCap(uncappingEnabled: true, isIsekai))
				{
					int valueOrDefault = (__instance.ExperienceTable?.Bonuses?.Length).GetValueOrDefault();
					__result = IsekaiCharacterLevelCap(__result, uncappingEnabled: true, isIsekai: true, valueOrDefault);
				}
			}
		}
	}

	[HarmonyPatch(typeof(UnitProgressionData), "get_ExperienceTable")]
	private static class UnitProgressionData_ExperienceTable_Patch
	{
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return PatchGuard.Resolves(typeof(UnitProgressionData_ExperienceTable_Patch), typeof(UnitProgressionData), "get_ExperienceTable");
		}

		public static void Postfix(UnitProgressionData __instance, ref BlueprintStatProgression __result)
		{
			UnitEntityData unitEntityData = __instance?.Owner?.Unit;
			if (unitEntityData == null)
			{
				return;
			}
			XpTableKind xpTableKind = TableKindFor(unitEntityData);
			if (xpTableKind == XpTableKind.None)
			{
				return;
			}
			ProgressionRoot obj = BlueprintRoot.Instance?.Progression;
			BlueprintStatProgression blueprintStatProgression = obj?.XPTable;
			BlueprintStatProgression blueprintStatProgression2 = obj?.LegendXPTable;
			bool currentIsXpTable = __result == null || __result == blueprintStatProgression;
			bool currentIsLegendTable = __result != null && __result == blueprintStatProgression2;
			switch (ChooseTable(xpTableKind, Main.IsekaiContext.AddedContent.EnableLevelUncapping, currentIsXpTable, currentIsLegendTable))
			{
			case XpTableChoice.Legend:
				if (blueprintStatProgression2 != null)
				{
					__result = blueprintStatProgression2;
				}
				break;
			case XpTableChoice.VanillaThenLegend:
			{
				BlueprintStatProgression blueprintStatProgression3 = VanillaThenLegendTable(blueprintStatProgression, blueprintStatProgression2);
				if (blueprintStatProgression3 != null)
				{
					__result = blueprintStatProgression3;
				}
				break;
			}
			}
		}
	}

	[HarmonyPatch(typeof(BlueprintStatProgression), "GetBonus")]
	private static class BlueprintStatProgression_GetBonus_Patch
	{
		public static bool Prefix(BlueprintStatProgression __instance, int level, ref int __result)
		{
			int[] array = __instance?.Bonuses;
			if (array == null || array.Length == 0)
			{
				return true;
			}
			if (level < array.Length)
			{
				return true;
			}
			ProgressionRoot progressionRoot = BlueprintRoot.Instance?.Progression;
			if (!ClampsOutOfRangeReads(progressionRoot != null && __instance == progressionRoot.XPTable, progressionRoot != null && __instance == progressionRoot.LegendXPTable, s_VanillaThenLegend != null && __instance == s_VanillaThenLegend))
			{
				return true;
			}
			__result = ClampedBonus(array, level);
			return false;
		}
	}

	private const int IsekaiMaxCharacterLevel = 40;

	private const int VanillaLevelCap = 20;

	internal const string XpTableStorageName = "IsekaiMod.XpTables";

	private static readonly ConditionalWeakTable<UnitEntityData, KindBox> s_Session = new ConditionalWeakTable<UnitEntityData, KindBox>();

	private static BlueprintStatProgression s_VanillaThenLegend;

	private static BlueprintStatProgression s_VanillaSource;

	private static BlueprintStatProgression s_LegendSource;

	internal static bool StartedAsIsekai<T>(IList<T> classesOrder, T isekaiClass, Func<T, bool> isMythic) where T : class
	{
		if (classesOrder == null || isekaiClass == null || isMythic == null)
		{
			return false;
		}
		for (int i = 0; i < classesOrder.Count; i++)
		{
			T val = classesOrder[i];
			if (val != null && !isMythic(val))
			{
				return val == isekaiClass;
			}
		}
		return false;
	}

	internal static bool LevelledPastVanillaTable(int[] vanillaBonuses, int characterLevel, int experience)
	{
		if (vanillaBonuses == null || vanillaBonuses.Length == 0 || characterLevel <= 0)
		{
			return false;
		}
		return experience < ClampedBonus(vanillaBonuses, characterLevel);
	}

	internal static int[] VanillaThenLegendBonuses(int[] vanilla, int[] legend)
	{
		if (vanilla == null || vanilla.Length == 0)
		{
			return vanilla;
		}
		if (legend == null || legend.Length <= vanilla.Length)
		{
			return (int[])vanilla.Clone();
		}
		int num = vanilla.Length - 1;
		int[] array = new int[legend.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = ((i <= num) ? vanilla[i] : (vanilla[num] + (legend[i] - legend[num])));
		}
		return array;
	}

	internal static int LevelFor(int[] bonuses, int experience, int cap)
	{
		for (int num = cap; num > 0; num--)
		{
			if (experience >= ClampedBonus(bonuses, num))
			{
				return num;
			}
		}
		return 0;
	}

	internal static XpTableKind InferTableKind(bool startedAsIsekai, bool hasIsekaiLevels, int level, int experience, int[] vanilla)
	{
		if ((startedAsIsekai | hasIsekaiLevels) && LevelledPastVanillaTable(vanilla, level, experience))
		{
			return XpTableKind.Legend;
		}
		if (!startedAsIsekai)
		{
			return XpTableKind.None;
		}
		return XpTableKind.Vanilla;
	}

	internal static bool CanMoveToLegendWithoutAJump(int level, int experience, int[] vanilla, int[] legend)
	{
		if (vanilla == null || legend == null)
		{
			return false;
		}
		int num = LevelFor(legend, experience, 40);
		int num2 = LevelFor(VanillaThenLegendBonuses(vanilla, legend), experience, 40);
		if (num == num2)
		{
			return level <= num;
		}
		return false;
	}

	internal static XpTableKind ResolveTableKind(XpTableKind recorded, bool uncappingEnabled, bool startedAsIsekai, bool hasIsekaiLevels, int level, int experience, int[] vanilla, int[] legend)
	{
		XpTableKind xpTableKind = ((recorded != XpTableKind.None) ? recorded : InferTableKind(startedAsIsekai, hasIsekaiLevels, level, experience, vanilla));
		if (((xpTableKind == XpTableKind.Vanilla) & uncappingEnabled & startedAsIsekai) && CanMoveToLegendWithoutAJump(level, experience, vanilla, legend))
		{
			xpTableKind = XpTableKind.Legend;
		}
		return xpTableKind;
	}

	internal static XpTableChoice ChooseTable(XpTableKind kind, bool uncappingEnabled, bool currentIsXpTable, bool currentIsLegendTable)
	{
		if (kind == XpTableKind.None)
		{
			return XpTableChoice.Unchanged;
		}
		if (!currentIsXpTable && !currentIsLegendTable)
		{
			return XpTableChoice.Unchanged;
		}
		if (kind == XpTableKind.Legend)
		{
			return XpTableChoice.Legend;
		}
		if (currentIsLegendTable)
		{
			return XpTableChoice.Unchanged;
		}
		if (!uncappingEnabled)
		{
			return XpTableChoice.Unchanged;
		}
		return XpTableChoice.VanillaThenLegend;
	}

	internal static bool ShouldRaiseCharacterLevelCap(bool uncappingEnabled, bool isIsekai)
	{
		return uncappingEnabled & isIsekai;
	}

	internal static int IsekaiCharacterLevelCap(int currentCap, bool uncappingEnabled, bool isIsekai, int experienceTableLength)
	{
		if (!ShouldRaiseCharacterLevelCap(uncappingEnabled, isIsekai))
		{
			return currentCap;
		}
		if (experienceTableLength <= 1)
		{
			return currentCap;
		}
		int num = experienceTableLength - 1;
		int num2 = ((num < 40) ? num : 40);
		if (currentCap <= num2)
		{
			return num2;
		}
		return currentCap;
	}

	internal static int ClampedBonus(int[] bonuses, int level)
	{
		if (bonuses == null || bonuses.Length == 0)
		{
			return 0;
		}
		if (level < 0)
		{
			return 0;
		}
		if (level >= bonuses.Length)
		{
			return bonuses[bonuses.Length - 1];
		}
		return bonuses[level];
	}

	private static UnitEntityData Judged(UnitEntityData unit)
	{
		return unit?.PreviewOf.Entity ?? unit;
	}

	internal static bool StartedAsIsekaiUnit(UnitEntityData unit)
	{
		try
		{
			return StartedAsIsekai(Judged(unit)?.Progression?.ClassesOrder, IsekaiProtagonistClass.Get(), (BlueprintCharacterClass c) => c.IsMythic);
		}
		catch
		{
			return false;
		}
	}

	internal static string Encode(XpTableKind kind)
	{
		if (kind != XpTableKind.None)
		{
			return kind.ToString();
		}
		return null;
	}

	internal static XpTableKind Decode(string value)
	{
		if (value == "Legend")
		{
			return XpTableKind.Legend;
		}
		if (value == "Vanilla")
		{
			return XpTableKind.Vanilla;
		}
		return XpTableKind.None;
	}

	private static XpTableKind StoredKind(UnitEntityData judged)
	{
		return Decode(SaveScopedStorage.Get(SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.XpTables"), judged.UniqueId));
	}

	internal static XpTableKind TableKindFor(UnitEntityData unit)
	{
		try
		{
			UnitEntityData unitEntityData = Judged(unit);
			UnitProgressionData unitProgressionData = unitEntityData?.Progression;
			if (unitProgressionData == null)
			{
				return XpTableKind.None;
			}
			ProgressionRoot obj = BlueprintRoot.Instance?.Progression;
			int[] vanilla = obj?.XPTable?.Bonuses;
			int[] legend = obj?.LegendXPTable?.Bonuses;
			XpTableKind xpTableKind = (s_Session.TryGetValue(unitEntityData, out var value) ? value.Kind : StoredKind(unitEntityData));
			bool flag = StartedAsIsekai(unitProgressionData.ClassesOrder, IsekaiProtagonistClass.Get(), (BlueprintCharacterClass c) => c.IsMythic);
			bool flag2 = unitProgressionData.GetClassData(IsekaiProtagonistClass.Get()) != null;
			if (xpTableKind == XpTableKind.None && !flag && !flag2)
			{
				return XpTableKind.None;
			}
			XpTableKind xpTableKind2 = ResolveTableKind(xpTableKind, Main.IsekaiContext.AddedContent.EnableLevelUncapping, flag, flag2, unitProgressionData.CharacterLevel, unitProgressionData.Experience, vanilla, legend);
			if (xpTableKind2 != XpTableKind.None && (value == null || value.Kind != xpTableKind2))
			{
				s_Session.GetValue(unitEntityData, (UnitEntityData _) => new KindBox()).Kind = xpTableKind2;
			}
			return xpTableKind2;
		}
		catch
		{
			return XpTableKind.None;
		}
	}

	internal static void FlushRecords()
	{
		Player player = Game.Instance?.Player;
		List<UnitEntityData> list = player?.AllCharacters;
		if (list == null)
		{
			return;
		}
		Dictionary<string, string> dictionary = null;
		foreach (UnitEntityData item in list.ToList())
		{
			if (item == null || string.IsNullOrEmpty(item.UniqueId))
			{
				continue;
			}
			string text = Encode(TableKindFor(item));
			if (text != null)
			{
				dictionary = dictionary ?? SaveScopedStorage.Ensure(player, "IsekaiMod.XpTables");
				if (dictionary == null)
				{
					break;
				}
				if (SaveScopedStorage.Get(dictionary, item.UniqueId) != text)
				{
					dictionary[item.UniqueId] = text;
				}
			}
		}
	}

	internal static void PinBeforeRespec(UnitEntityData targetUnit)
	{
		TableKindFor(targetUnit);
		FlushRecords();
	}

	private static BlueprintStatProgression VanillaThenLegendTable(BlueprintStatProgression vanilla, BlueprintStatProgression legend)
	{
		if (vanilla == null || legend == null)
		{
			return vanilla;
		}
		if (s_VanillaThenLegend == null || s_VanillaSource != vanilla || s_LegendSource != legend)
		{
			s_VanillaThenLegend = new BlueprintStatProgression
			{
				name = "IsekaiVanillaThenLegendXPTable",
				Bonuses = VanillaThenLegendBonuses(vanilla.Bonuses, legend.Bonuses)
			};
			s_VanillaSource = vanilla;
			s_LegendSource = legend;
		}
		return s_VanillaThenLegend;
	}

	internal static void Init()
	{
		EventBus.Subscribe(new RecordKeeper());
	}

	internal static int ScriptedLevelExperience(int[] unitTableBonuses, int targetLevel)
	{
		return ClampedBonus(unitTableBonuses, targetLevel);
	}

	private static bool ApplyScriptedLevel(UnitEntityData unit, int targetLevel, bool log)
	{
		if (unit?.Descriptor?.Progression == null)
		{
			return true;
		}
		if (TableKindFor(unit) == XpTableKind.None)
		{
			return true;
		}
		int[] array = unit.Progression.ExperienceTable?.Bonuses;
		if (array == null || array.Length == 0)
		{
			return true;
		}
		unit.Descriptor.Progression.AdvanceExperienceTo(ScriptedLevelExperience(array, targetLevel), log);
		return false;
	}

	internal static bool ClampsOutOfRangeReads(bool isVanillaXpTable, bool isLegendXpTable, bool isMergedXpTable)
	{
		return isVanillaXpTable | isLegendXpTable | isMergedXpTable;
	}
}
