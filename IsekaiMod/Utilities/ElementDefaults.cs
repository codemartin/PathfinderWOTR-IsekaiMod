using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace IsekaiMod.Utilities;

internal static class ElementDefaults
{
	private sealed class ReferenceComparer : IEqualityComparer<object>
	{
		internal static readonly ReferenceComparer Instance = new ReferenceComparer();

		public new bool Equals(object x, object y)
		{
			return x == y;
		}

		public int GetHashCode(object obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}
	}

	private const int MaxDepth = 24;

	private const BindingFlags InstanceFields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

	private static readonly Dictionary<Type, FieldInfo[]> s_Fields = new Dictionary<Type, FieldInfo[]>();

	[ThreadStatic]
	private static int s_ValuesFilled;

	internal static int ValuesFilled => s_ValuesFilled;

	internal static void FillAll()
	{
		int num = 0;
		int num2 = 0;
		foreach (SimpleBlueprint item in BlueprintTextDefaultsPatch.RegisteredBlueprints())
		{
			try
			{
				num += NameElements(item);
				num2 += FillStartingEquipment(item);
			}
			catch (Exception)
			{
			}
		}
	}

	internal static int FillStartingEquipment(SimpleBlueprint blueprint)
	{
		BlueprintComponent[] array = (blueprint as BlueprintScriptableObject)?.ComponentsArray;
		if (array == null)
		{
			return 0;
		}
		int num = 0;
		BlueprintComponent[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i] is AddStartingEquipment blueprint2)
			{
				num += BlueprintFieldDefaults.FillNullFields(blueprint2);
			}
		}
		return num;
	}

	internal static int NameElements(SimpleBlueprint blueprint)
	{
		if (blueprint == null)
		{
			return 0;
		}
		HashSet<object> seen = new HashSet<object>(ReferenceComparer.Instance);
		int named = 0;
		string ownerId = blueprint.AssetGuid.ToString();
		FieldInfo[] array = FieldsOf(blueprint.GetType());
		foreach (FieldInfo fieldInfo in array)
		{
			object value;
			try
			{
				value = fieldInfo.GetValue(blueprint);
			}
			catch
			{
				continue;
			}
			Visit(value, blueprint, ownerId, fieldInfo.Name, seen, 0, ref named);
		}
		BlueprintComponent[] array2 = (blueprint as BlueprintScriptableObject)?.ComponentsArray;
		if (array2 != null)
		{
			for (int j = 0; j < array2.Length; j++)
			{
				BlueprintComponent blueprintComponent = array2[j];
				if (blueprintComponent == null)
				{
					continue;
				}
				string text = "Components[" + j + "]";
				array = FieldsOf(blueprintComponent.GetType());
				foreach (FieldInfo fieldInfo2 in array)
				{
					object value2;
					try
					{
						value2 = fieldInfo2.GetValue(blueprintComponent);
					}
					catch
					{
						continue;
					}
					Visit(value2, blueprint, ownerId, text + "." + fieldInfo2.Name, seen, 0, ref named);
				}
			}
		}
		return named;
	}

	internal static string ElementName(Type elementType, string ownerId, string path)
	{
		using MD5 mD = MD5.Create();
		byte[] b = mD.ComputeHash(Encoding.UTF8.GetBytes(ownerId + "|" + path));
		return "$" + (elementType?.Name ?? "Element") + "$" + new Guid(b).ToString();
	}

	internal static bool CanHoldElements(Type type)
	{
		if (type == null)
		{
			return false;
		}
		if (typeof(Element).IsAssignableFrom(type) || type == typeof(ActionList) || type == typeof(ConditionsChecker))
		{
			return true;
		}
		if (type == typeof(ContextDurationValue) || type == typeof(ContextDiceValue))
		{
			return true;
		}
		if (type.IsArray)
		{
			return CanHoldElements(type.GetElementType());
		}
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			return CanHoldElements(type.GetGenericArguments()[0]);
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
					if (CanHoldElements(fieldInfo.FieldType))
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

	private static void Visit(object node, SimpleBlueprint owner, string ownerId, string path, HashSet<object> seen, int depth, ref int named)
	{
		if (node == null || depth > 24 || !seen.Add(node))
		{
			return;
		}
		if (node is ActionList actionList)
		{
			if (actionList.Actions != null)
			{
				for (int i = 0; i < actionList.Actions.Length; i++)
				{
					Visit(actionList.Actions[i], owner, ownerId, path + ".Actions[" + i + "]", seen, depth + 1, ref named);
				}
			}
			return;
		}
		if (node is ConditionsChecker conditionsChecker)
		{
			if (conditionsChecker.Conditions != null)
			{
				for (int j = 0; j < conditionsChecker.Conditions.Length; j++)
				{
					Visit(conditionsChecker.Conditions[j], owner, ownerId, path + ".Conditions[" + j + "]", seen, depth + 1, ref named);
				}
			}
			return;
		}
		if (node is IList list && !(node is string))
		{
			for (int k = 0; k < list.Count; k++)
			{
				Visit(list[k], owner, ownerId, path + "[" + k + "]", seen, depth + 1, ref named);
			}
			return;
		}
		if (node is ContextDurationValue contextDurationValue)
		{
			s_ValuesFilled += FillZero(ref contextDurationValue.DiceCountValue) + FillZero(ref contextDurationValue.BonusValue);
			return;
		}
		if (node is ContextDiceValue contextDiceValue)
		{
			s_ValuesFilled += FillZero(ref contextDiceValue.DiceCountValue) + FillZero(ref contextDiceValue.BonusValue);
			return;
		}
		if (node is Element element)
		{
			if (string.IsNullOrEmpty(element.name))
			{
				element.name = ElementName(element.GetType(), ownerId, path);
				named++;
			}
			if (element.Owner == null)
			{
				element.Owner = owner;
			}
		}
		FieldInfo[] array = FieldsOf(node.GetType());
		foreach (FieldInfo fieldInfo in array)
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
			Visit(value, owner, ownerId, path + "." + fieldInfo.Name, seen, depth + 1, ref named);
		}
	}

	private static int FillZero(ref ContextValue value)
	{
		if (value != null)
		{
			return 0;
		}
		value = new ContextValue
		{
			ValueType = ContextValueType.Simple,
			Value = 0
		};
		return 1;
	}
}
