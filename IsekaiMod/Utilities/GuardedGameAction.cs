using System.Linq;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;

namespace IsekaiMod.Utilities;

[TypeId("9d8ae7c6c01e428c92dc83dee73fff03")]
public class GuardedGameAction : IsekaiGameAction
{
	public GameAction Inner;

	protected override void RunSafely()
	{
		Inner?.RunAction();
	}

	public override string GetCaption()
	{
		if (Inner != null)
		{
			return Inner.GetCaption();
		}
		return "Nothing";
	}

	internal static GameAction[] GuardAll(GameAction[] actions)
	{
		if (actions == null)
		{
			return new GameAction[0];
		}
		return (from a in actions
			where a != null
			select (!(a is IsekaiGameAction) && !(a is IsekaiContextAction)) ? new GuardedGameAction
			{
				Inner = a
			} : a).ToArray();
	}
}
