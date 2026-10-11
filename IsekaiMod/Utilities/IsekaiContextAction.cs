using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Utilities;

public abstract class IsekaiContextAction : ContextAction
{
	public sealed override void RunAction()
	{
		ElementSafety.Run(this, RunSafely);
	}

	protected abstract void RunSafely();
}
