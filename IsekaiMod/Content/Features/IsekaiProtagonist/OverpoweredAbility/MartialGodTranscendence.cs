using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
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
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class MartialGodTranscendence
	{
		private static readonly BlueprintAbility DimensionDoor = BlueprintTools.GetBlueprint<BlueprintAbility>("5bdc37e4acfa209408334326076a43bc");

		private static readonly Sprite Icon_Transcendence = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e4450dd9c06dc034fb7c0c08abcc202b"))?.m_Icon;

		public static void Add()
		{
			BlueprintBuff MartialGodTranscendenceBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodTranscendenceBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendental Velocity");
				bp.SetDescription(Main.IsekaiContext, "Having flash-stepped through sheer physical velocity, your next strikes catch the foe flat-footed and carry a +4 untyped bonus to attack rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Transcendence;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Invisible;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
			});
			BlueprintAbility MartialGodTranscendenceAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodTranscendenceAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendental Flash Step");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, instantaneously displace yourself up to 40 feet. Foes at your destination are caught flat-footed and your attacks gain a +4 bonus for 1 round.");
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
						m_Buff = MartialGodTranscendenceBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = Values.Duration.OneRound
					});
				});
			});
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodTranscendenceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Martial God Transcendence");
				bp.SetDescription(Main.IsekaiContext, "Exclusive to the Martial God archetype. You have ascended beyond mortal martial disciplines, turning your physical form into an engine of supreme combat superiority.\nBenefit: You gain +30 feet to base movement speed, +5 feet reach with melee weapons, +1 extra attack per round on a full attack, and your critical damage multiplier increases by 1 across all attack types. Additionally, you can use Transcendental Flash Step as a swift action at will.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Transcendence;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { MartialGodTranscendenceAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 30;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Reach;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(AddExtraOffHandAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
				{
					c.Type = WeaponRangeType.Melee;
					c.AdditionalMultiplier = 1;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
				{
					c.Type = WeaponRangeType.Ranged;
					c.AdditionalMultiplier = 1;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
				{
					c.Type = WeaponRangeType.Touch;
					c.AdditionalMultiplier = 1;
				});
				bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.m_Archetype = MartialGodArchetype.GetReference();
					c.Level = 1;
				});
			}));
		}
	}
}
