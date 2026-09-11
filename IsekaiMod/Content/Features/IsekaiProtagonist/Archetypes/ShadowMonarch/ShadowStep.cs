using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch
{
	internal class ShadowStep
	{
		private static readonly BlueprintAbility DimensionDoor = BlueprintTools.GetBlueprint<BlueprintAbility>("5bdc37e4acfa209408334326076a43bc");

		public static void Add()
		{
			BlueprintAbilityResource ShadowStepResource = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowStepResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Charisma
				};
			});
			BlueprintAbility ShadowStepAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowStepAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Step");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, the Shadow Monarch steps into the world of shadows, emerging instantly at a target location up to 40 feet away.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)DimensionDoor)?.m_Icon;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = ShadowStepResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityCustomDimensionDoor c)
				{
					c.Radius = new Feet(0f);
					c.PortalFromPrefab = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.PortalFromPrefab;
					c.PortalToPrefab = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.PortalToPrefab;
					c.PortalBone = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.PortalBone ?? "";
					c.CasterDisappearFx = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.CasterDisappearFx;
					c.CasterAppearFx = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.CasterAppearFx;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowStepFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Step");
				bp.SetDescription(Main.IsekaiContext, "At 3rd level, the Shadow Monarch can slip through physical space by stepping into the shadow dimension as a swift action.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)DimensionDoor).m_Icon;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = ShadowStepResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ShadowStepAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
