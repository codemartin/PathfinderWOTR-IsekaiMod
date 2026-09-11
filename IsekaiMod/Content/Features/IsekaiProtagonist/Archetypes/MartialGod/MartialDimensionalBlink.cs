using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod
{
	internal class MartialDimensionalBlink
	{
		private static readonly BlueprintAbility DimensionDoor = BlueprintTools.GetBlueprint<BlueprintAbility>("5bdc37e4acfa209408334326076a43bc");

		public static void Add()
		{
			BlueprintBuff MartialBlinkBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialBlinkBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial Step Advantage");
				bp.SetDescription(Main.IsekaiContext, "Having flash-stepped behind the enemy through condensed Ki propulsion, your attacks catch foes flat-footed for 1 round.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)DimensionDoor)?.m_Icon;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Invisible;
				});
			});
			BlueprintAbility MartialDimensionalBlinkAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialDimensionalBlinkAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial Step");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, the Martial God propels their body through instantaneous spatial displacement up to 40 feet, reappearing instantly and catching all nearby foes flat-footed for 1 round.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)DimensionDoor)?.m_Icon;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.AddComponent(delegate(AbilityCustomDimensionDoor c)
				{
					c.Radius = new Feet(0f);
					c.PortalFromPrefab = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.PortalFromPrefab;
					c.PortalToPrefab = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.PortalToPrefab;
					c.PortalBone = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.PortalBone ?? "";
					c.CasterDisappearFx = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.CasterDisappearFx;
					c.CasterAppearFx = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.CasterAppearFx;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = MartialBlinkBuff.ToReference<BlueprintBuffReference>(),
						Permanent = false,
						DurationValue = Values.Duration.OneRound,
						IsNotDispelable = true
					});
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MartialDimensionalBlinkFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial Step");
				bp.SetDescription(Main.IsekaiContext, "At 7th level, the Martial God masters instant flash-step movement, gaining the ability to displace through space as a swift action up to 40 feet.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)DimensionDoor)?.m_Icon;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { MartialDimensionalBlinkAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
