using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Blueprints;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

[HarmonyPatch(typeof(BlueprintTools), "AddBlueprint", new Type[]
{
	typeof(ModContextBase),
	typeof(SimpleBlueprint),
	typeof(BlueprintGuid)
})]
internal static class BlueprintTextDefaultsPatch
{
	private static readonly object Gate = new object();

	private static readonly List<SimpleBlueprint> Registered = new List<SimpleBlueprint>();

	private static int FieldsFilled;

	internal static int FilledFieldCount
	{
		get
		{
			lock (Gate)
			{
				return FieldsFilled;
			}
		}
	}

	[HarmonyPrepare]
	private static bool Prepare()
	{
		return PatchGuard.Resolves(typeof(BlueprintTextDefaultsPatch), typeof(BlueprintTools), "AddBlueprint", new Type[3]
		{
			typeof(ModContextBase),
			typeof(SimpleBlueprint),
			typeof(BlueprintGuid)
		});
	}

	internal static List<SimpleBlueprint> RegisteredBlueprints()
	{
		lock (Gate)
		{
			return new List<SimpleBlueprint>(Registered);
		}
	}

	[HarmonyPostfix]
	private static void Postfix(ModContextBase modContext, SimpleBlueprint blueprint)
	{
		try
		{
			if (blueprint == null || Main.IsekaiContext == null || modContext != Main.IsekaiContext)
			{
				return;
			}
			int num = BlueprintFieldDefaults.FillNullLocalizedStrings(blueprint);
			lock (Gate)
			{
				Registered.Add(blueprint);
				FieldsFilled += num;
			}
		}
		catch (Exception)
		{
		}
	}

	internal static void FillComponents()
	{
		int num = 0;
		int num2 = 0;
		foreach (SimpleBlueprint item in RegisteredBlueprints())
		{
			BlueprintComponent[] array = (item as BlueprintScriptableObject)?.ComponentsArray;
			if (array == null)
			{
				continue;
			}
			BlueprintComponent[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				int num3 = BlueprintFieldDefaults.FillNullLocalizedStrings(array2[i]);
				if (num3 > 0)
				{
					num += num3;
					num2++;
				}
			}
		}
		lock (Gate)
		{
			FieldsFilled += num;
		}
	}
}
