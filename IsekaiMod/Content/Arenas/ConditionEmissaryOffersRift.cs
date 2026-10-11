using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("7a3a461770994d87bd52a5bd6c2592a3")]
public class ConditionEmissaryOffersRift : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return PlanarGatewayEmissary.CurrentlyOffersRift();
	}

	public override string GetConditionCaption()
	{
		return "The Otherworldly Emissary offers the open rift act's site here";
	}
}
