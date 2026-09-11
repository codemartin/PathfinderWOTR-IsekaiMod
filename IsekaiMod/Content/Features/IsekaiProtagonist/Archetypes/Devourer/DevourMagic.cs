using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer
{
	internal class DevourMagic
	{
		private static readonly Sprite Icon_DevourMagic = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("f0f761b808dc4b149b08eaf44b99f633"))?.m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource DevourMagicResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DevourMagicResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Constitution
				};
			});
			BlueprintAbility DevourMagicAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DevourMagicAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Devour Magic");
				bp.SetDescription(Main.IsekaiContext, "As a standard action, the Slime consumes the magical energies wreathing an enemy target. Strikes the target with a targeted Greater Dispel Magic. Whenever an active spell is successfully consumed, the Slime heals 10 hit points per spell level and gains 5 temporary hit points. Usable 3 + Constitution modifier times per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_DevourMagic;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Close;
				bp.CanTargetEnemies = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = DevourMagicResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionDispelMagic
					{
						m_BuffType = ContextActionDispelMagic.BuffType.All,
						OnlyTargetEnemyBuffs = true,
						m_CheckType = RuleDispelMagic.CheckType.CasterLevel,
						OneRollForAll = true,
						m_StopAfterCountRemoved = true,
						m_CountToRemove = 1,
						m_MaxSpellLevel = 10,
						m_CloneBuffsToCaster = true
					}, new ContextActionOnContextCaster
					{
						Actions = Helpers.CreateActionList(new ContextActionHealTarget
						{
							Value = new ContextDiceValue
							{
								DiceType = DiceType.Zero,
								DiceCountValue = 0,
								BonusValue = 30
							}
						})
					});
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "DevourMagicFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Devour Magic");
				bp.SetDescription(Main.IsekaiContext, "At 11th level, the Slime gains the ability to feast upon hostile magical enchantments, dispelling enemy buffs while restoring health (3 + Constitution modifier uses per day).");
				((BlueprintUnitFact)bp).m_Icon = Icon_DevourMagic;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = DevourMagicResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { DevourMagicAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
