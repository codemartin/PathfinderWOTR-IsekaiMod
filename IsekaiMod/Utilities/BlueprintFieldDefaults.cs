using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;

namespace IsekaiMod.Utilities;

internal static class BlueprintFieldDefaults
{
	private const BindingFlags DeclaredInstance = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

	internal static int FillNullFields(object blueprint)
	{
		if (blueprint == null)
		{
			return 0;
		}
		int num = 0;
		Type type = blueprint.GetType();
		while (type != null && type != typeof(SimpleBlueprint) && type != typeof(object))
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!fieldInfo.IsInitOnly && !fieldInfo.IsLiteral && fieldInfo.GetValue(blueprint) == null)
				{
					object obj = DefaultFor(fieldInfo.FieldType);
					if (obj != null)
					{
						fieldInfo.SetValue(blueprint, obj);
						num++;
					}
				}
			}
			type = type.BaseType;
		}
		return num;
	}

	internal static IEnumerable<FieldInfo> NullLocalizedStringFields(object target)
	{
		if (target == null)
		{
			yield break;
		}
		Type type = target.GetType();
		while (type != null && type != typeof(object))
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!(fieldInfo.FieldType != typeof(LocalizedString)) && !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral && fieldInfo.GetValue(target) == null)
				{
					yield return fieldInfo;
				}
			}
			type = type.BaseType;
		}
	}

	internal static int FillNullLocalizedStrings(object target)
	{
		int num = 0;
		foreach (FieldInfo item in new List<FieldInfo>(NullLocalizedStringFields(target)))
		{
			item.SetValue(target, new LocalizedString());
			num++;
		}
		return num;
	}

	private static object DefaultFor(Type fieldType)
	{
		if (typeof(BlueprintReferenceBase).IsAssignableFrom(fieldType) && !fieldType.IsAbstract)
		{
			return Activator.CreateInstance(fieldType);
		}
		if (fieldType == typeof(LocalizedString))
		{
			return new LocalizedString();
		}
		if (fieldType == typeof(ConditionsChecker))
		{
			return new ConditionsChecker
			{
				Conditions = Array.Empty<Condition>()
			};
		}
		if (fieldType == typeof(ActionList))
		{
			return new ActionList
			{
				Actions = Array.Empty<GameAction>()
			};
		}
		if (fieldType.IsArray)
		{
			return Array.CreateInstance(fieldType.GetElementType(), 0);
		}
		if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
		{
			return (IList)Activator.CreateInstance(fieldType);
		}
		return null;
	}
}
