using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class TrainingMontage
	{
		private static readonly Sprite Icon_LegendaryProportions = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("da1b292d91ba37948893cdbe9ea89e28")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToMartialSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "TrainingMontage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Training Montage");
				bp.SetDescription(Main.IsekaiContext, "Through relentless training and an unwavering desire to better yourself, you gain a bonus to all attributes scaling from +1 at level 2 to +8 at level 20.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_LegendaryProportions;
				bp.ReapplyOnLevelUp = true;
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[9]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 1,
							ProgressionValue = 0
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 4,
							ProgressionValue = 1
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 7,
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 3
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 12,
							ProgressionValue = 4
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 5
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 17,
							ProgressionValue = 6
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 19,
							ProgressionValue = 7
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 8
						}
					};
				});
				StatType[] array = new StatType[6]
				{
					StatType.Strength,
					StatType.Dexterity,
					StatType.Constitution,
					StatType.Intelligence,
					StatType.Wisdom,
					StatType.Charisma
				};
				foreach (StatType stat in array)
				{
					bp.AddComponent(delegate(AddContextStatBonus c)
					{
						c.Stat = stat;
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
					});
				}
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}
	}
}
