using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HarmonyLib;
using Kingmaker.Blueprints;
using TabletopTweaks.Core.Config;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(Blueprints), "GetGUID")]
internal static class BlueprintIdentifierFallback
{
	internal static readonly Guid Namespace = new Guid("712aa4df-ad84-403b-b3a8-c2c34cc90d38");

	private static readonly HashSet<string> Unregistered = new HashSet<string>(StringComparer.Ordinal);

	internal static IReadOnlyCollection<string> UnregisteredNames => Unregistered;

	[HarmonyPrepare]
	private static bool Prepare()
	{
		bool num = AccessTools.Method(typeof(Blueprints), "GetGUID", new Type[1] { typeof(string) }) != null;
		if (!num)
		{
			Main.Log("ERROR: Blueprints.GetGUID(string) not found in TabletopTweaks-Core; unregistered blueprint names will get an empty identifier.");
		}
		return num;
	}

	[HarmonyPostfix]
	private static void GetGUID_Postfix(Blueprints __instance, string name, ref BlueprintGuid __result)
	{
		try
		{
			if (!(__result != BlueprintGuid.Empty) && name != null && Main.IsekaiContext != null && __instance.Context == Main.IsekaiContext)
			{
				__result = new BlueprintGuid(NameDerivedGuid(name));
				if (Unregistered.Add(name))
				{
					Main.IsekaiContext.Logger.LogError($"Blueprint name '{name}' is missing from Config/Blueprints.json; using name-derived id {__result}. Register it with this id.");
				}
			}
		}
		catch (Exception ex)
		{
			Main.Log("ERROR: BlueprintIdentifierFallback failed for '" + name + "': " + ex);
		}
	}

	internal static Guid NameDerivedGuid(string name)
	{
		byte[] array = Namespace.ToByteArray();
		ToNetworkOrder(array);
		byte[] bytes = Encoding.UTF8.GetBytes(name);
		byte[] sourceArray;
		using (SHA1 sHA = SHA1.Create())
		{
			sourceArray = sHA.ComputeHash(array.Concat(bytes).ToArray());
		}
		byte[] array2 = new byte[16];
		Array.Copy(sourceArray, array2, 16);
		array2[6] = (byte)((array2[6] & 0xF) | 0x50);
		array2[8] = (byte)((array2[8] & 0x3F) | 0x80);
		ToNetworkOrder(array2);
		return new Guid(array2);
	}

	private static void ToNetworkOrder(byte[] guid)
	{
		Swap(guid, 0, 3);
		Swap(guid, 1, 2);
		Swap(guid, 4, 5);
		Swap(guid, 6, 7);
	}

	private static void Swap(byte[] bytes, int a, int b)
	{
		byte b2 = bytes[a];
		bytes[a] = bytes[b];
		bytes[b] = b2;
	}

	internal static void LogSummary()
	{
		if (Unregistered.Count != 0)
		{
			Main.IsekaiContext.Logger.LogError(string.Format("[REGRESSION ASSERTION FAILED] {0} blueprint name(s) are missing from Config/Blueprints.json and were given name-derived ids: {1}", Unregistered.Count, string.Join(", ", Unregistered)));
		}
	}
}
