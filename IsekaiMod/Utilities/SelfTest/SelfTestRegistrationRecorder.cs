using System;
using HarmonyLib;
using Kingmaker.Blueprints;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities.SelfTest;

[HarmonyPatch(typeof(BlueprintTools), "AddBlueprint", new Type[]
{
	typeof(ModContextBase),
	typeof(SimpleBlueprint),
	typeof(BlueprintGuid)
})]
internal static class SelfTestRegistrationRecorder
{
	[HarmonyPrepare]
	private static bool Prepare()
	{
		if (IsekaiSelfTest.Enabled)
		{
			return PatchGuard.Resolves(typeof(SelfTestRegistrationRecorder), typeof(BlueprintTools), "AddBlueprint", new Type[3]
			{
				typeof(ModContextBase),
				typeof(SimpleBlueprint),
				typeof(BlueprintGuid)
			});
		}
		return false;
	}

	[HarmonyPostfix]
	private static void Postfix(ModContextBase modContext, SimpleBlueprint blueprint, BlueprintGuid assetId)
	{
		try
		{
			if (blueprint != null && Main.IsekaiContext != null && modContext == Main.IsekaiContext)
			{
				SelfTestRecorder.AddCreated(blueprint.name, assetId, blueprint);
				SimpleBlueprint simpleBlueprint = ResourcesLibrary.TryGetBlueprint(assetId);
				if (simpleBlueprint != blueprint)
				{
					SelfTestRecorder.AddCollision(string.Format("'{0}' was not added: id {1} already belongs to '{2}'.", blueprint.name, assetId, simpleBlueprint?.name ?? "nothing"));
				}
			}
		}
		catch
		{
		}
	}
}
