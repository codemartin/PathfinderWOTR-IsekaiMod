using Kingmaker.ElementsSystem;

namespace IsekaiMod.Utilities;

public abstract class IsekaiCondition : Condition
{
	protected virtual bool ResultIfBroken => false;

	public sealed override bool CheckCondition()
	{
		return ElementSafety.Check(this, CheckSafely, ElementSafety.BeforeNot(ResultIfBroken, Not));
	}

	protected abstract bool CheckSafely();
}
