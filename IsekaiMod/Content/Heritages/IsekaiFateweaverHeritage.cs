using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiFateweaverHeritage
	{
		public static void Add()
		{
			Sprite Icon_Luck = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("d86807f4e6d5ae34185f86c78979a995")).m_Icon;
			BlueprintAbility DimDoorAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("5bdc37e4acfa209408334326076a43bc");
			BlueprintFeature SneakAttack = BlueprintTools.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
			BlueprintAbilityResource ShadowStepResource = Helpers.CreateBlueprint(Main.IsekaiContext, "FateweaverShadowStepResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintAbility ShadowStepAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "FateweaverShadowStepAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Step");
				bp.SetDescription(Main.IsekaiContext, "Instantly fold spatial coordinates to step through shadows to a target location within close range as a swift action.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)DimDoorAbility)?.m_Icon;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = ShadowStepResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityCustomDimensionDoor c)
				{
					c.Radius = new Feet(0f);
					c.PortalFromPrefab = DimDoorAbility?.GetComponent<AbilityCustomDimensionDoor>()?.PortalFromPrefab;
					c.PortalToPrefab = DimDoorAbility?.GetComponent<AbilityCustomDimensionDoor>()?.PortalToPrefab;
					c.PortalBone = DimDoorAbility?.GetComponent<AbilityCustomDimensionDoor>()?.PortalBone ?? "";
					c.CasterDisappearFx = DimDoorAbility?.GetComponent<AbilityCustomDimensionDoor>()?.CasterDisappearFx;
					c.CasterAppearFx = DimDoorAbility?.GetComponent<AbilityCustomDimensionDoor>()?.CasterAppearFx;
				});
			});
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiFateweaverHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Fateweaver Halfling");
				bp.SetDescription(Main.IsekaiContext, "Reincarnated with the supernatural ability to see, pluck, and reweave the invisible threads of fate and chance. Fateweavers slip effortlessly past impossible odds and strike where destiny is most vulnerable.\nThe Fateweaver Halfling gains a +4 racial bonus to Dexterity and Charisma, a +2 luck bonus on all attack rolls and saving throws, +2d6 Sneak Attack damage, and the Shadow Step swift teleportation ability usable 3 times per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Luck;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[2]
					{
						SneakAttack.ToReference<BlueprintUnitFactReference>(),
						SneakAttack.ToReference<BlueprintUnitFactReference>()
					};
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = ShadowStepResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ShadowStepAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
				bp.ReapplyOnLevelUp = true;
			});
			BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b3bebe76e6c64e2ca11585f9e3e2554a")?.AddToSelection(feature);
		}
	}
}
