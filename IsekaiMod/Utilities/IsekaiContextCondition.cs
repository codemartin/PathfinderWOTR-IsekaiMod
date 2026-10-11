using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace IsekaiMod.Utilities;

public abstract class IsekaiContextCondition : ContextCondition
{
	protected virtual bool ResultIfBroken => false;

	public sealed override bool CheckCondition()
	{
		return ElementSafety.Check(this, CheckSafely, ElementSafety.BeforeNot(ResultIfBroken, Not));
	}

	protected abstract bool CheckSafely();
}
