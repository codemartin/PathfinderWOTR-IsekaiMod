using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class ParallelProcessing
	{
		private static readonly Sprite Icon_Quicken = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("ef7ece7bb5bb66a41b256976b27f424e"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd"))?.m_Icon;

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(TTCoreExtensions.CreateToggleBuffFeature("ParallelProcessing", "Overpowered Ability - Parallel Processing", "By dividing your consciousness into multiple parallel computation threads, you completely eliminate the physical reaction bottleneck. Your minds operate simultaneously in perfect synchronization, enabling you to cast spells and trigger abilities with zero latency.\nBenefit: While this stance is active, your spells and spell-like abilities of 6th level or lower are automatically quickened without increasing their casting time or spell slot level. Additionally, your martial execution operates in parallel, granting 1 extra attack per round on full attacks, a +4 untyped bonus to Initiative, and 2 extra Attacks of Opportunity per round.", "Parallel Processing Stance", "Spells and spell-like abilities of 6th level or lower are automatically quickened. You gain 1 extra attack on full attack, +4 Initiative, and +2 Attacks of Opportunity.", Icon_Quicken, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOrSpellLike;
					c.Metamagic = Metamagic.Quicken;
					c.MaxSpellLevel = 6;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = false;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Initiative;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AttackOfOpportunityCount;
					c.Value = 2;
				});
			}));
		}
	}
}
