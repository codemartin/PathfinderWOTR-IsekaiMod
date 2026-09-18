using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class ExtremeSpeed
	{
		private static readonly Sprite Icon_SupersonicSpeed = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("505456aa17dd18a4e8bd8172811a4fdc")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToMartialSelection(TTCoreExtensions.CreateToggleAuraBuffFeature("ExtremeSpeed", "Allies within 40 feet of you gain a +20-foot bonus to speed, plus an additional 5 feet for every 4 character levels.", "This creature gains a {g|Encyclopedia:Bonus}bonus{/g} to their {g|Encyclopedia:Speed}speed{/g}.", Icon_SupersonicSpeed, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
					c.Multiplier = 5;
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.DivStep;
					c.m_StepLevel = 4;
				});
			}));
		}
	}
}
