using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace IsekaiMod.Utilities.SelfTest;

public static class IlInspector
{
	public sealed class Body
	{
		public readonly List<MethodBase> Calls = new List<MethodBase>();

		public readonly List<FieldInfo> StaticReads = new List<FieldInfo>();
	}

	private static readonly Dictionary<byte, OpCode> OneByte;

	private static readonly Dictionary<byte, OpCode> TwoByte;

	static IlInspector()
	{
		OneByte = new Dictionary<byte, OpCode>();
		TwoByte = new Dictionary<byte, OpCode>();
		FieldInfo[] fields = typeof(OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
		for (int i = 0; i < fields.Length; i++)
		{
			if (fields[i].GetValue(null) is OpCode value)
			{
				if (value.Size == 1)
				{
					OneByte[(byte)(value.Value & 0xFF)] = value;
				}
				else
				{
					TwoByte[(byte)(value.Value & 0xFF)] = value;
				}
			}
		}
	}

	public static Body Read(MethodBase method)
	{
		Body body = new Body();
		byte[] array;
		try
		{
			array = method?.GetMethodBody()?.GetILAsByteArray();
		}
		catch
		{
			return body;
		}
		if (array == null)
		{
			return body;
		}
		Type[] genericTypeArguments = null;
		Type[] genericMethodArguments = null;
		try
		{
			if (method.DeclaringType != null && method.DeclaringType.IsGenericType)
			{
				genericTypeArguments = method.DeclaringType.GetGenericArguments();
			}
			if (method.IsGenericMethod)
			{
				genericMethodArguments = method.GetGenericArguments();
			}
		}
		catch
		{
		}
		Module module = method.Module;
		int num = 0;
		while (num < array.Length)
		{
			byte b = array[num++];
			OpCode value;
			if (b == 254)
			{
				if (num >= array.Length || !TwoByte.TryGetValue(array[num++], out value))
				{
					break;
				}
			}
			else if (!OneByte.TryGetValue(b, out value))
			{
				break;
			}
			switch (value.OperandType)
			{
			case OperandType.ShortInlineBrTarget:
			case OperandType.ShortInlineI:
			case OperandType.ShortInlineVar:
				num++;
				break;
			case OperandType.InlineVar:
				num += 2;
				break;
			case OperandType.InlineI8:
			case OperandType.InlineR:
				num += 8;
				break;
			case OperandType.InlineSwitch:
			{
				if (num + 4 > array.Length)
				{
					return body;
				}
				int num2 = BitConverter.ToInt32(array, num);
				num += 4 + 4 * num2;
				break;
			}
			case OperandType.InlineMethod:
			{
				if (num + 4 > array.Length)
				{
					return body;
				}
				int metadataToken2 = BitConverter.ToInt32(array, num);
				num += 4;
				if (!(value == OpCodes.Call) && !(value == OpCodes.Callvirt) && !(value == OpCodes.Newobj) && !(value == OpCodes.Ldftn) && !(value == OpCodes.Ldvirtftn))
				{
					break;
				}
				try
				{
					MethodBase methodBase = module.ResolveMethod(metadataToken2, genericTypeArguments, genericMethodArguments);
					if (methodBase != null)
					{
						body.Calls.Add(methodBase);
					}
				}
				catch
				{
				}
				break;
			}
			case OperandType.InlineField:
			{
				if (num + 4 > array.Length)
				{
					return body;
				}
				int metadataToken = BitConverter.ToInt32(array, num);
				num += 4;
				if (!(value == OpCodes.Ldsfld) && !(value == OpCodes.Ldsflda))
				{
					break;
				}
				try
				{
					FieldInfo fieldInfo = module.ResolveField(metadataToken, genericTypeArguments, genericMethodArguments);
					if (fieldInfo != null)
					{
						body.StaticReads.Add(fieldInfo);
					}
				}
				catch
				{
				}
				break;
			}
			default:
				num += 4;
				break;
			case OperandType.InlineNone:
				break;
			}
		}
		return body;
	}

	public static IEnumerable<MethodBase> MethodsOf(Type type)
	{
		List<MethodBase> list = new List<MethodBase>();
		try
		{
			list.AddRange(from m in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
				where !m.IsAbstract
				select m);
			list.AddRange(type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
			Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
			foreach (Type type2 in nestedTypes)
			{
				list.AddRange(MethodsOf(type2));
			}
		}
		catch
		{
		}
		return list;
	}

	public static string EnclosingMethodName(MethodBase method)
	{
		if (method == null)
		{
			return null;
		}
		string text = NameInBrackets(method.Name);
		if (text != null)
		{
			return text;
		}
		return NameInBrackets(method.DeclaringType?.Name);
	}

	private static string NameInBrackets(string name)
	{
		if (string.IsNullOrEmpty(name) || name[0] != '<')
		{
			return null;
		}
		int num = name.IndexOf('>');
		if (num <= 1)
		{
			return null;
		}
		return name.Substring(1, num - 1);
	}

	public static bool IsObjectiveCompletion(MethodBase method)
	{
		if (method == null || method.Name != "CompleteObjective")
		{
			return false;
		}
		Type declaringType = method.DeclaringType;
		if (declaringType != null)
		{
			if (!(declaringType.FullName == "Kingmaker.AreaLogic.QuestSystem.QuestBook"))
			{
				return declaringType.FullName == "Kingmaker.Designers.GameHelper+Quests";
			}
			return true;
		}
		return false;
	}

	public static Dictionary<FieldInfo, string> FieldsReadByCompletingCode(IEnumerable<Type> types, Func<MethodBase, bool> completes)
	{
		Dictionary<string, KeyValuePair<MethodBase, Body>> bodies = new Dictionary<string, KeyValuePair<MethodBase, Body>>(StringComparer.Ordinal);
		foreach (Type item in types ?? Enumerable.Empty<Type>())
		{
			foreach (MethodBase item2 in MethodsOf(item))
			{
				string text = KeyOf(item2);
				if (text != null && !bodies.ContainsKey(text))
				{
					bodies[text] = new KeyValuePair<MethodBase, Body>(item2, Read(item2));
				}
			}
		}
		Dictionary<FieldInfo, string> reached = new Dictionary<FieldInfo, string>();
		foreach (KeyValuePair<MethodBase, Body> item3 in bodies.Values.ToList())
		{
			MethodBase key = item3.Key;
			if (!item3.Value.Calls.Any(completes))
			{
				continue;
			}
			string because = (key.DeclaringType?.FullName ?? "?") + "." + key.Name;
			TakeWithCallees(key, because);
			string text2 = EnclosingMethodName(key);
			if (text2 == null)
			{
				continue;
			}
			Type type = OuterType(key.DeclaringType);
			foreach (MethodBase item4 in bodies.Values.Select((KeyValuePair<MethodBase, Body> v) => v.Key).ToList())
			{
				if (item4.Name == text2 && OuterType(item4.DeclaringType) == type)
				{
					TakeWithCallees(item4, because);
				}
			}
		}
		return reached;
		void Take(MethodBase reader, string value2)
		{
			string text3 = KeyOf(reader);
			if (text3 == null || !bodies.TryGetValue(text3, out var value))
			{
				return;
			}
			foreach (FieldInfo staticRead in value.Value.StaticReads)
			{
				if (!reached.ContainsKey(staticRead))
				{
					reached[staticRead] = value2;
				}
			}
		}
		void TakeWithCallees(MethodBase reader, string because2)
		{
			Take(reader, because2);
			string text3 = KeyOf(reader);
			if (text3 == null || !bodies.TryGetValue(text3, out var value))
			{
				return;
			}
			foreach (MethodBase call in value.Value.Calls)
			{
				Take(call, because2);
			}
		}
	}

	private static string KeyOf(MethodBase method)
	{
		try
		{
			return (method == null) ? null : (method.Module.ScopeName + ":" + method.MetadataToken);
		}
		catch
		{
			return null;
		}
	}

	public static Type OuterType(Type type)
	{
		while (type != null && type.IsNested && type.Name.StartsWith("<", StringComparison.Ordinal))
		{
			type = type.DeclaringType;
		}
		return type;
	}
}
