using System;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord
{
	public class ContextActionToggleOverlordForm : ContextAction
	{
		public BlueprintBuffReference m_OverlordBuff;

		public BlueprintActivatableAbilityReference m_OverlordAbility;

		public BlueprintBuffReference m_Stance1;

		public BlueprintBuffReference m_Stance2;

		public BlueprintBuffReference m_Stance3;

		public override string GetCaption()
		{
			return "Toggle Skeletal Overlord Form";
		}

		public override void RunAction()
		{
			UnitEntityData unitEntityData = base.Context?.MaybeCaster ?? base.Target?.Unit;
			if (unitEntityData == null)
			{
				return;
			}
			BlueprintBuff blueprintBuff = m_OverlordBuff?.Get();
			if (blueprintBuff == null)
			{
				return;
			}
			if (unitEntityData.Descriptor.Buffs.HasFact(blueprintBuff))
			{
				unitEntityData.Descriptor.Buffs.RemoveFact(blueprintBuff);
				BlueprintActivatableAbility actBp = m_OverlordAbility?.Get();
				if (actBp != null)
				{
					ActivatableAbility activatableAbility = unitEntityData.Descriptor.ActivatableAbilities.Enumerable.FirstOrDefault((ActivatableAbility a) => a.Blueprint == actBp);
					if (activatableAbility != null && activatableAbility.IsOn)
					{
						activatableAbility.IsOn = false;
					}
				}
				BlueprintBuff blueprintBuff2 = m_Stance1?.Get();
				if (blueprintBuff2 != null && unitEntityData.Descriptor.Buffs.HasFact(blueprintBuff2))
				{
					unitEntityData.Descriptor.Buffs.RemoveFact(blueprintBuff2);
				}
				BlueprintBuff blueprintBuff3 = m_Stance2?.Get();
				if (blueprintBuff3 != null && unitEntityData.Descriptor.Buffs.HasFact(blueprintBuff3))
				{
					unitEntityData.Descriptor.Buffs.RemoveFact(blueprintBuff3);
				}
				BlueprintBuff blueprintBuff4 = m_Stance3?.Get();
				if (blueprintBuff4 != null && unitEntityData.Descriptor.Buffs.HasFact(blueprintBuff4))
				{
					unitEntityData.Descriptor.Buffs.RemoveFact(blueprintBuff4);
				}
			}
			else
			{
				unitEntityData.Descriptor.AddBuff(blueprintBuff, base.Context, new TimeSpan(24000, 0, 0));
			}
		}
	}
}
