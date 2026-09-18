using System;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer
{
	public class ContextActionToggleSlimeForm : ContextAction
	{
		public BlueprintBuffReference m_SlimeBuff;

		public BlueprintActivatableAbilityReference m_SlimeAbility;

		public override string GetCaption()
		{
			return "Toggle Slime Form";
		}

		public override void RunAction()
		{
			UnitEntityData unitEntityData = base.Context?.MaybeCaster ?? base.Target?.Unit;
			if (unitEntityData == null)
			{
				return;
			}
			BlueprintBuff blueprintBuff = m_SlimeBuff?.Get();
			if (blueprintBuff == null)
			{
				return;
			}
			if (unitEntityData.Descriptor.Buffs.HasFact(blueprintBuff))
			{
				unitEntityData.Descriptor.Buffs.RemoveFact(blueprintBuff);
				BlueprintActivatableAbility actBp = m_SlimeAbility?.Get();
				if (actBp != null)
				{
					ActivatableAbility activatableAbility = unitEntityData.Descriptor.ActivatableAbilities.Enumerable.FirstOrDefault((ActivatableAbility a) => a.Blueprint == actBp);
					if (activatableAbility != null && activatableAbility.IsOn)
					{
						activatableAbility.IsOn = false;
					}
				}
				BlueprintBuff blueprint = BlueprintTools.GetBlueprint<BlueprintBuff>("b1eb553893a2a874891e8685657b9be7");
				if (blueprint != null && unitEntityData.Descriptor.Buffs.HasFact(blueprint))
				{
					unitEntityData.Descriptor.Buffs.RemoveFact(blueprint);
				}
				BlueprintBuff blueprint2 = BlueprintTools.GetBlueprint<BlueprintBuff>("81bf22b0af505554f806b18822da197a");
				if (blueprint2 != null && unitEntityData.Descriptor.Buffs.HasFact(blueprint2))
				{
					unitEntityData.Descriptor.Buffs.RemoveFact(blueprint2);
				}
				BlueprintBuff blueprint3 = BlueprintTools.GetBlueprint<BlueprintBuff>("463ce16a6166a474795ad6452efbe3a8");
				if (blueprint3 != null && unitEntityData.Descriptor.Buffs.HasFact(blueprint3))
				{
					unitEntityData.Descriptor.Buffs.RemoveFact(blueprint3);
				}
				BlueprintBuff blueprint4 = BlueprintTools.GetBlueprint<BlueprintBuff>("46d4867d7d76c7d4583bdd6636a983ef");
				if (blueprint4 != null && unitEntityData.Descriptor.Buffs.HasFact(blueprint4))
				{
					unitEntityData.Descriptor.Buffs.RemoveFact(blueprint4);
				}
			}
			else
			{
				unitEntityData.Descriptor.AddBuff(blueprintBuff, base.Context, new TimeSpan(24000, 0, 0));
			}
		}
	}
}
