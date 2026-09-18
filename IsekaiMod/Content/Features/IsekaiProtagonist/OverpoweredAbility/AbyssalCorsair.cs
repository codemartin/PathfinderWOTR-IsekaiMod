using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class AbyssalCorsair
	{
		private static readonly Sprite Icon_Corsair = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_ENERGY_DARK.png");

		private static readonly BlueprintBuff FreedomOfMovementBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("1533e782fca42b84ea370fc1dcbf4fc1");

		public static void Add()
		{
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "AbyssalCorsairFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Abyssal Corsair");
				bp.SetDescription(Main.IsekaiContext, "Master of the Midnight Isles and sovereign navigator of the Abyssal Ocean. You plunder planar riches with impunity.\nBenefit: You gain permanent Freedom of Movement, bonus base movement speed (+10 ft at levels 1-9, +20 ft at levels 10-14, and +30 ft at level 15+), a dodge bonus to Armor Class (+2 at levels 1-9, +3 at levels 10-14, and +4 at level 15+), and a luck bonus to attack and damage rolls (+1 at levels 1-9, +2 at levels 10-14, and +3 at level 15+).\nNote: Mutually exclusive with Eminence in the Shadow (Theatrical Subterfuge).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Corsair;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				if (FreedomOfMovementBuff != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { FreedomOfMovementBuff.ToReference<BlueprintUnitFactReference>() };
					});
				}
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
							ProgressionValue = 10
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 20
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 30
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
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
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 3
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 4
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
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
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalDamage;
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "EminenceInTheShadowFeature");
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
