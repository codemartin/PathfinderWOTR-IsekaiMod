using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class InfiniteArmory
	{
		private static readonly Sprite Icon_Armory = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("36c8971e91f1745418cc3ffdfac17b74"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0a5ddfbcfb3989543ac7c936fc256889"))?.m_Icon;

		public static BlueprintFeature InfiniteArmoryFeature;

		public static void Add()
		{
			InfiniteArmoryFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "InfiniteArmoryFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power: Infinite Armory");
				bp.SetDescription(Main.IsekaiContext, "Dimensional rifts open behind you at will, manifesting an infinite treasury of phantasmal weapons that strike your foes in unison.\nBenefit: Whenever you strike an enemy with a weapon attack, ethereal blades barrage the target, dealing an additional 2d6 piercing damage and 2d6 pure force damage. At character level 15, this increases to 4d6 piercing and 4d6 force damage.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Armory;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
				{
					c.OnlyHit = true;
					c.Action = Helpers.CreateActionList(new ContextActionDealDamage
					{
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Physical,
							Common = new DamageTypeDescription.CommomData(),
							Physical = new DamageTypeDescription.PhysicalData
							{
								Form = PhysicalDamageForm.Piercing
							}
						},
						Duration = Values.Duration.Zero,
						Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = Values.CreateContextRankValue(AbilityRankType.Default),
							BonusValue = 0
						}
					}, new ContextActionDealDamage
					{
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Force,
							Common = new DamageTypeDescription.CommomData(),
							Physical = new DamageTypeDescription.PhysicalData()
						},
						Duration = Values.Duration.Zero,
						Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = Values.CreateContextRankValue(AbilityRankType.Default),
							BonusValue = 0
						}
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[2]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 4
						}
					};
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			});
			SpecialPowerSelection.AddToAuthoritySelection(InfiniteArmoryFeature);
		}
	}
}
