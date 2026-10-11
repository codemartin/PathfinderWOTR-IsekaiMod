using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Kingmaker.Blueprints;
using Newtonsoft.Json;
using UnityEngine;

namespace IsekaiMod.Utilities.SelfTest;

internal sealed class BlueprintReferenceWalker
{
	internal sealed class Found
	{
		public object Owner;

		public string Path;

		public BlueprintReferenceBase Reference;

		public Type DeclaredType;

		public bool InCollection;
	}

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

	private const int MaxDepth = 14;

	private static readonly Dictionary<Type, FieldInfo[]> FieldCache = new Dictionary<Type, FieldInfo[]>();

	private readonly HashSet<object> seen = new HashSet<object>(ReferenceComparer.Instance);

	private readonly List<Found> found = new List<Found>();

	internal static List<Found> ReferencesIn(SimpleBlueprint blueprint)
	{
		BlueprintReferenceWalker blueprintReferenceWalker = new BlueprintReferenceWalker();
		if (blueprint != null)
		{
			blueprintReferenceWalker.WalkFields(blueprint, "", 0);
		}
		return blueprintReferenceWalker.found;
	}

	internal static List<Found> ReferencesInObject(object value)
	{
		BlueprintReferenceWalker blueprintReferenceWalker = new BlueprintReferenceWalker();
		if (value != null)
		{
			blueprintReferenceWalker.WalkFields(value, "", 0);
		}
		return blueprintReferenceWalker.found;
	}

	internal static Type TargetType(Type referenceType)
	{
		Type type = referenceType;
		while (type != null)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(BlueprintReference<>))
			{
				return type.GetGenericArguments()[0];
			}
			type = type.BaseType;
		}
		return typeof(SimpleBlueprint);
	}

	private void WalkFields(object owner, string path, int depth)
	{
		FieldInfo[] array = SerializedFields(owner.GetType());
		foreach (FieldInfo fieldInfo in array)
		{
			object value;
			try
			{
				value = fieldInfo.GetValue(owner);
			}
			catch
			{
				continue;
			}
			string path2 = ((path.Length == 0) ? fieldInfo.Name : (path + "." + fieldInfo.Name));
			Walk(owner, value, path2, fieldInfo.FieldType, inCollection: false, depth + 1);
		}
	}

	private void Walk(object owner, object value, string path, Type declaredType, bool inCollection, int depth)
	{
		if (depth > 14)
		{
			return;
		}
		if (value == null)
		{
			if (declaredType != null && typeof(BlueprintReferenceBase).IsAssignableFrom(declaredType))
			{
				found.Add(new Found
				{
					Owner = owner,
					Path = path,
					Reference = null,
					DeclaredType = declaredType,
					InCollection = inCollection
				});
			}
			return;
		}
		if (value is BlueprintReferenceBase reference)
		{
			found.Add(new Found
			{
				Owner = owner,
				Path = path,
				Reference = reference,
				DeclaredType = value.GetType(),
				InCollection = inCollection
			});
			return;
		}
		Type type = value.GetType();
		if (type.IsPrimitive || type.IsEnum || type.IsValueType || value is string || value is SimpleBlueprint || value is UnityEngine.Object || value is Delegate || value is IDictionary)
		{
			return;
		}
		if (value is IList list)
		{
			Type declaredType2 = (type.IsArray ? type.GetElementType() : (type.IsGenericType ? type.GetGenericArguments().FirstOrDefault() : typeof(object)));
			for (int i = 0; i < list.Count; i++)
			{
				object obj = list[i];
				string path2 = ((obj is BlueprintComponent) ? $"{path}[{i}]<{obj.GetType().Name}>" : $"{path}[{i}]");
				Walk(owner, obj, path2, declaredType2, inCollection: true, depth + 1);
			}
		}
		else if (IsGameType(type) && seen.Add(value))
		{
			WalkFields(value, path, depth);
		}
	}

	private static bool IsGameType(Type type)
	{
		string text = type.Assembly.GetName().Name ?? "";
		if (!text.StartsWith("Assembly-CSharp", StringComparison.Ordinal) && !text.StartsWith("IsekaiMod", StringComparison.Ordinal) && !text.StartsWith("TabletopTweaks", StringComparison.Ordinal))
		{
			return text.StartsWith("Owlcat", StringComparison.Ordinal);
		}
		return true;
	}

	private static FieldInfo[] SerializedFields(Type type)
	{
		lock (FieldCache)
		{
			if (FieldCache.TryGetValue(type, out var value))
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
					if (!fieldInfo.IsNotSerialized && !fieldInfo.IsDefined(typeof(JsonIgnoreAttribute), inherit: true) && (!fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true) || fieldInfo.IsDefined(typeof(JsonPropertyAttribute), inherit: true)) && (fieldInfo.IsPublic || fieldInfo.IsDefined(typeof(SerializeField), inherit: true) || fieldInfo.IsDefined(typeof(JsonPropertyAttribute), inherit: true)))
					{
						list.Add(fieldInfo);
					}
				}
				type2 = type2.BaseType;
			}
			FieldInfo[] array = list.ToArray();
			FieldCache[type] = array;
			return array;
		}
	}
}
