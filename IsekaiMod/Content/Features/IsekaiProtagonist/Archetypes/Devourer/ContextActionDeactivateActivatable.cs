using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer
{
	public class ContextActionDeactivateActivatable : ContextAction
	{
		public BlueprintActivatableAbilityReference m_Activatable;

		public override string GetCaption()
		{
			return "Deactivate Activatable Ability";
		}

		public override void RunAction()
		{
			UnitEntityData unitEntityData = base.Context?.MaybeCaster ?? base.Target?.Unit;
			if (unitEntityData == null)
			{
				return;
			}
			BlueprintActivatableAbility bp = m_Activatable?.Get();
			if (bp != null)
			{
				ActivatableAbility activatableAbility = unitEntityData.Descriptor.ActivatableAbilities.Enumerable.FirstOrDefault((ActivatableAbility a) => a.Blueprint == bp);
				if (activatableAbility != null && activatableAbility.IsOn)
				{
					activatableAbility.IsOn = false;
				}
			}
		}
	}
}
