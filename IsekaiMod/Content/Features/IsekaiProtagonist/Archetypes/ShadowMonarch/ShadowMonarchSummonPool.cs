using Kingmaker.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;

internal static class ShadowMonarchSummonPool
{
	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMonarchSummonPool", delegate(BlueprintSummonPool bp)
		{
			bp.Limit = 0;
			bp.DoNotRemoveDeadUnits = false;
		});
	}

	public static BlueprintSummonPoolReference GetReference()
	{
		return BlueprintTools.GetModBlueprintReference<BlueprintSummonPoolReference>(Main.IsekaiContext, "ShadowMonarchSummonPool");
	}

	public static BlueprintSummonPoolReference[] LegacyPools()
	{
		return new BlueprintSummonPoolReference[1] { BlueprintTools.GetBlueprintReference<BlueprintSummonPoolReference>("d94c93e7240f10e41ae41db4c83d1cbe") };
	}
}
