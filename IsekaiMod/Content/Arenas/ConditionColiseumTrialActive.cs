using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("865ef3044195427fa2f9f1b1decda3fb")]
public class ConditionColiseumTrialActive : IsekaiCondition
{
	protected override bool CheckSafely()
	{
		return DimensionalArenaManager.Instance.IsActive;
	}

	public override string GetConditionCaption()
	{
		return "A Planar Coliseum trial is underway";
	}
}
