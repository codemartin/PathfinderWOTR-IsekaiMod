using Kingmaker.ElementsSystem;

namespace IsekaiMod.Utilities;

public abstract class IsekaiGameAction : GameAction
{
	public sealed override void RunAction()
	{
		ElementSafety.Run(this, RunSafely);
	}

	protected abstract void RunSafely();
}
