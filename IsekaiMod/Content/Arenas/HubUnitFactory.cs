using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using System;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas
{
	/// <summary>
	/// The coliseum's hub NPCs used to be built as bare units: no stats, no skills, no body, no brain.
	/// UnitDescriptor.Initialize calls SetSkills on the blueprint's Skills without a null check, so every
	/// spawn threw and the NPCs never appeared in the hub. Each NPC is now a copy of the template unit
	/// (the Monadic Deva) with the template's components and facts stripped, so it has a complete stat
	/// block and body before the per-NPC setup runs.
	/// </summary>
	internal static class HubUnitFactory
	{
		public static BlueprintUnit Create(BlueprintUnit template, string name, Action<BlueprintUnit> init)
		{
			if (template != null)
			{
				return template.CreateCopy(Main.IsekaiContext, name, delegate(BlueprintUnit bp)
				{
					bp.ComponentsArray = new BlueprintComponent[0];
					bp.m_AddFacts = new BlueprintUnitFactReference[0];
					bp.m_StartingInventory = new BlueprintItemReference[0];
					if (bp.Skills == null)
					{
						bp.Skills = new BlueprintUnit.UnitSkills();
					}
					init?.Invoke(bp);
				});
			}
			return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintUnit bp)
			{
				bp.Skills = new BlueprintUnit.UnitSkills();
				bp.Strength = 10;
				bp.Dexterity = 10;
				bp.Constitution = 10;
				bp.Intelligence = 10;
				bp.Wisdom = 10;
				bp.Charisma = 10;
				bp.Speed = new Kingmaker.Utility.Feet(30f);
				init?.Invoke(bp);
			});
		}

		/// <summary>
		/// Makes a spawned NPC talkable. SpawnerInteractionDialog only registers its interaction when a
		/// UnitSpawner initialises the unit, which never happens for units spawned in code, so the
		/// interaction wrapper is registered on the unit's interaction part directly. Safe to call again on
		/// a unit that already has it.
		/// </summary>
		public static void AttachDialog(Kingmaker.EntitySystem.Entities.UnitEntityData unit, Kingmaker.DialogSystem.Blueprints.BlueprintDialog dialog)
		{
			if (unit == null || dialog == null)
			{
				return;
			}
			try
			{
				Kingmaker.View.UnitEntityView view = unit.View;
				if (view == null)
				{
					return;
				}
				Kingmaker.UnitLogic.Interaction.SpawnerInteractionDialog component = view.gameObject.GetComponent<Kingmaker.UnitLogic.Interaction.SpawnerInteractionDialog>()
					?? view.gameObject.AddComponent<Kingmaker.UnitLogic.Interaction.SpawnerInteractionDialog>();
				component.m_Dialog = dialog.ToReference<BlueprintDialogReference>();
				if (component.Conditions == null)
				{
					component.Conditions = new Kingmaker.ElementsSystem.ConditionsReference();
				}
				component.OverrideDistance = 3f;
				Kingmaker.UnitLogic.Parts.UnitPartInteractions interactions = unit.Ensure<Kingmaker.UnitLogic.Parts.UnitPartInteractions>();
				// Drop any earlier wrapper for this component so a reload does not register it twice.
				interactions.RemoveInteractions((Kingmaker.UnitLogic.Interaction.IUnitInteraction i) => i is Kingmaker.UnitLogic.Interaction.SpawnerInteractionPart.Wrapper w && w.Source == component);
				bool alreadyRegistered = false;
				if (!alreadyRegistered)
				{
					interactions.AddInteraction(new Kingmaker.UnitLogic.Interaction.SpawnerInteractionPart.Wrapper { Source = component });
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error attaching dialog to unit: " + ex);
			}
		}
	}
}
