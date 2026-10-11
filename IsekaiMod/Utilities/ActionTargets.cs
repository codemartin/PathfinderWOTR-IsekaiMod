using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Utilities;

internal static class ActionTargets
{
	private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
	{
		internal static readonly ReferenceEqualityComparer Instance = new ReferenceEqualityComparer();

		public new bool Equals(object x, object y)
		{
			return x == y;
		}

		public int GetHashCode(object obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}
	}

	private const int MaxDepth = 16;

	private const BindingFlags InstanceFields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

	private static readonly Dictionary<Type, FieldInfo[]> s_Fields = new Dictionary<Type, FieldInfo[]>();

	internal static List<BlueprintBuff> BuffsIn(object holder)
	{
		List<BlueprintBuff> list = new List<BlueprintBuff>();
		Walk(holder, list, null);
		return list;
	}

	internal static List<BlueprintAbilityAreaEffect> AreaEffectsIn(object holder)
	{
		List<BlueprintAbilityAreaEffect> list = new List<BlueprintAbilityAreaEffect>();
		Walk(holder, null, list);
		return list;
	}

	private static void Walk(object holder, List<BlueprintBuff> buffs, List<BlueprintAbilityAreaEffect> areas)
	{
		if (holder == null)
		{
			return;
		}
		HashSet<object> seen = new HashSet<object>(ReferenceEqualityComparer.Instance);
		try
		{
			Visit(holder, buffs, areas, seen, 0);
		}
		catch
		{
		}
	}

	internal static bool CanHoldActions(Type type)
	{
		if (type == null)
		{
			return false;
		}
		if (type == typeof(ActionList) || typeof(GameAction).IsAssignableFrom(type))
		{
			return true;
		}
		if (type.IsArray)
		{
			return CanHoldActions(type.GetElementType());
		}
		return false;
	}

	private static FieldInfo[] FieldsOf(Type type)
	{
		lock (s_Fields)
		{
			if (s_Fields.TryGetValue(type, out var value))
			{
				return value;
			}
			List<FieldInfo> list = new List<FieldInfo>();
			Type type2 = type;
			while (type2 != null && type2 != typeof(object))
			{
				FieldInfo[] fields = type2.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (FieldInfo fieldInfo in fields)
				{
					if (CanHoldActions(fieldInfo.FieldType))
					{
						list.Add(fieldInfo);
					}
				}
				type2 = type2.BaseType;
			}
			FieldInfo[] array = list.ToArray();
			s_Fields[type] = array;
			return array;
		}
	}

	private static void Visit(object node, List<BlueprintBuff> buffs, List<BlueprintAbilityAreaEffect> areas, HashSet<object> seen, int depth)
	{
		if (node == null || depth > 16 || !seen.Add(node))
		{
			return;
		}
		if (node is ActionList actionList)
		{
			if (actionList.Actions != null)
			{
				GameAction[] actions = actionList.Actions;
				for (int i = 0; i < actions.Length; i++)
				{
					Visit(actions[i], buffs, areas, seen, depth + 1);
				}
			}
			return;
		}
		if (node is Array array)
		{
			{
				foreach (object item in array)
				{
					Visit(item, buffs, areas, seen, depth + 1);
				}
				return;
			}
		}
		if (node is ContextActionApplyBuff contextActionApplyBuff && buffs != null)
		{
			BlueprintBuff blueprintBuff = contextActionApplyBuff.m_Buff?.Get();
			if (blueprintBuff != null && !buffs.Contains(blueprintBuff))
			{
				buffs.Add(blueprintBuff);
			}
		}
		if (node is ContextActionSpawnAreaEffect contextActionSpawnAreaEffect && areas != null)
		{
			BlueprintAbilityAreaEffect blueprintAbilityAreaEffect = contextActionSpawnAreaEffect.m_AreaEffect?.Get();
			if (blueprintAbilityAreaEffect != null && !areas.Contains(blueprintAbilityAreaEffect))
			{
				areas.Add(blueprintAbilityAreaEffect);
			}
		}
		FieldInfo[] array2 = FieldsOf(node.GetType());
		foreach (FieldInfo fieldInfo in array2)
		{
			object value;
			try
			{
				value = fieldInfo.GetValue(node);
			}
			catch
			{
				continue;
			}
			if (value != null)
			{
				Visit(value, buffs, areas, seen, depth + 1);
			}
		}
	}
}
