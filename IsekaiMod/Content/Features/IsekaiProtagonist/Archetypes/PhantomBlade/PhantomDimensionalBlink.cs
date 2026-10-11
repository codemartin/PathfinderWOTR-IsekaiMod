using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.PhantomBlade;

internal class PhantomDimensionalBlink
{
	private static readonly BlueprintAbility DimensionDoor = BlueprintTools.GetBlueprint<BlueprintAbility>("5bdc37e4acfa209408334326076a43bc");

	private static readonly Sprite Icon_PhantomStep = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("045351f1421ee3f449a9143db701d192"))?.m_Icon ?? ((BlueprintUnitFact)DimensionDoor)?.m_Icon;

	public static void Add()
	{
		BlueprintAbilityResource PhantomStepResource = Helpers.CreateBlueprint(Main.IsekaiContext, "PhantomStepResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = DailyUses.Amount(3, StatType.Charisma);
		});
		BlueprintBuff PhantomBlinkBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "PhantomBlinkBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Phantom Blink Advantage");
			bp.SetDescription(Main.IsekaiContext, "Having phased behind the enemy through dimensional displacement, your attacks catch foes flat-footed for 1 round.");
			((BlueprintUnitFact)bp).m_Icon = Icon_PhantomStep;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddCondition c)
			{
				c.Condition = UnitCondition.Invisible;
			});
		});
		BlueprintAbility PhantomDimensionalBlinkAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PhantomDimensionalBlinkAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Phantom Step");
			bp.SetDescription(Main.IsekaiContext, "As a swift action, the Phantom Blade phases through spatial coordinates up to 40 feet, reappearing instantly and catching all nearby foes flat-footed for 1 round. Usable 3 + Charisma modifier times per day.");
			DailyUses.Limit(bp, PhantomStepResource);
			((BlueprintUnitFact)bp).m_Icon = Icon_PhantomStep;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Close;
			bp.CanTargetPoint = true;
			bp.CanTargetSelf = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.ActionType = UnitCommand.CommandType.Swift;
			bp.AvailableMetamagic = Metamagic.Quicken;
			bp.AddComponent(TeleportMechanics.CasterOnlyTeleport());
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
				{
					m_Buff = PhantomBlinkBuff.ToReference<BlueprintBuffReference>(),
					Permanent = false,
					DurationValue = Values.Duration.OneRound,
					IsNotDispelable = true
				});
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "PhantomDimensionalBlinkFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Phantom Step");
			bp.SetDescription(Main.IsekaiContext, "At 7th level, the Phantom Blade masters spatial evasion, gaining the ability to phase through dimensions as a swift action up to 40 feet, 3 + Charisma modifier times per day.");
			DailyUses.Grant(bp, PhantomStepResource);
			((BlueprintUnitFact)bp).m_Icon = Icon_PhantomStep;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { PhantomDimensionalBlinkAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}
}
