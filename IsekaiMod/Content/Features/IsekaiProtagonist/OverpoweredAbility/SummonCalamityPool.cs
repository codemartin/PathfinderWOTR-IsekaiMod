using Kingmaker.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal static class SummonCalamityPool
{
	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "SummonCalamityPool", delegate(BlueprintSummonPool bp)
		{
			bp.Limit = 0;
			bp.DoNotRemoveDeadUnits = false;
		});
	}

	public static BlueprintSummonPoolReference GetReference()
	{
		return BlueprintTools.GetModBlueprintReference<BlueprintSummonPoolReference>(Main.IsekaiContext, "SummonCalamityPool");
	}
}
