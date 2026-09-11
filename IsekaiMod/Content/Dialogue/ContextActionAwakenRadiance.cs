using System;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionAwakenRadiance : ContextAction
	{
		public RadianceForm InitialForm;

		public override string GetCaption()
		{
			return $"Awakens Radiance into {InitialForm}";
		}

		public override void RunAction()
		{
			try
			{
				Player player = Game.Instance?.Player;
				if (player == null)
				{
					return;
				}
				UnitEntityData value = player.MainCharacter.Value;
				if (value == null)
				{
					return;
				}
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "RadiantSoulFeature");
				if (modBlueprint != null && !value.Descriptor.HasFact(modBlueprint))
				{
					value.Descriptor.AddFact(modBlueprint);
				}
				BlueprintAbility modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintAbility>(Main.IsekaiContext, "TransmuteRadianceAbility");
				if (modBlueprint2 != null && !value.Descriptor.HasFact(modBlueprint2))
				{
					value.Descriptor.AddFact(modBlueprint2);
				}
				BlueprintItemWeapon weapon = IsekaiRadiance.GetRadianceBlueprint(1, InitialForm);
				if (weapon != null)
				{
					player.Inventory.Add(weapon, 1);
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#FFD700><b>[Radiance Awakened]</b></color> <b>" + weapon.Name + "</b> has bonded with your soul!");
					});
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ContextActionAwakenRadiance: " + ex);
			}
		}
	}
}
