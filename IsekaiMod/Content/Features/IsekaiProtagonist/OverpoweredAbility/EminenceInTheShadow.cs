using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class EminenceInTheShadow
	{
		private static readonly Sprite Icon_Shadow = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_REALM_SHADOW.png");

		public static void Add()
		{
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "EminenceInTheShadowFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Eminence in the Shadow");
				bp.SetDescription(Main.IsekaiContext, "Operating from the theatrical pinnacle of unseen power, you strike with apocalyptic precision while remaining completely undetected.\nBenefit: You gain a competence bonus to Stealth checks (+5 at levels 1-9, +8 at levels 10-14, and +10 at level 15+), an insight bonus to Initiative (+4 at levels 1-9, +6 at levels 10-14, and +8 at level 15+), bonus Sneak Attack damage (+1d6 at levels 1-9, +2d6 at levels 10-14, and +3d6 at level 15+), a +4 bonus on attack rolls to confirm critical hits, and all critical strike multipliers increase by 1 starting at level 10.\nNote: Mutually exclusive with Abyssal Corsair (Planar Privateer).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shadow;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 5
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 8
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 10
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.DamageBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 4
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 6
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 8
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = Values.CreateContextRankValue(AbilityRankType.DamageBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 1
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 3
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SneakAttack;
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
				bp.AddComponent(delegate(CriticalConfirmationBonus c)
				{
					c.Bonus = 4;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncreaseScaled c)
				{
					c.Type = WeaponRangeType.Melee;
					c.AdditionalMultiplier = 1;
					c.MinCharacterLevel = 10;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncreaseScaled c)
				{
					c.Type = WeaponRangeType.Ranged;
					c.AdditionalMultiplier = 1;
					c.MinCharacterLevel = 10;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncreaseScaled c)
				{
					c.Type = WeaponRangeType.Touch;
					c.AdditionalMultiplier = 1;
					c.MinCharacterLevel = 10;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "AbyssalCorsairFeature");
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
