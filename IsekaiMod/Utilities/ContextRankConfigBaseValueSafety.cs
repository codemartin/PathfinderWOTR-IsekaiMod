using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.ModLogic;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(ContextRankConfig), "GetBaseValue", new Type[] { typeof(MechanicsContext) })]
internal static class ContextRankConfigBaseValueSafety
{
	private static readonly HashSet<string> reported = new HashSet<string>();

	private static Exception Finalizer(Exception __exception, ContextRankConfig __instance, MechanicsContext context, ref int __result)
	{
		if (__exception == null)
		{
			return null;
		}
		__result = 0;
		string text = __instance?.OwnerBlueprint?.name ?? "(no owner)";
		if (reported.Add(text))
		{
			string text2 = "";
			try
			{
				List<string> list = new List<string>();
				foreach (ClassData item in context?.MaybeCaster?.Descriptor?.Progression?.Classes ?? new List<ClassData>())
				{
					list.Add((item?.CharacterClass?.name ?? "<null class>") + ":" + ((item?.Archetypes == null) ? "<null archetypes>" : item.Archetypes.Count.ToString()));
				}
				text2 = string.Join(", ", list);
			}
			catch
			{
			}
			((ModContextBase)Main.IsekaiContext).Logger.LogWarning(string.Format("ContextRankConfig on {0} ({1}) could not be evaluated for {2}; using 0. archetype={3} additional={4} classes=[{5}] {6}: {7}", text, __instance?.m_BaseValueType, context?.MaybeCaster?.CharacterName, (__instance?.Archetype == null) ? "null" : "set", (__instance?.m_AdditionalArchetypes == null) ? "null" : __instance.m_AdditionalArchetypes.Length.ToString(), text2, __exception.GetType().Name, __exception.Message));
		}
		return null;
	}
}
