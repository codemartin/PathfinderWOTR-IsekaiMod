using System;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionChronoSurge : ContextAction
	{
		public override string GetCaption()
		{
			return "Refunds standard and move actions";
		}

		public override void RunAction()
		{
			UnitEntityData unit = base.Target.Unit ?? base.Context?.MaybeCaster ?? Game.Instance?.Player?.SafeGetMainCharacter();
			if (!(unit == null))
			{
				try
				{
					unit.CombatState.Cooldown.StandardAction = 0f;
					unit.CombatState.Cooldown.MoveAction = 0f;
					unit.CombatState.Cooldown.SwiftAction = 0f;
					unit.CombatState.TBM?.ActionState?.Clear();
				}
				catch (Exception arg)
				{
					Main.Log($"ChronoSurge error: {arg}");
				}
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#00FFFF><b>[Chrono-Surge]</b></color> " + unit.CharacterName + " warps the temporal stream! Standard and Move actions restored!");
				});
			}
		}
	}
}
