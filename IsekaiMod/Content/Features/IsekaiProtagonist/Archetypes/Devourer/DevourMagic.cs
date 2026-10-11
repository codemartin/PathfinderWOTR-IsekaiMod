using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;

internal class DevourMagic
{
	private static readonly Sprite Icon_DevourMagic = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("f0f761b808dc4b149b08eaf44b99f633"))?.m_Icon;

	public static void Add()
	{
		BlueprintAbilityResource DevourMagicResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DevourMagicResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = DailyUses.Amount(3, StatType.Constitution);
		});
		BlueprintAbility DevourMagicAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DevourMagicAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Devour Magic");
			bp.SetDescription(Main.IsekaiContext, "As a standard action, the Slime consumes the magical energies wreathing an enemy target. Strikes the target with a targeted Greater Dispel Magic. If an active effect is successfully consumed, the Slime heals 30 hit points and takes a copy of that effect onto itself. Usable 3 + Constitution modifier times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_DevourMagic;
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Close;
			bp.CanTargetEnemies = true;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
			DailyUses.Limit(bp, DevourMagicResource);
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionDispelMagic d)
				{
					d.m_BuffType = ContextActionDispelMagic.BuffType.All;
					d.OnlyTargetEnemyBuffs = true;
					d.m_CheckType = RuleDispelMagic.CheckType.CasterLevel;
					d.OneRollForAll = true;
					d.m_StopAfterCountRemoved = true;
					d.m_CountToRemove = 1;
					d.m_MaxSpellLevel = 10;
					d.m_CloneBuffsToCaster = true;
					d.ContextBonus = 0;
					d.OnFail = Helpers.CreateActionList();
					d.OnSuccess = Helpers.CreateActionList(new ContextActionOnContextCaster
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
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "DevourMagicFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Devour Magic");
			bp.SetDescription(Main.IsekaiContext, "At 11th level, the Slime gains the ability to feast upon hostile magical enchantments, dispelling enemy buffs while restoring health, 3 + Constitution modifier times per day.");
			((BlueprintUnitFact)bp).m_Icon = Icon_DevourMagic;
			bp.IsClassFeature = true;
			DailyUses.Grant(bp, DevourMagicResource);
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { DevourMagicAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}
}
