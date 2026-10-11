using System.Linq;
using IsekaiMod.Config;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Settlements;

namespace IsekaiMod.Content.Crusade;

[TypeId("8483051cc180439f8331e956b3b886d8")]
public class OncePerCrusadeOfferedRestriction : BuildingRestriction
{
	public OfferedContent Content;

	internal static bool MayBuild(bool offered, int copiesInCrusade)
	{
		if (offered)
		{
			return copiesInCrusade <= 0;
		}
		return false;
	}

	private static int CopiesInCrusade(BlueprintSettlementBuilding blueprint)
	{
		try
		{
			return (KingdomState.Instance?.SettlementsManager?.Settlements)?.Sum((SettlementState s) => (s?.Buildings?.Count((SettlementBuilding b) => b?.Blueprint == blueprint)).GetValueOrDefault()) ?? 0;
		}
		catch
		{
			return 0;
		}
	}

	public override bool CanBuild(BlueprintSettlementBuilding blueprint, SettlementState settlement)
	{
		return MayBuild(ContentOffering.IsOffered(Content), CopiesInCrusade(blueprint));
	}

	public override bool CanBuildHere(BlueprintSettlementBuilding blueprint, SettlementState settlement, SettlementGridTopology.Slot slot)
	{
		if (slot?.Building != null)
		{
			return true;
		}
		return CanBuild(blueprint, settlement);
	}
}
