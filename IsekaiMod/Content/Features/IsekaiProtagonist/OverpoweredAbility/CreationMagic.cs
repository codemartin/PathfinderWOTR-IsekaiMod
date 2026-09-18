using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class CreationMagic
	{
		private static readonly Sprite Icon_Forge = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_DEUS_EX_MACHINA.png");

		public static void Add()
		{
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CreationMagicFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Creation Magic");
				bp.SetDescription(Main.IsekaiContext, "Through mastery of conceptual creation magic, you materialize supreme weaponry, armor, and endless munitions from pure astral mana.\nBenefit: All weapon attacks deal additional physical force damage (+2 at levels 1-9, +4 at levels 10-14, and +6 at level 15+) and bypass all enemy damage reduction. You gain an enhancement bonus to attack rolls (+1 at levels 1-9, +2 at levels 10-14, and +3 at level 15+), an armor bonus to AC (+2 at levels 1-9, +3 at levels 10-14, and +4 at level 15+), and DR/Adamantine (3 at levels 1-9, 6 at levels 10-14, and 10 at level 15+).\nNote: Mutually exclusive with Primal Chimera (Beast Metamorphosis).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Forge;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
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
							ProgressionValue = 4
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 6
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = Values.CreateContextRankValue(AbilityRankType.DamageBonus);
				});
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
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.ProjectilesCount;
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
					c.Descriptor = ModifierDescriptor.Armor;
					c.Stat = StatType.AC;
					c.Value = Values.CreateContextRankValue(AbilityRankType.ProjectilesCount);
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
							ProgressionValue = 3
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 6
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 10
						}
					};
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
					c.BypassedByMaterial = true;
					c.Material = PhysicalDamageMaterial.Adamantite;
				});
				bp.AddComponent<IgnoreDamageReductionOnAttack>();
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteNoFeature c)
			{
				c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "PrimalChimeraFeature");
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
