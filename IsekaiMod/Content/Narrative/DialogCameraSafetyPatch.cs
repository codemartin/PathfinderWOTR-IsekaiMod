using System;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Controllers.Dialog;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Narrative
{
	[HarmonyPatch(typeof(DialogController), "set_CurrentCue")]
	internal static class DialogCameraSafetyPatch
	{
		private static bool _suppressed;

		private static DialogSpeaker _targetSpeaker;

		[HarmonyPrefix]
		public static void Prefix(BlueprintCue value)
		{
			_suppressed = false;
			_targetSpeaker = null;
			try
			{
				if (value?.Speaker == null || !value.Speaker.MoveCamera || value.Speaker.Blueprint == null)
				{
					return;
				}
				Player player = Game.Instance?.Player;
				if (player == null)
				{
					return;
				}
				UnitEntityData entity = value.Speaker.GetEntity(value);
				if (!(entity == null) && entity.IsInGame)
				{
					UnitDescriptor descriptor = entity.Descriptor;
					if ((descriptor == null || descriptor.State?.IsDead != true) && (!entity.IsPlayerFaction || player.Party.Contains(entity)))
					{
						return;
					}
				}
				_suppressed = true;
				_targetSpeaker = value.Speaker;
				value.Speaker.MoveCamera = false;
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in DialogCameraSafetyPatch: " + ex);
			}
		}

		[HarmonyPostfix]
		public static void Postfix()
		{
			if (_suppressed && _targetSpeaker != null)
			{
				_targetSpeaker.MoveCamera = true;
				_suppressed = false;
				_targetSpeaker = null;
			}
		}
	}
}
