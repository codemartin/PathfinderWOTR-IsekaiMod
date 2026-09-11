using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch
{
	internal class ShadowExchange
	{
		private static readonly BlueprintAbility DimensionDoor = BlueprintTools.GetBlueprint<BlueprintAbility>("5bdc37e4acfa209408334326076a43bc");

		public static void Add()
		{
			BlueprintAbility ShadowExchangeAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowExchangeAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Exchange");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, the Shadow Monarch swaps coordinates with the shadow realm, teleporting up to 60 feet away to escape danger or reposition behind enemy ranks.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)DimensionDoor)?.m_Icon;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Medium;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
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
			Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowExchangeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Exchange & Intercept");
				bp.SetDescription(Main.IsekaiContext, "At 15th level, the Shadow Monarch can instantly swap positions through shadows as a swift action. Furthermore, the Monarch's connection with their shadow legion grants a +4 dodge bonus to Armor Class and Reflex saves.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)DimensionDoor).m_Icon;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ShadowExchangeAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
