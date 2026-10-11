using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("ac511f7b40bd4872a48ada566bf45046")]
public class ConditionColiseumCupOffered : IsekaiCondition
{
	public string CupId;

	protected override bool CheckSafely()
	{
		return DimensionalArenaManager.IsCupOfferedHere(ColiseumCups.ById(CupId));
	}

	public override string GetConditionCaption()
	{
		return "The Planar Coliseum offers cup " + CupId + " here";
	}
}
