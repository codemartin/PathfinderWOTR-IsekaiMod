using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class EminenceInTheShadow
	{
		private static readonly Sprite Icon_Shadow = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_REALM_SHADOW.png");

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "EminenceInTheShadowFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Eminence in the Shadow");
				bp.SetDescription(Main.IsekaiContext, "Operating from the theatrical pinnacle of unseen power, you strike with apocalyptic precision while remaining completely undetected.\nBenefit: You gain a +10 competence bonus to Stealth checks, a +8 insight bonus to Initiative, +3d6 Sneak Attack damage, and all critical strike multipliers increase by 1.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shadow;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = 8;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SneakAttack;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
				{
					c.Type = WeaponRangeType.Melee;
					c.AdditionalMultiplier = 1;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
				{
					c.Type = WeaponRangeType.Ranged;
					c.AdditionalMultiplier = 1;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
				{
					c.Type = WeaponRangeType.Touch;
					c.AdditionalMultiplier = 1;
				});
			}));
		}
	}
}
