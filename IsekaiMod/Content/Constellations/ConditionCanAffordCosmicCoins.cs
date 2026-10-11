using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Constellations;

[TypeId("c53d04f44bb54b5c995080976c559396")]
public class ConditionCanAffordCosmicCoins : IsekaiCondition
{
	public int Cost;

	protected override bool CheckSafely()
	{
		return DivineTokens.CanAffordPurchase(Cost);
	}

	public override string GetConditionCaption()
	{
		return $"The main character can pay {Cost} Cosmic Coins";
	}
}
