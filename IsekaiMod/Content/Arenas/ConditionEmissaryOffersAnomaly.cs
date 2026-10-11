using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("a67d68f9c6df4878802b21f492b75edd")]
public class ConditionEmissaryOffersAnomaly : IsekaiCondition
{
	public int Act;

	protected override bool CheckSafely()
	{
		return PlanarGatewayEmissary.CurrentlyOffersAnomaly(Act);
	}

	public override string GetConditionCaption()
	{
		return "The Otherworldly Emissary offers Barren Anomaly " + Act + " here";
	}
}
