using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class SupremeBeing
	{
		private static readonly Sprite Icon_KiPowerSelection = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3049386713ff04245a38b32483362551")).m_Icon;

		public static BlueprintFeature CosmicSupremeBeingFeature;

		public static void Add()
		{
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SupremeBeing", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Supreme Being");
				bp.SetDescription(Main.IsekaiContext, "You have tapped into supreme divine essence, achieving perfection in body and mind infused with divine powers. \nBenefit: Gain an initial +2 inherent bonus to all attributes. Additionally, gain a +1 inherent bonus to all attributes for every 4 character levels (up to +7 at level 20).");
				((BlueprintUnitFact)bp).m_Icon = Icon_KiPowerSelection;
				bp.ReapplyOnLevelUp = true;
				StatType[] obj = new StatType[6]
				{
					StatType.Strength,
					StatType.Dexterity,
					StatType.Constitution,
					StatType.Intelligence,
					StatType.Wisdom,
					StatType.Charisma
				};
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[6]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 3,
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 7,
							ProgressionValue = 3
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 11,
							ProgressionValue = 4
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 15,
							ProgressionValue = 5
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 19,
							ProgressionValue = 6
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 7
						}
					};
				});
				StatType[] array = obj;
				foreach (StatType stat in array)
				{
					bp.AddComponent(delegate(AddContextStatBonus c)
					{
						c.Stat = stat;
						c.Descriptor = ModifierDescriptor.Inherent;
						c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
					});
				}
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 5;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
			CosmicSupremeBeingFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicSupremeBeingFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Crown of the Supreme Entity");
				bp.SetDescription(Main.IsekaiContext, "You have completely shattered mortal limitations, achieving transcendent perfection in body and mind infused with powers from a deity. \nBenefit: Gain an initial +5 untyped bonus to all attributes. Additionally, gain +1 untyped bonus to all attributes for every 2 character levels and +1 for every mythic rank.");
				((BlueprintUnitFact)bp).m_Icon = Icon_KiPowerSelection;
				bp.ReapplyOnLevelUp = true;
				StatType[] obj = new StatType[6]
				{
					StatType.Strength,
					StatType.Dexterity,
					StatType.Constitution,
					StatType.Intelligence,
					StatType.Wisdom,
					StatType.Charisma
				};
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Div2;
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
					c.m_Progression = ContextRankProgression.AsIs;
				});
				StatType[] array = obj;
				foreach (StatType stat in array)
				{
					bp.AddComponent(delegate(AddStatBonus c)
					{
						c.Stat = stat;
						c.Value = 5;
						c.Descriptor = ModifierDescriptor.UntypedStackable;
					});
					bp.AddComponent(delegate(AddContextStatBonus c)
					{
						c.Stat = stat;
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
					});
					bp.AddComponent(delegate(AddContextStatBonus c)
					{
						c.Stat = stat;
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
					});
				}
			});
		}
	}
}
