using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Combat;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;
using Kingmaker.TurnBasedMode;
using TurnBased.Controllers;

namespace IsekaiMod.Content.Constellations;

[TypeId("701f0c06c39742caa4b81f77e5f8ad3a")]
public class ContextActionChronoSurge : IsekaiContextAction
{
	internal static LocalizedString SurgeMessage;

	public override string GetCaption()
	{
		return "Refunds standard, move and swift actions";
	}

	protected override void RunSafely()
	{
		UnitEntityData unitEntityData = base.Target?.Unit ?? base.Context?.MaybeCaster;
		if (!(unitEntityData == null))
		{
			Refund(unitEntityData);
			if (SurgeMessage != null)
			{
				DivineTokens.Log(SurgeMessage, unitEntityData.CharacterName);
			}
		}
	}

	internal static void Refund(UnitEntityData unit)
	{
		UnitCombatState.Cooldowns cooldowns = unit.CombatState?.Cooldown;
		if (cooldowns != null)
		{
			cooldowns.StandardAction = 0f;
			cooldowns.MoveAction = 0f;
			cooldowns.SwiftAction = 0f;
			TurnController turnController = Game.Instance?.TurnBasedCombatController?.CurrentTurn;
			if (turnController != null && turnController.Rider == unit && turnController.Mount == null)
			{
				turnController.m_RiderActionState = new ActionsState(turnController.ComputeCurrentActionStates(unit), unit.IsSurprising());
			}
			else
			{
				unit.CombatState.TBM?.ActionState?.Clear();
			}
		}
	}
}
